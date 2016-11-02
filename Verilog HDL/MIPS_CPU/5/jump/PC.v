	module PC(Clk,reset,PCSource,PC4,BrPC,JPC,EPC,PCWrite,PC_out);
	input Clk,reset,PCWrite;
	input[2:0] PCSource;
	input[31:0] PC4;
	input[31:0] BrPC,JPC;
	input[31:0] EPC;
	output[31:0] PC_out;
	reg[31:0] PC_out;
	//`define EXPCEPTION	32'h80000180
	//`define INT		32'h80000200
	always@(posedge Clk,posedge reset)
	begin
		if(reset)
			PC_out<=32'hBFC00000;
		else
			begin
			case(PCSource)
			3'b001:if(PCWrite) PC_out<=PC4;
			3'b010:if(PCWrite) PC_out<=BrPC;
			3'b011:if(PCWrite) PC_out<=JPC;
			3'b100:if(PCWrite) PC_out<=32'h80000180;
			3'b101:if(PCWrite) PC_out<=32'h80000200;
			3'b110:if(PCWrite) PC_out<=EPC;
			default:	;
			endcase
			end
	end
	endmodule