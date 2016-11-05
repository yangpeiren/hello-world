//8λ�첽�����źŷ�����
//��ģ�鰴�չ̶��Ĳ����ʣ�9600bit/s�������մ����źţ�
//������ֹʽ�첽Э�飬��ʼλΪһλ�ĵ͵�ƽ�����ݳ���Ϊ8λ����������żУ��λ��ֹͣλΪ2λ�ߵ�ƽ��
//��ģ��ʹ��һ��ʹ���źţ���׼��״̬ʱ������ʹ��״̬ʱ����load�ź�ʱ���������ݣ���ͬʱ���͡������źŵ�ͬʱ
//���������ڷ���״̬��ready==0��������ɺ�ָ�׼��״̬��
module rs_232_out(clock, enable, sset, load, data_in, shiftout, ready);
	input clock;				//������ʱ�ӣ�9600hz
	input enable;				//������ʹ��
	input sset;					//ͬ������
	input load;					//���������ź�
	input[7:0] data_in;			//8λ������������
	
	output shiftout;			//�첽�����ź����
	output ready;				//������״̬���

	reg shiftout;				//�첽�����ź����
	reg ready;					//������״̬���
	reg[7:0] data;				//8λ���ݼĴ���
	reg[3:0] count;				//������

	always @(posedge clock)
	begin
		//ʹ���źſ���
		if(enable)
		begin
			//��׼��״̬�£���load�źŲ�����Ӧ��
			if(load && ready)
			begin
				ready = 0;				//���봫��״̬
				data = data_in;		//��������
				shiftout = 0;			//��������źŵ���ʼλ
			end
			//�ڴ���״̬�´����ź�
			else if(/*!load && */!ready)
			begin
				case(count)
					//�ֱ�ӵ͵������8λ����
					0 : shiftout = data[0];		
					1 : shiftout = data[1];
					2 : shiftout = data[2];
					3 : shiftout = data[3];
					4 : shiftout = data[4];
					5 : shiftout = data[5];
					6 : shiftout = data[6];
					7 : shiftout = data[7];
					//�����ֹλ			
					8 : shiftout = 1;
					//����תΪ׼��״̬������������
					9 : begin
						 	shiftout =1; 
							count = -1; 
							ready = 1; 
						end
				endcase
				count = count + 4'b1;		//����������
			end
		end
		//ͬ������
		if(sset)
		begin
			ready = 1;
			count = 0;
			shiftout = 1;
		end
	end
endmodule
	
















