#include<stdio.h>
extern int err;
void Error(int i,char *s)
{
	err++;
	printf("Error in %d:%s\n",i,s);
}
