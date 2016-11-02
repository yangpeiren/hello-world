`define mul     'b0;
`define div     'b1;
`define sel_high    'b1;
`define sel_low     'b0; 
module    DIV(clk,reset,start,selhl,selmd,write,flag,db,da,dc);
input  clk , reset , start , selhl , selmd , write ;
input[31:0]   db , da ;
output    flag;
output[31:0]   dc;
reg[31:0]   hi;
reg[31:0]   lo;
reg   working;
reg[63:0]   result;
reg   finish;
reg   finish2;
function[63:0]  result_mul;
   input[31:0]   a,b;
      begin
        result_mul=a*b;
      end
endfunction
function[63:0]  result_div;
   input[31:0]   a,b;
      begin
        result_div[31:0]=a/b;result_div[63:32]=a%b;
      end
endfunction
assign   flag=finish;
assign   dc=finish?((selhl=='b1)?hi:lo):32'b0;

always    @(posedge   clk)
     begin
       if(reset)
          begin   working<=1'b0;finish<=1'b1;   end
       else if(start)
          begin   finish<=1'b0;working<=1'b1;   end
       else if(finish2)
          begin   finish<=1'b1;working<=1'b0;   end
     end

always   @(posedge  working)
     begin
       if(selmd==0)
          begin  result<=result_mul(da,db);finish2<=1'b1;  end
       else if(selmd==1)
          begin  result<=result_div(da,db);finish2<=1'b1;  end
     end

always    @(posedge   clk)
      begin
         if(reset)
            begin
                hi=0;lo=0;
            end
         else if(write)
            begin
               if(selhl==1'b1)  hi<=db;
               else   lo<=db;
            end
         else if(finish)
            begin
                hi=result[63:32];lo=result[31:0];
            end
      end
endmodule  