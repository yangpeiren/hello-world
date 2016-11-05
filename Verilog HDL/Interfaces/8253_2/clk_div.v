module clk_div(clkin,clkout);
input clkin;
output clkout;
reg clkout;
reg [31:0]count;
always@(posedge clkin)
begin
		if(count==11250000)
			begin
			count<=0;
			clkout=~clkout;
			end
		else
			begin
			count=count+1;
			end

end


endmodule
