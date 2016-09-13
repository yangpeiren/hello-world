
#define		NUL					0			// null
#define		IDENTF				1			// identifer标识符
#define		INTEGER				2			// 整数 int
#define		FLOAT			    3		    // float
#define		CHAR				4			// 字符 char
#define		STRING				5			// 字符串 string

#define		PLUS				10			// +   算符
#define		MINUS				11			// -
#define		MUL			     	12			// *
#define		DIV			 	    13			// /
#define		LEFTP				14			// (
#define		RIGHTP				15			// )
#define		LBRACKET            16			// {
#define		RBRACKET			17			// }
#define		COMMA				18			// ,
#define     COLON               19          // :
#define     SEMICOLON			20			// ;
#define		EVALUE				21			// =
#define		UNEQL				22			// !=
#define		LESSTHAN			23			// <
#define		MORETHAN			24			// >
#define		NLESSTHAN			25			// <=
#define		NMORETHAN			26			// >=
#define		EQUAL				27			// ==
#define		QUOTATION			28			// "

#define		IF				100			// if     各种语句保留字
#define		ELSE			101			// else
#define		WHILE			102			// while
#define		FOR				103			// for
#define		PRINTF			104			// printf
#define		SCANF			105			// scanf
#define		RETURN			106			// return
#define		CONST			107			// const

#define		VOIDSYM				108			// void  函数类型
#define		INTSYM				109			// int
#define		FLOATSYM			110			// float
#define		CHARSYM				111			// char
#define		MAINSYM				112			// main
#define		OFFSET				100			// 保留字的偏移值

#define     STRINGLENGTH        255          //字符串最大长度
#define		VARLENGTH			16		  	 // 变量名的最大长度	
#define		INTLENGTH           2047483647	// 整数的最大值
#define		FLOATLENGTH      3.402823466E+38	// 浮点数的最大

#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <ctype.h>
#include <math.h>

const char reserved[13][10]={"if","else","while","for","printf","scanf","return","const","void","int","float","char","main"};

struct rebound
{
	int type;                  //取词类型
	char ch;				   //返回字符
	char Str[STRINGLENGTH];    //返回字符串
	int intval;                //返回整型数
	float realval;             //返回实数
};


