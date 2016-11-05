module imr(clr,writemask,datain,imrreg);
input clr;
input writemask;
input [7:0]datain;
output [7:0]imrreg;

assign imrreg=clr?8'b0:(writemask?datain:imrreg);

endmodule
