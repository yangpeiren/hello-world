module select4_1(reset,sel,in,out1,out2,out3,out4,mode);
input reset;
input sel;
input [7:0] in;
output [7:0] out1;
output [7:0] out2;
output [7:0] out3;
output [7:0] out4;
output [1:0]mode;
reg [7:0] out1;
reg [7:0] out2;
reg [7:0] out3;
reg [7:0] out4;
reg [1:0] mode;
	always@(negedge sel or posedge reset)
	begin
			if(reset)//singal reset ,set out1=in;
			begin
			mode=2'b00;
			
			end
			else
			begin
				mode=mode+1;//mode add 1
	
			end
		
	end//end of always
	always
	begin
		case(mode)
			2'b00:begin out1=in;
					end
			2'b01:begin out2=in;
					end
			2'b10:begin out3=in;
					end
			2'b11:begin out4=in;
					end
		endcase

	end

endmodule
