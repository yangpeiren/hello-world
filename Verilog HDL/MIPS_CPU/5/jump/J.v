module J(IR,PC,out,clk);
input[25:0] IR;
input clk;
input[31:28] PC;
output[31:0] out;
reg[31:0] out;
always @(posedge clk)
begin
out<={PC,IR,2'b00};
end
endmodule