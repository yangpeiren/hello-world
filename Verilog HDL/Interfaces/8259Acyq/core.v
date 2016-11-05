module core(cs,rd,wr,inta,a0,reset,isrset,code,datain,busdatain,Itim,sm,rd_isr,rd_imr,wr_imr,clr_imr,freeze,rd_irr,eor,setzero,sp,dataout,busdataout,o1,en,ocw3);
input cs;
input rd,wr;
input inta;
input a0;
input reset;
input [7:0]isrset;
input [2:0]code;
input [7:0]datain,busdatain;//Õâ¸öĞÅºÅ»¹¿ÉÄÜÊÇÀ´×ÔÓÚIRRµÄ7~0
//µ±INTAµÄµÚÒ»¸öÉÏÉıÑØÖ®ºó
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

wire o1;//ÓÃÓÚ±íÃ÷µ±Ç°µÄ×´Ì¬CPUÒÑ¾­Ïò¿ØÖÆÄ£¿éĞ´ÈëÁËOCW1
//edge1 not declared
//³õÊ¼»¯¹¤×÷£¬Ö÷ÒªÊÇÉèÖÃ×´Ì¬»ú£¬¸ù¾İ×´Ì¬»úµÄÌø×ªÀ´ÉèÖÃICWºÍOCW

//assign write1=(~cs&&~wr&&rd&&~a0)?1:0;//Ğ´ICW1
//assign write2=(~cs&&~wr&&rd&&a0)?1:0;//Ğ´ICW2

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
	
	

	

	always@(posedge clk or posedge reset)//clkĞÅºÅÓÉwrite1/write2Ó°Ïì£¬write1 write2ÓÉcs,wr,rd,a0Ó°Ïì
		begin
			if(reset)//ÖØÖÃ×´Ì¬»ú
				begin
				state<=1;
			
				end
			else //Èç¹û²»ÊÇreset£¬ÄÇ¾ÍÊÇclkÒıÆğµÄ
				begin
					case(state)//×´Ì¬×ª»»
						1:begin
						if(write2)
						state<=2;
						end
						2:begin//Èç¹ûµ±Ç°×´Ì¬ÊÇ2
							if(~icw1[1])//Èç¹ûICW1[1]=0£¬Ôò±íÊ¾ÓĞ¼¶Áª£¬·ñÔòÊÇµ¥Æ¬Ê¹ÓÃ
								begin
							//×¢ÒâÕâÀïÊ¹ÓÃÁËÒ»¸öedge1±äÁ¿£¬ÊÇÒ»¸ö2Î»µÄĞÅºÅ£¬µ±ËüÎ»01Ê±²ÅÄÜ½øÈë×´Ì¬3£¬ÎÒÏëÕâÓ¦¸ÃÊÇÓÃÓÚ¼ÆÊıµÄ£¬¼´Ã¿´Î½øĞĞ¿ØÖÆ×ÖµÄĞ´Èë£¬¾Í¼ÆÊıÒ»´Î
							//ÒòÎªÒ»¹²Ö»ÓĞICW1,ICW2,ICW3,ICW4ĞèÒªĞ´Èë¼ÆÊı£¬ËùÒÔ2Î»¹»ÁË£¬Ó¦¸ÃÒ²ÊÇÒ»¸öalways
								if(edge1==2'b01)
								state<=3;//Èç¹ûÓĞ¼¶Áª£¬ÔòÓ¦¸ÃĞ´ÈëICW3			£¬ËùÒÔ½øÈë×´Ì¬3	
								end				
							else if(icw1[0])//Èç¹ûÃ»ÓĞ¼¶Áª£¬ÔòÓ¦¸Ã²é¿´ÊÇ·ñĞ´ÈëICW4£¬ËùÒÔ½øÈë×´Ì¬4
								begin	
									
									if(edge1==2'b01)
									state<=4;//set ICW4
									else if(edge1==2'b01)//Èç¹ûÃ»ÓĞ¼¶Áª£¬²¢ÇÒ²»ĞèÒªIC3,ICW4£¬ÔòÖ±½Ó½øÈë×´Ì¬5
										begin
										state<=5;										
										end
								end
							end//end of case 2
						3:begin//ĞèÒªĞ´ÈëICW3
							if(icw1[0])
								begin//²é¿´ÊÇ·ñĞèÒªĞ´ÈëICW4£¬ÕâÀïºÜÆæ¹Ö£¬Ó¦¸ÃÒ²ÊÇÒª¼ì²éedge1µÄ£¬
							//µ«ÊÇÈ´Ã»ÓĞ¼ì²é£¬¶øÊÇ²»ĞèÒªÉèÖÃICW4µÄÊ±ºòÈ¥×´Ì¬5²Å¼ì²é
								if(edge1==2'b10)
									state<=4;
								end
							else if(edge1==2'b10)
								begin
								state<=5;//²»ĞèÒªÉèÖÃICW4£¬Ö±½Ó½øÈë×´Ì¬5£¬OCWµÄ¶ÁĞ´½×¶Î
								
								end
							end//end of case 3
						4://set icw4£¬Èç¹ûĞèÒªĞ´ÈëICW4£¬Ôòµ±ICW4Ğ´ÈëÍê³ÉÖ®ºó½øÈë×´Ì¬5
							if(edge1>=2'b10)
								begin
							state<=5;
							
								end
						5://¾ÓÈ»»¹ÓĞÒ»¸öÂÖ»Ø£¡Õâ¸ö¿´²»Ã÷°×£¬µ±A0=0Ê±£¬Èç¹ûÊäÈëµÄ¿ØÖÆÃüÁîdatain[4]==1¾Í»áÆğµ½resetµÄ¹¦ÄÜ£¡
							if(write1&&datain[4])
							state<=1;
						default:state<=state;
					endcase		
				end//end of else
			
		end//end of always


//assign clk=write1|write2;//Á½Õß½áºÏ²úÉúÊ±ÖÓ
//set ICW OCW£¬¸ù¾İ×´Ì¬Ğ´ÈëICWºÍOCW

assign icw1=reset?8'h00:(state==1&&write1?datain:icw1);
assign icw2=reset?8'h00:(state==2&&write2?datain:icw2);
assign icw3=reset?8'h00:(state==3&&write2?datain:icw3);
//assign icw4=reset?8'h00:(state==4&&~write2?datain:icw4);
assign ocw1=reset?8'h00:(state==5&&write2?datain:ocw1);//Õâ¸öµØ·½ÖµµÃ×¢Òâ£¬Ğ´OCW1ĞèÒªAOÎª1£¡ÕâÊÇÒòÎªOCW1[4]¿ÉÄÜÎª1£¬ÎªÁË±ÜÃâ±»ÖØÖÃ£¬Ö»ºÃÓÃA0=1
assign ocw2=reset?8'h00:(state==5&&write1&&(datain[4:3]==2'b00)?datain:ocw2);//OCW2,OCW3µÄdatain[4]ÓÀÔ¶Îª0£¬·ñÔò±»ÖØÖÃ
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

assign Itim=icw1[3];//±ßÔµ´¥·¢»¹ÊÇµçÆ½´¥·¢
//read imr irr or isr 
//flag1 flag2 both not decaler 
//Êä³öÊı¾İ£¬µ±CPU·¢³ö¶ÁÈ¡IMR,ISR,IRRµÄÖ¸ÁîÊ±£¬ÕâĞ©Òª¶ÁÈ¡µÄÊı¾İ´ÓÕâÀï±»ËÍ³ö
//ICW2[7:3]5Î»±íÊ¾ÖÕ¶ËÀàĞÍºÅ£¬¶øcode[2:0]´ú±íµÄÊÇ·¢ÉúÖĞ¶ÏµÄÇëÇóµÄ×éºÏ
assign dataout=(read2||(read1&&ocw3[1]==1))?busdatain:((flag2==0&&flag1==1)?{icw2[7:3],code[2:0]}:8'h00);
assign en=(read2||(read1&&ocw3[1]==1))?0:1;
//CPUµÄ¶ÁĞÅºÅ
//flag1==1±íÊ¾µÚÒ»¸öÉÏÉıÑØ£¬setzeroºÍEOIÓ¦¸ÃÊÇÒ»ÑùµÄÖµ
assign read1=(~cs&&wr&&~rd&&~a0)?1:0;
assign read2=(~cs&&wr&&~rd&&a0)?1:0;
assign rd_imr=read2;
//Êä³öÖĞ¶ÏÆÁ±Î²Ù×÷ÃüÁî×Ö¼´OCW1£¬ÆäÊä³öµÄÄ¿µÄµØ²»ÊÇCPU£¬Ó¦¸ÃÊÇÖĞ¶ÏÆÁ±Î¼Ä´æÆ÷IMRµÄÊäÈëdatain
assign busdataout=o1?ocw1:8'b0;//imr?

assign o1=(state==5)&&write2;
//Ê²Ã´Ê±ºòÇå¿ÕIMRÄØ£¿¿´ÊéÉÏµÄÑù×Ó£¬Ó¦¸Ã ÊÇÔÚÉèÖÃÁËICW2Ö®ºó£¬¶øICW2ÊÇ¸ºÔğÖĞ¶ÏÀàĞÍºÅÉèÖÃµÄ£¬
//ÊéÉÏËµICW1ÃüÁîÇå³ıÖĞ¶ÏÆÁ±ÎIMR
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
//OCW2µÄ×÷ÓÃÊÇÖĞ¶ÏÅÅ¶Ó·½Ê½µÄÉèÖÃ£¬·ÖÎªÓÅÏÈÈ¨¹Ì¶¨ºÍÓÅÏÈÈ¨ÂÖ»»·½Ê½
assign codeocw2={code,ocw2[2:0]};//ÕâÊÇÒ»¸ö6Î»µÄĞÅºÅ£¬¸ßÈıÎ»ÊÇ·¢ÉúÖĞ¶ÏµÄ¼Ä´æÆ÷ºÅ×éºÏ£¬µÍÈıÎ»¾Í²»Ò»¶¨ÓĞÒâÒåÁË
//µ±SL¼´OCW2[6]=0Ê±£¬ÕâÈıÎ»¶¼ÊÇ0£¬¹Ì¶¨ÓÅÏÈ¼¶£¿·ñÔòÊÇÖ¸¶¨µÄÖĞ¶ÏµÈ¼¶0~7£¬¸ÃµÈ¼¶ÊÇÎªÁË¶ÔÖ¸¶¨µÄISR¸´Î»
//»òÖ´ĞĞÓÅÏÈ¼¶Ö¸¶¨ÂÖ»»·½Ê½
//mclr not declared
//erÃ»ÓĞÉùÃ÷
//codeÊÇÓÉPRÊä³ö¸ø¿ØÖÆµ¥ÔªµÄÕıÔÚ½øĞĞ·şÎñµÄÖĞ¶ÏÔ´
wire o2;
assign o2=(state==5)&&write1&&(datain[4:3]==2'b00);
	always@(posedge inta or posedge reset)//flag1 flag2£¬µ±µÚÒ»´ÎintaÉÏÉıÑØµÄÊ±ºò£¬flag1=1,flag2=0,Õâ¸öÊ±ºòÓ¦¸ÃµÈ´ıĞ´ÈëOCW3£¬µ±µÚ¶ş´ÎIntaÉÏÉıÑØµÄÊ±ºòÊÇ±íÊ¾ÖĞ¶ÏÒÑ¾­½áÊø£¬flag1=0,flag2=1
	begin
		if(reset)
			begin
			flag1=0;			
			end
		else
			flag1=~flag1;
	end
		always@(negedge inta or posedge reset)//flag1 flag2£¬µ±µÚÒ»´ÎintaÉÏÉıÑØµÄÊ±ºò£¬flag1=1,flag2=0,Õâ¸öÊ±ºòÓ¦¸ÃµÈ´ıĞ´ÈëOCW3£¬µ±µÚ¶ş´ÎIntaÉÏÉıÑØµÄÊ±ºòÊÇ±íÊ¾ÖĞ¶ÏÒÑ¾­½áÊø£¬flag1=0,flag2=1
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
	always@(codeocw2)//Èç¹ûÖĞ¶ÏÓÅÏÈ¼¶
	begin
		if(reset||mclr)begin er<=8'hff; end//er?mclr?
		else if(icw4[1])//Èç¹ûICW4[1]×Ô¶¯½áÊø·½Ê½£¬ISR×Ô¶¯¸´Î»£¬ÎŞĞè·¢ËÍEOI,¼´er,¿ÉÊÇÕâÀïÎªÊ²Ã´»¹ÒªÉèÖÃEOI£¿
		begin
			if(flag1)begin//intaµÄµÚ¶ş¸öºóÑÓ£¬ÕâÊ±Ó¦¸ÃÓÉ¿ØÖÆÆ÷Êä³öISRÉèÖÃ£¬²é¿´µ±Ç°ÕıÔÚ·şÎñµÄÖĞ¶ÏºÅcode£¬È»ºó½«¶ÔÓ¦µÄEOIÉèÖÃÎª1
			case (code)
				0:er<=8'b00000001;//reg 0 È
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
		end//o2Ó¦¸ÃºÍICW4[1]¶ÔÓ¦£¬µ±²»ÊÇ×Ô¶¯½áÊøÊ±£¬ÒªµÈCPUĞ´ÈëOCW2
		else if(o2)//±íÊ¾OCW2ÒÑ¾­ÉèÖÃÍê³É£¬ISR²»ÄÜÔÚINTAµÄµÚ¶ş¸öÏÂ½µÑØÇå³ı£¬ÕâÊ±ĞèÒªOCW2ÖĞµÄEOIÀ´¿ØÖÆÁË
				begin
				if(ocw2[5])begin //´ËÊ±ÕâÒ»Î»Ó¦¸ÃÎª1£¬·¢ËÍÖĞ¶Ï½áÊøÃüÁî¸øISR£¬µ«ÊÇ²»ÊÇsetzero,setzeroÓ¦¸ÃÔÚµÚÒ»¸öÏÂ½µÑØµÄÊ±ºò
					if(ocw2[6])begin //SL=1ËµÃ÷OCW2µÄ×îµÍÈıÎ»ÊÇÓĞĞ§µÄ
						case(ocw2[2:0])
							0:er<=8'b00000001;//reg 0 Èç¹ûµ±Ç°ÕıÔÚ·şÎñµÄÖĞ¶ÏÔ´ÊÇ
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
					else begin//·ñÔò×îµÍÈıÎ»ÓÀÔ¶ÊÇ0
							case(code)
							0:er<=8'b00000001;//reg 0 Èç¹ûµ±Ç°ÕıÔÚ·şÎñµÄÖĞ¶ÏÔ´ÊÇ
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
//ÏÂÃæµÄ´úÂë¸ºÔğÓÅÏÈ¼¶ÉèÖÃ£¬Ö®Ç°ÉèÖÃÁËER£¬Ò²¾ÍÊä³ö¸øprµÄEOI£¬prÓÖ¸øÁËISRcode
//µ«ÊÇÓÅÏÈ¼¶ÉèÖÃºÍOCW2[7]ÓĞ¹Ø£¬ÉèÖÃµÄÓÅÏÈ¼¶¿ÉÄÜÀ´×ÔÓÚOCW2[2:0]£¬Ò²¿ÉÄÜÀ´×ÔÓÚÆäËûµÄ
	always@(erocw2)
		begin		
			if(reset||mclr)//IMRµÄÇå¿ÕĞÅºÅ
			pri<=7;			//Õâ¸öÖµ±íÊ¾IR0ÓÅÏÈ¼¶×î¸ß
			else if(icw4[1])begin
				if(!ocw2[7])begin//Õâ¸ö±íÊ¾ÊÇ×Ô¶¯ÂÖ»»×´Ì¬£¬ÔòerÒªÖĞ¶ÏµÄÄÇ¸öµÄÏÂÒ»¸öÊÇÖØµã£¬pri¾ÍÊÇPRµÄsp
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
			
				else if(o2&&ocw2[7])begin//·Ç×Ô¶¯EOIÑ­»··½Ê½£¬·Ç×Ô¶¯Ñ­»··½Ê½µÄ»°²»Ó¦¸ÃÊÇOCW[7]==0Âğ£¿
						if(ocw2[6])//SL=1,d0~d2£¬Ö¸¶¨×¨ÃÅµÄÓÅÏÈ¼¶
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
						else //SL=0	£¬ÎŞ×¨ÃÅÖ¸¶¨µÄÓÅÏÈ¼¶µÈ¼¶
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
			0:setzero<=8'b00000001;//reg 0 Èç¹ûµ±Ç°ÕıÔÚ·şÎñµÄÖĞ¶ÏÔ´ÊÇ
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
