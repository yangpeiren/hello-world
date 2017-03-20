//��8250�����̶���ʽ����ͨ�Žӿڿ��ƿ���оƬ
//��оƬ�Ĺ���Ϊ�򵥵�8250�Ŀ��Ʋ��֣�������ƶ�cpu�Ŀ��Ƶ���Ӧ��
module ctrl_8250_download(cs, wr, rd, a0, dis, dos, 
	   cts, dsr, dcd, ri, data_in, data_finish, 
	   rts, dtr, load, data_out, datain, dataout, data_en);
	input cs;					//ѡƬ��Ϊ0����������Ϊ1��ֹͣ����
	input wr, rd;				//��д����
	input a0, dis, dos;			//���������������
	input cts;					//��������
	input dsr;					//����װ��׼����:�ߵ�ƽ��?
	input dcd;					//ͨ���豸���յ�Զ���ز�
	input ri;					//ͨ���豸֪ͨ�նˡ�ͨ����·�ѽ�ͨ
					
	input[7:0] data_in;			//7λ�����������룬���źŽ��������ݿ�����
	input data_finish;			//�źŽ�������ź�

	input[7:0] datain;			//˫��������
	output[7:0] dataout;
	output data_en;
				
	output rts;					//������
	output dtr;					//�����ն�׼����
	output[7:0] data_out;   	//7λ���������������ϵ�÷��������ݿ�����
	output load;				//�źŷ��������������ź�
	reg rts;					
	reg dtr;


	reg[7:0] data_out;			//���ݷ��ͼĴ���
	reg[7:0] data_bus;			//���ݽ��ռĴ���

	wire write,read;			//״̬��д�ź�

	assign write=!(cs|wr);		//д�źţ���cs,wrͬʱΪ�͵�ƽʱ��?
	assign read=!(cs|rd);		//���źţ���cs,rdͬʱΪ�ߵ�ƽʱ��?

	wire dis_new,dos_new;		//���ݶ�д�ź�

	assign dis_new=dis&a0;		//д���źţ���a0Ϊ1ʱ��dis�ߵ�ƽ��?
	assign dos_new=dos&!a0;		//�����źţ���a0Ϊ0ʱ��dos�ߵ�ƽ��?

	assign load=dis_new;		//load�źŷ�����dis_new�ź�

	always @(posedge dis_new)	//�����ݶ��ź���?ʱ����d�ڵ����ݶ��뷢�ͼĴ���
	begin
		data_out=datain;
	end

	always @(posedge data_finish or posedge read)	//�Խ��ռĴ����Ĳ���
	begin
		if(data_finish)				//����������ź���?��ʱ�򣬽���data_in���ݶ�����ռĴ���
			data_bus=data_in;
	else
		begin						//����״̬�ź���?ʱ������״̬
			data_bus[3]=cts;		//cts����3λ
			data_bus[2]=dsr;		//dsr����2λ
			data_bus[1]=ri;			//ri����1λ
			data_bus[0]=dcd;		//dcd����0λ
		end
	end

	always @(posedge write)			//��д״̬�ź���?��ʱ��д��״̬
	begin
		rts=datain[7];				//rtsΪd[7]
		dtr=datain[6];				//dtrΪ[6]
	end	

	assign dataout=(dos_new|read)?data_bus:8'b0;  //data�ڶ����ݣ��Ͷ�״̬��ʱ��������ռĴ�������������������̬
	assign data_en=dos_new|read;
endmodule