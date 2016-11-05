module irr(ir0,ir1,ir2,ir3,ir4,ir5,ir6,ir7,Itim,freeze,rd,setzero,data,busdata,en);
input ir0,ir1,ir2,ir3,ir4,ir5,ir6,ir7;//
input Itim;
input freeze;
input rd;
input [7:0]setzero;
output [7:0]data;
output [7:0]busdata;
output en;
wire [7:0]irrreg,senselatch;
assign data=irrreg;
assign busdata=irrreg;
assign en=rd;
assign irrreg[0]=freeze?(senselatch[0]&ir0)|(Itim&ir0):irrreg[0];
assign senselatch[0]=setzero[0]?0:(ir0?1:senselatch[0]);

assign irrreg[1]=freeze?(senselatch[1]&ir1)|(Itim&ir1):irrreg[1];
assign senselatch[1]=setzero[1]?0:(ir1?1:senselatch[1]);

assign irrreg[2]=freeze?(senselatch[2]&ir2)|(Itim&ir2):irrreg[2];
assign senselatch[2]=setzero[2]?0:(ir2?1:senselatch[2]);

assign irrreg[3]=freeze?(senselatch[3]&ir3)|(Itim&ir3):irrreg[3];
assign senselatch[3]=setzero[3]?0:(ir1?1:senselatch[3]);

assign irrreg[4]=freeze?(senselatch[4]&ir4)|(Itim&ir4):irrreg[4];
assign senselatch[4]=setzero[4]?0:(ir4?1:senselatch[4]);

assign irrreg[5]=freeze?(senselatch[5]&ir5)|(Itim&ir5):irrreg[5];
assign senselatch[5]=setzero[5]?0:(ir5?1:senselatch[5]);

assign irrreg[6]=freeze?(senselatch[6]&ir6)|(Itim&ir6):irrreg[6];
assign senselatch[6]=setzero[6]?0:(ir6?1:senselatch[6]);

assign irrreg[7]=freeze?(senselatch[7]&ir7)|(Itim&ir7):irrreg[7];
assign senselatch[7]=setzero[7]?0:(ir7?1:senselatch[7]);

endmodule
