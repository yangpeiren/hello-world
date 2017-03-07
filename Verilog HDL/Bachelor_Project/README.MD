# The Design and Implementation of Memory Management Unit on MIPS CPU

## Abstract
MIPS (Microprocessor without Interlocked Pipeline Stages) is a reduced instruction set computer (RISC) instruction set architecture (ISA) developed by MIPS Technologies. Among the RISC CPUs, MIPS CPU has a very brief and exquisite design, and it is the most efficient architecture, too. MIPS CPU is one of the earliest commercialized CPU. 
At the beginning of this dissertation, we firstly overviewed the features of MIPS CPU and its current state on domestic and international. Then we gave a brief introduction of our COCOA platform.
In the second part, based on the theory, we took the different Implementation Methods of TLB (Translation Look-aside Buffers) and MMU (Memory Management Unit) into analysis; we try to get the advantages and disadvantages of different Implementation Methods. Integrate theory with our hardware environment and the complexity of implement, we proposed an original solution.
 In the third part, we realized the solution which was proposed in the second part, we gave an elaborate introduction and discussion on the entire solution. We designed the Page Table Entry and TLB pages. We modified CU in corresponding control logic, to join the corresponding exception access points. We updated the Operating System, added codes of TLB page fault.
 The fourth part we tested and verified the previous solution. Firstly we tested the independent part of the design, and then we built the blocks together to test the entire solution. In this part, we used three different powerful test tools, i.e. Quartus II 7.2, ModelSim and Logic Analyzer. Finally, the result of the serial test and simulate shows that our design meets the original requirement; our solution can successfully reaches our goal.

**Key words**\:`COCOA` `Virtual Memory` `MMU` `TLB`
