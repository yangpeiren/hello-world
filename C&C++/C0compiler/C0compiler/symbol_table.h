#define		SYMTOP				10001

#define		CONSTANT			1001				
#define     VAR					1002
#define     PROC				1003
#define     FUNC				1004
#define		PARA				1005

#define     TYPEOFF				107         // 类型标识符和实际类型定义值的差
#define     NIL					3			// 定义隐式参数区大小

struct symtable							// 符号表结构
{
	char name[STRINGLENGTH+1];			// 名字
	int type;							// 类型
	int kind;							// 种类
	char cval;							// 若是字符，存于此
	int ival;							// 若是整形数，存于此
	float fval;							// 若是浮点数存于此
	int lev;							// 层号
};

