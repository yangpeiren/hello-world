module I8253f(datain, dataout, gate0, gate1, gate2, reset, CS, RD, WR, A1, A0, clk0, clk1, clk2, clk_out, cnt0, en);
	input gate0, gate1, gate2;
	input reset, CS, RD, WR;
	input A1, A0;
	input clk0, clk1, clk2;
	input[7:0] datain;
	
	output[2:0] clk_out;
	output en;
	output[7:0] dataout;
	output[15:0]cnt0;


	reg[2:0] clk_out;							//���������
	reg[7:0] dataout;
	reg[7:0] cmd;								//����ָ��
	reg[5:0] cmd0, cmd1, cmd2;					//��������������ָ��
	reg[15:0] set0, set1, set2;					//װ�������ֵ�ļĴ���
	reg[15:0] lock;								//�����ü�����
	reg write0, write1, write2, read0, read1, read2;
	wire all_set0, all_gate0, all_set1, all_gate1, all_set2, all_gate2, reg1a;	//���е��������ſ��ź�
	reg[1:0]  wlh0, rlh0, wlh1, rlh1, wlh2, rlh2;								//��¼��������˳��
	wire edge0a, edge1a, edge2a, start0, start1, start2, reg0, reg2;			//��־�ź�
	reg reg0a, reg2a, edge0, edge1, edge2;
	wire dataoutall;												//��־�ź�
	wire[4:0] cbus;													//�����ź�
	reg[15:0] cnt0, cnt1, cnt2;										//������
	wire[15:0] buffer;												//������
	reg wreset1, rreset1, wover2, rover2, wreset2, rreset2, wover0, rover0, wreset0, rreset0, wover1, rover1, reg1;
	//write �źű�ʾ�Լ���������Ĵ�����д����   
	//read  �źű�ʾ�Լ���������Ĵ����Ķ�����
	//wover �źű�ʾд�������� 
	//rover �źű�ʾ����������
	//wreset �������Ĵ���
	//rreset �������Ĵ���

	assign cbus = {CS, RD, WR, A1, A0};
	// �Լ�����0������
	assign all_set0 = (write0 && (cmd0[3:1] == 3'b000)) || 								// ��ʽ0
					  (edge0 && (cmd0[3:1] == 3'b001)) || 								// ��ʽ1
					  (((buffer == 0) || ~gate0) && cmd0[3:2] == 3'b01) ||				// ��ʽ2��3
					  ((write0 || ~gate0) && cmd0[3:1] == 3'b100) || 					// ��ʽ4
					  (((start0 && ~write0) || edge0) && cmd0[3:1] == 3'b101) || reset;	// ��ʽ5
	assign all_gate0 = (gate0 && cnt0 != 16'h0000 && cmd0[3:1] == 3'b000) || 			// ��ʽ0
					   (cnt0 != 16'h0000 && cmd0[3:1] == 3'b001) || 					// ��ʽ1
					   (gate0 && cmd0[3:1] == 3'b010) || 								// ��ʽ2
					   (gate0 && cmd0[3:1] == 3'b011) || 								// ��ʽ3
					   (gate0 && cnt0 != 16'h0000 && cmd0[3:1] == 3'b100) || 			// ��ʽ4
					   (cnt0 != 16'h0000 && cmd0[3:1] == 3'b101);						// ��ʽ5
	// �Լ�����1������
	assign all_set1 = (write1 && (cmd1[3:1] == 3'b000)) || 								// ��ʽ0
					  (~edge1 && (cmd1[3:1] == 3'b001)) || 								// ��ʽ1
					  (((cnt1 == 16'h0000) || ~gate1) && cmd1[3:1] == 3'b010) || 		// ��ʽ2
					  (((cnt1 == 16'h0000) || ~gate1) && cmd1[3:1] == 3'b011) || 		// ��ʽ3
					  ((start1 || edge1) && cmd1[3:1] == 3'b100) || 					// ��ʽ4
					  ((start1 || edge1) && cmd1[3:1] == 3'b101) || reset;				// ��ʽ5
	assign all_gate1 = (gate1 && cnt1 != 16'h0000 && cmd1[3:1] == 3'b000) || 			// ��ʽ0
					   (cnt1 != 16'h0000 && cmd1[3:1] == 3'b001) || 					// ��ʽ1
					   (gate1 && cnt1 != 16'h0000 && cmd1[3:1] == 3'b010) || 			// ��ʽ2
					   (gate1 && cmd1[3:1] == 3'b011) || 								// ��ʽ3
					   (gate1 && cnt1 != 16'h0000 && cmd1[3:1] == 3'b100) || 			// ��ʽ4
					   (cnt1 != 16'h0000 && cmd1[3:1] == 3'b101);						// ��ʽ5
	// �Լ�����2������
	assign all_set2 = (write2 && (cmd2[3:1] == 3'b000)) ||								// ��ʽ0	 
	  				  (~edge2 && (cmd2[3:1] == 3'b001)) || 								// ��ʽ1
					  (((cnt2 == 16'h0000) || ~gate2) && cmd2[3:1] == 3'b010) || 		// ��ʽ2
					  (((cnt2 == 16'h0000) || ~gate2) && cmd2[3:1] == 3'b011) || 		// ��ʽ3
					  ((start2 || edge2) && cmd2[3:1] == 3'b100) || 					// ��ʽ4
					  ((start2 || edge2) && cmd2[3:1] == 3'b101) || reset;				// ��ʽ5
	assign all_gate2 = (gate2 && cnt2 != 16'h0000 && cmd2[3:1] == 3'b000) || 			// ��ʽ0
					  (cnt2 != 16'h0000 && cmd2[3:1] == 3'b001) || 						// ��ʽ1
					  (gate2 && cnt2 == 16'h0000 && cmd2[3:1] == 3'b010) || 			// ��ʽ2
					  (gate2 && cmd2[3:1] == 3'b011) || 								// ��ʽ3
					  (gate2 && cnt2 != 16'h0000 && cmd2[3:1] == 3'b100) || 			// ��ʽ4
					  (cnt2 != 16'h0000 && cmd2[3:1] == 3'b101);						// ��ʽ5
	// ����������
	assign en = (read0 || read1 || read2);
	
	assign start1 = (cnt1 == 16'h0000 || ~gate1) ? (reg1 ? 1 : write1) : 0;
	assign reg1a = start1 & ~write1;
	// �ſ��ź�Ϊ�ߣ��Ҽ����������õĳ�ʼֵ
	assign edge1a = (gate1 && cnt1 == set1);
	assign buffer = (cnt0 != buffer || write0) ? cnt0 : buffer;
	
	assign start0 = (buffer == 16'h0000 || ~gate0) ? (reg0 ? 1 : write0) : 0;
	assign reg0 =  (start0 && ~write0) ? 0 : reg0a;
	assign edge0a = (gate0 && cnt0 == set0);
	assign start2 = (cnt2 == 16'h0000 || ~gate2) ? (reg2 ? 1 : write2) : 0;
	assign reg2 =  (start2 && ~write2) ? 0 : reg2a;
	assign edge2a = (gate2 && cnt2 == set2);
	assign dataoutall = cmd0[5] == 1'b1 || cmd0[5:4] == 2'b01 || 
		   cmd1[5] == 1'b1 || cmd1[5:4] == 2'b01 || 
		   cmd2[5] == 1'b1 || cmd2[5:4] == 2'b01;

	// �Ƿ��������е��źŶ�Ӧ�������أ������ɻ󣡣���
	always @(cbus)
	begin
		case(cbus)
			5'b01000 : write0 <= 1; 			//����T/C0
        	5'b01001 : 
			begin
				write1 <= 1; 					//����T/C1
				write0 <= 0;		
			end											
       		5'b01010 : 
			begin	
				write2 <= 1; 					//����T/C2
				write0 <= 0;		
			end								
			5'b01011 : 
			begin	
				cmd <= datain;					//����ƼĴ���д��ʽ������
				write0 <= 0;	
			end								
        	5'b00100 : 
			begin	
				read0 <= 1; 					//��T/C0
				write0 <= 0;					
			end											
        	5'b00101 : 
			begin	
				read1 <= 1; 					//��T/C1
				write0 <= 0; 					
			end										
        	5'b00110 : 
			begin	
				read2 <= 1; 					//��T/C2
				write0 <= 0;					
			end										
        	default : 
			begin	
				write0 <= 0; 
				write1 <= 0; 
				write2 <= 0; 
				read0 <= 0; 
				read1 <= 0; 
				read2 <= 0; 	
			end
		endcase
	end

	always @(cmd)					// ���������
	begin
		case(cmd)
			8'b00000000 : 
				lock <= cnt0;		//����counter0����ʽ0��2���Ƽ���
			8'b01000000 : 
				lock <= cnt1;		//����counter1����ʽ0��2���Ƽ���
			8'b10000000 : 
				lock <= cnt2;		//����counter2����ʽ0��2���Ƽ���
			default : 
			begin
				if(cmd[7:6] == 2'b00 && cmd[5:4] != 2'b00)	
					cmd0 <= cmd[5:0];						//counter0����
				if(cmd[7:6] == 2'b01 && cmd[5:4] != 2'b00)	
					cmd1 <= cmd[5:0];						//counter1����
				if(cmd[7:6] == 2'b10 && cmd[5:4] != 2'b00)	
					cmd2 <= cmd[5:0];						//counter2����
			end
		endcase
	end
	
	// 1�ż�����
	always @(posedge write1 or posedge reg1a)				// ��reg�ź�
	begin
		if(reg1a)	
			reg1 <= 0;
		else if(cnt1 != 16'h0000 && cmd1[3:2] == 2'b10)		// ��ʽ4��5
			reg1 <= 1;
	end

	always @(posedge gate1 or posedge edge1a)				// ��edge�ź�												
	begin
		if(edge1a)		
			edge1 <= 0;
		else if(cmd1[2:1] == 2'b01 || cmd1[3:1] == 3'b100)	// ��ʽ1��ʽ5��ʽ4��
			edge1 <= 1;
	end

	always @(posedge write1 or posedge wreset1)				// ��wlh�ź�
	begin
		if(wreset1)		
			wlh1 <= 2'b00;
		else if(wlh1 == 2'b10)		
			wlh1 <= 2'b01;
		else		
			wlh1 <= wlh1 + 1;
	end

	always @(posedge read1 or posedge rreset1)				//��rlh�ź�
	begin
		if(rreset1)		
			rlh1 <= 2'b00;
		else if(rlh1 == 2'b10)		
			rlh1 <= 2'b01;
		else		
			rlh1 <= rlh1 + 1;
	end

	always @(negedge clk1 or posedge all_set1)				// ������1��������
	begin
		if(all_set1)	
			cnt1 <= set1;
		else if(all_gate1)	
			cnt1 <= cnt1 - 1;
	end
	
	always @(cmd1[3:1])
	begin
		case(cmd1[3:1])
			3'b000 : clk_out[1] <= (cnt1 == 16'h0000) ? 1 : 0;				//0��ʽ���
			3'b001 : clk_out[1] <= (cnt1 == 16'h0000) ? 1 : 0;				//1��ʽ���
			3'b010 : clk_out[1] <= (cnt1 == 16'h0001) ? 0 : 1;				//2��ʽ���
			3'b011 : clk_out[1] <= (cnt1 < {1'b0, set1[14:0]}) ?	1 : 0;	//3��ʽ���
			3'b100 : clk_out[1] <= (cnt1 == 16'h0001) ? 0 : 1;				//4��ʽ���
			3'b101 : clk_out[1] <= (cnt1 == 16'h0001) ? 0 : 1; 				//5��ʽ���
		endcase
	end
	
	//0�ż�����
	always @(posedge write0)
	begin
		if(cnt0 != 16'h0000 && cmd0[3:2] == 2'b10)							// 0�ż���������ʽ4��5	
			reg0a <= 1;
	end

	always @(posedge gate0 or posedge edge0a)
	begin
		if(edge0a)		
			edge0 <= 0;
		else if(cmd0[2:1] == 2'b01 || cmd0[3:1] == 3'b100)					// 0�ż���������ʽ1��4��5
			edge0 <= 1;
	end

	always @(posedge write0 or posedge wreset0)
	begin
		if(wreset0)		
			wlh0 <= 2'b00;
		else if(wlh0 == 2'b10)		
			wlh0 <= 2'b01;
		else		
			wlh0 <= wlh0 + 1;
	end

	always @(posedge read0 or posedge rreset0)
	begin
		if(rreset0)		
			rlh0 <= 2'b00;
		else if(rlh0 == 2'b10)		
			rlh0 <= 2'b01;
		else		
			rlh0 <= rlh0 + 1;
	end

	always @(negedge clk0 or posedge all_set0)					// ��������
	begin
		if(all_set0)	
			cnt0 <= set0;	
		else if(all_gate0)	
			cnt0 <= cnt0 - 1;
	end

	always @(cmd0[3:1])
	begin
		case(cmd0[3:1])
			3'b000 : clk_out[0] <= (cnt0 != 16'h0000) ? 0 : 1;
			3'b001 : clk_out[0] <= (cnt0 == 16'h0000) ? 1 : 0;
			3'b010 : clk_out[0] <= (cnt0 == 16'h0001) ? 0 : 1;
			3'b011 : clk_out[0] <= cnt0 < ((set0[11:0] >> 1) + 1) ? 1 : 0;
			3'b100 : clk_out[0] <= (cnt0 == 16'h0001) ? 0 : 1;
			3'b101 : clk_out[0] <= (cnt0 == 16'h0001) ? 0 : 1;
		endcase
	end
	
	//2�ż�����
	always @(posedge write2)
	begin
		if(cnt2 != 16'h0000 && cmd2[3:2] == 2'b10)				// ��ʽ4��5
			reg2a <= 1;
	end

	always @(posedge gate2 or posedge edge2a)
	begin
		if(edge2a)		
			edge2 <= 0;
		else if(cmd2[2:1] == 2'b01 || cmd2[3:1] == 3'b100)		// ��ʽ1��4��5
			edge2 <= 1;
	end

	always @(posedge write2 or posedge wreset2)
	begin
		if(wreset2)		
			wlh2 <= 2'b00;
		else if(wlh2 == 2'b10)		
			wlh2 <= 2'b01;
		else		
			wlh2 <= wlh2 + 1;
	end

	always @(posedge read2 or posedge rreset2)
	begin
		if(rreset2)	
			rlh2 <= 2'b00;
		else if(rlh2 == 2'b10)		
			rlh2 <= 2'b01;
		else		
			rlh2 <= rlh2 + 1;
	end

	always @(negedge clk2 or posedge all_set2)				// ����������
	begin
		if(all_set2)	
			cnt2 <= set2;
		else if(all_gate2)	
			cnt2 <= cnt2 - 1;
	end

	always @(cmd2[3:1])
	begin
		case(cmd2[3:1])
		3'b000 : clk_out[2] <= (cnt2 == 16'h0000) ? 1 : 0;
		3'b001 : clk_out[2] <= (cnt2 == 16'h0000) ? 0 : 1;		
		3'b010 : clk_out[2] <= (cnt2 == 16'h0001) ? 0 : 1;		
		3'b011 : clk_out[2] <= (cnt2 < {1'b0, set2[14:0]}) ?	1 : 0;	
		3'b100 : clk_out[2] <= (cnt2 == 16'h0001) ? 0 : 1;		
		3'b101 : clk_out[2] <= (cnt2 == 16'h0001) ? 0 : 1;		
		endcase
	end
	
	// ��д����
	always @(dataoutall)
	begin
		// 0�ż�����
		if(write0)
		begin
			case(cmd0[5:4])
				2'b01 : 
				begin	
					set0[7:0] <= datain; 		// д���ֽ�
					set0[15:8] <= 8'h00; 
					wover0 <= 1;		
				end								
				2'b10 : 
				begin	
					set0[15:8] <= datain; 		// д���ֽ�
					wover0 <= 1;		
				end								
				2'b11 : 						// д�����ֽ�
				begin
					if(wlh0 == 2'b01)			
						set0[7:0] <= datain;    // �ȵ�
					else if(wlh0 == 2'b10)	
					begin	
						set0[15:8] <= datain; 
						wover0 <= 1;	
					end							// ���
				end
			endcase
		end
		else if(wover0)							// д��������
		begin
			wreset0 <= 1;
			wover0 <= 0;
		end
		else 
			wreset0 <= 0;
			
		// 1�ż�����
		if(write1)
		begin
			case(cmd1[5:4])
				2'b01 : 						// ֻ��/д���������ֽ�
				begin	
					set1[7:0] <= datain; 
					set1[15:8] <= 0; 
					wover1 <= 1;	
				end
				2'b10 : 						// ֻ��/д���������ֽ�
				begin	
					set1[15:8] <= datain; 
					wover1 <= 1;		
				end
				2'b11 : 						// �ȶ�/д���������ֽڡ����д���������ֽ�
				begin
					if(wlh1 == 2'b01)			
						set1[7:0] <= datain;
					else if(wlh1 == 2'b10)	
					begin	
						set1[15:8] <= datain; 
						wover1 <= 1;	
					end
				end
			endcase
		end
		else if(wover1)
		begin
			wreset1 <= 1;
			wover1 <= 0;
		end
		else 
			wreset1 <= 0;
		// 2�ż�����
		if(write2)
		begin
			case(cmd2[5:4])
				2'b01 : 
				begin	
					set2[7:0] <= datain; 
					set2[15:8] <= 0;	
					wover2 <= 1;		
				end
				2'b10 : 
				begin	
					set2[15:8] <= datain; 
					wover2 <= 1;		
				end
				2'b11 : 
				begin
					if(wlh2 == 2'b01)			
						set2[7:0] <= datain;
					else if(wlh2 == 2'b10)	
					begin	
						set2[15:8] <= datain; 
						wover2 <= 1;	
					end
				end
			endcase
		end
		else if(wover2)
		begin
			wreset2 <= 1;
			wover2 <= 0;
		end
		else 
			wreset2 <= 0;
		//0�ż�����
		if(read0)
		begin
			case(cmd0[5:4])
				2'b01 : 
				begin	
					dataout <= lock[7:0]; 
					rover0 <= 1;		
				end								// �����ֽ�
				2'b10 : 
				begin	
					dataout <= lock[15:8]; 
					rover0 <= 1;		
				end								// �����ֽ�
				2'b11 : 						// �������ֽ�
				begin
					if(rlh0 == 2'b01)			
						dataout <= lock[7:0];	// �ȵ�
					else if(rlh0 == 2'b10)	
					begin	
						dataout <= lock[15:8]; 
						rover0 <= 1;	
					end				 			// ���
				end
			endcase
		end
		else if(rover0)
		begin	
			rreset0 <= 1;
			rover0 <= 0;
		end
		//1�ż�����
		if(read1)
		begin
			case(cmd1[5:4])
				2'b01 :
				begin	
					dataout <= lock[7:0]; 
					rover1 <= 1; 		
				end
				2'b10 : 
				begin	
					dataout <= lock[15:8]; 
					rover1 <= 1;		
				end
				2'b11 : 
				begin
					if(rlh1 == 2'b01)			
						dataout <= lock[7:0];
					else if(rlh1 == 2'b10)	
					begin	
						dataout <= lock[15:8]; 
						rover1 <= 1;		
					end
				end
			endcase	
		end
		else if(rover1)
		begin	
			rreset1 <= 1;
			rover1 <= 0;
		end
		//2�ż�����
		if(read2)
		begin
			case(cmd2[5:4])
				2'b01 : 
				begin	
					dataout <= lock[7:0]; 
					rover2 <= 1; 		
				end
				2'b10 : 
				begin	
					dataout <= lock[15:8]; 
					rover2 <= 1;		
				end
				2'b11 : 
				begin
					if(rlh2 == 2'b01)			
						dataout <= lock[7:0];
					else if(rlh2 == 2'b10)	
					begin	
						dataout <= lock[15:8]; 
						rover2 <= 1;	
					end
				end
			endcase
		end
		else if(rover2)
		begin	
			rreset2 <= 1;
			rover2 <= 0;
		end
	end
endmodule