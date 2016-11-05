module core(cs,rd,wr,inta,a0,reset,isrset,code,datain,busdatain,Itim,sm,rd_isr,rd_imr,wr_imr,clr_imr,freeze,rd_irr,eor,setzero,sp,dataout,busdataout,o1,en,ocw3);
input cs;
input rd,wr;
input inta;
input a0;
input reset;
input [7:0]isrset;
input [2:0]code;
input [7:0]datain,busdatain;//����źŻ�������������IRR��7~0
//��INTA�ĵ�һ��������֮��
output Itim;
output sm;
output rd_isr;
output rd_imr,wr_imr,clr_imr;
output rd_irr;
output [7:0]eor;
output [7:0]setzero;
reg [7:0]setzero;
output [2:0]sp;
output [7:0]dataout;
output [7:0]busdataout;
output freeze;
output o1;
output en;
//output clk,write1,write2;
//output [3:0]state;
wire en;

wire read1;
wire read2;

wire [7:0]icw1,icw2,icw3;
reg [7:0]icw4;//,icw4;
//output [7:0]icw1,icw2,icw3,icw4;
wire [7:0]ocw1,ocw2,ocw3;
//output [7:0]ocw1,ocw2,ocw3;
output [7:0]ocw3;
reg write1,write2;
wire [5:0]codeocw2;
wire [11:0]erocw2;
reg [3:0] state;//state
reg [7:0]er;
reg [1:0]edge1;
//output [1:0]edge1;
reg flag1,flag2;
reg [2:0]pri;
assign freeze=1;
reg clr_imr;
reg wr_imr;
reg clk;

wire o1;//���ڱ�����ǰ��״̬CPU�Ѿ������ģ��д����OCW1
//edge1 not declared
//��ʼ����������Ҫ������״̬��������״̬������ת������ICW��OCW

//assign write1=(~cs&&~wr&&rd&&~a0)?1:0;//дICW1
//assign write2=(~cs&&~wr&&rd&&a0)?1:0;//дICW2

	always
	begin
	
		if(~cs&&~wr&&rd)
			begin
			if(!a0)
				begin
				write1<=1;
				write2<=0;
				
				end
			else if(a0&&!write1)
				begin
				write2<=1;
				write1<=0;								
				end
			end
		else
			begin
			write1<=0;
			write2<=0;
			end
		if(write1||write2)
			clk=1;
		else
			clk=0;
end
	always@(negedge write2 or posedge reset )
		begin
			if(reset)begin
				edge1=2'b00;
					end
			else 
			begin
				edge1=edge1+1;
			end
	
		end
	
	

	

	always@(posedge clk or posedge reset)//clk�ź���write1/write2Ӱ�죬write1 write2��cs,wr,rd,a0Ӱ��
		begin
			if(reset)//����״̬��
				begin
				state<=1;
			
				end
			else //�������reset���Ǿ���clk�����
				begin
					case(state)//״̬ת��
						1:begin
						if(write2)
						state<=2;
						end
						2:begin//�����ǰ״̬��2
							if(~icw1[1])//���ICW1[1]=0�����ʾ�м����������ǵ�Ƭʹ��
								begin
							//ע������ʹ����һ��edge1��������һ��2λ���źţ�����λ01ʱ���ܽ���״̬3��������Ӧ�������ڼ����ģ���ÿ�ν��п����ֵ�д�룬�ͼ���һ��
							//��Ϊһ��ֻ��ICW1,ICW2,ICW3,ICW4��Ҫд�����������2λ���ˣ�Ӧ��Ҳ��һ��always
								if(edge1==2'b01)
								state<=3;//����м�������Ӧ��д��ICW3			�����Խ���״̬3	
								end				
							else if(icw1[0])//���û�м�������Ӧ�ò鿴�Ƿ�д��ICW4�����Խ���״̬4
								begin	
									
									if(edge1==2'b01)
									state<=4;//set ICW4
									else if(edge1==2'b01)//���û�м��������Ҳ���ҪIC3,ICW4����ֱ�ӽ���״̬5
										begin
										state<=5;										
										end
								end
							end//end of case 2
						3:begin//��Ҫд��ICW3
							if(icw1[0])
								begin//�鿴�Ƿ���Ҫд��ICW4���������֣�Ӧ��Ҳ��Ҫ���edge1�ģ�
							//����ȴû�м�飬���ǲ���Ҫ����ICW4��ʱ��ȥ״̬5�ż��
								if(edge1==2'b10)
									state<=4;
								end
							else if(edge1==2'b10)
								begin
								state<=5;//����Ҫ����ICW4��ֱ�ӽ���״̬5��OCW�Ķ�д�׶�
								
								end
							end//end of case 3
						4://set icw4�������Ҫд��ICW4����ICW4д�����֮�����״̬5
							if(edge1>=2'b10)
								begin
							state<=5;
							
								end
						5://��Ȼ����һ���ֻأ�����������ף���A0=0ʱ���������Ŀ�������datain[4]==1�ͻ���reset�Ĺ��ܣ�
							if(write1&&datain[4])
							state<=1;
						default:state<=state;
					endcase		
				end//end of else
			
		end//end of always


//assign clk=write1|write2;//���߽�ϲ���ʱ��
//set ICW OCW������״̬д��ICW��OCW

assign icw1=reset?8'h00:(state==1&&write1?datain:icw1);
assign icw2=reset?8'h00:(state==2&&write2?datain:icw2);
assign icw3=reset?8'h00:(state==3&&write2?datain:icw3);
//assign icw4=reset?8'h00:(state==4&&~write2?datain:icw4);
assign ocw1=reset?8'h00:(state==5&&write2?datain:ocw1);//����ط�ֵ��ע�⣬дOCW1��ҪAOΪ1��������ΪOCW1[4]����Ϊ1��Ϊ�˱��ⱻ���ã�ֻ����A0=1
assign ocw2=reset?8'h00:(state==5&&write1&&(datain[4:3]==2'b00)?datain:ocw2);//OCW2,OCW3��datain[4]��ԶΪ0����������
assign ocw3=reset?8'h00:(state==5&&write1&&(datain[4:3]==2'b01)?datain:ocw3);

always@(negedge write2 or posedge reset )
begin
		if(reset)
			begin
			icw4<=0;
			end
		else
			begin
				if(state==4)
					icw4<=datain;
				else
					icw4<=icw4;
			end


end

assign Itim=icw1[3];//��Ե�������ǵ�ƽ����
//read imr irr or isr 
//flag1 flag2 both not decaler 
//������ݣ���CPU������ȡIMR,ISR,IRR��ָ��ʱ����ЩҪ��ȡ�����ݴ����ﱻ�ͳ�
//ICW2[7:3]5λ��ʾ�ն����ͺţ���code[2:0]������Ƿ����жϵ���������
assign dataout=(read2||(read1&&ocw3[1]==1))?busdatain:((flag2==0&&flag1==1)?{icw2[7:3],code[2:0]}:8'h00);
assign en=(read2||(read1&&ocw3[1]==1))?0:1;
//CPU�Ķ��ź�
//flag1==1��ʾ��һ�������أ�setzero��EOIӦ����һ����ֵ
assign read1=(~cs&&wr&&~rd&&~a0)?1:0;
assign read2=(~cs&&wr&&~rd&&a0)?1:0;
assign rd_imr=read2;
//����ж����β��������ּ�OCW1���������Ŀ�ĵز���CPU��Ӧ�����ж����μĴ���IMR������datain
assign busdataout=o1?ocw1:8'b0;//imr?

assign o1=(state==5)&&write2;
//ʲôʱ�����IMR�أ������ϵ����ӣ�Ӧ�� ����������ICW2֮�󣬶�ICW2�Ǹ����ж����ͺ����õģ�
//����˵ICW1��������ж�����IMR
always@(reset or write2)
	begin
		if(reset)
		 clr_imr<=1;
		else if(write2)
		 clr_imr<=0;

	end
always@( o1 )
	begin
		wr_imr<=o1;
	end
//OCW2���������ж��Ŷӷ�ʽ�����ã���Ϊ����Ȩ�̶�������Ȩ�ֻ���ʽ
assign codeocw2={code,ocw2[2:0]};//����һ��6λ���źţ�����λ�Ƿ����жϵļĴ�������ϣ�����λ�Ͳ�һ����������
//��SL��OCW2[6]=0ʱ������λ����0���̶����ȼ���������ָ�����жϵȼ�0~7���õȼ���Ϊ�˶�ָ����ISR��λ
//��ִ�����ȼ�ָ���ֻ���ʽ
//mclr not declared
//erû������
//code����PR��������Ƶ�Ԫ�����ڽ��з�����ж�Դ
wire o2;
assign o2=(state==5)&&write1&&(datain[4:3]==2'b00);
	always@(posedge inta or posedge reset)//flag1 flag2������һ��inta�����ص�ʱ��flag1=1,flag2=0,���ʱ��Ӧ�õȴ�д��OCW3�����ڶ���Inta�����ص�ʱ���Ǳ�ʾ�ж��Ѿ�������flag1=0,flag2=1
	begin
		if(reset)
			begin
			flag1=0;			
			end
		else
			flag1=~flag1;
	end
		always@(negedge inta or posedge reset)//flag1 flag2������һ��inta�����ص�ʱ��flag1=1,flag2=0,���ʱ��Ӧ�õȴ�д��OCW3�����ڶ���Inta�����ص�ʱ���Ǳ�ʾ�ж��Ѿ�������flag1=0,flag2=1
	begin
		if(reset)
			begin
			flag2=0;			
			end
		else
			flag2=~flag2;
	end
	
	wire mclr;
	assign mclr=clr_imr;
	always@(codeocw2)//����ж����ȼ�
	begin
		if(reset||mclr)begin er<=8'hff; end//er?mclr?
		else if(icw4[1])//���ICW4[1]�Զ�������ʽ��ISR�Զ���λ�����跢��EOI,��er,��������Ϊʲô��Ҫ����EOI��
		begin
			if(flag1)begin//inta�ĵڶ������ӣ���ʱӦ���ɿ��������ISR���ã��鿴��ǰ���ڷ�����жϺ�code��Ȼ�󽫶�Ӧ��EOI����Ϊ1
			case (code)
				0:er<=8'b00000001;//reg 0 �
				1:er<=8'b00000010;//reg 1
				2:er<=8'b00000100;//reg 2
				3:er<=8'b00001000;//reg 3
				4:er<=8'b00010000;//reg 4
				5:er<=8'b00100000;//reg 5
				6:er<=8'b01000000;//reg 6
				7:er<=8'b10000000;//reg 7
				default:er<=8'h00;
			endcase//
			end//end of if flag
		end//o2Ӧ�ú�ICW4[1]��Ӧ���������Զ�����ʱ��Ҫ��CPUд��OCW2
		else if(o2)//��ʾOCW2�Ѿ�������ɣ�ISR������INTA�ĵڶ����½����������ʱ��ҪOCW2�е�EOI��������
				begin
				if(ocw2[5])begin //��ʱ��һλӦ��Ϊ1�������жϽ��������ISR�����ǲ���setzero,setzeroӦ���ڵ�һ���½��ص�ʱ��
					if(ocw2[6])begin //SL=1˵��OCW2�������λ����Ч��
						case(ocw2[2:0])
							0:er<=8'b00000001;//reg 0 �����ǰ���ڷ�����ж�Դ��
							1:er<=8'b00000010;//reg 1
							2:er<=8'b00000100;//reg 2
							3:er<=8'b00001000;//reg 3
							4:er<=8'b00010000;//reg 4
							5:er<=8'b00100000;//reg 5
							6:er<=8'b01000000;//reg 6
							7:er<=8'b10000000;//reg 7
							default:er<=8'h00;
						endcase						
						end
					else begin//���������λ��Զ��0
							case(code)
							0:er<=8'b00000001;//reg 0 �����ǰ���ڷ�����ж�Դ��
							1:er<=8'b00000010;//reg 1
							2:er<=8'b00000100;//reg 2
							3:er<=8'b00001000;//reg 3
							4:er<=8'b00010000;//reg 4
							5:er<=8'b00100000;//reg 5
							6:er<=8'b01000000;//reg 6
							7:er<=8'b10000000;//reg 7
							default:er<=8'h00;
							endcase
						end //edn of else	
					end//if(ocw2[5])
				end //end of if(o2)
		else if(!mclr)
			er<=0;	
	end//end of always
assign erocw2={er,ocw2[2:0]};
//����Ĵ��븺�����ȼ����ã�֮ǰ������ER��Ҳ�������pr��EOI��pr�ָ���ISRcode
//�������ȼ����ú�OCW2[7]�йأ����õ����ȼ�����������OCW2[2:0]��Ҳ����������������
	always@(erocw2)
		begin		
			if(reset||mclr)//IMR������ź�
			pri<=7;			//���ֵ��ʾIR0���ȼ����
			else if(icw4[1])begin
				if(!ocw2[7])begin//�����ʾ���Զ��ֻ�״̬����erҪ�жϵ��Ǹ�����һ�����ص㣬pri����PR��sp
							//aotu to circle
							case(er)
							8'h01:pri<=0;
							8'h02:pri<=1;
							8'h04:pri<=2;
							8'h08:pri<=3;
							8'h10:pri<=4;
							8'h20:pri<=5;
							8'h40:pri<=6;
							8'h80:pri<=7;
							endcase	
					end
			
				else if(o2&&ocw2[7])begin//���Զ�EOIѭ����ʽ�����Զ�ѭ����ʽ�Ļ���Ӧ����OCW[7]==0��
						if(ocw2[6])//SL=1,d0~d2��ָ��ר�ŵ����ȼ�
							begin
						/*	case(ocw2[2:0])//L2~L1
							0:pri<=0;
							1:pri<=1;
							2:pri<=2;
							3:pri<=3;
							4:pri<=4;
							5:pri<=5;
							6:pri<=6;
							7:pri<=7;
							endcase*/
							pri<=ocw2[2:0];
							end	
						else //SL=0	����ר��ָ�������ȼ��ȼ�
							begin
							case(er)
							8'h01:pri<=0;
							8'h02:pri<=1;
							8'h03:pri<=2;
							8'h04:pri<=3;
							8'h05:pri<=4;
							8'h06:pri<=5;
							8'h07:pri<=6;
							8'h08:pri<=7;
							endcase					
							end //else if SL=0
				end //else if(o2&&ocw2[7])begin//BEGIN no EOI 
			end
		else begin//no EOI ocw2 d7=0
				pri<=pri;
			end
	end//end of always
		
assign sm=(ocw3[6:5]==2'b10)?1:0;
assign sp=pri;
assign eor=er;
always@(inta or reset )
	begin
		if(reset)
			begin
				setzero<=8'hff;
			end
		else if(flag2)			
		 begin
			case(code)
			0:setzero<=8'b00000001;//reg 0 �����ǰ���ڷ�����ж�Դ��
			1:setzero<=8'b00000010;//reg 1
			2:setzero<=8'b00000100;//reg 2
			3:setzero<=8'b00001000;//reg 3
			4:setzero<=8'b00010000;//reg 4
			5:setzero<=8'b00100000;//reg 5
			6:setzero<=8'b01000000;//reg 6
			7:setzero<=8'b10000000;//reg 7
			default:setzero<=8'h00;
			endcase	
		end
	else
		 setzero<=0;

	end
//read irr
assign rd_irr=(read1&&ocw3[1:0]==2'b10)?1:0;
assign rd_isr=(read1&&ocw3[1:0]==2'b11)?1:0;
endmodule
