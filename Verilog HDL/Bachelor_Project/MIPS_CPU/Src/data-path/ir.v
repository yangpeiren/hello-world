/*
 * This is part of COCOA project, which covers the 5 basic disciplines
 * in computer science: digital logic, computer organization, compiler, 
 * computer architecture,  and operating system.
 * Copy right belongs to BUAA, 2008
 */
 `timescale  1ns/1ns


//`include "../../cpu/src/cocoa_head.v"

/*
 * This is the instruction register for MIPS-C processor. 
 */
`timescale  1ns/1ns

module IR(
	CLK_I,
	Reset_I,
	MemData_I,
	IRWrite,
	IR_O
);
////////////////////// declarations /////////////////////////
/*			IO ports			*/			
input 	CLK_I;		         // system clock 
input  	Reset_I;		      // reset signal 
input	[31:0]MemData_I;		// memory data
input 	IRWrite;		       // write enable for IR register
output[31:0]IR_O;			    // IR's output 

/*		internal register			*/
reg	[31:0]ir;

/////////////////// implementation codes ////////////////////
//output 
assign 	IR_O = ir;

//write register
always@(posedge CLK_I or posedge Reset_I)
begin
	if(Reset_I)	ir <= 32'b0;
	else if(IRWrite) 	ir <= MemData_I;
end

endmodule




