module core(cs,rd,wr,inta,a0,reset,isrset,code,datain,busdatain,Itim,sm,rd_isr,rd_imr,wr_imr,clr_imr,rd_irr,eor,setzero,sp,dataout,busdataout,freeze);
input cs;
input rd,wr;
input inta;
input a0;
input reset;
input [7:0]isrset;
input [2:0]code;
input [7:0]datain,busdatain;//这个信号还可能是来自于IRR的7~0
//当INTA的第一个上升沿之后
output Itim;
output sm;
output rd_isr;
output rd_imr,wr_imr,clr_imr;
output rd_irr;
output [7:0]eor;
output [7:0]setzero;
output [2:0]sp;
output [7:0]dataout;
output [7:0]busdataout;
output freeze;
wire write1;
wire write2;
wire read1;
wire read2;
wire clk;
wire [7:0]icw1,icw2,icw3,icw4;
wire [7:0]ocw1,ocw2,ocw3;
wire [5:0]codeocw2;
wire [11:0]erocw2;
reg [3:0] state;//state
reg [7:0]er;
reg [1:0]edge1;
reg [1:0]flag1,flag2;
reg [2:0]pri;
assign freeze=1;
//edge1 not declared
//初始化工作，主要是设置状态机，根据状态机的跳转来设置ICW和OCW

	

	always@(posedge clk or posedge reset)//clk信号由write1/write2影响，write1 write2由cs,wr,rd,a0影响
		begin
			if(reset)//重置状态机
				state<=1;
			else //如果不是reset，那就是clk引起的
				begin
					case(state)//状态转换
						1://当前状态是1
						if(write2)//如果是write2有效，则a0=1,这种情况是要写入ICW2，没有else是因为如果是write2有效，则写入的是ICW1
						state<=2;//进入状态2，外面的assign语句会写入ICW2
						2:begin//如果当前状态是2
							if(~icw1[1])//如果ICW1[1]=0，则表示有级联，否则是单片使用
								begin
							//注意这里使用了一个edge1变量，是一个2位的信号，当它位01时才能进入状态3，我想这应该是用于计数的，即每次进行控制字的写入，就计数一次
							//因为一共只有ICW1,ICW2,ICW3,ICW4需要写入计数，所以2位够了，应该也是一个always
								if(edge1==2'b01)state<=3;//如果有级联，则应该写入ICW3			，所以进入状态3	
								end				
							else if(icw1[1]==0)//如果没有级联，则应该查看是否写入ICW4，所以进入状态4
									if(edge1==2'b01)state<=4;//set ICW4
							else if(edge1==2'b01)//如果没有级联，并且不需要IC3,ICW4，则直接进入状态5
									state<=5;
							end//end of case 2
						3:begin//需要写入ICW3
							if(icw1[0])//查看是否需要写入ICW4，这里很奇怪，应该也是要检查edge1的，
							//但是却没有检查，而是不需要设置ICW4的时候去状态5才检查
									state<=4;
								else if(edge1==2'b10)state<=5;//不需要设置ICW4，直接进入状态5，OCW的读写阶段
							end//end of case 3
						4://set icw4，如果需要写入ICW4，则当ICW4写入完成之后进入状态5
							if(edge1==2'b11)state<=5;
						5://居然还有一个轮回！这个看不明白，当A0=0时，如果输入的控制命令datain[4]==1就会起到reset的功能！
							if(write1&&datain[4])state<=1;
					endcase		
				end//end of else
		
		end//end of always
	always@(negedge write2 or posedge reset )
		begin
			if(reset)
				edge1=2'b00;
			else
				edge1=edge1+1;
		end
assign write1=(~cs&&~wr&&rd&&~a0)?1:0;//写ICW1
assign write2=(~cs&&~wr&&rd&&a0)?1:0;//写ICW2
assign clk=write1|write2;//两者结合产生时钟
//set ICW OCW，根据状态写入ICW和OCW
assign icw1=reset?8'h00:(state==1&&write1?datain:icw1);
assign icw2=reset?8'h00:(state==2&&write2?datain:icw2);
assign icw3=reset?8'h00:(state==3&&write2?datain:icw3);
assign icw4=reset?8'h00:(state==4&&write2?datain:icw4);
assign ocw1=reset?8'h00:(state==5&&write2?datain:ocw1);//这个地方值得注意，写OCW1需要AO为1！这是因为OCW1[4]可能为1，为了避免被重置，只好用A0=1
assign ocw2=reset?8'h00:(state==5&&write1&&(datain[4:3]==2'b00)?datain:ocw2);//OCW2,OCW3的datain[4]永远为0，否则被重置
assign ocw3=reset?8'h00:(state==5&&write1&&(datain[4:3]==2'b01)?datain:ocw3);

assign Itim=icw1[3];//边缘触发还是电平触发
//read imr irr or isr 
//flag1 flag2 both not decaler 
//输出数据，当CPU发出读取IMR,ISR,IRR的指令时，这些要读取的数据从这里被送出
//ICW2[7:3]5位表示终端类型号，而code[2:0]代表的是发生中断的请求的组合
assign dataout=(read2||(read1&&ocw3[1]==1))?busdatain:((flag2==0&&flag1==1)?{icw2[7:3],code[2:0]}:8'h00);
//CPU的读信号
//flag1==1表示第一个上升沿，setzero和EOI应该是一样的值
assign read1=(~cs&&wr&&~rd&&~a0)?1:0;
assign read2=(~cs&&wr&&~rd&&a0)?1:0;
assign rd_imr=read2;
//输出中断屏蔽操作命令字即OCW1，其输出的目的地不是CPU，应该是中断屏蔽寄存器IMR的输入datain
assign busdataout=o1?ocw1:8'b0;//imr?
wire o1;//用于表明当前的状态CPU已经向控制模块写入了OCW1
assign o1=state==5&&write2;
//什么时候清空IMR呢？看书上的样子，应该 是在设置了ICW2之后，而ICW2是负责中断类型号设置的，
//书上说ICW1命令清除中断屏蔽IMR
assign clr_imr=write1;
assign wr_imr=o1;
//OCW2的作用是中断排队方式的设置，分为优先权固定和优先权轮换方式
assign codeocw2={code,ocw2[2:0]};//这是一个6位的信号，高三位是发生中断的寄存器号组合，低三位就不一定有意义了
//当SL即OCW2[6]=0时，这三位都是0，固定优先级？否则是指定的中断等级0~7，该等级是为了对指定的ISR复位
//或执行优先级指定轮换方式
//mclr not declared
//er没有声明
//code是由PR输出给控制单元的正在进行服务的中断源
wire o2;
assign o2=state==5&&write1&&(datain[4:3]==2'b00);
	always@(inta,reset)//flag1 flag2，当第一次inta上升沿的时候，flag1=1,flag2=0,这个时候应该等待写入OCW3，当第二次Inta上升沿的时候是表示中断已经结束，flag1=0,flag2=1
	begin
		if(reset)
			begin
			flag1=2'b00;
			flag2=2'b00;
			end
		else if(inta)
		begin
		if(flag1==2'b00)//first
		flag1=2'b01;
		else if(flag1==2'b01)//second
		flag1=2'b10;
		
		end
		else if(~inta)
			begin
				if(flag2==2'b00)
						flag2=2'b01;
				else //second
						begin
						flag2=2'b00;
						flag1=2'b00;
						end
						
			end

	end
	wire mclr;
	assign mclr=clr_imr;
	always@(codeocw2)//如果中断优先级
	begin
		if(reset||mclr)begin er<=8'hff; end//er?mclr?
		else if(icw4[1])//如果ICW4[1]自动结束方式，ISR自动复位，无需发送EOI,即er,可是这里为什么还要设置EOI？
		begin
			if(flag2)begin//inta的第二个后延，这时应该由控制器输出ISR设置，查看当前正在服务的中断号code，然后将对应的EOI设置为1
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
		end//o2应该和ICW4[1]对应，当不是自动结束时，要等CPU写入OCW2
			else if(o2)//表示OCW2已经设置完成，ISR不能在INTA的第二个下降沿清除，这时需要OCW2中的EOI来控制了
				begin
				if(ocw2[5])begin //此时这一位应该为1，发送中断结束命令给ISR，但是不是setzero,setzero应该在第一个下降沿的时候
					if(ocw2[6])begin //SL=1说明OCW2的最低三位是有效的
						case(ocw2[2:0])
							0:er<=8'b00000001;//reg 0 如果当前正在服务的中断源是
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
					else begin//否则最低三位永远是0
							case(code)
							0:er<=8'b00000001;//reg 0 如果当前正在服务的中断源是
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
		

	end//end of always
assign erocw2={er,ocw2[2:0]};
//下面的代码负责优先级设置，之前设置了ER，也就输出给pr的EOI，pr又给了ISRcode
//但是优先级设置和OCW2[7]有关，设置的优先级可能来自于OCW2[2:0]，也可能来自于其他的
	always@(erocw2)
		begin		
			if(reset||mclr)//IMR的清空信号
			pri<=7;			//这个值表示IR0优先级最高
			else if(icw4[1])begin
				if(ocw2[7])begin//这个表示是自动轮换状态，则er要中断的那个的下一个是重点，pri就是PR的sp
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
				else if(o2&&ocw2[7])begin//非自动EOI循环方式，非自动循环方式的话不应该是OCW[7]==0吗？
						if(ocw2[6])//SL=1,d0~d2，指定专门的优先级
							begin
							case(ocw2[2:0])//L2~L1
							0:pri<=0;
							1:pri<=1;
							2:pri<=2;
							3:pri<=3;
							4:pri<=4;
							5:pri<=5;
							6:pri<=6;
							7:pri<=7;
							endcase
							end	
						else //SL=0	，无专门指定的优先级等级
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
				else begin//no EOI ocw2 d7=0
				pri<=pri;
			end
				
		end//end of if (o2)
	end//end of always
		
assign sm=(ocw3[6:5]==2'b10)?1:0;
assign sp=pri;
assign eor=er;
assign setzero=eor;
//read irr
assign rd_irr=(read1&&ocw3[1:0]==2'b10)?1:0;
assign rd_isr=(read1&&ocw3[1:0]==2'b11)?1:0;
endmodule
