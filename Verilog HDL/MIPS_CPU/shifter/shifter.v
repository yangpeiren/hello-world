module shifter(clk,m,co,s,d,qb,cn);
input clk,m,co;
input[1:0] s;
input[7:0] d;
output cn;
output[7:0] qb;
reg cn,tmp;
reg[7:0] qb;
initial tmp=co;
always@(posedge clk)
begin
      case(s)
        'b00:qb<=qb;
        'b10:if(m==0)  begin qb<={qb[0],qb[7:1]};cn<=qb[0];end
           else begin qb<={tmp,qb[7:1]};cn<=qb[0];tmp<=qb[0];end
        'b01:if(m==0) begin qb<={qb[6:0],qb[7]};cn<=qb[7];end
           else begin qb<={qb[6:0],tmp};cn<=qb[7];tmp<=qb[7];end
        'b11:qb<=d; 
      endcase
end
endmodule
 