`timescale 1ns/1ns
//`include "../../cpu/src/cocoa_head.v"
/** the divider of the MIPS-C CPU **/

module Divider(
   Reset,
   Clk,
   Dividend,
   Divisor,
   Bussiness,
   Reminder,
   Work,
   Done
);

////
///   Bussiness = Dividend / Divisor
///   Reminder  = Dividend % Divisor
////

parameter NBit = 32;

input                  Reset;
input                  Clk;
input [NBit - 1:0]     Dividend;
input [NBit - 1:0]     Divisor;
input                  Work;


output [NBit - 1:0]    Bussiness;
output [NBit - 1:0]    Reminder;
output     reg            Done;
/////
reg	[2*NBit + 1:0]  result;
reg	[NBit - 1:0]	counter;
reg              restored;
reg	[NBit :0]	negdiv;

wire	[NBit :0] actualDivisor;


assign actualDivisor = (result[2*NBit + 1] == 1)? {1'b0,Divisor} : negdiv;
assign Bussiness = result[NBit - 1:0];
assign Reminder = result[2*NBit :NBit + 1];


always@(Divisor)
	negdiv <= {1'b1, (~Divisor) + 1};


always@(posedge Clk or posedge Reset)
	if(Reset)begin
		Done 	= 0;
		counter 	= 0;
		restored	= 1;
		result		= 0;
	end else begin
		if(Work)begin
			if(Divisor == 0)begin
				Done	= 1;
				result 	= 0;
			end
			else if(counter != NBit + 1) begin
				result = {result[2 * NBit : 0], 1'b0};
				result[2*NBit + 1: NBit + 1] = result[2*NBit + 1 : NBit + 1] + actualDivisor;
				if(result[2*NBit + 1] == 1)
					result[0] = 0;
				else
					result[0] = 1;			
				counter = counter + 1;
			end
			else if(!restored)begin
				if(result[0] == 0)
					result[2*NBit + 1: NBit + 1] = result[2*NBit + 1: NBit + 1] + {1'b0, Divisor};
				restored = 1;
				Done = 1;
			end
		end else begin
			counter = 0;
			Done = 0;
			restored = 0;	
			result 	= {{(NBit + 1){1'b0}}, 1'b0, Dividend};	
		end
	end




/*
always@(posedge Clk or posedge Work or posedge Reset)
   if(Reset) begin
      finished = 0;
      counter = 0;
      working = 0;
   end else if(Work) begin
 		// do some initial work
		
		if(Divisor == 0) begin 
		   finished = 1;
		   restoreWatcher = 0;
		   counter = NBit;
		   result 	= {{(NBit + 2){1'b1}}, {NBit{1'b1}}};
		   working = 0;
		end
		else begin 
		   finished	= 0;
		   restoreWatcher = 1;
		   counter 	= 0;
		   
		   working = 1;
		end
		
	end else begin
		if(counter != NBit + 1 && working)begin
		
		end else if(working)begin
		   if(restoreWatcher && result[0] == 0)begin
		       
			end
			restoreWatcher = 0;
			working = 0;
		end
	end
*/




endmodule




