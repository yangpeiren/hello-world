	module Mem(Clk,BE,CS,RW,Addr,DataIn,Reset,DataOut,DataReady);
	input        Clk,CS,RW,Reset;
	input[3:0]   BE;
	input[31:2]  Addr;
	input[31:0]  DataIn;
	output[31:0] DataOut;
	output       DataReady;
	
	(*ram_init_file = "Mem.mif"*)
	reg[31:0]    memory[31:0];
	reg[31:0]    DataOut;
	
	assign DataReady=1;
	
	always@(posedge Clk or posedge Reset)
	  if(Reset)begin
	    memory[0]<=32'b00000000000000000000000000000010;//data1
	    memory[1]<=32'b00000000000000000000000000001001;//data2
	    memory[2]<=32'b00111100000010001011111111000000;//lui $t0,16'hBFC0;  set duan di zhi
	    memory[3]<=32'b10001101000100000000000000000000;//lw $s0,0; load data1 in s0
	    memory[4]<=32'b10001101000100010000000000000100;//lw $s1,4;load data2 in s1
	    memory[5]<=32'b00000010000100010100100000100011;//subu $t1,$s0,$s1; compare
	    memory[6]<=32'b00011101001000000000000000000010;//bgtz St1,2; jump while t1>0
	    memory[7]<=32'b00000010001000001000000000100001;//addu $s0,S1,$zero; else move s1 to s0
	    memory[8]<=32'b10101100000100000000000000000000;//sw $s0,0 ;  the final answer is in data1
	  end
	  else if(Clk)
	  begin
	    if(RW)begin
	      memory[Addr[6:2]][31:16]<=(BE[3:2]==2'b11)?DataIn[31:16]:16'b0;
	      memory[Addr[6:2]][15:8] <=(BE[1]==1'b1)?DataIn[15:8]:8'b0;
	      memory[Addr[6:2]][7:0] <=DataIn[7:0];
	    end
	    else begin
	      DataOut[31:16]<=(BE[3:2]==2'b11)?memory[Addr[6:2]][31:16]:16'b0;
	      DataOut[15:8] <=(BE[1]==1'b1)?memory[Addr[6:2]][15:8]:8'b0;
	      DataOut[7:0]  <=memory[Addr[6:2]][7:0];
	    end
	  end
	endmodule