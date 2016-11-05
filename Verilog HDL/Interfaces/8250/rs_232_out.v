//8位异步串行信号发生器
//本模块按照固定的波特率（9600bit/s）来接收串行信号；
//采用起止式异步协议，起始位为一位的低电平，数据长度为8位，不设置奇偶校验位，停止位为2位高电平；
//本模块使用一个使能信号，在准备状态时，当在使能状态时，在load信号时，读入数据，并同时发送。发送信号的同时
//发送器处于发送状态（ready==0）发送完成后恢复准备状态；
module rs_232_out(clock, enable, sset, load, data_in, shiftout, ready);
	input clock;				//发送器时钟：9600hz
	input enable;				//发送器使能
	input sset;					//同步清零
	input load;					//载入数据信号
	input[7:0] data_in;			//8位并行数据输入
	
	output shiftout;			//异步串行信号输出
	output ready;				//发送器状态检查

	reg shiftout;				//异步串行信号输出
	reg ready;					//发送器状态检查
	reg[7:0] data;				//8位数据寄存器
	reg[3:0] count;				//计数器

	always @(posedge clock)
	begin
		//使能信号控制
		if(enable)
		begin
			//在准备状态下，对load信号产生反应；
			if(load && ready)
			begin
				ready = 0;				//进入传送状态
				data = data_in;		//载入数据
				shiftout = 0;			//输出串行信号的起始位
			end
			//在传送状态下传送信号
			else if(/*!load && */!ready)
			begin
				case(count)
					//分别从低到高输出8位数据
					0 : shiftout = data[0];		
					1 : shiftout = data[1];
					2 : shiftout = data[2];
					3 : shiftout = data[3];
					4 : shiftout = data[4];
					5 : shiftout = data[5];
					6 : shiftout = data[6];
					7 : shiftout = data[7];
					//输出中止位			
					8 : shiftout = 1;
					//结束转为准备状态，计数器清零
					9 : begin
						 	shiftout =1; 
							count = -1; 
							ready = 1; 
						end
				endcase
				count = count + 4'b1;		//计数器计数
			end
		end
		//同步清零
		if(sset)
		begin
			ready = 1;
			count = 0;
			shiftout = 1;
		end
	end
endmodule
	
















