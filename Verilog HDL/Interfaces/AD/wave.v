module wave(clk, count, max, mode, reset, sign2, disp_mode);
	input clk, reset, mode;
	input[3:0] max; 
	
	output[3:0] count;
	output[1:0] sign2, disp_mode;
	
	reg[3:0] count, length;
	reg[1:0] sign2, disp_mode;
	reg sign1;

	always @(posedge mode)												//disp_mode=0	ÉÏÉıĞ±²¨	
	begin																//disp_mode=1	ÏÂ½µĞ±²¨
		disp_mode <= disp_mode + 2'b1;										//disp_mode=2	Èı½Ç²¨
	end 																//disp_mode=3	ÌİĞÎ²¨

	always @(posedge clk or posedge reset)
	begin
		if(reset)
			count <= 0;
		else
		begin
			case(disp_mode)
			2'b00:														//ÉÏÉıĞ±²¨
			begin
				if(count == max)		
					count <= 0;
				else	
					count <= count + 4'b1;
			end
			2'b01:														//ÏÂ½µĞ±²¨
			begin
				if(count == 0)		
					count <= max;
				else	
					count <= count - 4'b1;
			end
			2'b10:														//Èı½Ç²¨
			begin
				if(count == max - 1)
					sign1 <= 1;
				else if(count == 1)
					sign1 <= 0;
				count <= sign1 ? count - 4'b1 : count + 4'b1 ;
				
			end
			2'b11:														//ÌİĞÎ²¨
			begin
				case(sign2)
				2'b00:							//ÌİĞÎÉÏÉıĞ±²¨Ñü
				begin	
					if(count == max)	
						sign2 <= sign2 + 2'b1;
					else			
						count <= count + 4'b1;
				end					
				2'b01:							//ÌİĞÎÉÏµ×
				begin	
					if(length == max)	
						sign2 <= sign2 + 2'b1;
					else			
					begin	
						count <= max; 	
						length <= length + 4'b1;	
					end
				end					
				2'b10:							//ÌİĞÎÏÂ½µĞ±²¨Ñü
				begin	
					if(count == 0)	
						sign2 <= sign2 + 2'b1;
					else			
						count <= count - 4'b1;	
				end										
				2'b11: 							//ÌİĞÎÏÂµ×
				begin	
					if(length == 0)	
						sign2 <= sign2 + 2'b1;
					else			
					begin		
						count <= 0;   
						length <= length - 4'b1;	
					end
				end								
			endcase
		end
		endcase
	end
end
endmodule