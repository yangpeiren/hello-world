	module Extent(Imm16,ExImm32);
	input[15:0] Imm16;
	output[31:0] ExImm32;
	reg[31:0] ExImm32;
	always
		begin
			if(Imm16[15]==0)
				begin
					ExImm32[31:16]<=16'b0;
					ExImm32[15:0]<=Imm16[15:0];
				end
			else if(Imm16[15]==1)
				begin
					ExImm32[31:16]<=16'b1111111111111111;
					ExImm32[15:0]<=Imm16[15:0];
				end
		end
	endmodule