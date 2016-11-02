	module SymbolExpasion(Imm16,ExtImm32);
	input[15:0] Imm16;
	output[31:0] ExtImm32;
	reg[31:0] ExtImm32;
	always
		begin
			if(Imm16[15]==0)
				begin
					ExtImm32[31:16]<=16'b0;
					ExtImm32[15:0]<=Imm16[15:0];
				end
			else if(Imm16[15]==1)
				begin
					ExtImm32[31:16]<=16'b1111111111111111;
					ExtImm32[15:0]<=Imm16[15:0];
				end
		end
	endmodule