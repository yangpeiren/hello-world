module reg8(old,in,en,reset,out);
input [7:0]in,old;
input en;
input reset;
output [7:0]out;
reg [7:0]out;
reg count;
 always@(posedge en or posedge reset)
	begin
		if(reset)
			count=0;
		else if(en)
			begin
				if(in==old)
				out=in;
				
			end
		
	
	end
endmodule
