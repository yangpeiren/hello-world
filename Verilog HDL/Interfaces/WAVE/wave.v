module wave(clk, count, max, mode, reset, sign2, disp_mode);
	input clk, reset, mode;
	input[3:0] max; 
	
	output[3:0] count;
	output[1:0] sign2, disp_mode;
	
	reg[3:0] count, length;
	reg[1:0] sign2, disp_mode;
	reg sign1;

	always @(posedge mode)												//disp_mode=0	����б��	
	begin																//disp_mode=1	�½�б��
		disp_mode <= disp_mode + 2'b1;										//disp_mode=2	���ǲ�
	end 																//disp_mode=3	���β�

	always @(posedge clk or posedge reset)
	begin
		if(reset)
			count <= 0;
		else
		begin
			case(disp_mode)
			2'b00:														//����б��
			begin
				if(count == max)		
					count <= 0;
				else	
					count <= count + 4'b1;
			end
			2'b01:														//�½�б��
			begin
				if(count == 0)		
					count <= max;
				else	
					count <= count - 4'b1;
			end
			2'b10:														//���ǲ�
			begin
				if(count == max - 1)
					sign1 <= 1;
				else if(count == 1)
					sign1 <= 0;
				count <= sign1 ? count - 4'b1 : count + 4'b1 ;
				
			end
			2'b11:														//���β�
			begin
				case(sign2)
				2'b00:							//��������б����
				begin	
					if(count == max)	
						sign2 <= sign2 + 2'b1;
					else			
						count <= count + 4'b1;
				end					
				2'b01:							//�����ϵ�
				begin	
					if(length == max)	
						sign2 <= sign2 + 2'b1;
					else			
					begin	
						count <= max; 	
						length <= length + 4'b1;	
					end
				end					
				2'b10:							//�����½�б����
				begin	
					if(count == 0)	
						sign2 <= sign2 + 2'b1;
					else			
						count <= count - 4'b1;	
				end										
				2'b11: 							//�����µ�
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