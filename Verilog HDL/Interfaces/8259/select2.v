module select2(en,in,out1,out2);
input en;
input [7:0]in;
output [7:0]out1,out2;
reg [7:0]out1,out2;
	always
		begin
			if(en)
			begin
			out2<=in;
			out1<=0;
			end
			else
			begin
			out1<=in;
			out2<=0;
			end
		end

endmodule
