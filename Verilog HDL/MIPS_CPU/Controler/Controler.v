module Controler(IR,PClk,Func,Reset,ALU_Zero,MemData_Ready,Mul_Ready,ExtInt,IE,ALU_Overflow,CP0_rs,Branch_al,
PCWrite,MemRW,BE,IRWrite,RegWrite,ALUOp,CP0Write,IorD,RegDst,RFSource,ALUSrcA,
ALUSrcB,SHTNumSrc,ALUOutSrc,CP0Src,PCSource,SHTOp,MULSelMD,MULStart,MULSelHL,MULWrite,
ExCode,Exception,state,MemSign);
input[31:26] IR;
wire[5:0] opcode;
assign opcode=IR;
input[25:21] CP0_rs;//IR[25:21] to identify CP0 instrucions
input[20:16] Branch_al;//IR[20:16] to identify bgezal && bltzal
input[5:0] Func;
input Reset,PClk,ALU_Overflow,ALU_Zero,MemData_Ready,Mul_Ready,ExtInt,IE;
output PCWrite;
output MemRW,MemSign;
output[3:0] BE;
output IRWrite;
output RegWrite;
output[3:0] ALUOp;
output CP0Write;
output IorD;
output[1:0] RegDst;
output[1:0] RFSource;
output ALUSrcA;
output[1:0] ALUSrcB;
output SHTNumSrc;
output[1:0] ALUOutSrc;
output CP0Src;
output[2:0] PCSource;
	output[1:0] SHTOp;
output MULSelMD,MULStart,MULSelHL,MULWrite,Exception;
output[4:0] ExCode;
output[31:0] state;
`define ALUOp_B				4'b0000
`define ALUOp_ADDU			4`b0001
`define ALUOp_ADD			4'b0010
`define ALUOp_SUBU			4'b0011
`define ALUOp_SUB			4'b0100
`define ALUOp_AND			4'b0101
`define ALUOp_OR			4'b0110
`define ALUOp_NOR			4'b0111
`define ALUOp_XOR			4'b1000
`define ALUOp_SLTU			4'b1001
`define ALUOp_SLT			4'b1010
`define ALUOp_SLET			4'b1011
`define ALUOp_LU			4'b1100
`define IorD_PC	 			0
`define IorD_ALU			1
`define RegDst_rt			2'b00
`define RegDst_rd			2'b01
`define RegDst_ra			2'b11
`define RFSource_ALU		2'b00
`define RFSource_MDR		2'b01
`define RFSource_CP0		2'b10
`define RFSource_PC			2'b11
`define ALUSrcA_PC			0
`define ALUSrcA_A			1
`define ALUSrcB_B			2'b00
`define ALUSrcB_4			2'b01
`define ALUSrcB_Imm			2'b10
`define ALUSrcB_4Imm		2'b11
`define SHTNumSrc_B			0
`define SHTNumSrc_IR		1
`define ALUOutSrc_ALU		2'b00
`define ALUOutSrc_SHIFTER	2'b01
`define ALUOutSrc_MUL		2'b10
`define CP0Src_EPC			0
`define CP0Src_cs			1	
`define PCSource_PC4		3'b001
`define PCSource_J			3'b011
`define PCSource_BRANCH		3'b010
`define PCSource_EXCEPTION	3'b100
`define PCSource_INT		3'b101
`define PCSource_EPC		3'b110
`define BE_WORD				4'b1111
`define BE_HALF				4'b0111
`define BE_BYTE				4'b0011
	`define BE_DEFAULT			4'b0000
`define MemRW_Read			0
`define MemRW_Write			1
`define Mul_SelMD_DIV		1
`define MUL_SelMD_MUL		0
`define SHTOp_NOP			2'b00
`define SHTOp_LOGLEFT		2'b01
`define SHTOp_LOGRIGHT		2'b10
`define SHTOp_ARIGHT		2'b11	
//macro of each state
`define STATE_IR			19'b000_00000000_00000001	//取指
`define STATE_OPDECODE		19'b000_00000000_00000010	//译码
`define STATE_MEMCALC		19'b000_00000000_00000100	//存储计算地址
`define STATE_MEMREAD		19'b000_00000000_00001000	//读存储
`define STATE_MEMWRITE		19'b000_00000000_00010000	//写存储
`define STATE_WRITEBACK		19'b000_00000000_00100000	//回写
`define STATE_R				19'b000_00000000_01000000	//执行R指令
`define STATE_I				19'b000_00000000_10000000	//执行I指令
`define STATE_CP0			19'b000_00000001_00000000	//执行CP0指令
`define STATE_REND			19'b000_00000010_00000000	//R指令完成
`define STATE_IEND			19'b000_00000100_00000000	//I指令完成
`define STATE_BEND			19'b000_00001000_00000000	//分支指令完成
`define STATE_JEND			19'b000_00010000_00000000	//跳转指令完成
`define STATE_CP0END		19'b000_00100000_00000000	//mtc0指令完成
`define STATE_EI			19'b000_01000000_00000000	//指令异常
`define STATE_ESC			19'b000_10000000_00000000	//SystemCall异常
`define STATE_EBP			19'b001_00000000_00000000	//Break异常
`define STATE_EBK			19'b010_00000000_00000000	//外部中断
`define STATE_EA			19'b100_00000000_00000000 	//算术异常
//state of the statemathine
reg[18:0] state;	
//for STATE_OPDECODE
wire is_sl,is_r,is_i,is_cp0,is_branch,is_j,is_ex;
assign is_sl	= IR[31]&!IR[30]&!IR[29];
assign is_r		=!IR[31]&!IR[30]&!IR[29]&!IR[28]&!IR[27]&!IR[26];
assign is_i		=!IR[31]&!IR[30]& IR[29];
assign is_cp0	=!IR[31]& IR[30]&!IR[29];
assign is_branch= IR[31]& IR[30]&!IR[29];
	assign is_jl	= IR[31]&!IR[30]& IR[29];		
assign is_esc	= IR[31]& IR[30]& IR[29]& IR[28]& IR[27]& IR[26]& Func[1]&!Func[0];
assign is_ebp	= IR[31]& IR[30]& IR[29]& IR[28]& IR[27]& IR[26]&!Func[1]& Func[0];
assign is_nop	=!(IR[31:29]||Func);	
//for STATE_R
wire is_mf;
//mfhi is 010101	mflo is 010110
assign is_mf=!Func[5]&&Func[4]&&!Func[3]&&Func[2]&&((Func[1]&&!Func[0])||(!Func[1]&&Func[0]));
always@(posedge PClk or posedge Reset)
	if (Reset)
	begin
		state<=`STATE_IR;
	end
	else
	begin
		case (state)
		`STATE_IR:
			begin	//fetch ins
				if (MemData_Ready)
					state<=`STATE_OPDECODE;
			end
		`STATE_OPDECODE:
			begin	//decode
				if (is_sl)
						state<=`STATE_MEMCALC;	//Store Load, change to Memory Calculate
				else if (is_r)
					state<=`STATE_R;		//R type, change to Execute
				else if (is_branch)
					state<=`STATE_BEND;		//Branch type, change to Branch End
				else if (is_jl)
					state<=`STATE_JEND;		//J type, change to Jump End
				else if (is_i)
					state<=`STATE_I;		//I type, change to Imm ALU
				else if (is_cp0)
					state<=`STATE_CP0;		//CP0-type
				else if (is_esc)
					state<=`STATE_ESC;		//syscall
				else if (is_ebp)
					state<=`STATE_EBP;		//break
				else if (is_nop)
					state<=`STATE_IR;		//nop
				else
					state<=`STATE_EI;		//other
			end
		`STATE_MEMCALC:
			begin	//Save&Load type
				if (IR[28]&&IR[27:26])	//load instruction
					state<=`STATE_WRITEBACK;
				else					//save instruction
					state<=`STATE_MEMREAD;
			end
		`STATE_MEMREAD:
			begin	//read
				if (MemData_Ready)		//wait for mem
					state<=`STATE_WRITEBACK;
				else
					state<=`STATE_MEMREAD;
			end
		`STATE_MEMWRITE:
			begin	//write
				if (MemData_Ready)
					state<=`STATE_IR;
				else
						state<=`STATE_MEMWRITE;
			end
		`STATE_WRITEBACK:
			state<=`STATE_IR;
		`STATE_R:
			begin	//R type
					if (is_mf&&!Mul_Ready)
					state<=`STATE_R;
				else
					state<=`STATE_REND;
			end
		`STATE_I:	//I type
			state<=`STATE_IEND;
		`STATE_CP0:
			begin
				if (CP0_rs[25]&&!CP0_rs[24:21])	//eret
					state<=`STATE_IR;
				else if (!CP0_rs[22:21]&&!CP0_rs[25:24]&&CP0_rs[23])	//mtc0
					state<=`STATE_CP0END;
				else if (ExtInt&&IE)
					state<=`STATE_EBK;
				else
					state<=`STATE_IR;
			end
		`STATE_REND:
			begin	//R finished
				if (ALU_Overflow && IE)
						state<=`STATE_EA;
				else if (ExtInt && IE)
					state<=`STATE_EBK;
				else
					state<=`STATE_IR;
			end
		`STATE_IEND:
			begin	//I finished
				if (ALU_Overflow&&IE)
					state<=`STATE_EA;
				else if (ExtInt&&IE)
					state<=`STATE_EBK;
				else
						state<=`STATE_IR;
			end
		`STATE_BEND:
			begin	//branch
				if (ExtInt&&IE)
					state<=`STATE_EBK;
				else
					state<=`STATE_IR;
			end
		`STATE_JEND:
			begin	//jump
				if (ExtInt&&IE)
					state<=`STATE_EBK;
				else
						state<=`STATE_IR;
			end
		`STATE_CP0END:
			begin	//mtc0
				if (ExtInt&&IE)
					state<=`STATE_EBK;
				else
					state<=`STATE_IR;
			end
		default:state<=`STATE_IR;
		endcase
	end
	assign is_beq	=  opcode[5] &&  opcode[4] && !opcode[3] && !opcode[2] && !opcode[1] && !opcode[0];
	assign is_bgez	=  opcode[5] &&  opcode[4] && !opcode[3] && !opcode[2] && !opcode[1] &&  opcode[0];
	assign is_bgtz	=  opcode[5] &&  opcode[4] && !opcode[3] && !opcode[2] &&  opcode[1] && !opcode[0];
	assign is_bltz	=  opcode[5] &&  opcode[4] && !opcode[3] && !opcode[2] &&  opcode[1] &&  opcode[0];
	assign is_blez	=  opcode[5] &&  opcode[4] && !opcode[3] &&  opcode[2] && !opcode[1] && !opcode[0];
	assign is_bne	=  opcode[5] &&  opcode[4] && !opcode[3] &&  opcode[2] && !opcode[1] &&  opcode[0];
	assign is_Branch_al1	=!(Branch_al[16]||Branch_al[17]||Branch_al[18]||Branch_al[19]||Branch_al[20]);
	assign is_Branch_al2	=(!(Branch_al[17]||Branch_al[18]||Branch_al[19]||Branch_al[20]))&&Branch_al[16];
	assign is_CP0_rs1	=!(CP0_rs[25]||CP0_rs[24]||CP0_rs[23]||CP0_rs[22]||CP0_rs[21]);
	assign is_CP0_rs2	=(!(CP0_rs[25]||CP0_rs[24]))&&CP0_rs[23]&&(!(CP0_rs[22]||CP0_rs[21]));
	assign is_CP0_rs3	=CP0_rs[25]&&(!(CP0_rs[24]||CP0_rs[23]||CP0_rs[22]||CP0_rs[21]));
	assign is_Func1	=!(Func[5]||Func[4]||Func[3]);
	assign is_Func2	=(!(Func[5]||Func[4]))&&Func[3]&&((!Func[2]||Func[1]));
	assign is_Func3	=(!(Func[5]||Func[4]))&&Func[3]&&(!Func[2])&&Func[1]&&(!Func[0]);
	assign is_Func4	=(!(Func[5]||Func[4]))&&Func[3]&&(!Func[2])&&Func[1]&&Func[0];
	assign is_Func5	=(!(Func[5]||Func[4]))&&Func[3]&&Func[2]&&(!(Func[1]&&Func[0]));
	assign is_Func6	=(!Func[5])&&Func[4]&&(!(Func[3]||Func[2]))&&Func[1]&&Func[0];
	assign is_Func7	=(!Func[5])&&Func[4]&&(!Func[3])&&Func[2]&&(!(Func[1]||Func[0]));
	assign is_Func8	=(!Func[5])&&Func[4]&&(!Func[3])&&Func[2]&&(!Func[1])&&Func[0];
	assign is_Func9	=(!Func[5])&&Func[4]&&(!Func[3])&&Func[2]&&Func[1]&&(!Func[0]);
	assign is_Func10=(!Func[5])&&Func[4]&&(!Func[3])&&Func[2]&&Func[1]&&Func[0];
	assign is_Func11=(!Func[5])&&Func[4]&&Func[3]&&(!(Func[2]||Func[1]||Func[0]));
	assign is_Func12=(!(Func[5]||Func[4]))&&Func[3]&&Func[2]&&!Func[1]&&Func[0];
	assign is_Func13=(!(Func[5]||Func[4]))&&Func[3]&&Func[2]&&Func[1]&&!Func[0];
	assign is_Func14=(!(Func[5]||Func[4]))&&Func[3]&&Func[2]&&Func[1]&&Func[0];
	assign is_Func15=(!Func[5])&&Func[4]&&(!(Func[3]||Func[2]||Func[1]||Func[0]));
	assign is_Func16=(!Func[5])&&Func[4]&&(!(Func[3]||Func[2]||Func[1]))&&Func[0];
	assign is_Func17=(!Func[5])&&Func[4]&&(!(Func[3]||Func[2]))&&Func[1]&&(!Func[0]);
	assign is_opcode1=(!(opcode[5]||opcode[4]))&&opcode[3]&&(!(opcode[2]||opcode[1]||opcode[0]));
	assign is_opcode2=(!(opcode[5]||opcode[4]))&&opcode[3]&&(!(opcode[2]||opcode[1]))&&opcode[0];
	assign is_opcode3=(!(opcode[5]||opcode[4]))&&opcode[3]&&(!opcode[2])&&opcode[1]&&(!opcode[0]);
	assign is_opcode4=(!(opcode[5]||opcode[4]))&&opcode[3]&&(!opcode[2])&&opcode[1]&&opcode[0];
	assign is_opcode5=(!(opcode[5]||opcode[4]))&&opcode[3]&&opcode[2]&&(!(opcode[1]||opcode[0]));
	assign is_opcode6=(!(opcode[5]||opcode[4]))&&opcode[3]&&opcode[2]&&(!opcode[1])&&opcode[0];
	assign is_opcode7=(!(opcode[5]||opcode[4]))&&opcode[3]&&opcode[2]&&opcode[1]&&(!opcode[0]);
	assign is_opcode8=(!(opcode[5]||opcode[4]))&&opcode[3]&&opcode[2]&&opcode[1]&&opcode[0];
	assign is_R1	=  opcode[5] && !opcode[4] && !opcode[3] && !opcode[2] &&  opcode[1];
	assign is_R2	=  opcode[5] && !opcode[4] && !opcode[3] &&  opcode[2] && !opcode[1] && !opcode[0];
	assign is_W1	=  opcode[5] && !opcode[4] && !opcode[3] &&  opcode[2] &&  opcode[1] && !opcode[0];
	assign is_W2	=  opcode[5] && !opcode[4] && !opcode[3] &&  opcode[2] &&  opcode[1] &&  opcode[0];
	assign is_j		=  opcode[5] && !opcode[4] && !opcode[3]&&(!(opcode[2] ||  opcode[1] ||  opcode[0]));
	assign is_jal	=  opcode[5] && !opcode[4] &&  opcode[3] && !opcode[2] && !opcode[1] &&  opcode[0];
	assign is_jalr	=  opcode[5] && !opcode[4] &&  opcode[3] && !opcode[2] &&  opcode[1] &&  opcode[0];
	assign is_jr	=  opcode[5] && !opcode[4] &&  opcode[3] && !opcode[2] &&  opcode[1] && !opcode[0];
	//PCWrite
	assign PCWrite=state[0]||					//75
			(state[8]&&is_CP0_rs3)||			//131
			(state[11]&&is_beq&&ALU_Zero)||					//138
			(state[11]&&is_bgez&&ALU_Zero&&is_Branch_al1)||	//139
			(state[11]&&is_bgez&&ALU_Zero&&is_Branch_al2)||	//140
			(state[11]&&is_bgtz&&ALU_Zero)||				//141
			(state[11]&&is_bltz&&!ALU_Zero&&is_Branch_al1)||//142
			(state[11]&&is_bltz&&!ALU_Zero&&is_Branch_al2)||//143
			(state[11]&&is_blez&&!ALU_Zero)||				//144
			(state[11]&&is_bne&&!ALU_Zero)||				//145
			state[12]||state[14]||state[15]||state[16]||state[17]||state[18];
	//MemRW
	assign MemRW=state[4];
	//MemSign
	assign MemSign=state[3]&&opcode[0];
	//BE
	assign BE[0]=state[0]||state[3]||state[4];
	assign BE[1]=state[0]||state[3]||state[4];
	assign BE[2]=state[0]||(state[3]&&is_R1)||(state[3]&&is_R2)||(state[4]&&is_W1)||(state[4]&&is_W2);
	assign BE[3]=state[0]||(state[3]&&is_R2)||(state[4]&&is_W2);	
	//IRWrite
	assign IRWrite=state[0];	
	//RegWrite
	assign RegWrite=state[5]||(state[8]&&is_CP0_rs1)||state[9]||state[10]||
					(state[11]&&is_bgez&&is_Branch_al2)||
					(state[11]&&is_bltz&&is_Branch_al2)||
					(state[12]&&is_jal)||(state[12]&&is_jalr);				
	//ALUOp
	assign ALUOp[3]=(state[6]&&is_Func1&&Func[3])||
					(state[6]&&is_Func2&&Func[3])||
					(state[6]&&is_Func3&&Func[3])||
					(state[7]&&is_opcode1)||
					(state[7]&&is_opcode6)||
					(state[7]&&is_opcode7)||
					(state[7]&&is_opcode8)||
					(state[11]&&is_bgez&&is_Branch_al1)||
					(state[11]&&is_bgez&&is_Branch_al2)||
					(state[11]&&is_bgtz)||
					(state[11]&&is_bltz&&is_Branch_al1)||
					(state[11]&&is_bltz&&is_Branch_al2)||
					(state[11]&&is_blez);
	assign ALUOp[2]=(state[6]&&is_Func1&&Func[2])||
					(state[6]&&is_Func2&&Func[2])||
					(state[6]&&is_Func3&&Func[2])||
					(state[7]&&is_opcode1)||
					(state[7]&&is_opcode4)||
					(state[7]&&is_opcode5);
	assign ALUOp[1]=state[1]||state[2]||
					(state[6]&&is_Func1&&Func[1])||
					(state[6]&&is_Func2&&Func[1])||
					(state[6]&&is_Func3&&Func[1])||
					(state[7]&&is_opcode3)||
					(state[7]&&is_opcode5)||
					(state[7]&&is_opcode8)||
					state[11]||state[14]||state[15]||state[16]||state[17]||state[18];
	assign ALUOp[0]=state[0]||
					(state[6]&&is_Func1&&Func[0])||
					(state[6]&&is_Func2&&Func[0])||
					(state[6]&&is_Func3&&Func[0])||
					(state[7]&&is_opcode2)||
					(state[7]&&is_opcode4)||
					(state[7]&&is_opcode7)||
					(state[11]&&is_beq)||
					(state[11]&&is_bgtz)||
					(state[11]&&is_blez)||
					(state[11]&&is_bne)||
					(state[11]&&is_jalr)||
					(state[11]&&is_jr)||
					state[14]||state[15]||state[16]||state[17]||state[18];
	//CP0Write
	assign CP0Write=state[13]||state[14]||state[15]||state[16]||state[17]||state[18];	
	//IorD
	assign IorD=state[3]||state[4];
	//RegDst
	assign RegDst[1]=(state[11]&&is_bgez&&is_Branch_al2)||
					(state[11]&&is_bltz&&is_Branch_al2)||
					(state[12]&&is_jal);
	assign RegDst[0]=state[9]||
					(state[11]&&is_bgez&&is_Branch_al2)||
					(state[11]&&is_bltz&&is_Branch_al2)||
					(state[12]&&is_jal)||
					(state[12]&&is_jalr);				
	//RFSource
	assign RFSource[1]=(state[8]&&is_CP0_rs1)||
					(state[11]&&is_bgez&&is_Branch_al2)||
					(state[11]&&is_bltz&&is_Branch_al2)||
					(state[12]&&is_jal)||
					(state[12]&&is_jalr);
	assign RFSource[0]=state[5]||
					(state[11]&&is_bgez&&is_Branch_al2)||
					(state[11]&&is_bltz&&is_Branch_al2)||
					(state[12]&&is_jal)||
					(state[12]&&is_jalr);				
	//ALUSrcA
	assign ALUSrcA=state[2]||
				(state[6]&&is_Func1)||
				(state[6]&&is_Func2)||
				(state[6]&&is_Func3)||
				state[7]||
				state[11]||
				(state[12]&&is_jalr)||
				(state[12]&&is_jr);					
	//ALUSrcB
	assign ALUSrcB[1]=state[1]||state[2]||state[7];
	assign ALUSrcB[0]=state[0]||state[1]||state[14]||state[15]||state[16]||state[17]||state[18];	
	//SHTNumSrc
	assign SHTNumScr=(state[6]&&is_Func12)||
					(state[6]&&is_Func14)||
					(state[6]&&is_Func16);		
	//ALUOutSrc
	assign ALUOutSrc[1]=(state[6]&&is_Func8)||
						(state[6]&&is_Func9);
	assign ALUOutSrc[0]=(state[6]&&is_Func12)||
						(state[6]&&is_Func13)||
						(state[6]&&is_Func14)||
						(state[6]&&is_Func15)||
						(state[6]&&is_Func16)||
						(state[6]&&is_Func17);
							
	//CP0Src
	assign CP0Src=(state[8]&&is_CP0_rs1)||
				(state[8]&&is_CP0_rs2)||
				state[13];
					
	//ExCode
	assign ExCode[4]=0;
	assign ExCode[3]=state[14]||state[15]||state[16]||state[17];
	assign ExCode[2]=0;
	assign ExCode[1]=state[14];
	assign ExCode[0]=state[16];	
	//PCSource
	assign PCSource[2]=(state[8]&&is_CP0_rs3)||
					state[14]||state[15]||state[16]||state[17]||state[18];
	assign PCSource[1]=(state[8]&&is_CP0_rs3)||
					(state[12]&&is_j)||
					(state[12]&&is_jal)||
					state[11]||state[14]||state[15]||state[16]||state[18];
	assign PCSource[0]=state[0]||state[12]||state[17];
	//SHTOp
	assign SHTOp[1]=(state[6]&&is_Func14)||
					(state[6]&&is_Func15)||
					(state[6]&&is_Func16)||
					(state[6]&&is_Func17);
	assign SHTOp[0]=(state[6]&&is_Func12)||
					(state[6]&&is_Func13)||
					(state[6]&&is_Func14)||
					(state[6]&&is_Func15);					
	//MULSelMD
	assign MULSelMD=(state[6]&&is_Func4)||
					(state[6]&&is_Func5);					
	//MULStart
	assign MULStart=state[6]&&(is_Func4||is_Func5||is_Func6||is_Func7);		
	//MULSelHL
	assign MULSelHL=(state[6]&&is_Func8)||
					(state[6]&&is_Func10);					
	//MULWrite
	assign MULWrite=(state[6]&&is_Func10)||
					(state[6]&&is_Func11);						
	//Exception
	assign Exception=state[14]||state[15]||state[16]||state[17]||state[18];		
endmodule