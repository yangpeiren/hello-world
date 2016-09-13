//////////////////////////////////////////////////////////////////////////////////
// This is part of COCOA project, which covers the 5 basic disciplines in computer
// science: digital logic, computer organization, compiler, computer architecture, 
// and operating system.
//
// Copy right belongs to SCSE of BUAA, 2011
// Author:          Gao Xiaopeng
// Design Name: 	Data path
// Module Name:     DataPath 
// Description:     This module is the data path of MIPS-C which is described 
//                  briefly as below.
//    S0          S1              S2                   S3                  S4         S5
//    PC | Memory Interface | IR          | Register File     | A/B   | ALU       | ALUOut
//                          | Data Latch  | Sign Ext - Shift Left 2   | Shifter   | CP0
//                                                                    | Mul/Div 
//////////////////////////////////////////////////////////////////////////////////

`include "processor_head.v"
`timescale  1ns/1ns

module DataPath(   
    // MIPS-core Controller
    MA_Low2_O,
    // PC
	PCWrite_I, PCSrc_I, 
    // memory interface
    IorD, Addr_O, Data_I, Data_O,
    // IR
    IRWrite_I, IR,
    // Memory data extension
    ExtMemFunct_I, 
    // Register file
    RegWrite_I, RegDst_I, RegSrc_I,
    // Sign extension
    ExtImmFunct_I, 
    // ALU
    ALUSrcA_I, ALUSrcB_I, ALUOp_I, ALUOutSrc_I, ALU_Zero, ALU_Overflow, ALU_Compare,
    // Shifter
    SHTNumSrc_I, 
    // Multiplier and Divider
    MorD_I, MDStart_I, HorL_I, MD_Sign_I, MDFlag_O, 
    // CP0
    CP0_IorE_I, CP0_Idx_I, CP0Wr_I, CP0_IM_O, CP0_IE_O, TLB_Func_I, TLB_Wr_I,  MMU_Ack_O,
	//MMU
	RW_En_I,Pr_Req_I,MMU_Pr_Ack_O, TLB_Err_O,TLB_Fault_O,
    // processor status
    PR_Exc_Enter_I, PR_ExcCode_I, 
    // interrupt 
    HW_Int_I, 
    // System
    Clk, Reset) ;
    // MIPS-core controller
    output  [1:0]               MA_Low2_O ;         // lowest 2 addresses
    // PC
	input 					    PCWrite_I ;         // PC write enable from processor controller
	input   [2:0]               PCSrc_I ;
    // memory interface
    input                       IorD ;              // 0: PC as memory address; 1: ALU output as memory address. route to M1
    output  [31:0]              Addr_O ;            // memory address
    input   [31:0]              Data_I ;            // data read from memory
    output  [31:0]              Data_O ;            // data write to memory
    // instruction register
    input						IRWrite_I ;			// IR write enable from processor controller
    output  [31:0]              IR ;                // instruction register
    // 
    input   [1:0]               ExtMemFunct_I ;     // 
    // register file
    input						RegWrite_I ;        // RF write enable from processor controller
    input	[1:0]               RegDst_I ;		    // destination register selection(route to M2)
    input	[1:0]			    RegSrc_I ;          // RF data write-back data source selection(route to M3)
    // Sign extension
    input   [1:0]               ExtImmFunct_I ;     // 
    // ALU
    input                       ALUSrcA_I ;
    input   [1:0]               ALUSrcB_I ;
    input   [4:0]               ALUOp_I ;
    input   [2:0]               ALUOutSrc_I ;
    output                      ALU_Zero ;
    output                      ALU_Overflow ;
    output                      ALU_Compare ;
    // shifter
    input                       SHTNumSrc_I ;
    // MUl/DIV
    input                       MorD_I ;            // multiplier or divide select
    input                       MDStart_I ;         // MUL_DIV start
    input                       HorL_I;             // Hi or Low internal register select
    input                       MD_Sign_I ;         // signed or unsigned
    output                      MDFlag_O ;          // MUL_DIV status
    // CP0
    input   [1:0]               CP0_IorE_I ;        // PC or ALUoutput write to CP0
    input                       CP0_Idx_I ;         // 0Eh or IR[15:11]
    input                       CP0Wr_I ;           // CP0 registers write enable
    output  [7:0]               CP0_IM_O ;          // interrupt masks
    output                      CP0_IE_O ;          // global interrupt enable
	input   [5:0]               TLB_Func_I ;
	 input                        TLB_Wr_I;        
    output                      MMU_Ack_O ;
    // processor status
    input                       PR_Exc_Enter_I ;    // processor is entering an exception
    input   [6:2]               PR_ExcCode_I ;      // which exception processor is entering
    // hardware interrupt
    input   [7:2]               HW_Int_I ;          // hardware interrupt 
    // system interface
    input                       Clk ;
    input                       Reset ;
        
        
        
        
    // PC
    wire    [31:0]              pc, next_pc /* synthesis syn_keep=1 */;
    // IR
    wire    [31:0]              wIR ;
    // temporary register for data read/write from/to memory
    wire    [31:0]              data_temp ;
    wire    [31:0]              datatmp2rf ;
    wire    [31:0]              breg2mem ;
    // sign extension
    wire    [31:0]              sign_ext, sign_ext_sl2 ;
    // Register file
    wire    [4:0]               rs1, rs2, rd ;
    wire    [31:0]              rf_data_i, rf_rs1, rf_rs2 ;
    wire    [31:0]              a_reg, b_reg ;
    // ALU, shifter, multiplier/divider
    wire    [31:0]              alu_da, alu_db, alu_dc, alu_out_di, alu_out /* synthesis syn_keep=1 */;
    wire    [31:0]              shifter ;
    wire    [4:0]               shift_num ;
    wire    [31:0]              mul_div ;
    // CP0
    wire    [31:0]              cp0_din ;                   // CP0 input
    wire    [31:0]              cp0_dout ;                  // CP0 output
    wire                        cp0_bev ;
    wire    [31:0]              cp0_epc ;                   // CP0 EPC output
    wire    [4:0]               cp0_idx ;                   // CP0 destination register index
    //
    wire    [31:0]              exc_vector ;                // entry vector of exception
    
    
    /* MMU & CP0 */ 
	input 			RW_En_I;
	input			Pr_Req_I;
	output  MMU_Pr_Ack_O;
	output			TLB_Err_O;
	output[2:0]		TLB_Fault_O;
	
	
	wire			MMU_Req ;
	wire[31:0]		MMU_BadVAddr,MMU_EntryHi,MMU_EntryLo1,MMU_EntryLo0,MMU_PageMask;
	wire[2:0]		MMU_Index;
	wire			MMU_AckP,MMU_AckR,MMU_Matched;
	wire[31:0]		CP0_EntryHi,CP0_PageMask,CP0_EntryLo1,CP0_EntryLo0;
	wire[1:0]		CP0_MMU_Func;
	wire [2:0]		CP0_MMU_Index;
	wire			CP0_RdReq;
	wire   CP0_WrReq;
	wire 			CP0_MMU_Mode;// '0' = UserMode , '1' = Kernel Mode
	wire [7:0]		CP0_ASID;
	wire			TLB_Err;
	wire [2:0]		TLB_Fault;
	wire [31:0]		MMU_Pr_RAddr;
	wire [31:0]		Pr_VAddr_t;
	reg  [31:0]		Pr_VAddr;
    /* MIPS-core controller */
                  // 
    
    /* PC */
    PC  U_PC( Clk, Reset, next_pc, PCWrite_I, pc ) ;
	 
	 

    /* Memory */
//    assign  Data_O = b_reg ;                                // data output to memory come from temporary register B
    assign  Data_O = breg2mem ;                                // data output to memory come from extension of temporary register B
	 
    ExtBReg2Mem U_ExtBReg( b_reg, ExtMemFunct_I, alu_out[1:0], breg2mem ) ;
    
	 
	 
    /* IR and Data temporary register */
	 
    IR  U_IR( Clk, Reset, Data_I, IRWrite_I, wIR) ;         //
    Register    U_DataTemp( Data_I, data_temp, Clk ) ;
        defparam U_DataTemp.WIDTH_REG=32 ;
    ExtMemData  U_ExtMemData( data_temp, ExtMemFunct_I, alu_out[1:0], datatmp2rf ) ;
	 
    assign  IR = wIR ;                                      // output IR to controller
	 
	 
	 
    /* Register file */
    RegFile	U_RF( Clk, rs1, rs2, rd, RegWrite_I, rf_rs1, rf_rs2, rf_data_i ) ;
	 
    // RS1, RS2, RD
    assign  rs1 = wIR[25:21] ;
    assign  rs2 = wIR[20:16] ;
    
	 
	 
	 
    /* A & B */
    Register    U_A( rf_rs1, a_reg, Clk ) ;
        defparam    U_A.WIDTH_REG = 32 ;
    Register    U_B( rf_rs2, b_reg, Clk ) ;
        defparam    U_B.WIDTH_REG = 32 ;
    
	 
    /* Sign extension and shift left 2bits */
    Ext		U_SIGN_EXT(	ExtImmFunct_I, wIR[15:0], sign_ext );
	 
    assign  sign_ext_sl2 = {sign_ext[29:0], 2'b0} ;
	 
	 


    /* ALU */
    Alu		U_ALU( alu_da, alu_db, ALUOp_I, alu_dc, ALU_Zero, ALU_Overflow, ALU_Compare ) ;
    
	 
	 
    /* Shifter */
    Shifter	U_SHIFT( b_reg, shift_num, ALUOp_I, shifter ) ;
    
	 
	 
    /* Multiplier and Divider */
    Mul		U_MUL_DIV( MDFlag_O, Reset, a_reg, b_reg, Clk, mul_div, HorL_I, MDStart_I, MorD_I, MD_Sign_I) ;
    
	 
	 
    /* ALU output register */
    Register    U_ALUOUT( alu_out_di, alu_out, Clk ) ;
        defparam    U_ALUOUT.WIDTH_REG = 32 ;
    
	 
	 
	 
	 
    /* CP0 */
/*  Why ZhaiYan uses the output of ALU as the input of CP0 not ALUOutput?    
    I think ALU's output could be used as CP0's input since the other 2 inputs of 
    MUX7 could not be used as EPC. But be careful of CP0's write timing!
 */
 
    /*CP0     U_CP0( cp0_idx, cp0_din, cp0_dout, CP0Wr_I,
                   PR_Exc_Enter_I, PR_ExcCode_I, 
                   cp0_bev, CP0_IM_O, CP0_IE_O, cp0_epc,                                  //
                   HW_Int_I, 
                   Clk, Reset ) ;*/
    CP0 	U_CP0(cp0_idx, cp0_din, cp0_dout, CP0Wr_I,
					PR_Exc_Enter_I, PR_ExcCode_I,
					TLB_Func_I, TLB_Wr_I,  MMU_Req,
					MMU_BadVAddr,MMU_EntryHi,MMU_EntryLo1,MMU_EntryLo0,MMU_PageMask,MMU_Index,
					MMU_AckP,MMU_AckR,MMU_Matched,
					CP0_EntryHi,CP0_PageMask,CP0_EntryLo1,CP0_EntryLo0,CP0_MMU_Func,
					CP0_MMU_Index,CP0_MMU_Mode,CP0_RdReq, CP0_WrReq , CP0_ASID,
					cp0_bev, CP0_IM_O, CP0_IE_O, cp0_epc, 
					HW_Int_I,
					Clk, Reset );
    ExcVector U_EV( cp0_bev, PR_ExcCode_I, exc_vector ) ;
	 
 
	/* MMU */ 
	// assign CP0_MMU_Mode = 1;
	MMU 	U_MMU(Clk,Reset,RW_En_I,CP0_EntryHi,CP0_PageMask,CP0_EntryLo0,CP0_EntryLo1,
					CP0_MMU_Func,CP0_MMU_Index,CP0_MMU_Mode,
					Pr_VAddr,
					CP0_ASID,
					Pr_Req_I,
					CP0_RdReq, CP0_WrReq, 
					MMU_Pr_RAddr,TLB_Err_O,TLB_Fault_O,
					MMU_Req,MMU_BadVAddr,MMU_EntryHi,
					MMU_EntryLo0,MMU_EntryLo1,MMU_PageMask,
					MMU_Pr_Ack_O,
					MMU_Index,
					MMU_AckP,MMU_AckR,MMU_Matched);
	 
    assign  MMU_Ack_O = MMU_AckP|MMU_AckR;
	  assign	Addr_O = {MMU_Pr_RAddr[31:2], MA_Low2_O} ;  
    /* MUX */
	 
	 
	 always @(negedge Clk)
	 if (Reset) Pr_VAddr<=0;
	  else if (Pr_Req_I) Pr_VAddr<=Pr_VAddr_t;
	 
	  assign  MA_Low2_O = Pr_VAddr[1:0] ;       
	 
	 
    // M1 : memory address {PC, ALU output}
    assign      Pr_VAddr_t[1:0] = IorD ?  alu_out[1:0] : 2'b00 ;
    Mux2to1     U_M1( pc[31:2], alu_out[31:2], IorD, Pr_VAddr_t[31:2]) ;
        defparam U_M1.WIDTH_DATA=30 ;   
		  
    // M2 : index of destination register
// GXP    Mux2to1_5   U_M2( wIR[20:16], wIR[15:11], RegDst_I, rd ) ;

    Mux3to1     U_M2( wIR[20:16], wIR[15:11], 5'd31, RegDst_I, rd ) ;
        defparam U_M2.WIDTH_DATA=5 ;        
    // M3 : select which data to be written to RF
// GXP    Mux3to1_32  U_M3( alu_out, datatmp2rf, CP0, RegSrc_I, rf_data_i ) ;

    Mux3to1     U_M3( alu_out, datatmp2rf, pc, RegSrc_I, rf_data_i ) ;
        defparam U_M3.WIDTH_DATA=32 ;
		  
    // M4 : ALU A input
    Mux2to1     U_M4( pc, a_reg, ALUSrcA_I, alu_da ) ;
        defparam U_M4.WIDTH_DATA=32 ; 
		  
    // M5 : ALU B input
    Mux4to1     U_M5( b_reg, 4, sign_ext, sign_ext_sl2, ALUSrcB_I, alu_db ) ;
        defparam U_M5.WIDTH_DATA=32 ; 
    // M6 : shifter input
	 
    Mux2to1     U_M6( a_reg[4:0], wIR[10:6], SHTNumSrc_I, shift_num ) ;
        defparam U_M6.WIDTH_DATA=5 ; 
		  
    // M7 : ALUOut temporary register input
    // GXPMux3to1_32  U_M7( alu_dc, shifter, mul_div, ALUOutSrc_I, alu_out_di ) ;
	 
    Mux5to1     U_M7( alu_dc, shifter, mul_div, sign_ext, cp0_dout, ALUOutSrc_I, alu_out_di ) ;
        defparam U_M7.WIDTH_DATA=32 ; 
		  
    // M8 : CP0 destination register index
    Mux2to1     U_M8( 5'h0E, wIR[15:11], CP0_Idx_I, cp0_idx ) ;
        defparam U_M8.WIDTH_DATA=5 ; 
		  
		  
    // M9 : PC {PC+4, branch, }
// GXP    Mux6to1_32  U_M9( alu_dc, alu_out, {pc[31:28], wIR[25:0], 2'b00}, 32'h80000180, 32'h80000200, 32'h0, PCSrc_I, next_pc ) ;
//    Mux7to1     U_M9( alu_dc, alu_out, {pc[31:28], wIR[25:0], 2'b00}, a_reg, `VECTOR_EXCEPTION, `VECTOR_INTERRUPT, {cp0_epc,2'b0}, PCSrc_I, next_pc ) ;
//        defparam U_M9.WIDTH_DATA=32 ;


    Mux6to1     U_M9( alu_dc, alu_out, {pc[31:28], wIR[25:0], 2'b00}, a_reg, exc_vector, cp0_epc, PCSrc_I, next_pc ) ;
        defparam U_M9.WIDTH_DATA=32 ;
		  
		  
    // M10 : CP0's data port for instructions writing internal registers or saving PC to EPC during exception
//    Mux2to1     U_M10( alu_out, pc, CP0_IorE_I, cp0_din ) ;
//        defparam U_M10.WIDTH_DATA=32 ;


    Mux3to1     U_M10( alu_out, alu_dc, pc, CP0_IorE_I, cp0_din ) ;
        defparam U_M10.WIDTH_DATA=32 ;
        
endmodule
