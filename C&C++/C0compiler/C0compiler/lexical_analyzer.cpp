#include"lexical_analyzer.h"
#include "headeclare.h"
extern int lineNo;
extern int c;				
extern FILE*infp;

void GETCHAR()
{

	static int line=1;					// 当前字符所在的行号
	lineNo=line;						// 处理的单词实际所在行号(由于预读造成lineNo总滞后于line)
	if (feof(infp))
		c=-1;
	else
	{
		c=fgetc(infp);	
		while ((c=='\n'))
		{
			line++;
			c=fgetc(infp);
		}
		
	}
}

int isreserved(char s[])//是否为保留字
{
	int i;
	for (i=0;i<13;i++)
		if (strcmp(s,reserved[i])==0)
			return OFFSET+i;
	return -1;
}

rebound lex()//词法分析
{
	int i,k=0;
	rebound re;
	re.Str[0]='\0';
	while(c==' '||c=='\t')
		GETCHAR();	
	if(isalpha(c)||c =='_')
	{
		re.Str[k++]=c;
		GETCHAR();
		while(isalpha(c)||isdigit(c)||c =='_')
		{
			if(k<VARLENGTH)
				re.Str[k++]=c;
			GETCHAR();
		}
		if (k>VARLENGTH)				// 判断标识符是否超过指定长度
		{						
			Error(lineNo,"Identifer is too long!");
			re.Str[VARLENGTH]='\0';
		}
		else  re.Str[k]='\0';
		if((i=isreserved(re.Str))!=-1)
			re.type=i;
		else re.type=IDENTF;
	}
	else if(isdigit(c))
	{
		if(c!='0')
		{	while (isdigit(c))
			{
				if (k<STRINGLENGTH)
					re.Str[k++]=c;
				else
					k++;
				GETCHAR();
			}
		}
		else
		{
			GETCHAR();
			if(isdigit(c)||isalpha(c))
			{
				Error(lineNo,"Illegal number in this line!");
				while(isdigit(c)||isalpha(c))
					GETCHAR();
			}
			re.Str[k++]='0';
		}
		re.type=INTEGER;
		if (c=='.')
		{							// 判断是浮点数还是整数
			re.Str[k++]=c;
			GETCHAR();
			if(!isdigit(c))
				re.Str[k]='\0';
			while (isdigit(c))
			{
				if (k<STRINGLENGTH)
					re.Str[k++]=c;
				else 
					k++;
				GETCHAR();
			}
			re.type=FLOAT;
		}
		if (k<STRINGLENGTH)
		{
			if(re.Str[k-1]=='.')//处理 1.空 这种数字
				re.Str[k-1]='\0';
			else re.Str[k]='\0';
		}
		else
			re.Str[STRINGLENGTH]='\0';

		if ((re.intval=atoi(re.Str))>INTLENGTH)		// 判断整数是否超出范围
			Error(lineNo,"The integer in your program is out of range!");
		if ((re.realval=(float)atof(re.Str))>FLOATLENGTH)			// 判断浮点数是否超出范围
			Error(lineNo,"The realnum in your program is out of range!");
	}
	else switch (c)
		{
		case '\'':							// 处理字符常量
			GETCHAR();
			re.ch=c;
			re.type=CHAR;
			GETCHAR();
			if (c!='\'')
				Error(lineNo," The char is lack of \' ");
			else
				GETCHAR();	
			break;
		case '"':
			GETCHAR();
			while (c!='"')
			{
				if (k<STRINGLENGTH)
					re.Str[k++]=c;
				else
					k++;
				GETCHAR();
			}
			if (c=='"') 
				GETCHAR();
			if (k<=STRINGLENGTH)							// 判断字符串是否过长
				re.Str[k]='\0';
			else
			{
				Error(lineNo,"The string is too long!");
				re.Str[STRINGLENGTH-1]='\0';
			}
			re.type=STRING;
			break;
		case '=':
			GETCHAR();
			if (c=='=')
			{
				re.type=EQUAL;
				GETCHAR();
			}
			else
				re.type=EVALUE;
			break;
		case '<':										// 处理 <= 和 <
			GETCHAR();
			if (c=='=')
			{
				re.type=NLESSTHAN;
				GETCHAR();
			}
			else
				re.type=LESSTHAN;
			break;
		case '>':							// 处理 >= 和 >
			GETCHAR();
			if (c=='=')
			{
				re.type=NMORETHAN;
				GETCHAR();
			}
			else
				re.type=MORETHAN;
			break;
		case '!':							// 处理 !=
			GETCHAR();
			if (c=='=')
			{
				re.type=UNEQL;
				GETCHAR();
			}
			else
			{
				Error(lineNo,"Illegal words in this line");
				GETCHAR();
			}
			break;
		case '+': 
			re.type=PLUS;
			GETCHAR(); 
			break;
		case '-': 
			re.type=MINUS; 
			GETCHAR();
			break;
		case '*': 
			re.type=MUL;
			GETCHAR();
			break;
		case '/':
			re.type=DIV; 
			GETCHAR();
			break;
		case '(':
			re.type=LEFTP;
			GETCHAR(); 
			break;
		case ')': 
			re.type=RIGHTP;
			GETCHAR();
			break;
		case '{': 
			re.type=LBRACKET;
			GETCHAR();
			break;
		case '}':
			re.type=RBRACKET;
			GETCHAR();
			break;
		case ',': 
			re.type=COMMA;	
			GETCHAR();
			break;
		case ';':
			re.type=SEMICOLON;
			GETCHAR();
			break;
		case ':':
			re.type=COLON; 
			GETCHAR();
			break;
		case  -1:
			re.type=-1;
			GETCHAR();
			break;
		default: 
			Error(lineNo,"Your character is beyond the grammar");
			GETCHAR();
	   }
	   if(re.type == IDENTF)//标识符不区分大小写
	   {
		   for(int x=0;re.Str[x]!='\0';x++)
		   {
			   if(re.Str[x]>='a'&&re.Str[x]<='z')
				   re.Str[x]-='a'-'A';
		   }
	   }
	return re;
}





