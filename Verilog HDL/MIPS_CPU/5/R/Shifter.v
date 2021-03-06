	module Shifter(SHT_DA,SHT_DB,SHT_Func,SHT_DC);
	input[31:0]  SHT_DA;
	input[4:0]   SHT_DB;
	input[1:0]   SHT_Func;
	output[31:0] SHT_DC;
	reg[31:0]    SHT_DC;
	
	always
	  begin
	    case(SHT_Func)
	      2'b00:SHT_DC=0;
	      2'b01:SHT_DC=(SHT_DA<<SHT_DB);
	      2'b10:SHT_DC=(SHT_DA>>SHT_DB);
	      2'b11:
	        begin
	          if(SHT_DA[31])SHT_DC=~((~SHT_DA)>>SHT_DB);
	        end
	    endcase
	  end
	endmodule
	