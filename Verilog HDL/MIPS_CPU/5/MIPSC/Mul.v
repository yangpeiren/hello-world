	module Mul(Clk,Reset,MUL_Start,MUL_SelHL,MUL_SelMD,MUL_Write,MUL_Flag,MUL_DB,MUL_DA,MUL_DC);
	input        Clk,Reset,MUL_Start,MUL_SelHL,MUL_SelMD,MUL_Write;
	output       MUL_Flag; 
	input[31:0]  MUL_DA,MUL_DB;
	output[31:0] MUL_DC;
	reg[31:0]    hi,lo;
	reg          working,finish,finish2;
	reg[63:0]    result;
	`define MUL_MUL      'b0
	`define MUL_DIV      'b1
	`define MUL_SEL_HIGH 'b1
	`define MUL_SEL_LOW  'b0
	
		function[63:0] result_mul;
			input[31:0] a,b;
				begin
					result_mul = a*b;
				end
		endfunction
		
		function[63:0] result_div;
			input[31:0] a,b;
				begin
					result_div[31:0]=a/b;
					result_div[63:32]=a%b;
				end
		endfunction
		
		assign MUL_Flag=finish;
		assign MUL_DC=finish?((MUL_SelHL==`MUL_SEL_HIGH)?hi:lo):32'b0;
		
		always@(posedge Clk)
		begin
			if(Reset)
			  begin working<=0;finish<=0;end
			else if(MUL_Start)
			  begin finish<=0;working<=1;end
			else if(finish2)
			  begin finish<=1;working<=0;end
		end		
		
		always@(posedge working)
		begin
	    if(working)
		  if(!finish)
		  begin
			if(MUL_SelMD==0) 
				result=result_mul(MUL_DA,MUL_DB);
			else 
				result=result_div(MUL_DA,MUL_DB);
			finish2=1;
		  end
		  else ;
	    else;
	    end
	
		always@(posedge Clk)
		begin
			if(Reset)
			begin
				hi<=0;
				lo<=0;
			end
			else if(MUL_Write)
			begin
				if(MUL_SelHL==1)
					hi<=MUL_DA;
				else
					lo<=MUL_DA;
			end
			else if(finish)
			begin
				hi=result[63:32];
				lo=result[31:0];
			end
		end
				
	endmodule	
	