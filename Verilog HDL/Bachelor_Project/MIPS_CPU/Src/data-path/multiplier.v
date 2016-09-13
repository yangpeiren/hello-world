`timescale 1ns/1ns
/** The implemtation of the multiplier */
/* 原设计有错，并且考虑到乘法器可以在数字逻辑讲解，因此这里不给出详细设计 */

module Multiplier( MA, MB, MOut, Work, Sign, Done, Clk, Reset ) ;
    parameter NBit =32;
    parameter DELAY_MULTIPLIER=31 ;                     // 31 = 32 - 1 
    input [NBit - 1:0]                  MA;
    input [NBit - 1:0]                  MB;
    output [2 * NBit - 1:0]             MOut;
    input                               Work;   
    input                               Sign ;   
    output reg                          Done ;
    input                               Clk;
    input                               Reset;

    ///////////
    wire [30:0]                         a_abs, b_abs ;
    wire                                sign_bit ;
    reg  [63:0]                         c_abs ;
    reg  [4:0]                          counter;    

    //  
    assign  MOut = (Sign) ? ((sign_bit==1) ? {1'b1,~c_abs[62:0]+1} : c_abs) : c_abs ;
    
    // 
    assign  a_abs = MA[31] ? ~MA[30:0]+1 : MA ;
    assign  b_abs = MB[31] ? ~MB[30:0]+1 : MB ;

    //
    always  @( posedge Clk )
        if ( Work )
            if ( Sign )
                c_abs <= a_abs * b_abs ;
            else
                c_abs <= MA * MB ;
    //
    assign  sign_bit =  {MA[31],MB[31]}==2'b00 ? 0 :        // positive 
                        {MA[31],MB[31]}==2'b11 ? 0 :        // positive
                                                 1 ;        // negative 
    
    /*  A count-down counter is used to simulate the cycles needed to execute a 
        multiplier operation. 
     */        
    always  @( posedge Clk or posedge Reset )
        if ( Reset )
    	    begin
    		    counter <= 0;
    		    Done <= 0;
    	    end 
        else
            if ( Work )
                if ( counter != 0 )
                    counter <= counter - 1 ;
                else
                    Done <= 1 ;
            else
                begin
                    counter <= DELAY_MULTIPLIER ;
                    Done <= 0 ;
                end

endmodule

