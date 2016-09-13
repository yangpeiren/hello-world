#include "lexical_analyzer.h"
#include "headeclare.h"
#include "grammar_semantic.h"
#include "symbol_table.h"
#include "mainproc.h"

void main()
{
	char s[VARLENGTH+1];
	int ch;
	printf("请输入您的源文件文件名:\n");
	scanf("%s",s);
	if ((infp=fopen(s,"r"))==NULL)
	{
		printf("不存在该文件\t或该文件不再程序文件夹里\n");
		return;
	}
	printf("请输入您要输出的文件文件名:\n");
	scanf("%s",s);
	if ((outfp=fopen(s,"w"))==NULL)
	{
		printf("文件创建错误\n");
		return;
	}
	maininitial();
	Program();
	printf("Total %d errors!\n",err);
	if(err==0)
		printf("编译成功!\n\nP-CODE输出到:%s\n\n\n是否要运行程序(y/n):\n\n",s);
	else goto L0;
	while((ch = getchar())=='\n')
		;
	if(ch!='y')
		return;
	fclose(infp);
	fclose(outfp);
	printf("\n\n程序运行结果如下:\n\n");
	if(err==0)
		Interpreter(s);
L0:
	printf("\n\n请按任意键继续...\n\n");
	getchar();
	if(getchar())
		exit(0);
}
