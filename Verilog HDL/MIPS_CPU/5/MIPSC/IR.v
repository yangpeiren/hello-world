	module IR(Clk,Reset,MemData,IRWrite,IR);
	input 	Clk,Reset,IRWrite;
	input[31:0]	MemData;
	output[31:0] IR;
	reg[31:0] IR;
	always@(posedge Clk)
		begin
			if(Reset)
				IR=8'h0000000;
			else if(IRWrite)
				IR=MemData;
		end
	endmodule