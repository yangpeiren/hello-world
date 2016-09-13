//////////////////////////////////////////////////////////////////////////////////
// This is part of COCOA project, which covers the 5 basic disciplines in computer
// science: digital logic, computer organization, compiler, computer architecture, 
// and operating system.
//
// Copy right belongs to SCSE of BUAA, 2011
// Author:          Gao Xiaopeng
// Design Name: 	Control unit
// Module Name:     ProcessorController 
// Description:     This module is the controller of data path of MIPS-C which 
//                  is consisted of 5 phase(6th phase is used to handle execption,
//                  which is not included in instruction execution normally).
// Note       :     1. Not support LB/SB well!
//                  2. BP not well(EPC maybe error too!)
//                  3. undefined instruction not well
//////////////////////////////////////////////////////////////////////////////////

//`include "cocoa_head.v"
`include "instruction_head.v"
`include "processor_head.v"

`timescale 1ns / 1ns

//
`define FIELD_OP        Instruction_I[31:26]            // operation code
`define FIELD_RS        Instruction_I[25:21]            // 1st source register
`define FIELD_RT        Instruction_I[20:16]            // 2nd source register
`define FIELD_RD        Instruction_I[15:11]            // destination register
`define FIELD_SHAMT     Instruction_I[10:6]             // number of bits to be shifted
`define FIELD_FUNCT6    Instruction_I[5:0]              // function code of R-type instructions
`define FIELD_FUNCT5    Instruction_I[4:0]              // function code of COP instructions(only ERET now)
`define FIELD_CONST     Instruction_I[15:0]             // constant
`define FIELD_ADDR      Instruction_I[15:0]             // address offset

`define OpHi3       Instruction_I[31:29]
`define OpLo3       Instruction_I[28:26]

module  ProcessorController(
    // word address
    MA_Low2_I,
    // instrcution
    Instruction_I, 
    //
    PC_Write_O, PC_Src_O,
    // memory interface
    MEM_Ack_I,  MEM_Req_O, MEM_RW_O, MEM_BE_O, MEM_IorD_O,
    // IR
    IR_Write_O,
    // Memory data extension
    ExtMemFunct_O,
    // register file
    RF_Write_O, RF_Dst_O, RF_Src_O,
    // sign-extend
    Ext_Func_O, 
    // ALU
    ALU_Zero_I, ALU_Overflow_I, ALU_Compare_I, ALU_SrcA_O, ALU_SrcB_O, ALU_Op_O, ALUOut_Src_O,
    // shifter
    SHT_Num_Src_O,
    // multiplier/divider
    MD_Flag_I, MD_MorD_O, MD_Sign_O, MD_Start_O, MD_HorL_O, 
    // CP0
    CP0_IM_I, CP0_IE_I, CP0_IorE_O, CP0_Idx_O, CP0_Wr_O, TLB_Func_O,TLB_Wr_O, MMU_Ack_I,
	 //MMU
	 MMU_Pr_Ack_I, TLB_Err_I,TLB_Fault_I, MMU_Req_O,
    // processor status
    PR_Exc_Enter, PR_ExcCode, 
    // hardware interrupts
    HW_Int_I, 
    // system signals
    Clk_I, Reset_I,
//GXP
    tst_mips_fsm  

  
    ) ;


    // 
    input   [1:0]               MA_Low2_I ;        // 
    // instruction
    input   [31:0]              Instruction_I ;     // instruction
    // PC
    output                      PC_Write_O ;
    output  [2:0]               PC_Src_O ;
    // memory interface
    input                       MEM_Ack_I ;         // memory access acknowledge from memory controller
    output                      MEM_RW_O ;          // memory access type ( 1 - R, 0 - W )
    output                      MEM_Req_O ;         // memory access request
    output  [3:0]               MEM_BE_O ;          // byte enables
    output                      MEM_IorD_O ;        // memory address source(for Mux1)
    // IR
    output                      IR_Write_O;         // instruction register write enable
    // Memory data extension
    output   [1:0]              ExtMemFunct_O ;     // 
    // register file
    output                      RF_Write_O ;        // RF write enable from processor controller
    output  [1:0]               RF_Dst_O ;		    // destination register selection(route to M2)
    output  [1:0]               RF_Src_O ;          // RF data write-back data source selection(route to M3)
    // Sign extension
    output  [1:0]               Ext_Func_O ;        // function code of sign-extend
    // ALU
    input                       ALU_Zero_I ;        
    input                       ALU_Overflow_I ;    
    input                       ALU_Compare_I ;     // 1 means the condition is satified
    output                      ALU_SrcA_O ;
    output  [1:0]               ALU_SrcB_O ;
    output  [4:0]               ALU_Op_O ;
    output  [2:0]               ALUOut_Src_O ;  
    // shifter
    output                      SHT_Num_Src_O ;  
    // multiplier/divider
    input                       MD_Flag_I ;
    output                      MD_MorD_O ;         // multiplier or divide select
    output                      MD_Start_O ;        // MUL_DIV start
    output                      MD_HorL_O;          // Hi or Low internal register select
    output                      MD_Sign_O ;         // Hi/Low register write enable
    //
    // CP0
    input   [7:0]               CP0_IM_I ;          // interrupt masks
    input                       CP0_IE_I ;          // global interrupt enable
	input						MMU_Ack_I;
    output  [1:0]               CP0_IorE_O ;        // control of MUX for PC or ALUoutput(write to CP0)
    output                      CP0_Idx_O ;         // control of MUX for index of registers 
    output                      CP0_Wr_O ;
    output  [5:0]               TLB_Func_O;
    output                      TLB_Wr_O;
    
	//MMU
	input          MMU_Pr_Ack_I;
	input						   TLB_Err_I;
	input	[2:0]				TLB_Fault_I;
	output  reg       MMU_Req_O;  
	
	
	
    // processor status
    output                      PR_Exc_Enter ;      // processor is entering an exception
    output  [6:2]               PR_ExcCode ;        // which exception processor is entering
    // hardware interrupt
    input   [7:2]               HW_Int_I ;          // hardware interrupt 
    //
    input                       Clk_I ;
    input                       Reset_I ;
//GXP
    output  [4:0]               tst_mips_fsm ;
        
    /* */
    reg [4:0]                   fsm_ctrl ;          // FSM
    parameter   S0_IDLE         = 0 ;               // initial state after reset
    parameter   S1_FETCH        = 1 ;               // fetech instruction
    parameter   S2_DCD_RF       = 2 ;               // decode and read RF
    
    parameter   S3_MA           = 3 ;               // calculate memory address for L/W instrcutions
    parameter   S3_EXE          = 4 ;               // instruction execution(ALU/Shifter/MulDiv)
    parameter   S3_BR           = 5 ;               // branch
    parameter   S3_JMP          = 6 ;               // jump
    parameter   S3_SC           = 7 ;               // exception: system call
    parameter   S3_BP           = 8 ;               // exception: breakpoint
    parameter   S3_UNDEF        = 9 ;               // exception: undefined instruction
    parameter   S3_MULDIV       = 16 ;
    parameter   S3_ERET         = 17 ;              // eret
    parameter   S3_TLB          = 20;
    
    parameter   S4_RDMEM        = 10 ;              // read memory
    parameter   S4_WRMEM        = 11 ;              // write memory
    parameter   S4_R_WB         = 12 ;              // R-type instruction execution over
    
    parameter   S5_M_WB         = 15 ;              // write back
//    parameter   FSM_ARITH       = 13 ;              // arithmetic instruction exception
//    parameter   FSM_INTR        = 14 ;              // external interrupt
    
    parameter   S6_HWINT        = 18 ;               // external hardware interrupt
    parameter   S6_MMU_EXC      = 19 ;

    //
    wire                        type_r ;            // flag : current instruction is register type
    wire                        type_imm ;          // flag : current instruction is immediate type
    wire                        type_j ;
    wire                        type_tlb;
    wire                        type_tlbw;
    //
    wire                        is_int ;            // flag : whether there is an interrupt asserted

    
    
    wire                        MMU_Exception;
  
  
  
  
  
  

//GXP
    assign  tst_mips_fsm =     fsm_ctrl ;
    
    
    
    
    
    /* decode */
    assign  type_r      =   (`FIELD_OP==`INST_R_TYPE) ;
    assign  type_imm    =   (`FIELD_OP==`INST_ADDI) |  
                            (`FIELD_OP==`INST_ADDIU)|
                            (`FIELD_OP==`INST_SLTI) | 
                            (`FIELD_OP==`INST_SLTIU)|
                            (`FIELD_OP==`INST_ANDI) |
                            (`FIELD_OP==`INST_ORI)  |
                            (`FIELD_OP==`INST_XORI) |
                            (`FIELD_OP==`INST_LUI) ;
    // arithmetic and logic
    assign  type_arilog =   type_imm | 
                            type_r & ((`FIELD_FUNCT6==`INST_SLL ) |
                                      (`FIELD_FUNCT6==`INST_SRL ) |
                                      (`FIELD_FUNCT6==`INST_SRA ) |
                                      (`FIELD_FUNCT6==`INST_SLLV) |
                                      (`FIELD_FUNCT6==`INST_SRLV) |
                                      (`FIELD_FUNCT6==`INST_SRAV) |
                                      (`FIELD_FUNCT6==`INST_ADD ) |
                                      (`FIELD_FUNCT6==`INST_ADDU) |
                                      (`FIELD_FUNCT6==`INST_SUB ) |
                                      (`FIELD_FUNCT6==`INST_SUBU) |
                                      (`FIELD_FUNCT6==`INST_AND ) |
                                      (`FIELD_FUNCT6==`INST_OR  ) |
                                      (`FIELD_FUNCT6==`INST_XOR ) |
                                      (`FIELD_FUNCT6==`INST_NOR ) |
                                      (`FIELD_FUNCT6==`INST_SLT ) |
                                      (`FIELD_FUNCT6==`INST_SLTU)) ;
    // jump
    assign  type_j      =   (`FIELD_OP==`INST_J)    | 
                            (`FIELD_OP==`INST_JAL)  |
                            (`FIELD_OP==`INST_R_TYPE)&(`FIELD_FUNCT6==`INST_JR) ;
    // branch
    assign  type_br     =   (`FIELD_OP==`INST_BEQ)          |
                            (`FIELD_OP==`INST_BNE)          |
                            (`FIELD_OP==`INST_BLEZ)         |
                            (`FIELD_OP==`INST_BGTZ)         |
                            (`FIELD_OP==`INST_BRANCH_RT) &
                                ((`FIELD_RT==`INST_BLTZ) | (`FIELD_RT==`INST_BGEZ)) ;
    // load and store
    assign  type_mem    =   (`FIELD_OP==`INST_LBU) |
                            (`FIELD_OP==`INST_LHU) |
                            (`FIELD_OP==`INST_LW ) |
                            (`FIELD_OP==`INST_SB ) |
                            (`FIELD_OP==`INST_SH ) |
                            (`FIELD_OP==`INST_SW ) ;
                            
    assign  type_tlb   =    (`FIELD_OP==`INST_CP_TYPE) & (`FIELD_RS==`INST_TLB_TYPE ) &
                            ( (`FIELD_FUNCT6==`INST_TLBR) | (`FIELD_FUNCT6==`INST_TLBWI) |
                              (`FIELD_FUNCT6==`INST_TLBWR) | (`FIELD_FUNCT6==`INST_TLBP) );
    assign  type_tlbw   =    (`FIELD_OP==`INST_CP_TYPE) & (`FIELD_RS==`INST_TLB_TYPE ) &
                            ( (`FIELD_FUNCT6==`INST_TLBWI) |(`FIELD_FUNCT6==`INST_TLBWR) );                          
    
    assign  TLB_Func_O = type_tlb ? `FIELD_FUNCT6 : 6'b0;
    /* Flag : whether interrupt assert */
    // Global interrupt enable asserted & at least one of 6 hardware interrtups asserted
    assign  is_int  =   CP0_IE_I &
                        ((HW_Int_I[7] & CP0_IM_I[7]) | 
                         (HW_Int_I[6] & CP0_IM_I[6]) |
                         (HW_Int_I[5] & CP0_IM_I[5]) |
                         (HW_Int_I[4] & CP0_IM_I[4]) |
                         (HW_Int_I[3] & CP0_IM_I[3]) |
                         (HW_Int_I[2] & CP0_IM_I[2])) ;

    /* PC */
    assign  PC_Write_O  =   (fsm_ctrl==S1_FETCH) & MEM_Ack_I    |           // pc <- pc + 4 (after fetch instruction)
                            (fsm_ctrl==S3_BR) & ALU_Compare_I   |           // branch instruction & comparation flag is set
                            (fsm_ctrl==S3_JMP)                  |           // jump instruction
                            (fsm_ctrl==S3_SC)                   |           // syscall instruction
                            (fsm_ctrl==S3_BP)                   |           // breakpoint instruction
                            (fsm_ctrl==S6_MMU_EXC)              |
                            (fsm_ctrl==S3_UNDEF)                |           // unrecognized instructions
                            (fsm_ctrl==S6_HWINT)                |           // external interrupt 
                            (fsm_ctrl==S3_ERET) ;                           // ERET instruction
    assign  PC_Src_O    = (fsm_ctrl==S1_FETCH)                                      ? 3'b000 :  // +4 : PC <- PC + 4(ALU)
                          (fsm_ctrl==S3_BR)                                         ? 3'b001 :  // branch : PC <- PC + offset(ALUOut)
                          (fsm_ctrl==S3_JMP) & 
                            ((`FIELD_OP==`INST_J)|(`FIELD_OP==`INST_JAL))           ? 3'b010 :  // J, JAL : PC <- {PC[31:28], IR[25:0], 2'b00}
                          (fsm_ctrl==S3_JMP) & 
                            (`FIELD_OP==`INST_R_TYPE) & (`FIELD_FUNCT6==`INST_JR)   ? 3'b011 :  // JR : PC <- R[rs]
                          ((fsm_ctrl==S3_SC) | (fsm_ctrl==S3_BP)| (fsm_ctrl==S6_MMU_EXC) |                              // syscall, break
                           (fsm_ctrl==S3_UNDEF) | (fsm_ctrl==S6_HWINT))             ? 3'b100 :  // unrecognized instructions, hardware interrupt
                                                                                      3'b101 ;
                                                                                        
    /* memory interface */
    assign  MEM_RW_O    =   ( (fsm_ctrl==S4_WRMEM) &                        // write memory
                            ~MMU_Req_O & ~MMU_Pr_Ack_I) ? 1'b0 : 1'b1 ;            // memory read/write# signal will be 0 only at S4_WRMEM stage
    assign  MEM_Req_O   =   ((fsm_ctrl==S1_FETCH) |                          // fetch instruction
                            (fsm_ctrl==S4_RDMEM) |                          // read memory
                            (fsm_ctrl==S4_WRMEM) )  &                        // write memory
                            ~MMU_Req_O & ~MMU_Pr_Ack_I ;
                            
                            
    assign  MEM_IorD_O  =   (fsm_ctrl==S1_FETCH) ? 0 : 1 ;                  // 0 : at fetch instrucion stage; 1 : all others stage
    assign  MEM_BE_O    =   (fsm_ctrl==S1_FETCH)  ?  4'b1111 :
									 ((`FIELD_OP==`INST_SB) | (`FIELD_OP==`INST_LBU) ) ? ((MA_Low2_I==2'b00) ? 4'b0001 :     // SB: A[1:0]=00
                                                     (MA_Low2_I==2'b01) ? 4'b0010 :     // SB: A[1:0]=01
                                                     (MA_Low2_I==2'b10) ? 4'b0100 :     // SB: A[1:0]=10
                                                                          4'b1000)  :   // SB: A[1:0]=11    
                            ((`FIELD_OP==`INST_SH) | (`FIELD_OP==`INST_LHU)) ? ((MA_Low2_I[1]==0)  ? 4'b0011 :     // SH: A1==0
                                                                         4'b1100)  :   // SH: A1==1																							  				 
																								  
                                                                          4'b1111 ;     // SW
    
    /* IR */
    assign  IR_Write_O  =   ((fsm_ctrl==S1_FETCH) && MEM_Ack_I) ;                          // IR only be written at FETCH stage
    
    /* memory data extension */
    assign  ExtMemFunct_O = (`FIELD_OP==`INST_LBU) ? `EXT_M_8BIT :
                            (`FIELD_OP==`INST_LHU) ? `EXT_M_16BIT :
                            (`FIELD_OP==`INST_SB)  ? `EXT_M_8BIT :
                            (`FIELD_OP==`INST_SH)  ? `EXT_M_16BIT :
                                                     `EXT_M_NOP ;    
    /* RF */
    assign  RF_Write_O  =   (fsm_ctrl==S4_R_WB) |                           // r-type instructions
                            (fsm_ctrl==S5_M_WB) |                           // load instructions
                            (fsm_ctrl==S3_JMP)&(`FIELD_OP==`INST_JAL) ;     // R-type or LOAD 
    assign  RF_Src_O    =   (fsm_ctrl==S5_M_WB) ? 2'b01 :                   // LOAD
                            (fsm_ctrl==S3_JMP)  ? 2'b10 :                   // JAL(write R[31])
                                                  2'b00 ;                   // R-type
    assign  RF_Dst_O    =   type_r                  ? 2'b01 :               // R-type - [15:11] 
                            (`FIELD_OP==`INST_JAL)  ? 2'b10 :               // JAL - 5'd31
                                                      2'b00 ;               // others - [20:16]

    /* sign-extend */
    assign  Ext_Func_O  =   (`FIELD_OP==`INST_LUI)      ? `EXT_SL16 :
                            ((`FIELD_OP==`INST_ANDI) |
                             (`FIELD_OP==`INST_ORI)  |
                             (`FIELD_OP==`INST_XORI))   ? `EXT_ZERO : 
                                                          `EXT_SIGN ;
    
    /* ALU */
    assign  ALU_SrcA_O  =   ((fsm_ctrl==S3_EXE)| 
                             (fsm_ctrl==S3_MA) | 
                             (fsm_ctrl==S4_RDMEM) |
                             (fsm_ctrl==S4_WRMEM) |
                             (fsm_ctrl==S3_BR)) ? 1 : 0 ;               // A will be used with R-type instruction and 
                                                                        // L/S instructions to calculate memory address
    assign  ALU_SrcB_O  =   (fsm_ctrl==S1_FETCH)            ? 2'b01 :   // FETCH: PC <- PC + 4
                            (fsm_ctrl==S2_DCD_RF)           ? 2'b11 :   // DECODE: PC <- PC + (sign_extend(IR[15:0]) << 2)
                            (fsm_ctrl==S3_MA)               ? 2'b10 :
                            (fsm_ctrl==S4_RDMEM)            ? 2'b10 :   // LW/ST : MemAddr <- PC + sign_extend(IR[15:0])
                            (fsm_ctrl==S4_WRMEM)            ? 2'b10 :
                            (fsm_ctrl==S3_EXE) & type_imm   ? 2'b10 :   // immediate type
                            (fsm_ctrl==S3_SC)               ? 2'b01 :   // syscall : pc <- pc - 4                 
                            (fsm_ctrl==S3_BP)               ? 2'b01 :   // breakpoint : pc <- pc - 4   
                            (fsm_ctrl==S6_MMU_EXC)          ? 2'b01 :           
                            (fsm_ctrl==S3_UNDEF)            ? 2'b01 :   // unrecognized instruction : pc <- pc - 4
                                                              2'b00 ;
    assign  ALU_Op_O =  (fsm_ctrl==S1_FETCH)            ?   `ALU_ADD :  // pc <- pc + 4
                        (fsm_ctrl==S3_MA)               ?   `ALU_ADD :
                        (fsm_ctrl==S2_DCD_RF)           ?   `ALU_ADD :  // ALUOut <- pc + (sign-extend(IR[15:0]) << 2)
                        (fsm_ctrl==S3_SC)               ?   `ALU_SUBU:  // syscall : pc <- pc - 4
                        (fsm_ctrl==S3_BP)               ?   `ALU_SUBU:  // breakpoint : pc <- pc - 4
                        (fsm_ctrl==S6_MMU_EXC)          ?   `ALU_SUBU:
                        (fsm_ctrl==S3_UNDEF)            ?   `ALU_SUBU:  // unrecognized instruction : pc <- pc - 4
                        // R type                
                        (`FIELD_OP==`INST_R_TYPE)       ? 
                            (`FIELD_FUNCT6==`INST_ADD)  ?   `ALU_ADD :  // add
                            (`FIELD_FUNCT6==`INST_ADDU) ?   `ALU_ADDU:  // addu
                            (`FIELD_FUNCT6==`INST_AND)  ?   `ALU_AND :  // and
                            (`FIELD_FUNCT6==`INST_NOR)  ?   `ALU_NOR :  // nor
                            (`FIELD_FUNCT6==`INST_OR)   ?   `ALU_OR  :  // or
                            (`FIELD_FUNCT6==`INST_SUB)  ?   `ALU_SUB :  // sub
                            (`FIELD_FUNCT6==`INST_SUBU) ?   `ALU_SUBU:  // subu
                            (`FIELD_FUNCT6==`INST_XOR)  ?   `ALU_XOR :  // xor
                            (`FIELD_FUNCT6==`INST_SLT)  ?   `ALU_SLT :  // slt
                            (`FIELD_FUNCT6==`INST_SLTU) ?   `ALU_SLTU:  // sltu
                            (`FIELD_FUNCT6==`INST_SLL ) ?   `ALU_SLL :  // sll
                            (`FIELD_FUNCT6==`INST_SRL ) ?   `ALU_SRL :  // srl
                            (`FIELD_FUNCT6==`INST_SRA ) ?   `ALU_SRA :  // sra
                            (`FIELD_FUNCT6==`INST_SLLV) ?   `ALU_SLL :  // sllv
                            (`FIELD_FUNCT6==`INST_SRLV) ?   `ALU_SRL :  // srlv
                            (`FIELD_FUNCT6==`INST_SRAV) ?   `ALU_SRA :  // srav
                                                            `ALU_ADD :  // other R-type instructions which should be generated
                        // immediate
                        (`FIELD_OP==`INST_ADDI)         ?   `ALU_ADD :  // addi                               
                        (`FIELD_OP==`INST_ADDIU)        ?   `ALU_ADDU:  // addiu                              
                        (`FIELD_OP==`INST_ANDI)         ?   `ALU_AND :  // andi                               
                        (`FIELD_OP==`INST_ORI)          ?   `ALU_OR  :  // ori                                
                        (`FIELD_OP==`INST_XORI)         ?   `ALU_XOR :  // xori                               
                        (`FIELD_OP==`INST_SLTI)         ?   `ALU_SLT :  // slti                               
                        (`FIELD_OP==`INST_SLTIU)        ?   `ALU_SLTU:  // sltiu   
                        (`FIELD_OP==`INST_LUI)          ?   `ALU_NOP :  // lui   
                        // branch                                                                 
                        (`FIELD_OP==`INST_BEQ)          ?   `ALU_EQ  :  // beq                                
                        (`FIELD_OP==`INST_BGTZ)         ?   `ALU_GTZ :  // bgtz                               
                        (`FIELD_OP==`INST_BLEZ)         ?   `ALU_LEZ :  // blez                               
                        (`FIELD_OP==`INST_BNE)          ?   `ALU_NEQ :  // bne    
                        (`FIELD_OP==`INST_BRANCH_RT)    ?
                            (`FIELD_RT==`INST_BGEZ)     ?   `ALU_GEZ :  // bgez
                            (`FIELD_RT==`INST_BLTZ)     ?   `ALU_LTZ :  // bltz
                                                            `ALU_ADD :  // other branch instructions which should be generated
                                                            `ALU_NOP ;  // other instructions include lbu, lhu, lw, sb, sh and sw
    
    assign  ALUOut_Src_O    =   (fsm_ctrl==S3_EXE) & (`FIELD_OP==`INST_R_TYPE) & ((`FIELD_FUNCT6==`INST_SLL ) |
                                                                                  (`FIELD_FUNCT6==`INST_SRL ) |
                                                                                  (`FIELD_FUNCT6==`INST_SRA ) |
                                                                                  (`FIELD_FUNCT6==`INST_SLLV) |
                                                                                  (`FIELD_FUNCT6==`INST_SRLV) |
                                                                                  (`FIELD_FUNCT6==`INST_SRAV))  ? 3'd1 :
                                (fsm_ctrl==S3_EXE) & (`FIELD_OP==`INST_R_TYPE) & ((`FIELD_FUNCT6==`INST_MFHI) |
                                                                                  //(`FIELD_FUNCT6==`INST_MTHI) |
                                                                                  (`FIELD_FUNCT6==`INST_MFLO))  ? 3'd2 :
                                                                                  //(`FIELD_FUNCT6==`INST_MTLO))  ? 3'd2 :
                                (fsm_ctrl==S3_EXE) & (`FIELD_OP==`INST_LUI)                                     ? 3'd3 :
                                (fsm_ctrl==S3_EXE) & (`FIELD_OP==`INST_CP_TYPE) & (`FIELD_RS==`INST_MFC0)       ? 3'd4 :
                                                                                                                  3'd0 ;
    assign  SHT_Num_Src_O   =   (`FIELD_FUNCT6==`INST_SLL)  ? 1 :       // sll
                                (`FIELD_FUNCT6==`INST_SRL)  ? 1 :       // srl 
                                (`FIELD_FUNCT6==`INST_SRA)  ? 1 :       // sra
                                                              0 ;       // other 3 shift instructions with 
                                                                        // variable numbers to shift
    
      /* muliplier/divider */
    assign  MD_MorD_O   = ((`FIELD_FUNCT6==`INST_MULT) | (`FIELD_FUNCT6==`INST_MULTU)) ? 0 : 1 ;
    assign  MD_Start_O  = (fsm_ctrl==S3_MULDIV) ;
    assign  MD_HorL_O   = (`FIELD_FUNCT6==`INST_MFHI) ? 1 : 0 ;
    assign  MD_Sign_O   = (`FIELD_FUNCT6==`INST_MULT) | (`FIELD_FUNCT6==`INST_DIV) ;

    /* Exception flags to CP0 in datapath */
//    assign  CP0_IorE_O  = (fsm_ctrl==S6_HWINT) ? 1 : 0 ;
    assign  CP0_IorE_O  = (fsm_ctrl==S3_SC)     ? 1 :                   // system call & break instruction & 
                          (fsm_ctrl==S3_BP)     ? 1 :                   // undefined instruction should save 
                          (fsm_ctrl==S6_MMU_EXC)? 1 :   
                          (fsm_ctrl==S3_UNDEF)  ? 1 :                   // (PC-4) to EPC
                          (fsm_ctrl==S6_HWINT)  ? 2 :                   // hardware interrupt : save PC(NPC) to EPC
                                                  0 ;
    assign  CP0_Idx_O   = ((fsm_ctrl==S3_SC) | (fsm_ctrl==S6_MMU_EXC) | (fsm_ctrl==S3_UNDEF) | (fsm_ctrl==S3_BP) | (fsm_ctrl==S3_TLB) | (fsm_ctrl==S6_HWINT)) ? 0 : 1 ;
    assign  CP0_Wr_O    = (fsm_ctrl==S3_SC) | (fsm_ctrl==S3_UNDEF) | (fsm_ctrl==S3_BP) | (fsm_ctrl==S6_HWINT) |  
                          ((fsm_ctrl==S4_R_WB) & (`FIELD_OP==`INST_CP_TYPE) & (`FIELD_RS==`INST_MTC0)) ;
                          
    assign  TLB_Wr_O   =  ( (fsm_ctrl==S3_TLB) & ((`FIELD_FUNCT6==`INST_TLBWI)|(`FIELD_FUNCT6==`INST_TLBWR)) );                       
                           
                          

    /* Processor status */
    assign  PR_Exc_Enter = (fsm_ctrl==S6_HWINT) | (fsm_ctrl==S3_SC) | 
                           (fsm_ctrl==S3_BP) | (fsm_ctrl==S3_UNDEF) |
                            (fsm_ctrl==S6_MMU_EXC);
    assign  PR_ExcCode   = (fsm_ctrl==S6_HWINT) 						   ? `EXCCODE_INT  :       // hardware interrupt
						               (TLB_Fault_I[1]== 1'b1 | TLB_Fault_I[0]== 1'b1) ? `EXCCODE_TLBL :// TLB not translated
						               (TLB_Fault_I[2]== 1'b1) 						 ? `EXCCODE_TLBS :// TLB unmatch
						               (TLB_Err_I== 1'b1)    						   ? `EXCCODE_AdEL :// In usermode,access to high 2G space
                           (fsm_ctrl==S3_SC)     						   ? `EXCCODE_SC   :// system call
                           (fsm_ctrl==S3_BP)     						   ? `EXCCODE_BP   :// break instruction
                           (fsm_ctrl==S3_UNDEF)  						   ? `EXCCODE_RI   :// undefined instruction
                                                   						   `EXCCODE_RSV ;
                                                   						   
                                                   						   
     assign  MMU_Exception = TLB_Err_I | (TLB_Fault_I!=0) ;                                              						   
                                                   						   
                                                   						   
    /* state machine */
    always  @( posedge Clk_I or posedge Reset_I )
        if ( Reset_I )
            begin fsm_ctrl <= S0_IDLE ; MMU_Req_O<=0; end
        else
            case (fsm_ctrl)
                S0_IDLE     : begin  fsm_ctrl <= S1_FETCH ; MMU_Req_O<=1; end
                S1_FETCH    :   
                  begin
                    MMU_Req_O<=0;
                    if ( MEM_Ack_I )  fsm_ctrl <= S2_DCD_RF ;
                    if (MMU_Pr_Ack_I & MMU_Exception)  fsm_ctrl <= S6_MMU_EXC;
                  end
                S2_DCD_RF   :   if ( type_arilog ) 
                                    fsm_ctrl <= S3_EXE ;
                                else if ( type_j )
                                    fsm_ctrl <= S3_JMP ;
                                else if ( type_br )
                                    fsm_ctrl <= S3_BR ;
                                else if ( type_mem )
                                    fsm_ctrl <= S3_MA ;
                                else if (type_tlb)
                                    fsm_ctrl <= S3_TLB;
                                else if ( `FIELD_OP==`INST_R_TYPE )
                                    case (`FIELD_FUNCT6)
                                        `INST_MFHI,                                     // mfhi or mflo
                                        `INST_MFLO      : fsm_ctrl <= S3_EXE ;          
                                        `INST_BREAK     : fsm_ctrl <= S3_BP ;
                                        `INST_SYSCALL   : fsm_ctrl <= S3_SC ;
                                        `INST_MULT      , 
                                        `INST_MULTU     , 
                                        `INST_DIV       , 
                                        `INST_DIVU      : fsm_ctrl <= S3_MULDIV ;
                                        default         : fsm_ctrl <= S3_UNDEF ;
                                    endcase
                                else if ( `FIELD_OP==`INST_CP_TYPE )
                                    case ( `FIELD_RS )
                                        `INST_MFC0      ,
                                        `INST_MTC0      : fsm_ctrl <= S3_EXE ;
                                        `INST_RET_TYPE  : if (`FIELD_FUNCT5==`INST_ERET) fsm_ctrl <= S3_ERET ;
                                        default         : fsm_ctrl <= S3_UNDEF ;
                                    endcase 
                                else fsm_ctrl <= S3_UNDEF ;
                S3_MA       :   case ( `FIELD_OP )
                                    `INST_SB    ,
                                    `INST_SH    ,
                                    `INST_SW    :   begin fsm_ctrl <= S4_WRMEM ; MMU_Req_O<=1; end
                                    default     :   begin fsm_ctrl <= S4_RDMEM ; MMU_Req_O<=1; end
                                endcase
                S3_EXE      :   fsm_ctrl <= S4_R_WB ;
                S3_MULDIV   :   if (MD_Flag_I) begin fsm_ctrl <= S1_FETCH ; MMU_Req_O<=1; end
                S3_BR       , 
                S3_JMP      :   if ( is_int )                                   // if external hardware interrupt asserted and IE be set
                                    fsm_ctrl <= S6_HWINT ;
                                else
                                    begin fsm_ctrl <= S1_FETCH ; MMU_Req_O<=1; end
                S3_SC       ,
                S3_BP       ,
                S3_ERET     ,
                S3_UNDEF    :  begin fsm_ctrl <= S1_FETCH ; MMU_Req_O<=1; end
				        S3_TLB      :	  if ( MMU_Ack_I | type_tlbw)
									                   begin fsm_ctrl <= S1_FETCH ; MMU_Req_O<=1; end
                S4_R_WB     :  begin  fsm_ctrl <= S1_FETCH ; MMU_Req_O<=1; end
                S4_RDMEM    :  
                  begin
                     MMU_Req_O<=0;
                     if ( MEM_Ack_I )  fsm_ctrl <= S5_M_WB ;    //fsm_ctrl <= S5_M_WB ;
                     if (MMU_Pr_Ack_I & MMU_Exception)  fsm_ctrl <= S6_MMU_EXC;
                  end
                
                S4_WRMEM    :   
                  begin
                    MMU_Req_O<=0;
                    if (MEM_Ack_I ) 	 begin fsm_ctrl <= S1_FETCH ;	 MMU_Req_O<=1; end  // wait for FLASH/SRAM/DDR2
                    if (MMU_Pr_Ack_I & MMU_Exception)  fsm_ctrl <= S6_MMU_EXC;
                  end
                                    
                // interrupt support here firstly
                S5_M_WB     :   if ( is_int )     // if external hardware interrupt asserted and IE be set
                                    fsm_ctrl <= S6_HWINT ;
                                else
                                    begin fsm_ctrl <= S1_FETCH ; MMU_Req_O<=1; end
                S6_HWINT    :   begin fsm_ctrl <= S1_FETCH ; MMU_Req_O<=1; end
                S6_MMU_EXC  :   begin fsm_ctrl <= S1_FETCH ; MMU_Req_O<=1; end
                default     :   fsm_ctrl <= S0_IDLE ;
            endcase      

endmodule

