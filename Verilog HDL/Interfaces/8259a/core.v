module core(cs, wr, rd, inta, a0, reset,
			isrset, code, datain, busdatain, 
			ltim, freeze, sm, rd_isr, rd_imr, wr_imr, clr_imr, rd_irr,
			eoi, setzero, sp, dataout, busdataout, 
			write1, write2,clk, er, mclr, codeocw2, o1, en);
			
	input cs, wr, rd, inta, a0, reset;
	input[2:0] code;
	input[7:0] isrset, datain, busdatain;
	
	output ltim, freeze, sm, rd_isr, rd_imr, wr_imr, clr_imr, rd_irr, mclr, o1,write1,write2,clk,en;
	output[2:0] sp;
	output[7:0] eoi, setzero, dataout, busdataout, er;
	output[10:0] codeocw2;
	
	wire read1, read2, freeze, ltim, mclr, clr_imr, o1, o2, o3, wr_imr, rd_imr, rd_irr, rd_isr, sm,en;
	wire[2:0] sp;
	wire[7:0] busdataout, dataout, setzero, eoi, icw1, icw2, icw3, icw4, ocw1, ocw2, ocw3;
	wire[10:0] erocw2, codeocw2;
	
	reg flag1, flag2;
	reg[1:0] edge1;
	reg[2:0] pri, state;
	reg[7:0] er;
	reg write1,write2,clk;

	// дICW1, OCW2, OCW3
//	assign write1 = (~cs && ~wr && rd && ~a0) ? 1 : 0;
	// дICW2~ICW4, OCW1
//	assign write2 = (~cs && ~wr && rd && a0) ? 1 : 0;
	// ��IRR, ISR, ��ѯ��
	always
    begin
    if(cs==0 && wr==0 && rd==1 && a0==0)
	   write1=1;
     else if(cs==0 && wr==0 && rd==1 && a0==1)
	   write2=1;
     else begin write1=0; write2=0;end
     end

	assign read1 = (~cs && wr && ~rd && ~a0) ? 1 : 0;
	// ��IMR
	assign read2 = (~cs && wr && ~rd && a0) ? 1 : 0;
	
	assign freeze = (~flag1 && ~flag2) ? 1 : 0;
	// ltim �жϴ�����ʽ��1Ϊ��ƽ������0Ϊ���ش���
	assign ltim = icw1[3];		
	// ���IMR�ж����μĴ���					
	assign mclr = (state == 1) ? 1 : 0;
	// ����ж����β���������
	assign clr_imr = mclr;							
	assign busdataout = o1 ? ocw1 : 8'b00000000;	
	// ��IMR��IRR��ISR�Ĵ������ж����ͺ�
	assign dataout = (read2 || (read1 && ocw3[1] == 1)) ? busdatain : ((flag2==0 && flag1==1) ? {icw2[7:3],code[2:0]} : 8'b00000000); 
	
	assign en = read2 || (read1 && ocw3[1]==1) || (flag2==0 && flag1==1);
	// OCW1�ж����β�����
	assign o1 = state == 5 && write2;
	// OCW2���ȼ�ѭ����ʽ���жϽ�����ʽ����������
	assign o2 = (state == 5 && write1 && datain[4:3] == 2'b00) ? 1 : 0;
	// OCW2�������η�ʽ���жϲ�ѯ��ʽ����������
	assign o3 = (state == 5 && write1 && datain[4:3] == 2'b01) ? 1 : 0;
	// д�ж����μĴ���
	assign wr_imr = o1 ? 1 : 0;
	// ���ж����μĴ���
	assign rd_imr = read2 ? 32'b1 : 32'b0;
	// ���ж����μĴ���
	assign rd_irr = (read1 && ocw3[1:0] == 2'b10) ? 1 : 0;
	// ���ж�����Ĵ���
	assign rd_isr = (read1 && ocw3[1:0] == 2'b11) ? 1 : 0;
	// �������η�ʽ��λ
	assign sm = (ocw3[6:5] == 2'b10) ? 1 : 0;
	// ����жϷ���Ĵ���
	assign setzero = mclr ? 8'b11111111 : isrset;
	// �����жϽ���λ
	assign eoi = er;
	// �����ж����ȼ�
	assign sp = pri;
	// дICW1~ICW4����OCW1~OCW2
	//assign clk = write1|write2;
	always
     begin
     clk=write1|write2;
     end
	assign erocw2 = {er, ocw2[2:0]};
	
	//���ȼ�����
	always @(erocw2)
	begin
		if(reset || mclr)	
			pri <= 7;
		else if(icw4[1])			// �Զ�EOI��ʽ
		begin
			if(ocw2[7])				// ѭ����ʽ
			begin
				case(er)
					8'h01 : pri <= 0;		
					8'h02 : pri <= 1;		
					8'h04 : pri <= 2;		
					8'h08 : pri <= 3;		
					8'h10 : pri <= 4;		
					8'h20 : pri <= 5;		
					8'h40 : pri <= 6;		
					8'h80 : pri <= 7;		
				endcase
			end
		end
		else if(o2 && ocw2[7])		// ���Զ�EOIѭ����ʽ
		begin
			if(ocw2[6])				// ָ��ר�ŵ����ȼ�
			begin
				case(ocw2[2:0])		// IR0, ��������
					0 : pri <= 0;		
					1 : pri <= 1;		
					2 : pri <= 2;		
					3 : pri <= 3;		
					4 : pri <= 4;	
					5 : pri <= 5;		
					6 : pri <= 6;		
					7 : pri <= 7;		
				endcase
			end
			else					// ��ר��ָ�������ȼ�
			begin
				case(er)
					8'h01 : pri <= 0;		
					8'h02 : pri <= 1;		
					8'h04 : pri <= 2;	
					8'h08 : pri <= 3;	
					8'h10 : pri <= 4;	
					8'h20 : pri <= 5;		
					8'h40 :	pri <= 6;	
					8'h80 : pri <= 7;	
				endcase
			end
		end
		else 
			pri <= pri;	
	end
	
	//�����жϽ���
	assign codeocw2 = {code, ocw2[2:0]};
	
	always @(codeocw2)
	begin
		if(reset || mclr)	
			er <= 8'hff;
		else if(icw4[1])				// �Զ�������ʽ
		begin
			if(flag2)
			begin
				case(code)
					0 : er <= 8'h01;	// 0�żĴ�������������	
					1 : er <= 8'h02;		
					2 : er <= 8'h04;		
					3 : er <= 8'h08;	
					4 : er <= 8'h10;
					5 : er <= 8'h20;		
					6 : er <= 8'h40;	
					7 : er <= 8'h80;	
					default : er <= 8'h00;	
				endcase
			end
			else	
				er <= 8'h00;
		end
		else if(o2)
		begin
			if(ocw2[5])					// �жϷ�������������Ҫ�����жϽ�������
			begin
				if(ocw2[6])				// ocw2[6] = 1,slλָ��L2,L1,L0�Ƿ���Ч
				begin
					case(ocw2[2:0])		// 0�żĴ�������������
						0 : er <= 8'h01;		
						1 : er <= 8'h02;		
						2 : er <= 8'h04;		
						3 : er <= 8'h08;	
						4 : er <= 8'h10;		
						5 : er <= 8'h20;		
						6 : er <= 8'h40;	
						7 : er <= 8'h80;		
						default : er <= 8'h00;	
					endcase
				end
				else						// ocw2[6] = 0, L2,L1,L0��Ч��
				begin
					case(code)				// ��code�ṩ�ļĴ��������ж�
						0 : er <= 8'h01;
						1 : er <= 8'h02;		
						2 : er <= 8'h04;		
						3 : er <= 8'h08;		
						4 : er <= 8'h10;		
						5 : er <= 8'h20;		
						6 : er <= 8'h40;		
						7 : er <= 8'h80;		
						default : er <= 8'h00;		
					endcase
				end
			end
			else 
				er <= 8'h00;
		end
		else er <= 8'h00;
	end

	//���inta�ź�������
	always @(posedge inta or posedge reset)
	begin
		if(reset)	
			flag1 <= 0;
		else		
			flag1 <= ~flag1;
	end
	//���inta�ź��½���
	always @(negedge inta or posedge reset)
	begin
		if(reset) 	
			flag2 <= 0;
		else 		
			flag2 <= ~flag2;
	end
	//���write2�ź��½���
	always @(negedge write2 or posedge mclr)
	begin
		if(mclr)	
			edge1 = 2'b00;
	else		
		edge1 = edge1 + 1;
	end	
	//�趨ICW��OCW
	assign icw1 = reset ? 8'h00 : (state == 1 && write1 ? datain : icw1);
	assign icw2 = reset ? 8'h00 : (state == 2 && write2 ? datain : icw2);
	assign icw3 = reset ? 8'h00 : (state == 3 && write2 ? datain : icw3);
	assign icw4 = reset ? 8'h00 : (state == 4 && write2 ? datain : icw4);
	assign ocw1 = reset ? 8'h00 : (state == 5 && write2 ? datain : ocw1); 
	assign ocw2 = reset ? 8'h00 : (state == 5 && write1 && datain[4:3] == 2'b00 ? datain : ocw2);
	assign ocw3 = reset ? 8'h00 : (state == 5 && write1 && datain[4:3] == 2'b01 ? datain : ocw3);
	
	//��ʼ���Զ���
	always @(posedge clk or posedge reset)
	begin
		if(reset)	
			state <= 1;
		else
		begin
			case(state)
				1 : 						// ��ʼ��ICW1���̣�����оƬ���Ƴ�ʼ��������
					if(write2)	
						state <= 2;			// �����ʼ��ICW2����
				2 : 						// ��ʼ��ICW2���̣������ж��������ֵ
				begin		
					if(~icw1[1])			// ����ʹ��
					begin
						if(edge1 == 2'b01)	
							state <= 3;		// �����ʼ��ICW3����
					end
					else if(icw1[0])		// ��Ҫ����ICW4
					begin
						if(edge1 == 2'b01)	
						state <= 4;			// �����ʼ��ICW4����
					end						// ����Ҫ����ICW4
					else if(edge1 == 2'b01)	
						state <= 5;			// �������������
		 		end
				3 : 						// ��ʼ��ICW3���̣���������Ƭ��ʼ��������
				begin
					if(icw1[0])				// ��Ҫ����ICW4
					begin
						if(edge1 == 2'b10)	
							state <= 4;
					end
					else if(edge1 == 2'b10)	
						state <= 5;
			 	end
				4 :							// ��ʼ��ICW4����
					if(edge1 == 2'b10 || edge1 == 2'b11)	
						state <= 5;		
				5 : 						// ���ò���������
					if(write1 && datain[4])	
						state <= 1;		
			endcase
		end
	end
endmodule
