

/* Device address */
#define BASE_DEV            0xA0000000


// UART
#define MINOR_BASE_UART0    0x500
    #define UART_DATA       0x0
    #define UART_IER        0x4
    #define UART_IIR        0x8
    #define UART_LCR        0xC
    #define UART_LSR        0x10
    #define UART_DIV_RCV    0x14


typedef struct {
    unsigned int    data ;
    unsigned int    ier ;
    unsigned int    iir ;
    unsigned int    lcr ;
    struct {
/*
 * Note: since the first bit defined must be the LSB!!!!
 */
        unsigned dr     : 1 ;   // receive data ready
        unsigned oe     : 1 ;   // receive buffer overflow
        unsigned pe     : 1 ;   // parity error
        unsigned fe     : 1 ;   // data format error
        unsigned        : 1 ;   // reserved
        unsigned thre   : 1 ;   // send buffer empty
        unsigned        : 26 ;  // reserved
        } lsr ;
    unsigned int    div_rcv ;
    unsigned int    div_snd ;
} cocoa_uart ; 



void printcharc(unsigned char ch)
{
	cocoa_uart  *p_uart ;

	p_uart  = (cocoa_uart *)(BASE_DEV + MINOR_BASE_UART0) ;

    while ( !p_uart->lsr.thre )
        ;

		
	p_uart->data = ch ;
	
	 
}



void printstr(unsigned char *s)
{
	while (*s)
	   printcharc(*s++);
}



int main()
{
     int i=0,n=10000;
 
     printstr("Hello, User Env D is running...\n");
     while(1)
     { 
	      n=++i;
          printcharc('D');
          if (i%50==0) printcharc('\n');
     }
	 
     printstr("Bye, User Env D end running...\n");
	

     return 0;
}
