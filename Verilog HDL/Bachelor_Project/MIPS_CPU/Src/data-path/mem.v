//`include "cocoa_head.v"
`timescale  1ns/1ns
//////////////////////////////////////////////////////////////////////////////////
// Company: 		BUAA
// Engineer: 		Jerry.Zhai,  BB.Wang
// 
// Create Date:    16:20:04 04/08/2008 
// Design Name: 	Simulated Memory
// Module Name:    	Mem
// Project Name: 	 MIPS-C processor
// Target Devices: Sparten-3A DSP-1800
// Tool versions:  9.2i
// Description: 	 MIPS-C processor is part of COCOA project of BUAA. This project 
//						 devotes to let students master the main displines of computer 
//						 science such like computer organize, operating system, etc, in 
//						 more depth, which requires them to design a computer and OS 
//						 by their own hands.
//
// Dependencies: 	 DDRII-memory controller ,system bridge ,External Interrupt Controller
//	
// Revision: 		0.02
// Revision 		0.02 - File Created
// Additional Comments: details about this unit, please refer to the documentation
//						Mips-C interface and design, current version is 0.1.0.2
//
//////////////////////////////////////////////////////////////////////////////////

module	Mem(
	Clk,
	CS,
	BE,
	Reset,
	RW,
	Addr,
	DataIn,
	DataOut,
	DataReady
);

input Clk;
input CS;
input [3:0] BE;
input RW;
input [31:2] Addr;
input [31:0] DataIn;
input Reset;

output [31:0] DataOut;
output DataReady;

////parameters
parameter MEM_READ = 0;
parameter MEM_WRITE = 1;
parameter RAM_WIDTH = 32;
parameter RAM_ADDR_BITS = 32;


////here we defined only 31 memory unit, just for testing
reg [31:0] memory[31:0];

reg [31:0] DataOut;   
reg		 	DataReady;


//  The folloing code is only necessary if you wish to initialize the RAM 
//  contents via an external file (use $readmemb for binary data)
initial begin
	memory[0] <=  32'b101000_00000_00000_00000_00000_001001;		//j   9
	memory[9] <=	32'b001000_00000_00010_0000_0000_0000_0010;		//li	'h0000_0010  to $2
	memory[10]<=	32'b001111_00000_00011_0000_0000_0000_1110;		//li	'h0000_1110  to $3
	memory[11]<=	32'b000000_00011_00010_00100_00000_010011;		//mul  $4 $3  $2   
	memory[12]<=	32'b100111_00000_00100_0000_0000_0000_1100;		//sw   $4 12($0)
	memory[13]<=	32'b000000_00011_00010_00100_00000_001011;		//div  $4 $3  $2	//mul
	memory[14]<=	32'b100111_00000_00100_0000_0000_0001_0000;		//sw   $4 16($0)     
	memory[15]<=	32'b000000_00000_00000_00100_00000_010101;		//mfhi $4
	memory[16]<=	32'b100111_00000_00100_0000_0000_0001_0100;		//sw 	$4 20($0)
	memory[17]<= 32'b101000_00000_00000_00000_00000_000000;
end

always@(posedge CS)
   if(RW == MEM_WRITE) begin
      memory[Addr[6:2]][31:16] <= (BE[3:2] == 2'b11) ? DataIn[31:16] : 16'b0;
      memory[Addr[6:2]][15:8]  <= (BE[1] == 1'b1) ? DataIn[15:8] : 8'b0;
      memory[Addr[6:2]][7:0] <= DataIn[7:0];
   end else begin
      DataOut[31:16] <= (BE[3:2] == 2'b11) ? memory[Addr[6:2]][31:16] : 16'b0;
      DataOut[15:8]  <= (BE[1] == 1'b1) ? memory[Addr[6:2]][15:8] : 8'b0;
      DataOut[7:0]   <= memory[Addr[6:2]][7:0];
   end
   
//always@(CS, posedge Reset) old  pds--------------------------------------------------------------------------
always@(posedge CS, posedge Reset)
	if(Reset) DataReady <= 0;	
	else if(CS) begin #(155) DataReady <= 1; end
	else begin #(210) DataReady <= 0; end	

endmodule
















