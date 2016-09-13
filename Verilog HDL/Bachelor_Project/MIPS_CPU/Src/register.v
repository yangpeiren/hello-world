`timescale 1ns/1ns

module  Register( din, dout, clk ) ;
    parameter WIDTH_REG = 3 ;
    input   [WIDTH_REG-1:0]         din ;
    output  [WIDTH_REG-1:0]         dout ;
    input                           clk ;
    
    reg     [WIDTH_REG-1:0]         dout ;

//    input   [31:0]         din ;
//    output  [31:0]         dout ;
//    input                           clk ;
//    
//    reg     [31:0]         dout ;
        
    always  @( posedge clk )
        dout <= din ;

endmodule

module  Reg8( din, dout, clk ) ;
    input   [7:0]         din ;
    output  [7:0]         dout ;
    input                           clk ;
    
    reg     [7:0]         dout ;
        
    always  @( posedge clk )
        dout <= din ;

endmodule

module  Reg16( din, dout, clk ) ;
    input   [15:0]         din ;
    output  [15:0]         dout ;
    input                           clk ;
    
    reg     [15:0]         dout ;
        
    always  @( posedge clk )
        dout <= din ;

endmodule

module  Reg32( din, dout, clk ) ;
    input   [31:0]         din ;
    output  [31:0]         dout ;
    input                           clk ;
    
    reg     [31:0]         dout ;
        
    always  @( posedge clk )
        dout <= din ;

endmodule
