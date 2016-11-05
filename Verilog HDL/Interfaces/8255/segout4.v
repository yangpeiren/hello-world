module segout4(iDIG,oSEG);
input [3:0]iDIG;
output [6:0]oSEG;
reg [6:0]oSEG;
always 
begin
	case(iDIG)
	4'b0000:oSEG=7'b0111111;//0
	4'b0001:oSEG=7'b0000110;//1-2,3up
	4'b0010:oSEG=7'b1011011;//2-1,2,7,5,6
	4'b0011:oSEG=7'b1001111;//3-5,6 down
	4'b0100:oSEG=7'b1100110;//4-7,6,2,3up
	4'b0101:oSEG=7'b1101101;//5-7,6,3,4,1
	4'b0110:oSEG=7'b1111101;//6-2down
	4'b0111:oSEG=7'b0000111;//7-123up
	4'b1000:oSEG=7'b1111111;//8 all up
	4'b1001:oSEG=7'b1100111;//9-5,4 down
	4'b1010:oSEG=7'b1110111;//10,4down
	4'b1011:oSEG=7'b1111100;//11,b,1,2,down
	4'b1100:oSEG=7'b0111001;//12,C,2,3,7down
	4'b1010:oSEG=7'b1011110;//13-d.1.6down
	4'b1011:oSEG=7'b1111001;//14,E2,3,down
	4'b1010:oSEG=7'b1110001;//F,15,2,3,4down
	default:oSEG=0;	
	endcase
end
endmodule
