	module Mem(Clk,BE,CS,RW,Addr,DataIn,Reset,DataOut,DataReady);
	input        Clk,CS,RW,Reset;
	input[3:0]   BE;
	input[31:2]  Addr;
	input[31:0]  DataIn;
	output[31:0] DataOut;
	output       DataReady;
	
	(*ram_int_file = "Mem.mif"*)
	reg[31:0]    memory[31:0];
	reg[31:0]    DataOut;
	
	assign DataReady=1;
	
	always@(posedge Clk or posedge Reset)
	  if(Reset)begin
	    memory[0]<=1;
	    memory[1]<=2;
	    memory[2]<=3;
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
	