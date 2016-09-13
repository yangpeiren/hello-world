#include "lexical_analyzer.h"
#include "headeclare.h"
#include "grammar_semantic.h"
#include "symbol_table.h"

extern int lineNo;
extern int err;
extern int c;
extern rebound reb,reb1,reb2;				//词法分析取到的三个词存放处
extern int line1,line2;						//紧接上面的reb1，reb2的行号
extern int curLine;							// 当前程序所在的行号
extern symtable Table[SYMTOP];					// 符号表
extern int Top;								// 符号表栈顶指针
extern int level;								// 层号
extern int subIndex[SYMTOP];					// 分程序索引表
extern FILE *outfp;
extern int label;								// 标号序号


void tableinsert(char n[],int t,int kind)			// 将变量填入符号表中
{
	if (Top>=SYMTOP)
	{
		Error(curLine,"Symbol table overflow!");
	}
	if (Isintable(n))
	{
		Error(curLine,"Identifer redefined!");
	}
	Table[Top].kind=kind;
	Table[Top].lev=level;
	Table[Top].type=t-TYPEOFF;
	strcpy(Table[Top].name,n);
	Top++;
	if (kind==PROC || kind==FUNC)
		subIndex[++level]=Top;				
}

bool Isintable(char n[])					// 查看要填入符号表的标识符是否在符号表中已经定义
{
	int i;
	i=Top;
	do
	{
		if(i==0)
			return false;
		i--;
		if (strcmp(Table[i].name,n)==0)
			return true;
	}while (Table[i].lev==level);	
	return false;
}

int Lookup(char n[],int &l,int &j)		// 查符号表，返回层号和偏移值
{
	int k=Top-1;
	while (Table[k].lev==level)			// 本层符号表
	{
		if (strcmp(n,Table[k].name)==0)
		{
			l=level;
			j=k-subIndex[l]+1;
			return 1;
		}
		k--;
	}
	k=0;
	while (Table[k].lev==0)				// 全局符号表
	{
		if (strcmp(n,Table[k].name)==0)
		{
			l=0;
			j=k+1;
			return 1;
		}
		k++;
	}
	return -1;
}
int Lookup(char n[])
{
	int k=Top-1;
	while (Table[k].lev==level)			// 本层符号表
	{
		if (strcmp(n,Table[k].name)==0)
		{
			return Table[k].kind;
		}
		k--;
	}
	k=0;
	while (Table[k].lev==0)				// 全局符号表
	{
		if (strcmp(n,Table[k].name)==0)
		{
			return Table[k].kind;
		}
		k++;
	}
	return -1;
}

bool Lookuproc(char n[],int &i,int &z)		// 根据函数名字查符号表，并返回符号表中位置和参数个数
{
	int k=0;
	while (k<Top)
	{
		if (strcmp(n,Table[k].name)==0)
		{
			i=k;
			while (Table[++k].kind==PARA)
				;
			z=k-i-1;
			return true;
		}
		k++;		
	}
	return false;
}

bool Lookuproc(char n[],int &type)		// 根据函数名字查符号表，返回函数返回值类型
{
	int k=0;
	while (k<Top)
	{
		if (strcmp(n,Table[k].name)==0)
		{
			type = Table[k].type;
			return true;
		}
		k++;		
	}
	return false;
}
void LOAD(int c)			// 将整形数压入数据栈
{
	Print("LOADI",c);
}

void LOAD(float c)		// 将浮点数压入数据栈
{
	Print("LOADI",c);
}

void LOAD(char c)			// 将字符压入数据栈
{
	Print("LOADI",c);
}

void STO(int t,int s,int l,int j)			// 将数据栈中的数据放入运行栈
{
	if ((t==INTEGER || t==CHAR) && s==FLOAT)
		Print("CONVERI $Top");
	if (t==FLOAT && (s==INTEGER || s==CHAR))
		Print("CONVERF $Top");
	Print("STO",l,j);
}

void Labelproduce(char lab[])				// 产生标号
{
	char *p=new char[4];
	strcpy(lab,"\0");
	itoa(label,p,10);
	strcat(lab,p);
	label++;
}

void Labelinsert(char s[])				// 将标号填入代码段
{
	if(err!=0)
		return;
	char ss[STRINGLENGTH+1];
	strcpy(ss,s);
	strcat(ss,": ");
	fprintf(outfp,"%s\n",ss);
}

void allocspace(int k)					// 为函数分配存储空间
{
	int i;
	if (k!=0)
		Print("ALLOCATE",k);				// 保存返回地址
	Print("STO",level,1-NIL);
	for (i=k;i>0;i--)
		Print("STO",level,i);				// 保存参数值
}
void QuitTable()						// 当函数结束时退符号表
{
	int lv;
	lv=level;
	while(Top>=0 && Table[Top].lev==lv)
	{
		Top--;
	}
	Top++;
	while(Table[Top].kind==PARA)
	{
		strcpy(Table[Top].name,"\0");
		Table[Top].lev=lv-1;
		Top++;
	}
	level--;
}

void Type(int l,int j,int &t1)		// 通过查表，返回标识符的类型
{
	t1=Table[subIndex[l]+j-1].type;
}

void conjump(int t1,int t2,int t)		// 根据条件语句中的条件，生成转移代码
{
	if (t1==FLOAT && t2==INTEGER)
		Print("CONVERF","$top");
	if (t1==INTEGER && t2==FLOAT)
		Print("CONVERF","$top-1");
	switch (t)
	{
	case EQUAL:  Print("EQ"); break;
	case UNEQL: Print("NOTEQ"); break;
	case LESSTHAN:  Print("LES"); break;
	case MORETHAN:  Print("GT"); break;
	case NLESSTHAN:  Print("LE"); break;
	case NMORETHAN:  Print("GE"); break;
	}
}

void Write(int t)									// 生成输出语句
{
	switch(t)
	{
	case INTEGER: Print("PRINTI"); break;
	case FLOAT: Print("PRINTF"); break;
	case CHAR: Print("PRINTC"); break;
	}
}

void Opr(int &s1,int s2,int tp)			// 加 减 乘 除(不同操作数类型的转换)
{
	if (s1==FLOAT && (s2==INTEGER || s2==CHAR))
	{
		Print("CONVERF $top-1");
		s1=FLOAT;
	}
	if ((s1==INTEGER || s1==CHAR) && s2==FLOAT)
	{
		Print("CONVERF $top");
		s1=FLOAT;
	}
	if (s1!=FLOAT && s2!=FLOAT)
		s1=INTEGER;
	switch (tp)
	{
	case PLUS:  Print("ADD");
		break;
	case MINUS: Print("SUB");
		break;
	case MUL: Print("MULT");
		break;
	case DIV:  Print("DIV");
	}		
}

void Upcnt(int j,int &k)					// 统计参数个数
{
	k=++j;
}

bool Chktype(int t,int i,int &m,int &z)		// 检查函数定义参数类型，个数与调用时参数类型个数是否匹配
{
	if (z<1)
	{
		Error(curLine,"Incorrect number of parameters!");
		return false;
	}
	m++;
	if (t==INTEGER && Table[i+m].kind==FLOAT)
	{
		Error(curLine,"Incorrect parameter type!");
		return false;
	}
	z--;
	return true;
}

bool ChkLength(int z)							// 检查函数形参个数与实参个数是否相等
{
	if (z!=0)
	{
		Error(curLine,"Incorrect number of parameters!");
		return false;
	}
	return true;
}
void Print(char p[])
{
	fprintf(outfp,"   %s\n",p);
}
void Print(char p[],int i)
{
	fprintf(outfp,"   %s %d\n",p,i);
}

void Print(char p[],int i,int j)
{
	fprintf(outfp,"   %s <%d,%d>\n",p,i,j+NIL);
}

void Print(char p[],float f)
{
	fprintf(outfp,"   %s %f\n",p,f);
}

void Print(char p[],char s[])
{
	fprintf(outfp,"   %s %s\n",p,s);
}