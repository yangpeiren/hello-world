FILE*infp;
FILE*outfp;

int lineNo;
int err;
int c;

rebound reb,reb1,reb2;					// 词法分析取到的三个词存放处
int line1,line2;						// 紧接上面的reb1，reb2的行号
int curLine;							// 当前程序所在的行号

symtable Table[SYMTOP];					// 符号表
int Top;								// 符号表栈顶指针
int level;								// 层号
int subIndex[2];						// 分程序索引表

int label;								// 标号序号

void maininitial()
{
	lineNo=1;
	err=0;
	Top=0;							
	level=0;							
	subIndex[0]=0;				
	label=0;		
	GETCHAR();
	reb=lex();
	reb1=lex();
	reb2=lex();
}