/*---------- debug version switch-------------*/
`define SIMULATION

////simulation macro
`define DELAY(x) `ifdef SIMULATION #(x); `endif


/*--------------------------- Instruction Definition -----------------------
  
  --------------------------------------------------------------------------*/
// GXP

//  Instruction Total  No: 50
//  Instruction R type   No: 25
`define INST_R_TYPE     6'b000000           // 00h : decode according to funct field (IR[5:0])
    // funct -- IR[5:0]
    `define INST_SLL        6'd00           // Shift Word Left Logical
    `define INST_SRL        6'd02           // Shfit Wrod Right Logical
    `define INST_SRA        6'd03           // Shift Word Right Arithmetic 
    `define INST_SLLV       6'd04           // Shift Word Left Logical Variable
    `define INST_SRLV       6'd06           // Shift Word Right Logical Variable
    `define INST_SRAV       6'd07           // Shift Word Right Arithmetic Variable
    `define INST_JR         6'd08       
//  `define INST_JALR       6'd09       
    `define INST_SYSCALL    6'd12       
    `define INST_BREAK      6'd13       
    `define INST_MFHI       6'd16           // Move from HI Register
//  `define INST_MTHI       6'd17           // Move to HI Register
    `define INST_MFLO       6'd18           // Move from LOW Register
//  `define INST_MTLO       6'd19           // Move to LOW Register  
    `define INST_MULT       6'd24       
    `define INST_MULTU      6'd25
    `define INST_DIV        6'd26
    `define INST_DIVU       6'd27
    `define INST_ADD        6'd32           // Add Word
    `define INST_ADDU       6'd33           // Add Unsigned Word
    `define INST_SUB        6'd34           // Subtract Word
    `define INST_SUBU       6'd35           // Subtract Unsigned Word
    `define INST_AND        6'd36
    `define INST_OR         6'd37
    `define INST_XOR        6'd38
    `define INST_NOR        6'd39
    `define INST_SLT        6'd42
    `define INST_SLTU       6'd43


//  Instrction I type   No: 8
`define INST_ADDI       6'b001000           
`define INST_ADDIU      6'b001001           
`define INST_SLTI       6'b001010
`define INST_SLTIU      6'b001011
`define INST_ANDI       6'b001100
`define INST_ORI        6'b001101
`define INST_XORI       6'b001110
`define INST_LUI        6'b001111


// Instruction Branch & Jump type  No: 8
`define INST_BRANCH_RT  6'b000001           // 01h :branch-type instruction : decode according to rt field ([20:16])
    // rt -- IR[20:16]
    `define INST_BLTZ       5'd00
    `define INST_BGEZ       5'd01
//  `define INST_BLTZAL     5'd16
//  `define INST_BGEZAL     5'd17

`define INST_J          6'b000010           
`define INST_JAL        6'b000011           
`define INST_BEQ        6'b000100           
`define INST_BNE        6'b000101
`define INST_BLEZ       6'b000110
`define INST_BGTZ       6'b000111


// Instruction  CP0  type  No: 3
`define INST_CP_TYPE    6'b010000           // 10h : rs([25:21), funct([4:0])
    // rs -- IR[25:21]
    `define INST_MFC0       5'd00
    `define INST_MTC0       5'd04
    `define INST_RET_TYPE   5'd16
    `define INST_TLB_TYPE   5'd16
        // funct -- IR[4:0]
        `define INST_ERET   5'd24
        // funct -- IR[5:0]
        `define INST_TLBR   6'd1
        `define INST_TLBWI  6'd2
        `define INST_TLBWR  6'd6
        `define INST_TLBP   6'd8
         


//  Instruction Memory type  No: 6
//`define INST_LB         6'b100000           //
//`define INST_LH         6'b100001           //
//`define INST_LW         6'b100011
`define INST_LBU        6'b100100
`define INST_LHU        6'b100101
`define INST_LW         6'b100011
`define INST_SB         6'b101000
`define INST_SH         6'b101001
`define INST_SW         6'b101011

// Instruction TLB type








/**	byte enable for the memory bus
 */
`define WORD			4'b1111
`define HALF_WORD			4'b0011
`define BYTE			4'b0001

/*
 *	ALU function code
    GXP    
 */
`define ALU_NOP         5'b00000    // pass through (C <- B)
`define ALU_ADDU        5'b00001    // add : unsigned
`define ALU_ADD         5'b00010    // add
`define ALU_SUBU	       5'b00011    // sub : unsigned
`define	ALU_SUB         5'b00100    // sub
`define	ALU_AND			      5'b00101    // and
`define	ALU_OR			       5'b00110    // or
`define	ALU_NOR			      5'b00111    // nor
`define	ALU_XOR			      5'b01000    // xor
`define	ALU_SLTU		      5'b01001    // set : when A is smaller than B(unsigned)
`define	ALU_SLT         5'b01010    // set : when A is smaller than B
`define ALU_LTZ         5'b01101    // cmp : A less than 0
`define	ALU_LEZ			      5'b01100    // cmp : A less than or equal to 0
`define	ALU_GTZ			      5'b01011    // cmp : A greater than 0
`define	ALU_GEZ			      5'b01110    // cmp : A greater than or equal to 0
`define	ALU_EQ			       5'b01111    // cmp : A equal to B
`define	ALU_NEQ			      5'b10000    // cmp : A not equal to B
`define ALU_SLL         5'b10001    // shift left logical
`define ALU_SRL         5'b10010    // shift right logical
`define ALU_SRA         5'b10011    // shift right arithmetic

/*
    Sign extend
    GXP
 */
`define EXT_ZERO        2'b00
`define EXT_SIGN        2'b01       // sign extend 
`define EXT_SL16        2'b10       // shift left 16-bit

/*
 */
`define EXT_M_NOP       2'b00       // don't extend
`define EXT_M_8BIT      2'b01       // keep lower 8bit, higher 24bits are zero
`define EXT_M_16BIT     2'b10       // keep lower 16bit, higher 16bits are zero


