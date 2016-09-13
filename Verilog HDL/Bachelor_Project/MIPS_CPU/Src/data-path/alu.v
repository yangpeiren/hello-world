/*
 * This is part of COCOA project, which covers the 5 basic disciplines
 * in computer science: digital logic, computer organization, compiler, 
 * computer architecture,  and operating system.
 * Copy right belongs to BUAA, 2008
 */

`timescale  1ns/1ns 
`include    "processor_head.v"      // 

/*
 *   This is ALU of the MIPS-C core. The function code please refer 
 *   to the FPGA-specification
 */
/*
    ALU supports 16 operations. There are 10 operations are arithmetic/logic 
    operations and the others are comparison operations.
    1. To simplify the design of judgement logic of processor controller, we 
       use an independent signal named Compare to output comparison operation's 
       result.
    2. For ADD/SUB/SLT operations, the sign bit must be involved in computing.
    3. As to USLT/SLT/EQ/NEQ, reuse the SUB operation should be used to decrease 
       the resource utilization. But in this version, they are implemented in 
       such a way to reduce the complexity and make the design more clear.
 */
module Alu( A, B, Ctrl, C, Zero, Overflow, Comparison );
    /*            IO ports          */
    input   [31:0]          A;                  // the 1st input data
    input   [31:0]          B;                  // the 2nd input data
    input   [4:0]           Ctrl;               // function code
    output  [31:0]          C;                  // calculation tmp_result 
    output                  Zero;               // 0 indicator
    output                  Overflow;           // whether the operation is overflowed
    output                  Comparison ;        // Compare condition is setup 

    /*   internal registers and wires   */
    reg     [32:0]          tmp_arith ;         // result of arithmetic and logic operations
    reg                     tmp_cmp ;           // result of comparison operations
    
    //
    assign  C           = tmp_arith[31:0] ;
    assign  Zero        = (tmp_arith[31:0]==0) ? 1 : 0 ;
    assign  Overflow    = (tmp_arith[32] != tmp_arith[31]) ? 1 : 0 ;
    assign  Comparison  = tmp_cmp ;
    
    // Note: "default" clause must be used to prevent a latch be synthesized
    always  @( A, B, Ctrl )
        case (Ctrl)
            `ALU_ADDU   :   tmp_arith   <= A + B ;
            `ALU_ADD    :   tmp_arith   <= {A[31], A} + {B[31], B} ;
            `ALU_SUBU   :   tmp_arith   <= A - B ;
            `ALU_SUB    :   tmp_arith   <= {A[31], A} - {B[31], B} ;
            `ALU_AND    :   tmp_arith   <= A & B ;
            `ALU_OR     :   tmp_arith   <= A | B ;
            `ALU_NOR    :   tmp_arith   <= ~(A | B) ;
            `ALU_XOR    :   tmp_arith   <= A ^ B ;
            `ALU_SLTU   :   tmp_arith   <= (A < B) ? 1 : 0 ;
            //`ALU_SLT    :   tmp_arith   <= ({A[31], A} < {B[31], B}) ? 1 : 0 ;
            `ALU_SLT    :   tmp_arith   <= ({A[31],B[31]} == 2'b00) ? (A[30:0] < B[30:0]) :
                                           ({A[31],B[31]} == 2'b01) ? 0 :
                                           ({A[31],B[31]} == 2'b10) ? 1 :
                                                                      (A[30:0] < B[30:0]) ;
                                           
            default     :   tmp_arith   <= B ;
        endcase

    // Note: "default" clause must be used to prevent a latch be synthesized
    always  @( A, B, Ctrl )
        case (Ctrl)
            `ALU_LTZ    :   tmp_cmp <=   A[31] ? 1 : 0 ;                      // A[31] is sign bit
            `ALU_LEZ    :   tmp_cmp <= ( A[31] | A[31:0]==0) ? 1 : 0 ;
            `ALU_GTZ    :   tmp_cmp <= (~A[31] & |A[30:0]) ? 1 : 0 ;
            `ALU_GEZ    :   tmp_cmp <=  ~A[31] ? 1 : 0 ;
            `ALU_EQ     :   tmp_cmp <= (A == B) ? 1 : 0 ;
            `ALU_NEQ    :   tmp_cmp <= (A != B) ? 1 : 0 ;      
            default     :   tmp_cmp <= 0 ;    
        endcase
// 
//     ////////////////////  implementation code   /////////////////////
//     
//     /*
//      * ALU's funciton are all finished in one cycle and we assume its 
//      * read and write funcition are finished immediately
//      */
//     /*      0 indicator    */
//     assign Zero = (C == 0)? 1: 0;
//     
//     /* input that has no symbol */
//     assign DB_No_Symbol = B;
//     assign DA_No_Symbol = A;
//     
//     /* don't support overflow */
//     assign Overflow = 0;
// 
// 
// always@(A, B)
//    begin
//        DA_Symbol = A;
//        DB_Symbol = B;
//    end
// 
// 
// assign C = (Ctrl == 'b0000)?  DB_No_Symbol:
//                   (Ctrl == 'b0001)?  DA_No_Symbol + DB_No_Symbol   :
//                   (Ctrl == 'b0010)?  DA_Symbol + DB_Symbol :
//                   (Ctrl == 'b0011)?  DA_No_Symbol - DB_No_Symbol   :
//                   (Ctrl == 'b0100)?  DA_Symbol - DB_Symbol :
//                   (Ctrl == 'b0101)?  DA_No_Symbol   & DB_No_Symbol   :
//                   (Ctrl == 'b0110)?  DA_No_Symbol   | DB_No_Symbol   :
//                   (Ctrl == 'b0111)?~(DA_No_Symbol   | DB_No_Symbol)  :
//                   (Ctrl == 'b1000)?  DA_No_Symbol   ^ DB_No_Symbol   :
//                   (Ctrl == 'b1001)? (DA_No_Symbol   < DB_No_Symbol ? 1: 0):
//                   (Ctrl == 'b1010)? (DA_Symbol   < DB_Symbol? 1: 0):
//                     (Ctrl == 'b1011)? ((DA_Symbol  <= DB_Symbol)? 1:0):
//                     (Ctrl == 'b1100)? ((DA_Symbol  >  DB_Symbol)? 1:0):
//                     (Ctrl == 'b1101)? ((DA_Symbol  >= DB_Symbol)? 1:0):
//                     (Ctrl == 'b1110)? ((DA_No_Symbol  != DB_No_Symbol)? 1:0):
//                     (Ctrl == 'b1111)? ((DA_No_Symbol  == DB_No_Symbol)? 1:0):
//                   32'b0;

endmodule
