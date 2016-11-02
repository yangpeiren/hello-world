module CP0(Clk,Reset,DataIn,RegIdx,CP0Write,DataOut,EPC,Cause,Exception);
input Clk,Reset,CP0Write,Exception;
input[6:2] Cause;
input[31:0] DataIn;
input[4:0] RegIdx;
output[31:0] DataOut;
output[31:0] EPC;

reg[31:0]regs[31:0];
`define SR 12
`define Cause 13
`define EPC 14
`define PRId 15
assign DataOut=regs[RegIdx];
assign EPC=regs[`EPC];
always @(posedge Clk or posedge Reset)
begin
if(Reset)
begin
regs[`Cause] <='h0000_0000;
regs[`PRId] <='h0000_0000;
regs[`EPC] <='h0000_0000;
end
else if(Clk)
if(CP0Write)
begin
if(!Exception)
regs[RegIdx] <=DataIn;
else
begin
regs[RegIdx] <=DataIn;
regs[`Cause][6:2] <=Cause;
end
end
end
endmodule