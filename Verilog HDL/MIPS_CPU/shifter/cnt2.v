module cnt2(clk,q);
input clk;
output[1:0] q;
reg[1:0] q;
initial q=0;
always@(posedge clk)
begin
q=q+1;
end
endmodule
