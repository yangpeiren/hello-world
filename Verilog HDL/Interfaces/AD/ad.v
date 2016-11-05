module ad(A, IOR, IOW, AEN, D7IN, D7OUT, EOC, START, OE, ALE, IRQ2, EN);
	input IOR, IOW, AEN, EOC;				//控制信号
	input[9:0] A;							//地址总线
	input D7IN;
	
	output START, OE, ALE, IRQ2, D7OUT, EN;
	wire START, OE, ALE, IRQ2, EN;

	assign START = A == 10'b1100001100 ? ~IOW & AEN : 0;
	assign D7OUT = A == 10'b1100001100 ? ~IOR & AEN & EOC : 0;
	assign IRQ2 = A == 10'b1100001101 ? ~IOW & AEN & ~D7IN & EOC : 0;
	assign ALE = A == 10'b1100001110 ? ~IOW & AEN : 0;
	assign OE = A == 10'b1100001110 ? ~IOR & AEN : 0;
	assign EN = A == 10'b1100001100 ? 1 : 0;
endmodule
