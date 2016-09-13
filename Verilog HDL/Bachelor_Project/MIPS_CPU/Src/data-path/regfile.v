
/*
 * This is part of COCOA project, which covers the 5 basic disciplines
 * in computer science: digital logic, computer organization, compiler, 
 * computer architecture,  and operating system.
 * Copy right belongs to BUAA, 2008
 */
 
 
//`include "cocoa_head.v"

 /*
  * This is the register files for the MIPS-C core
  * Version 0.1.0.0
  * Author Jerry.Zhai
  */
 `timescale 1ns/1ns
  
module RegFile(
     CLK_I,
//     Reset_I,
     RS1_I,
     RS2_I,
     RD_I,
     RegWrite_I,
     RData1_O,
     RData2_O,
     WData_I
     );
    /////////////////////////// declarations //////////////////////////
    /*         IO ports          */  
    input               CLK_I;         // system clock
//    input         Reset_I;       // reset signal
    input [4:0]         RS1_I;         // the index of the 1st register
    input [4:0]         RS2_I;         // the index of the 2nd register
    input [4:0]         RD_I;          // the index of the target register to be writen
    input               RegWrite_I;    // write enable
    input [31:0]        WData_I;       // the data to be writen into the register
    
    output [31:0]       RData1_O;      // 1st register's content 
    output [31:0]       RData2_O;      // 2nd register's content

/* internal registers and wires */
    reg    [31:0]       rf[31:1] ;     // 32 common registers

/* write register when posedge of the clock, and register0 is read only */
    integer             j, m, n ;
    
    always  @( RD_I or RS1_I or RS2_I )
        begin
        j <= RD_I ;
        m <= RS1_I ;
        n <= RS2_I ;
        end

    always @( posedge CLK_I ) 
//    always @( posedge CLK_I or posedge Reset_I ) 
//        if ( Reset_I )
//            for (i=1; i<=31; i=i+1)
//                rf[i] <= 32'b0 ;
//        else if ( RegWrite_I )
        if ( RegWrite_I )
            begin
            // GXP rf[j] <= WData_I ;
            rf[j] <= WData_I ;
//            `ifdef DEBUG
//                $display("R[00-07]=%8X, %8X, %8X, %8X, %8X, %8X, %8X, %8X", 0, rf[1], rf[2], rf[3], rf[4], rf[5], rf[6], rf[7]);
//                $display("R[08-15]=%8X, %8X, %8X, %8X, %8X, %8X, %8X, %8X", rf[8], rf[9], rf[10], rf[11], rf[12], rf[13], rf[14], rf[15]);
//                $display("R[16-23]=%8X, %8X, %8X, %8X, %8X, %8X, %8X, %8X", rf[16], rf[17], rf[18], rf[19], rf[20], rf[21], rf[22], rf[23]);
//                $display("R[24-31]=%8X, %8X, %8X, %8X, %8X, %8X, %8X, %8X", rf[24], rf[25], rf[26], rf[27], rf[28], rf[29], rf[30], rf[31]);
//            `endif
            end
    /* read register */
    assign   RData1_O  = (RS1_I == 0) ? 0 : rf[m];
    assign   RData2_O  = (RS2_I == 0) ? 0 : rf[n]; 
    
endmodule
