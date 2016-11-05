module clkdiv(clkin,clkout);
input clkin;
output clkout;
reg clkout;
reg [31:0]clk_count;
always@(posedge clkin)
begin
	if(clk_count==11250000)
	begin
		clkout=!clkout;
		clk_count=0;
	end
	else
		begin
		clk_count=clk_count+1;
		
		end
end
endmodule