
/*--------------------------------------------------------------------
 *	Part of the MIPS-C processor in COCOA project
 *
 *	Author: Jerry.Zhai, 2008.3.24
 *--------------------------------------------------------------------*/

//`include "../../cpu/src/cocoa_head.v"
`timescale  1ns/1ns

/* These are a serial of multiplexors used in MIPS-C processor 
 * The first port would be the default one if no extra instruction
 */

/* 2 select 1 */
module Mux2to1( in0, in1, sel, out );
    parameter WIDTH_DATA = 32 ;
    input   [WIDTH_DATA-1:0]        in0;
    input   [WIDTH_DATA-1:0]        in1;
    input                           sel;
    output  reg[WIDTH_DATA-1:0]     out;

    always@(in0, in1, sel)
    	case(sel)
    		0: out<= in0;
    		1: out<= in1;
    	endcase

endmodule

/* 3 select 1 */
module Mux3to1( in0, in1, in2, sel, out );
    parameter WIDTH_DATA = 32 ;
    input   [WIDTH_DATA-1:0]        in0;
    input   [WIDTH_DATA-1:0]        in1;
    input   [WIDTH_DATA-1:0]        in2;
    input   [1:0]                   sel;
    output  reg[WIDTH_DATA-1:0]     out;

    always@(in0, in1, in2, sel)
    	case(sel)
    		0: out<= in0;
    		1: out<= in1;
    		2: out<= in2;
    		default: out <= in0;
    	endcase

endmodule

/* 4 select 1 */
module Mux4to1( in0, in1, in2, in3, sel, out );
    parameter WIDTH_DATA = 32 ;
    input   [WIDTH_DATA-1:0]        in0;
    input   [WIDTH_DATA-1:0]        in1;
    input   [WIDTH_DATA-1:0]        in2;
    input   [WIDTH_DATA-1:0]        in3;
    input   [1:0]                   sel;
    output  reg[WIDTH_DATA-1:0]     out;

    always@(in0, in1, in2, in3, sel)
    	case(sel)
    		0:  out<= in0;
    		1:  out<= in1;
    		2:  out<= in2;
    		3:  out<= in3;
    	endcase

endmodule

/* 5 select 1 */
module Mux5to1( in0, in1, in2, in3, in4, sel, out );
    parameter WIDTH_DATA = 32 ;
    input   [WIDTH_DATA-1:0]        in0;
    input   [WIDTH_DATA-1:0]        in1;
    input   [WIDTH_DATA-1:0]        in2;
    input   [WIDTH_DATA-1:0]        in3;
    input   [WIDTH_DATA-1:0]        in4;
    input   [2:0]                   sel;
    output  reg[WIDTH_DATA-1:0]     out;

    always@(in0, in1, in2, in3, in4, sel)
    	case(sel)
    		0       : out <= in0;
    		1       : out <= in1;
    		2       : out <= in2;
    		3       : out <= in3;
    		4       : out <= in4;
    		default : out <= in0;
    	endcase
    	
endmodule

/* 6 select 1 */
module Mux6to1( in0, in1, in2, in3, in4, in5, sel, out );
    parameter WIDTH_DATA = 32 ;
    input   [WIDTH_DATA-1:0]        in0;
    input   [WIDTH_DATA-1:0]        in1;
    input   [WIDTH_DATA-1:0]        in2;
    input   [WIDTH_DATA-1:0]        in3;
    input   [WIDTH_DATA-1:0]        in4;
    input   [WIDTH_DATA-1:0]        in5;
    input   [2:0]                   sel;
    output  reg[WIDTH_DATA-1:0]     out;

    always@(in0, in1, in2, in3, in4, in5, sel)
    	case(sel)
    		0       : out <= in0;
    		1       : out <= in1;
    		2       : out <= in2;
    		3       : out <= in3;
    		4       : out <= in4;
    		5       : out <= in5;
    		default : out <= in0;
    	endcase

endmodule

/* 7 select 1 */
module Mux7to1( in0, in1, in2, in3, in4, in5, in6, sel, out );
    parameter WIDTH_DATA = 32 ;
    input   [WIDTH_DATA-1:0]        in0;
    input   [WIDTH_DATA-1:0]        in1;
    input   [WIDTH_DATA-1:0]        in2;
    input   [WIDTH_DATA-1:0]        in3;
    input   [WIDTH_DATA-1:0]        in4;
    input   [WIDTH_DATA-1:0]        in5;
    input   [WIDTH_DATA-1:0]        in6;
    input   [2:0]                   sel;
    output  reg[WIDTH_DATA-1:0]     out;

    always@(in0, in1, in2, in3, in4, in5, in6, sel)
    	case(sel)
    		0       : out <= in0;
    		1       : out <= in1;
    		2       : out <= in2;
    		3       : out <= in3;
    		4       : out <= in4;
    		5       : out <= in5;
    		6       : out <= in6;
    		default : out <= in0;
    	endcase

endmodule
