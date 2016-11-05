module core(cs, wr, rd, inta, a0, reset,
			isrset, code, datain, busdatain, 
			ltim, freeze, sm, rd_isr, rd_imr, wr_imr, clr_imr, rd_irr,
			eoi, setzero, sp, dataout, busdataout, 
			write1, write2,clk, er, mclr, codeocw2, o1, en);
			
	input cs, wr, rd, inta, a0, reset;
	input[2:0] code;
	input[7:0] isrset, datain, busdatain;
	
	output ltim, freeze, sm, rd_isr, rd_imr, wr_imr, clr_imr, rd_irr, mclr, o1,write1,write2,clk,en;
	output[2:0] sp;
	output[7:0] eoi, setzero, dataout, busdataout, er;
	output[10:0] codeocw2;
	
	wire read1, read2, freeze, ltim, mclr, clr_imr, o1, o2, o3, wr_imr, rd_imr, rd_irr, rd_isr, sm,en;
	wire[2:0] sp;
	wire[7:0] busdataout, dataout, setzero, eoi, icw1, icw2, icw3, icw4, ocw1, ocw2, ocw3;
	wire[10:0] erocw2, codeocw2;
	
	reg flag1, flag2;
	reg[1:0] edge1;
	reg[2:0] pri, state;
	reg[7:0] er;
	reg write1,write2,clk;

	// 写ICW1, OCW2, OCW3
//	assign write1 = (~cs && ~wr && rd && ~a0) ? 1 : 0;
	// 写ICW2~ICW4, OCW1
//	assign write2 = (~cs && ~wr && rd && a0) ? 1 : 0;
	// 读IRR, ISR, 查询字
	always
    begin
    if(cs==0 && wr==0 && rd==1 && a0==0)
	   write1=1;
     else if(cs==0 && wr==0 && rd==1 && a0==1)
	   write2=1;
     else begin write1=0; write2=0;end
     end

	assign read1 = (~cs && wr && ~rd && ~a0) ? 1 : 0;
	// 读IMR
	assign read2 = (~cs && wr && ~rd && a0) ? 1 : 0;
	
	assign freeze = (~flag1 && ~flag2) ? 1 : 0;
	// ltim 中断触发方式，1为电平触发，0为边沿触发
	assign ltim = icw1[3];		
	// 清除IMR中断屏蔽寄存器					
	assign mclr = (state == 1) ? 1 : 0;
	// 输出中断屏蔽操作命令字
	assign clr_imr = mclr;							
	assign busdataout = o1 ? ocw1 : 8'b00000000;	
	// 读IMR或IRR或ISR寄存器或中断类型号
	assign dataout = (read2 || (read1 && ocw3[1] == 1)) ? busdatain : ((flag2==0 && flag1==1) ? {icw2[7:3],code[2:0]} : 8'b00000000); 
	
	assign en = read2 || (read1 && ocw3[1]==1) || (flag2==0 && flag1==1);
	// OCW1中断屏蔽操作字
	assign o1 = state == 5 && write2;
	// OCW2优先级循环方式和中断结束方式操作命令字
	assign o2 = (state == 5 && write1 && datain[4:3] == 2'b00) ? 1 : 0;
	// OCW2特殊屏蔽方式和中断查询方式操作命令字
	assign o3 = (state == 5 && write1 && datain[4:3] == 2'b01) ? 1 : 0;
	// 写中断屏蔽寄存器
	assign wr_imr = o1 ? 1 : 0;
	// 读中断屏蔽寄存器
	assign rd_imr = read2 ? 32'b1 : 32'b0;
	// 读中断屏蔽寄存器
	assign rd_irr = (read1 && ocw3[1:0] == 2'b10) ? 1 : 0;
	// 读中断请求寄存器
	assign rd_isr = (read1 && ocw3[1:0] == 2'b11) ? 1 : 0;
	// 特殊屏蔽方式复位
	assign sm = (ocw3[6:5] == 2'b10) ? 1 : 0;
	// 清除中断服务寄存器
	assign setzero = mclr ? 8'b11111111 : isrset;
	// 设置中断结束位
	assign eoi = er;
	// 设置中断优先级
	assign sp = pri;
	// 写ICW1~ICW4或者OCW1~OCW2
	//assign clk = write1|write2;
	always
     begin
     clk=write1|write2;
     end
	assign erocw2 = {er, ocw2[2:0]};
	
	//优先级设置
	always @(erocw2)
	begin
		if(reset || mclr)	
			pri <= 7;
		else if(icw4[1])			// 自动EOI方式
		begin
			if(ocw2[7])				// 循环方式
			begin
				case(er)
					8'h01 : pri <= 0;		
					8'h02 : pri <= 1;		
					8'h04 : pri <= 2;		
					8'h08 : pri <= 3;		
					8'h10 : pri <= 4;		
					8'h20 : pri <= 5;		
					8'h40 : pri <= 6;		
					8'h80 : pri <= 7;		
				endcase
			end
		end
		else if(o2 && ocw2[7])		// 非自动EOI循环方式
		begin
			if(ocw2[6])				// 指定专门的优先级
			begin
				case(ocw2[2:0])		// IR0, 以下类推
					0 : pri <= 0;		
					1 : pri <= 1;		
					2 : pri <= 2;		
					3 : pri <= 3;		
					4 : pri <= 4;	
					5 : pri <= 5;		
					6 : pri <= 6;		
					7 : pri <= 7;		
				endcase
			end
			else					// 无专门指定的优先级
			begin
				case(er)
					8'h01 : pri <= 0;		
					8'h02 : pri <= 1;		
					8'h04 : pri <= 2;	
					8'h08 : pri <= 3;	
					8'h10 : pri <= 4;	
					8'h20 : pri <= 5;		
					8'h40 :	pri <= 6;	
					8'h80 : pri <= 7;	
				endcase
			end
		end
		else 
			pri <= pri;	
	end
	
	//处理中断结束
	assign codeocw2 = {code, ocw2[2:0]};
	
	always @(codeocw2)
	begin
		if(reset || mclr)	
			er <= 8'hff;
		else if(icw4[1])				// 自动结束方式
		begin
			if(flag2)
			begin
				case(code)
					0 : er <= 8'h01;	// 0号寄存器，以下类推	
					1 : er <= 8'h02;		
					2 : er <= 8'h04;		
					3 : er <= 8'h08;	
					4 : er <= 8'h10;
					5 : er <= 8'h20;		
					6 : er <= 8'h40;	
					7 : er <= 8'h80;	
					default : er <= 8'h00;	
				endcase
			end
			else	
				er <= 8'h00;
		end
		else if(o2)
		begin
			if(ocw2[5])					// 中断服务程序结束后需要发送中断结束命令
			begin
				if(ocw2[6])				// ocw2[6] = 1,sl位指明L2,L1,L0是否有效
				begin
					case(ocw2[2:0])		// 0号寄存器，以下类推
						0 : er <= 8'h01;		
						1 : er <= 8'h02;		
						2 : er <= 8'h04;		
						3 : er <= 8'h08;	
						4 : er <= 8'h10;		
						5 : er <= 8'h20;		
						6 : er <= 8'h40;	
						7 : er <= 8'h80;		
						default : er <= 8'h00;	
					endcase
				end
				else						// ocw2[6] = 0, L2,L1,L0无效。
				begin
					case(code)				// 按code提供的寄存器号来中断
						0 : er <= 8'h01;
						1 : er <= 8'h02;		
						2 : er <= 8'h04;		
						3 : er <= 8'h08;		
						4 : er <= 8'h10;		
						5 : er <= 8'h20;		
						6 : er <= 8'h40;		
						7 : er <= 8'h80;		
						default : er <= 8'h00;		
					endcase
				end
			end
			else 
				er <= 8'h00;
		end
		else er <= 8'h00;
	end

	//监测inta信号上升沿
	always @(posedge inta or posedge reset)
	begin
		if(reset)	
			flag1 <= 0;
		else		
			flag1 <= ~flag1;
	end
	//监测inta信号下降沿
	always @(negedge inta or posedge reset)
	begin
		if(reset) 	
			flag2 <= 0;
		else 		
			flag2 <= ~flag2;
	end
	//监测write2信号下降沿
	always @(negedge write2 or posedge mclr)
	begin
		if(mclr)	
			edge1 = 2'b00;
	else		
		edge1 = edge1 + 1;
	end	
	//设定ICW、OCW
	assign icw1 = reset ? 8'h00 : (state == 1 && write1 ? datain : icw1);
	assign icw2 = reset ? 8'h00 : (state == 2 && write2 ? datain : icw2);
	assign icw3 = reset ? 8'h00 : (state == 3 && write2 ? datain : icw3);
	assign icw4 = reset ? 8'h00 : (state == 4 && write2 ? datain : icw4);
	assign ocw1 = reset ? 8'h00 : (state == 5 && write2 ? datain : ocw1); 
	assign ocw2 = reset ? 8'h00 : (state == 5 && write1 && datain[4:3] == 2'b00 ? datain : ocw2);
	assign ocw3 = reset ? 8'h00 : (state == 5 && write1 && datain[4:3] == 2'b01 ? datain : ocw3);
	
	//初始化自动机
	always @(posedge clk or posedge reset)
	begin
		if(reset)	
			state <= 1;
		else
		begin
			case(state)
				1 : 						// 初始化ICW1过程，设置芯片控制初始化命令字
					if(write2)	
						state <= 2;			// 进入初始化ICW2过程
				2 : 						// 初始化ICW2过程，设置中断类型码基值
				begin		
					if(~icw1[1])			// 级联使用
					begin
						if(edge1 == 2'b01)	
							state <= 3;		// 进入初始化ICW3过程
					end
					else if(icw1[0])		// 需要设置ICW4
					begin
						if(edge1 == 2'b01)	
						state <= 4;			// 进入初始化ICW4过程
					end						// 不需要设置ICW4
					else if(edge1 == 2'b01)	
						state <= 5;			// 设置命令操作字
		 		end
				3 : 						// 初始化ICW3过程，设置主从片初始化命令字
				begin
					if(icw1[0])				// 需要设置ICW4
					begin
						if(edge1 == 2'b10)	
							state <= 4;
					end
					else if(edge1 == 2'b10)	
						state <= 5;
			 	end
				4 :							// 初始化ICW4过程
					if(edge1 == 2'b10 || edge1 == 2'b11)	
						state <= 5;		
				5 : 						// 设置操作命令字
					if(write1 && datain[4])	
						state <= 1;		
			endcase
		end
	end
endmodule
