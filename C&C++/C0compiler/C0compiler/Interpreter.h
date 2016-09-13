#include "lexical_analyzer.h"


#define DTOP				500			// 数据栈栈顶值
#define CMAX                15          // 指令集中最长的指令长度
#define CNO					26			// 指令集中指令的个数

struct sc
{
	int type;							
	struct// 表示v中数据的类型
	{						
		int ival;
		float fval;
	} v;
};

sc data[DTOP];							// 数据栈，只存放数据对象的指针（引用）
int dptr;								// 数据栈指针
int abp;								// 活动记录的基地址
int preabp;								// 调用活动记录的基地址
char c;									// 用于WRITES语句，打印字符串

sc run[DTOP];							// 运行栈
int rptr;								// 运行栈栈顶指针

FILE *fp;								// 文件指针

const char code[CNO][CMAX]={"ALLOCATE","STO","LOAD","LOADI","POP","JSR","RETURN",
							"ADD","SUB","MULT","DIV","EQ","NOTEQ","GT","LES","GE",
							"LE","BRF","BR","PRINTI","PRINTS","CONVERF","READ","PRINTF","PRINTC","CONVERI"};