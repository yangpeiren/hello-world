	module ALU(ALU_DA,ALU_DB,ALU_Func,ALU_Zero,ALU_DC,ALU_OverFlow);
	input[31:0]  ALU_DA,ALU_DB;
	input[3:0]   ALU_Func;
	output       ALU_Zero,ALU_OverFlow;
	output[31:0] ALU_DC;
	wire         ALU_Zero;
	reg[31:0]    ALU_DC;
	wire         ALU_OverFlow;
	integer      ALU_SymbolA;
	integer      ALU_SymbolB;
	wire         sign;
	
	assign sign=ALU_DC[31];
	assign ALU_Zero=(ALU_DC==32'b0)?1'b1:1'b0;
	
	always@(ALU_DA,ALU_DB)
	begin
	  ALU_SymbolA=ALU_DA;
	  ALU_SymbolB=ALU_DB;
	end
	
	always
	  begin
	    case(ALU_Func)
	      4'b0000:ALU_DC=ALU_DB;
	      4'b0001:ALU_DC=ALU_DA+ALU_DB;
	      4'b0010:ALU_DC=ALU_DA+ALU_DB;
	      4'b0011:ALU_DC=ALU_DA-ALU_DB;
	      4'b0100:ALU_DC=ALU_DA-ALU_DB;
	      4'b0101:ALU_DC=ALU_DA&ALU_DB;
	      4'b0110:ALU_DC=(ALU_DA|ALU_DB);
	      4'b0111:ALU_DC=~(ALU_DA|ALU_DB);
	      4'b1000:ALU_DC=ALU_DA^ALU_DB;
	      4'b1001:ALU_DC=(ALU_DA<ALU_DB)?1:0;
	      4'b1010:ALU_DC=(ALU_SymbolA<ALU_SymbolB)?1:0;
	      4'b1011:ALU_DC=(ALU_SymbolA<=ALU_SymbolB)?1:0;
	      4'b1100:ALU_DC={ALU_DB[15:0],16'b0};
	      default:ALU_DC=ALU_DB;
	   endcase
	  end
	
	assign ALU_OverFlow=((ALU_Func==4'b0010)&&((ALU_DA[31]==0)&&(ALU_DB[31]==0)&&(sign==1)))||((ALU_Func==4'b0010)&&((ALU_DA[31]==1)&&(ALU_DB[31]==1)&&(sign==0)))||((ALU_Func==4'b0100)&&((ALU_DA[31]==1)&&(ALU_DB[31]==0)&&(sign==0)))||((ALU_Func==4'b0100)&&((ALU_DA[31]==0)&&(ALU_DB[31]==1)&&(sign==1)));
	endmodule