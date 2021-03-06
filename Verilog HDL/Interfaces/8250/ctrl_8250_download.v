//简单8250——固定格式传行通信接口控制控制芯片
//本芯片的功能为简单的8250的控制部分，负责控制对cpu的控制的响应。
module ctrl_8250_download(cs, wr, rd, a0, dis, dos, 
	   cts, dsr, dcd, ri, data_in, data_finish, 
	   rts, dtr, load, data_out, datain, dataout, data_en);
	input cs;					//选片；为0正常工作：为1，停止工作
	input wr, rd;				//读写控制
	input a0, dis, dos;			//数据输入输出控制
	input cts;					//允许发送
	input dsr;					//数据装置准备好:高电平有?
	input dcd;					//通信设备接收到远程载波
	input ri;					//通信设备通知终端。通信线路已接通
					
	input[7:0] data_in;			//7位并行数据输入，与信号接收器数据口相连
	input data_finish;			//信号接收完成信号

	input[7:0] datain;			//双向数据线
	output[7:0] dataout;
	output data_en;
				
	output rts;					//请求发送
	output dtr;					//数据终端准备好
	output[7:0] data_out;   	//7位并行数据输出，与系好发送器数据口相连
	output load;				//信号发送器数据载入信号
	reg rts;					
	reg dtr;


	reg[7:0] data_out;			//数据发送寄存器
	reg[7:0] data_bus;			//数据接收寄存器

	wire write,read;			//状态读写信号

	assign write=!(cs|wr);		//写信号：当cs,wr同时为低电平时有?
	assign read=!(cs|rd);		//读信号：当cs,rd同时为高电平时有?

	wire dis_new,dos_new;		//数据读写信号

	assign dis_new=dis&a0;		//写入信号：当a0为1时，dis高电平有?
	assign dos_new=dos&!a0;		//读入信号：当a0为0时，dos高电平有?

	assign load=dis_new;		//load信号服从于dis_new信号

	always @(posedge dis_new)	//当数据读信号有?时，将d口的数据读入发送寄存器
	begin
		data_out=datain;
	end

	always @(posedge data_finish or posedge read)	//对接收寄存器的操作
	begin
		if(data_finish)				//当就收完成信号有?的时候，将，data_in数据读入接收寄存器
			data_bus=data_in;
	else
		begin						//当读状态信号有?时，读入状态
			data_bus[3]=cts;		//cts到第3位
			data_bus[2]=dsr;		//dsr到第2位
			data_bus[1]=ri;			//ri到第1位
			data_bus[0]=dcd;		//dcd到第0位
		end
	end

	always @(posedge write)			//当写状态信号有?的时候，写入状态
	begin
		rts=datain[7];				//rts为d[7]
		dtr=datain[6];				//dtr为[6]
	end	

	assign dataout=(dos_new|read)?data_bus:8'b0;  //data在读数据，和读状态的时候输出接收寄存器，其它情况输出高阻态
	assign data_en=dos_new|read;
endmodule