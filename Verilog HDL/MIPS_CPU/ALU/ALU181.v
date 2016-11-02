module ALU181(S,A,B,F,M,CN,CO);
input[3:0] S;
input[7:0] A,B;
output[7:0] F;
input M,CN;
output CO;
reg[7:0]F;
reg[8:0]A9,B9,F9;
reg CO;
always@(M or CN or A9 or B9)
begin
A9<={1'b0,A};
B9<={1'b0,B};
case(S)
	4'b0000:if(~M)F9<=A9+CN;else F9<=~A9;
	4'b0001:if(~M)F9<=(A9|B9)+CN;else F9<=~{A9|B9};
	4'b0010:if(~M)F9<=(A9|(~B9))+CN;else F9<=(~A9)&B9;
	4'b0011:if(~M)F9<=9'b000000000-CN;else F9<=9'b000000000;
	4'b0100:if(~M)F9<=A9+(A9&(~B9))+CN;else F9<=~(A9&B9);
	4'b0101:if(~M)F9<=(A9|B9)+(A9&(~B9))+CN;else F9<=~B9;
	4'b0110:if(~M)F9<=(A9-B9)-CN;else F9<=A9^B9;
	4'b0111:if(~M)F9<=(A9|(~B9))-CN;else F9<=A9&(~B9);
	4'b1000:if(~M)F9<=A9+(A9&B9)+CN;else F9<=(~A9)&B9;
	4'b1001:if(~M)F9<=A9+B9+CN;else F9<=~(A9^B9);
	4'b1010:if(~M)F9<=(A9|(~B9))+(A9&B9)+CN;else F9<=B9;
	4'b1011:if(~M)F9<=(A9&B9)-CN;else F9<=A9&B9;
	4'b1100:if(~M)F9<=(A9+A9)+CN;else F9<=9'b000000001;
	4'b1101:if(~M)F9<=(A9|B9)+A9+CN;else F9<=A9|(~B9);
	4'b1110:if(~M)F9<=((A9|(~B9))+A9)+CN;else F9<=A9|B9;
	4'b1111:if(~M)F9<=A9-CN;else F9<=A9;
	default:F9<=9'b000000000;
	endcase
	F<=F9[7:0];
	CO<=F9[8];
	end
endmodule
	