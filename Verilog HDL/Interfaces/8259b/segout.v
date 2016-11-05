module segout(	oSEG,iDIG	);
	input	[3:0]	iDIG;
	output	[6:0]	oSEG;
	reg		[6:0]	oSEG;

always @(iDIG)
	begin
			case(iDIG)
				8'h1: oSEG = 7'b1111001;	// ---t----
				8'h2: oSEG = 7'b0100100; 	// |	  |
				8'h3: oSEG = 7'b0110000; 	// lt	 rt
				8'h4: oSEG = 7'b0011001; 	// |	  |
				8'h5: oSEG = 7'b0010010; 	// ---m----
				8'h6: oSEG = 7'b0000010; 	// |	  |
				8'h7: oSEG = 7'b1111000; 	// lb	 rb
				8'h8: oSEG = 7'b0000000; 	// |	  |
				8'h9: oSEG = 7'b0011000; 	// ---b----
				8'ha: oSEG = 7'b0001000;
				8'hb: oSEG = 7'b0000011;
				8'hc: oSEG = 7'b1000110;
				8'hd: oSEG = 7'b0100001;
				8'he: oSEG = 7'b0000110;
				8'hf: oSEG = 7'b0001110;
				8'h0: oSEG = 7'b1000000;
				default:
					oSEG = 7'b1111111;
			endcase
	end
endmodule
