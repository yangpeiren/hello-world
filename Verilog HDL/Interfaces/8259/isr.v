module isr(rd,set,clr,data,busdata,en);
input [7:0]set;
input rd;
input[7:0]clr;
output[7:0]data;
output[7:0]busdata;
output en;

wire [7:0]isrreg;
assign data=isrreg;
assign busdata=isrreg;
assign en=rd;
assign isrreg[0]=clr[0]?0:(set[0]?1:isrreg[0]);

assign isrreg[1]=clr[1]?0:(set[1]?1:isrreg[1]);
assign isrreg[2]=clr[2]?0:(set[2]?1:isrreg[2]);
assign isrreg[3]=clr[3]?0:(set[3]?1:isrreg[3]);
assign isrreg[4]=clr[4]?0:(set[4]?1:isrreg[4]);
assign isrreg[5]=clr[5]?0:(set[5]?1:isrreg[5]);
assign isrreg[6]=clr[6]?0:(set[6]?1:isrreg[6]);
assign isrreg[7]=clr[7]?0:(set[7]?1:isrreg[7]);
endmodule
