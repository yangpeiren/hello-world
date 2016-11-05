module seg(	oSEG,iDIG	);
	input	[1:0]	iDIG;
	output	[6:0]	oSEG;
	reg		[6:0]	oSEG;

always @(iDIG)
	begin
			case(iDIG)
				8'h1: oSEG = 7'b1111001;
				8'h2: oSEG = 7'b0100100;
				8'h3: oSEG = 7'b0110000;
				8'h0: oSEG = 7'b1000000;
				default:
					oSEG = 7'b1111111;
			endcase
	end
endmodule
