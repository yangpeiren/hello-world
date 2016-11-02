	module Regfile(Read1,Read2,WriteReg,WriteData,RegWrite,Data1,Data2,clock);
	input[4:0] Read1,Read2,WriteReg;
	input[31:0] WriteData;
	input RegWrite,clock;
	output[31:0] Data1,Data2;
	reg[31:0] RF[31:0];
	assign Data1=RF[Read1];
	assign Data2=RF[Read2];
	
	always@(posedge clock)
	begin 
		if(RegWrite)
		RF[WriteReg]<=WriteReg?WriteData:0;
		end
	endmodule