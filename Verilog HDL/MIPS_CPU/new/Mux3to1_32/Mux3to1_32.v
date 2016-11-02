	module Mux3to1_32(in0,in1,in2,sel,out);
	input[31:0]      in0,in1,in2;
	input[1:0]       sel;
	output reg[31:0] out;
	
	always@(in0,in1,in2,sel)
	  case(sel)
	    0:out<=in0;
	    1:out<=in1;
	    2:out<=in2;
	    default:out<=in0;
	  endcase
	 endmodule