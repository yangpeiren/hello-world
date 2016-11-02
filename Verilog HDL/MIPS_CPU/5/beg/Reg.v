module Reg(data,data_out,clk);
input clk;
input[31:0] data;
output[31:0] data_out;
reg[31:0] data_out;
always@(posedge clk)
begin
data_out=data;
end
endmodule