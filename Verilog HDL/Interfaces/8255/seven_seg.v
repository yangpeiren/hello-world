module seven_seg(clock,din,dout);
input [3:0] din;
inout clock;
output [6:0] dout;
reg [6:0] dout;
always @(din or reset)
 begin
    case(din)
        4'b0000: dout<=7'b1111110;//
        4'b0001: dout<=7'b0110000;
        4'b0010: dout<=7'b1101101;
        4'b0011: dout<=7'b1111001;
        4'b0100: dout<=7'b0110011;
        4'b0101: dout<=7'b1011011;
        4'b0110: dout<=7'b1011111;
        4'b0111: dout<=7'b1110000;
        4'b1000: dout<=7'b1111111;
        4'b1001: dout<=7'b1111011;
	
        default: dout<=7'b0000000;
endcase
end
endmodule

