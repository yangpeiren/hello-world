`include "processor_head.v"

module MMU(clk,Reset,RW_En,CP0_EntryHi,CP0_PageMask,CP0_EntryLo0,CP0_EntryLo1,
			CP0_MMU_Func,CP0_MMU_Index,CP0_MMU_Mode,Pr_VAddr,CP0_ASID,Pr_Req,CP0_RdReq, CP0_WrReq,
			MMU_Pr_RAddr,TLB_Error,TLB_Fault,MMU_CP0_Req,MMU_CP0_BadVAddr,MMU_CP0_EntryHi,
			MMU_CP0_EntryLo0,MMU_CP0_EntryLo1,MMU_CP0_PageMask,MMU_Pr_Ack,MMU_CP0_Index,
			MMU_CP0_AckP,MMU_CP0_AckR,MMU_CP0_Matched);
//two parts of the CPU will connect to the MMU, i.e. Processor and CP0
input clk;
input Reset;
input RW_En;//Processor claim its use of the Address, Read or Write 
input[31:0] CP0_EntryHi;
input[31:0] CP0_PageMask;
input[31:0] CP0_EntryLo0;
input[31:0] CP0_EntryLo1;
input CP0_MMU_Mode;// '0' = UserMode , '1' = Kernel Mode
input[1:0] CP0_MMU_Func;
input[2:0] CP0_MMU_Index;
input[31:0] Pr_VAddr;
input[7:0] CP0_ASID;//temp name,does Pr export this value?
input Pr_Req;
input CP0_RdReq;
input CP0_WrReq;

output[31:0] MMU_Pr_RAddr;
output TLB_Error;//in UserMode,access to the higher 2G space will cause a TLB error
output[2:0] TLB_Fault;//TLB_Miss,TLB_InValid,TLB_Modified,three kind of errors
output MMU_CP0_Req;
output[31:0] MMU_CP0_BadVAddr;//bad virtual address
output[31:0] MMU_CP0_EntryHi;
output[31:0] MMU_CP0_EntryLo0;
output[31:0] MMU_CP0_EntryLo1;
output[31:0] MMU_CP0_PageMask;
output MMU_Pr_Ack;
output[2:0] MMU_CP0_Index;
output MMU_CP0_AckP;
output MMU_CP0_AckR;
output MMU_CP0_Matched;
wire TLB_Unmapped;

wire[31:12] VPN;
wire[7:0] ASID;
wire TLB0_Match,TLB1_Match,TLB2_Match,TLB3_Match,TLB4_Match,TLB5_Match,TLB6_Match,TLB7_Match;
wire[7:0] TLB_Match;
wire TLB_Update;
wire CP0_Update0,CP0_Update1,CP0_Update2,CP0_Update3,CP0_Update4,CP0_Update5,CP0_Update6,CP0_Update7;
wire TLB_InValid;
wire TLB0_Modified,TLB1_Modified,TLB2_Modified,TLB3_Modified,TLB4_Modified,TLB5_Modified,TLB6_Modified,TLB7_Modified;
wire TLB_Checked_OK;
wire TLB0_Checked_OK,TLB1_Checked_OK,TLB2_Checked_OK,TLB3_Checked_OK,TLB4_Checked_OK,TLB5_Checked_OK,TLB6_Checked_OK,TLB7_Checked_OK;
wire[93:0] TLB0_Page,TLB1_Page,TLB2_Page,TLB3_Page,TLB4_Page,TLB5_Page,TLB6_Page,TLB7_Page;
wire[19:0] PFN0,PFN1,PFN2,PFN3,PFN4,PFN5,PFN6,PFN7;
wire TLB0_Valid,TLB1_Valid,TLB2_Valid,TLB3_Valid,TLB4_Valid,TLB5_Valid,TLB6_Valid,TLB7_Valid;
wire[18:0] TLB_VPN2;
wire[7:0] TLB_ASID;
wire[15:0] TLB_PageMask;
wire TLB_G;
wire[19:0] TLB_PFN0;
wire[2:0] TLB_C0;
wire TLB_D0;
wire TLB_V0;
wire[19:0] TLB_PFN1;
wire[2:0] TLB_C1;
wire TLB_D1;
wire TLB_V1;

reg[93:0] TLB_Page;
reg[19:0] PFN;
reg[31:0] MMU_CP0_BadVAddr;
reg TLB_Miss;
reg TLB_Modified;
reg TLB_Valid;
reg TLB_Err;
reg Pr_Finish;
reg MMU_CP0_Matched;
reg[3:0] TLB_Matched_ID;
reg[2:0] MMU_CP0_Index;
reg CP0_FinishR;
reg CP0_FinishP;
reg[31:0] VAddrReg;


reg  Pr_Req_t;


assign TLB_Unmapped = (CP0_MMU_Mode & Pr_VAddr[31:30] == 2'b10);//here changed!!!!
//assign TLB_Err = !CP0_MMU_Mode & Pr_VAddr[31] & Pr_Req;
/*assign TLB_Checked_OK 
			= TLB0_Checked_OK | TLB1_Checked_OK | TLB2_Checked_OK | TLB3_Checked_OK | 
			TLB4_Checked_OK | TLB5_Checked_OK | TLB6_Checked_OK | TLB7_Checked_OK 
			| TLB_Unmapped; */
assign TLB_Match ={TLB7_Match,TLB6_Match,TLB5_Match,TLB4_Match,TLB3_Match,TLB2_Match,TLB1_Match,TLB0_Match};
//assign TLB_Miss = !(|TLB_Match);
assign TLB_InValid = !TLB_Valid;//!(TLB_Checked_OK | TLB_Valid)
//each error has its level,just like the interrupt of the computer
//high level can mask the low level
assign TLB_Error = TLB_Err;
assign TLB_Fault = TLB_Unmapped ? 3'b0 : 
						TLB_Err ? 3'b0 : 
						{TLB_Miss,TLB_InValid & !TLB_Miss,TLB_Modified & !TLB_InValid & !TLB_Miss};
assign VPN = CP0_RdReq ? CP0_EntryHi[31:12] : Pr_VAddr[31:12];
assign ASID = CP0_RdReq ? CP0_EntryHi[7:0] : CP0_ASID;
assign MMU_Pr_Ack = Pr_Finish;

TLB TLB0(clk,Reset,RW_En,VPN,ASID,CP0_Update0,CP0_EntryHi,CP0_PageMask,CP0_EntryLo0,CP0_EntryLo1,
			TLB0_Modified,TLB0_Valid,TLB0_Match,PFN0,TLB0_Page);
TLB TLB1(clk,Reset,RW_En,VPN,ASID,CP0_Update1,CP0_EntryHi,CP0_PageMask,CP0_EntryLo0,CP0_EntryLo1,
			TLB1_Modified,TLB1_Valid,TLB1_Match,PFN1,TLB1_Page);
TLB TLB2(clk,Reset,RW_En,VPN,ASID,CP0_Update2,CP0_EntryHi,CP0_PageMask,CP0_EntryLo0,CP0_EntryLo1,
			TLB2_Modified,TLB2_Valid,TLB2_Match,PFN2,TLB2_Page);
TLB TLB3(clk,Reset,RW_En,VPN,ASID,CP0_Update3,CP0_EntryHi,CP0_PageMask,CP0_EntryLo0,CP0_EntryLo1,
			TLB3_Modified,TLB3_Valid,TLB3_Match,PFN3,TLB3_Page);
TLB TLB4(clk,Reset,RW_En,VPN,ASID,CP0_Update4,CP0_EntryHi,CP0_PageMask,CP0_EntryLo0,CP0_EntryLo1,
			TLB4_Modified,TLB4_Valid,TLB4_Match,PFN4,TLB4_Page);
TLB TLB5(clk,Reset,RW_En,VPN,ASID,CP0_Update5,CP0_EntryHi,CP0_PageMask,CP0_EntryLo0,CP0_EntryLo1,
			TLB5_Modified,TLB5_Valid,TLB5_Match,PFN5,TLB5_Page);
TLB TLB6(clk,Reset,RW_En,VPN,ASID,CP0_Update6,CP0_EntryHi,CP0_PageMask,CP0_EntryLo0,CP0_EntryLo1,
			TLB6_Modified,TLB6_Valid,TLB6_Match,PFN6,TLB6_Page);
TLB TLB7(clk,Reset,RW_En,VPN,ASID,CP0_Update7,CP0_EntryHi,CP0_PageMask,CP0_EntryLo0,CP0_EntryLo1,
			TLB7_Modified,TLB7_Valid,TLB7_Match,PFN7,TLB7_Page);

always @(TLB_Match)
begin
	begin
		case(TLB_Match)
			8'b0000_0000: TLB_Matched_ID <= 4'b1000;
			8'b0000_0001: TLB_Matched_ID <= 4'b0000;
			8'b0000_0010: TLB_Matched_ID <= 4'b0001;
			8'b0000_0100: TLB_Matched_ID <= 4'b0010;
			8'b0000_1000: TLB_Matched_ID <= 4'b0011;
			8'b0001_0000: TLB_Matched_ID <= 4'b0100;
			8'b0010_0000: TLB_Matched_ID <= 4'b0101;
			8'b0100_0000: TLB_Matched_ID <= 4'b0110;
			8'b1000_0000: TLB_Matched_ID <= 4'b0111;
			default: TLB_Matched_ID <= 4'b1111;
		endcase
	end
end

always @(posedge clk or posedge Reset)
begin
	if(Reset)
	begin
		PFN <= 20'b0;
		Pr_Finish <= 0;
	end
	else if(!TLB_Matched_ID[3] & Pr_Req & !CP0_RdReq)
	begin
		case(TLB_Matched_ID[2:0])
			3'b000: PFN <= PFN0;
			3'b001: PFN <= PFN1;
			3'b010: PFN <= PFN2;
			3'b011: PFN <= PFN3;
			3'b100: PFN <= PFN4;
			3'b101: PFN <= PFN5;
			3'b110: PFN <= PFN6;
			3'b111: PFN <= PFN7;
			default: PFN <= 20'b0;
		endcase
		Pr_Finish <= 1;
	end
	else if(TLB_Matched_ID[3] & Pr_Req & !CP0_RdReq)
	begin
		PFN <= 20'b0;
		Pr_Finish <= 1;
	end
	else
		Pr_Finish <= 0;
end


always @(posedge clk or posedge Reset)
if (Reset) Pr_Req_t<=0;
else if (Pr_Req) Pr_Req_t<=1;
else Pr_Req_t<=0;

always @(posedge clk or posedge Reset)//error handle
begin
	if(Reset)
	begin
		TLB_Valid <= 1'b0;
		TLB_Miss <= 1'b0;
		TLB_Modified <= 1'b0;
		TLB_Err <= 1'b0;
	end
	else if(Pr_Req | Pr_Req_t)
	begin
		case(TLB_Matched_ID[2:0])
			3'b000:begin TLB_Valid <= TLB0_Valid; TLB_Modified <= TLB0_Modified; end
			3'b001:begin TLB_Valid <= TLB1_Valid; TLB_Modified <= TLB1_Modified; end
			3'b010:begin TLB_Valid <= TLB2_Valid; TLB_Modified <= TLB2_Modified; end
			3'b011:begin TLB_Valid <= TLB3_Valid; TLB_Modified <= TLB3_Modified; end
			3'b100:begin TLB_Valid <= TLB4_Valid; TLB_Modified <= TLB4_Modified; end
			3'b101:begin TLB_Valid <= TLB5_Valid; TLB_Modified <= TLB5_Modified; end
			3'b110:begin TLB_Valid <= TLB6_Valid; TLB_Modified <= TLB6_Modified; end
			3'b111:begin TLB_Valid <= TLB7_Valid; TLB_Modified <= TLB7_Modified; end
			default:begin TLB_Valid <= 1'b0; TLB_Modified <= 1'b1; end
		endcase
		if(TLB_Matched_ID[3] & !TLB_Unmapped)
			TLB_Miss <= 1'b1;
		else
			TLB_Miss <= 1'b0;
		if(!CP0_MMU_Mode & Pr_VAddr[31])
			TLB_Err <= 1'b1;
		else
			TLB_Err <= 1'b0;
	end
	else
	begin
		TLB_Valid <= 1'b1;
		TLB_Miss <= 1'b0;
		TLB_Modified <= 1'b0;
		TLB_Err <= 1'b0;
	end
end  

assign MMU_Pr_RAddr = ((|TLB_Fault) | TLB_Err) ? 32'b0 	 :
					      (TLB_Unmapped)	   ? VAddrReg: 
												 {PFN,VAddrReg[11:0]};//output real address
//TLB Update
assign TLB_Update =  CP0_WrReq;
assign CP0_Update0 = TLB_Update & (CP0_MMU_Index == 3'b000);
assign CP0_Update1 = TLB_Update & (CP0_MMU_Index == 3'b001);
assign CP0_Update2 = TLB_Update & (CP0_MMU_Index == 3'b010);
assign CP0_Update3 = TLB_Update & (CP0_MMU_Index == 3'b011);
assign CP0_Update4 = TLB_Update & (CP0_MMU_Index == 3'b100);
assign CP0_Update5 = TLB_Update & (CP0_MMU_Index == 3'b101);
assign CP0_Update6 = TLB_Update & (CP0_MMU_Index == 3'b110);
assign CP0_Update7 = TLB_Update & (CP0_MMU_Index == 3'b111);
//when an error occured, MMU will export the bad virtual address
//to CP0's BadVAddr Register
assign MMU_CP0_Req = (|TLB_Fault) | TLB_Err;
//TLBR
always @(posedge clk or posedge Reset)
begin
	if(Reset)
	begin
		TLB_Page <= 94'b0;
		CP0_FinishR <= 1'b0;
	end
	else if((CP0_MMU_Func == `MMU_TLBR) & CP0_RdReq)
	begin
		case(CP0_MMU_Index)
			3'b000: TLB_Page <= TLB0_Page;
			3'b001: TLB_Page <= TLB1_Page;
			3'b010: TLB_Page <= TLB2_Page;
			3'b011: TLB_Page <= TLB3_Page;
			3'b100: TLB_Page <= TLB4_Page;
			3'b101: TLB_Page <= TLB5_Page;
			3'b110: TLB_Page <= TLB6_Page;
			3'b111: TLB_Page <= TLB7_Page;
			default: TLB_Page <= 94'b0;
		endcase
		CP0_FinishR <= 1'b1;
	end
	else
	begin
		TLB_Page <= 94'b0;
		CP0_FinishR <= 1'b0;
	end
end
assign TLB_VPN2 = TLB_Page[93:75];
assign TLB_ASID = TLB_Page[74:67];
assign TLB_PageMask = TLB_Page[66:51];
assign TLB_G = TLB_Page[50];
assign TLB_PFN0 = TLB_Page[49:30];
assign TLB_C0 = TLB_Page[29:27];
assign TLB_D0 = TLB_Page[26];
assign TLB_V0 = TLB_Page[25];
assign TLB_PFN1 = TLB_Page[24:5];
assign TLB_C1 = TLB_Page[4:2];
assign TLB_D1 = TLB_Page[1];
assign TLB_V1 = TLB_Page[0];

assign  MMU_CP0_EntryHi = {TLB_VPN2,5'b0,TLB_ASID};
assign  MMU_CP0_EntryLo0 = {6'b0,TLB_PFN0,TLB_C0,TLB_D0,TLB_V0,TLB_G};
assign  MMU_CP0_EntryLo1 = {6'b0,TLB_PFN1,TLB_C1,TLB_D1,TLB_V1,TLB_G};
assign  MMU_CP0_PageMask = {3'b0,TLB_PageMask,13'b0};
//TLBP
//assign MMU_CP0_Index = (!TLB_Matched_ID[3]) ? TLB_Matched_ID[2:0] : 3'b0;
always @(posedge clk or posedge Reset)
begin
	if(Reset)
		begin
			MMU_CP0_Matched <= 0;
			CP0_FinishP <= 0;
		end
	else if(!TLB_Matched_ID[3] & (CP0_MMU_Func == `MMU_TLBP) & CP0_RdReq)
		begin
			MMU_CP0_Matched <= 1;
			CP0_FinishP <= 1;
			MMU_CP0_Index <= TLB_Matched_ID[2:0];
		end
	else if((CP0_MMU_Func == `MMU_TLBP) & CP0_RdReq)
		begin
			MMU_CP0_Matched <= 0;
			CP0_FinishP <= 1;
			MMU_CP0_Index <= 3'b0;
		end
	else
		begin
			MMU_CP0_Matched <= 0;
			CP0_FinishP <= 0;
			MMU_CP0_Index <= 3'b0;
		end
end
assign MMU_CP0_AckP =  CP0_FinishP;
assign MMU_CP0_AckR =  CP0_FinishR;

always @(posedge clk or posedge Reset)
begin
	if(Reset)
	begin
		MMU_CP0_BadVAddr <= 32'b0;
		VAddrReg		 <= 32'b0;
	end
	else
	begin
		MMU_CP0_BadVAddr <= Pr_VAddr;
		VAddrReg		 <= Pr_VAddr;
	end
end

endmodule
