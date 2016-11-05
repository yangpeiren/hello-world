module pr(sm,mask,request,isr,sp,eoi,isr_set,isr_clr,intr,code);
input [7:0]mask;
input sm;
input [7:0]request;
input [7:0]isr;
input [7:0]sp;
input [7:0]eoi;
output[7:0]isr_set;
output[7:0]isr_clr;
output intr;
output[2:0]code;
reg[7:0] position;
reg[7:0] hp_nmr;
reg[7:0]hp_isr;
wire [7:0]nmr;
assign nmr=~mask&request;
	always@(sp)
	begin
		if(nmr!=0)
		begin
			case(sp)
			7:
				begin
					if(nmr[0]==1)
					begin
					position<=0;
					hp_nmr<=0;
					end
					else if(nmr[1]==1)//else if !????
					begin
					position<=1;
					hp_nmr<=1;
					end
					else if(nmr[2]==1)
					begin
					position<=2;
					hp_nmr<=2;
					end
					else if(nmr[3]==1)
					begin
					position<=3;
					hp_nmr<=3;
					end
					else if(nmr[4]==1)
					begin
					position<=4;
					hp_nmr<=4;
					end
					else if(nmr[5]==1)
					begin
					position<=5;
					hp_nmr<=5;
					end
					else if(nmr[6]==1)
					begin
					position<=6;
					hp_nmr<=6;
					end
					else if(nmr[7]==1)
					begin
					position<=7;
					hp_nmr<=7;
					end
				end
				//case 7
			6:
			begin
					if(nmr[7]==1)
					begin
					position<=7;
					hp_nmr<=7;
					end
					else if(nmr[0]==1)
					begin
					position<=0;
					hp_nmr<=0;
					end
					else if(nmr[1]==1)//else if !????
					begin
					position<=1;
					hp_nmr<=1;
					end
					else if(nmr[2]==1)
					begin
					position<=2;
					hp_nmr<=2;
					end
					else if(nmr[3]==1)
					begin
					position<=3;
					hp_nmr<=3;
					end
					else if(nmr[4]==1)
					begin
					position<=4;
					hp_nmr<=4;
					end
					else if(nmr[5]==1)
					begin
					position<=5;
					hp_nmr<=5;
					end
					else if(nmr[6]==1)
					begin
					position<=6;
					hp_nmr<=6;
					end		
			end//end case 6
			5:
			begin
				 if(nmr[6]==1)
					begin
					position<=6;
					hp_nmr<=6;
					end
					else if(nmr[7]==1)
					begin
					position<=7;
					hp_nmr<=7;
					end
					else if(nmr[0]==1)
					begin
					position<=0;
					hp_nmr<=0;
					end
					else if(nmr[1]==1)//else if !????
					begin
					position<=1;
					hp_nmr<=1;
					end
					else if(nmr[2]==1)
					begin
					position<=2;
					hp_nmr<=2;
					end
					else if(nmr[3]==1)
					begin
					position<=3;
					hp_nmr<=3;
					end
					else if(nmr[4]==1)
					begin
					position<=4;
					hp_nmr<=4;
					end
					else if(nmr[5]==1)
					begin
					position<=5;
					hp_nmr<=5;
					end					
		
				end//case 5
			4:
			begin
				 if(nmr[5]==1)
					begin
					position<=5;
					hp_nmr<=5;
					end	
					else if(nmr[6]==1)
					begin
					position<=6;
					hp_nmr<=6;
					end
					else if(nmr[7]==1)
					begin
					position<=7;
					hp_nmr<=7;
					end
					else if(nmr[0]==1)
					begin
					position<=0;
					hp_nmr<=0;
					end
					else if(nmr[1]==1)//else if !????
					begin
					position<=1;
					hp_nmr<=1;
					end
					else if(nmr[2]==1)
					begin
					position<=2;
					hp_nmr<=2;
					end
					else if(nmr[3]==1)
					begin
					position<=3;
					hp_nmr<=3;
					end
					else if(nmr[4]==1)
					begin
					position<=4;
					hp_nmr<=4;
					end
					
		
			end//edn of case 4
			3:begin
			if(nmr[4]==1)
					begin
					position<=4;
					hp_nmr<=4;
					end
					else if(nmr[5]==1)
					begin
					position<=5;
					hp_nmr<=5;
					end	
					else if(nmr[6]==1)
					begin
					position<=6;
					hp_nmr<=6;
					end
					else if(nmr[7]==1)
					begin
					position<=7;
					hp_nmr<=7;
					end
					else if(nmr[0]==1)
					begin
					position<=0;
					hp_nmr<=0;
					end
					else if(nmr[1]==1)//else if !????
					begin
					position<=1;
					hp_nmr<=1;
					end
					else if(nmr[2]==1)
					begin
					position<=2;
					hp_nmr<=2;
					end
					else if(nmr[3]==1)
					begin
					position<=3;
					hp_nmr<=3;
					end
			end//end of case 3
			2:begin
				 if(nmr[3]==1)
					begin
					position<=3;
					hp_nmr<=3;
					end	
					else if(nmr[4]==1)
					begin
					position<=4;
					hp_nmr<=4;
					end
					else if(nmr[5]==1)
					begin
					position<=5;
					hp_nmr<=5;
					end	
					else if(nmr[6]==1)
					begin
					position<=6;
					hp_nmr<=6;
					end
					else if(nmr[7]==1)
					begin
					position<=7;
					hp_nmr<=7;
					end
					else if(nmr[0]==1)
					begin
					position<=0;
					hp_nmr<=0;
					end
					else if(nmr[1]==1)//else if !????
					begin
					position<=1;
					hp_nmr<=1;
					end
					else if(nmr[2]==1)
					begin
					position<=2;
					hp_nmr<=2;
					end
					
			end//edn of case 2
			1:begin
				 if(nmr[2]==1)
					begin
					position<=2;
					hp_nmr<=2;
					end
					else if(nmr[3]==1)
					begin
					position<=3;
					hp_nmr<=3;
					end	
					else if(nmr[4]==1)
					begin
					position<=4;
					hp_nmr<=4;
					end
					else if(nmr[5]==1)
					begin
					position<=5;
					hp_nmr<=5;
					end	
					else if(nmr[6]==1)
					begin
					position<=6;
					hp_nmr<=6;
					end
					else if(nmr[7]==1)
					begin
					position<=7;
					hp_nmr<=7;
					end
					else if(nmr[0]==1)
					begin
					position<=0;
					hp_nmr<=0;
					end
					else if(nmr[1]==1)//else if !????
					begin
					position<=1;
					hp_nmr<=1;
					end
					
			end//end of case 1
			0:begin
			if(nmr[1]==1)//else if !????
					begin
					position<=1;
					hp_nmr<=1;
					end
					else if(nmr[2]==1)
					begin
					position<=2;
					hp_nmr<=2;
					end
					else if(nmr[3]==1)
					begin
					position<=3;
					hp_nmr<=3;
					end	
					else if(nmr[4]==1)
					begin
					position<=4;
					hp_nmr<=4;
					end
					else if(nmr[5]==1)
					begin
					position<=5;
					hp_nmr<=5;
					end	
					else if(nmr[6]==1)
					begin
					position<=6;
					hp_nmr<=6;
					end
					else if(nmr[7]==1)
					begin
					position<=7;
					hp_nmr<=7;
					end
					else if(nmr[0]==1)
					begin
					position<=0;
					hp_nmr<=0;
					end				
				end//end of case0
			default:begin 
					position<=8;
					hp_nmr<=8;
					end
	endcase
		end//end of if(nmr!=0)
end//end of always
	always@(sp)
	begin
		if(isr!=0)
		begin
			case(sp)
				7:begin	
					if(isr[0]==1)
					hp_isr<=0;
					else if(isr[1]==1)
					hp_isr<=1;
					else if(isr[2]==1)
					hp_isr<=2;
					else if(isr[3]==1)
					hp_isr<=3;
					else if(isr[4]==1)
					hp_isr<=4;
					else if(isr[5]==1)
					hp_isr<=5;
					else if(isr[6]==1)
					hp_isr<=6;
					else if(isr[7]==1)
					hp_isr<=7;
					end//end of case 7
				6:begin	
					if(isr[7]==1)
					hp_isr<=7;
					else if(isr[0]==1)
					hp_isr<=0;
					else if(isr[1]==1)
					hp_isr<=1;
					else if(isr[2]==1)
					hp_isr<=2;
					else if(isr[3]==1)
					hp_isr<=3;
					else if(isr[4]==1)
					hp_isr<=4;
					else if(isr[5]==1)
					hp_isr<=5;
					else if(isr[6]==1)
					hp_isr<=6;
					end//end of case 6
				5:begin	
					 if(isr[6]==1)
					hp_isr<=6;
					else if(isr[7]==1)
					hp_isr<=7;
					else if(isr[0]==1)
					hp_isr<=0;
					else if(isr[1]==1)
					hp_isr<=1;
					else if(isr[2]==1)
					hp_isr<=2;
					else if(isr[3]==1)
					hp_isr<=3;
					else if(isr[4]==1)
					hp_isr<=4;
					else if(isr[5]==1)
					hp_isr<=5;
					
					end//end of case 5
				4:begin	
				 if(isr[5]==1)
					hp_isr<=5;
					else if(isr[6]==1)
					hp_isr<=6;
					else if(isr[7]==1)
					hp_isr<=7;
					else if(isr[0]==1)
					hp_isr<=0;
					else if(isr[1]==1)
					hp_isr<=1;
					else if(isr[2]==1)
					hp_isr<=2;
					else if(isr[3]==1)
					hp_isr<=3;
					else if(isr[4]==1)
					hp_isr<=4;
					end//end of case 4
				3:begin	
				if(isr[4]==1)
					hp_isr<=4;
					else if(isr[5]==1)
					hp_isr<=5;
					else if(isr[6]==1)
					hp_isr<=6;
					else if(isr[7]==1)
					hp_isr<=7;
					else if(isr[0]==1)
					hp_isr<=0;
					else if(isr[1]==1)
					hp_isr<=1;
					else if(isr[2]==1)
					hp_isr<=2;
					else if(isr[3]==1)
					hp_isr<=3;
					
					end//end of case 3
				2:begin	
				if(isr[3]==1)
					hp_isr<=3;
					else if(isr[4]==1)
					hp_isr<=4;
					else if(isr[5]==1)
					hp_isr<=5;
					else if(isr[6]==1)
					hp_isr<=6;
					else if(isr[7]==1)
					hp_isr<=7;
					else if(isr[0]==1)
					hp_isr<=0;
					else if(isr[1]==1)
					hp_isr<=1;
					else if(isr[2]==1)
					hp_isr<=2;
					end//end of case 2
				1:begin	
				 if(isr[2]==1)
					hp_isr<=2;
					else if(isr[3]==1)
					hp_isr<=3;
					else if(isr[4]==1)
					hp_isr<=4;
					else if(isr[5]==1)
					hp_isr<=5;
					else if(isr[6]==1)
					hp_isr<=6;
					else if(isr[7]==1)
					hp_isr<=7;
					else if(isr[0]==1)
					hp_isr<=0;
					else if(isr[1]==1)
					hp_isr<=1;
					end//end of case 1
				0:begin	
				 if(isr[1]==1)
					hp_isr<=1;
					else if(isr[2]==1)
					hp_isr<=2;
					else if(isr[3]==1)
					hp_isr<=3;
					else if(isr[4]==1)
					hp_isr<=4;
					else if(isr[5]==1)
					hp_isr<=5;
					else if(isr[6]==1)
					hp_isr<=6;
					else if(isr[7]==1)
					hp_isr<=7;
					else if(isr[0]==1)
					hp_isr<=0;					
					end//end of case 1
				default:begin
						hp_isr<=8;
						end//end of default
				endcase
		end//end of if(isr!=0)
	end //end of always
assign isr_set[0]=(sm&&hp_nmr!=8&&position==0)||(~sm&&hp_nmr<hp_isr&&position==0)?1:0;
assign isr_set[1]=(sm&&hp_nmr!=8&&position==1)||(~sm&&hp_nmr<hp_isr&&position==1)?1:0;
assign isr_set[2]=(sm&&hp_nmr!=8&&position==2)||(~sm&&hp_nmr<hp_isr&&position==2)?1:0;
assign isr_set[3]=(sm&&hp_nmr!=8&&position==3)||(~sm&&hp_nmr<hp_isr&&position==3)?1:0;
assign isr_set[4]=(sm&&hp_nmr!=8&&position==4)||(~sm&&hp_nmr<hp_isr&&position==4)?1:0;
assign isr_set[5]=(sm&&hp_nmr!=8&&position==5)||(~sm&&hp_nmr<hp_isr&&position==5)?1:0;
assign isr_set[6]=(sm&&hp_nmr!=8&&position==6)||(~sm&&hp_nmr<hp_isr&&position==6)?1:0;
assign isr_set[7]=(sm&&hp_nmr!=8&&position==7)||(~sm&&hp_nmr<hp_isr&&position==7)?1:0;
assign code=(isr==0)?3'b000:position;
assign intr=((hp_nmr!=8&&sm)||(hp_nmr!=8&&~sm&&hp_nmr<nmr<hp_isr))?1:0;
endmodule
