//////////////////////////////////////////////////////////////////////////////////
// This is part of COCOA project, which covers the 5 basic disciplines in computer
// science: digital logic, computer organization, compiler, computer architecture, 
// and operating system.
//
// Copy right belongs to SCSE of BUAA, 2011
// Author:          Gao Xiaopeng
// Design Name: 	Vector generator for exceptions
// Module Name:     ExcVector 
// Description:     This module is used to generate entry vector for processor
//                  when exceptions occury.
//////////////////////////////////////////////////////////////////////////////////

`include "processor_head.v"
`timescale 1ns / 1ns

module  ExcVector( BEV_I, PR_ExcCode_I, Vector_O ) ;
    input                   BEV_I ;
    input   [6:2]           PR_ExcCode_I ;
    output  [31:0]          Vector_O ;

    wire    [31:0]          vec_tlb, vec_int, vec_exc ;
    
    assign  vec_tlb = BEV_I ? `VECTOR_TLB_BEV_1 : `VECTOR_TLB_BEV_0 ;
	assign  vec_int = BEV_I ? `VECTOR_INT_BEV_1 : `VECTOR_INT_BEV_0 ;
    assign  vec_exc = BEV_I ? `VECTOR_EXC_BEV_1 : `VECTOR_EXC_BEV_0 ;
    
//    assign  Vector_O    = (PR_ExcCode_I==`EXCCODE_INT) ? (BEV_I ? `VECTOR_INT_BEV_1 : `VECTOR_INT_BEV_0) :
//                          ((PR_ExcCode_I==`EXCCODE_SC) |
//                           (PR_ExcCode_I==`EXCCODE_BP))? (BEV_I ? `VECTOR_EXC_BEV_1 : `VECTOR_EXC_BEV_0) :
//                                                         `VECTOR_REBOOT ;
    assign  Vector_O    = (PR_ExcCode_I==`EXCCODE_INT)  ? vec_int :             // hardware interrupt
						  (PR_ExcCode_I==`EXCCODE_TLBL) ? vec_tlb :				// TLB not translated
						  (PR_ExcCode_I==`EXCCODE_TLBS) ? vec_tlb :				// TLB unmatch
						  (PR_ExcCode_I==`EXCCODE_AdEL) ? vec_tlb :				// In usermode,access to high 2G space
                          (PR_ExcCode_I==`EXCCODE_SC)   ? vec_exc :             // system call
                          (PR_ExcCode_I==`EXCCODE_BP)   ? vec_exc :             // break 
                          (PR_ExcCode_I==`EXCCODE_RI)   ? vec_exc :             // undefined instruction
                                                          `VECTOR_REBOOT ;      // reserved
endmodule
