#include "cv.h"
#include "cxcore.h"
#include "highgui.h"
//#if _DEBUG
//#pragma comment(lib,"cv210d.lib")
//#pragma comment(lib,"highgui210d.lib")
//#pragma comment(lib,"cxcore210d.lib")
//#else
//#pragma comment(lib,"cv210.lib")
//#pragma comment(lib,"highgui210.lib")
//#pragma comment(lib,"cxcore210.lib")
//#endif

#pragma comment(lib,"cv.lib")
#pragma comment(lib,"highgui.lib")
#pragma comment(lib,"cxcore.lib")
typedef struct doubleMat{
	CvMat *fx;
	CvMat *fy;
}dmat;
dmat GVF(CvMat *f,float mu,int ITER);
CvMat *BoundMirrorExpand(CvMat *a);
CvMat *BoundMirrorEnsure(CvMat *a);
CvMat *BoundMirrorShrink(CvMat *a);
dmat gradient(CvMat *f);
void cvDel2(CvMat *src,CvMat *dst);
dmat snakedeform(CvMat *x,CvMat *y,double alpha,double beta,double gamma,double kappa,CvMat *fx,CvMat *fy,int ITER);
CvMat *interp2(CvMat *fx,CvMat *x,CvMat *y);
void main()
{
	CvSize hw;
	dmat uv,xy;
	IplImage *face = cvLoadImage("obama.jpg");
	IplImage *final = cvCreateImage(cvGetSize(face),face->depth,1);
	IplImage *Edge = cvCreateImage(cvGetSize(face),face->depth,1);	
	cvCvtColor(face,final,CV_BGR2GRAY);
	cvCanny(final,Edge,180,185,3);
	hw = cvGetSize(Edge);
	CvMat* dst = cvCreateMat(hw.height,hw.width,CV_32FC1);
	uv.fx=cvCreateMat(hw.height,hw.width,face->depth);
	uv.fy=cvCreateMat(hw.height,hw.width,face->depth);
	cvConvert(Edge,dst);
	uv = GVF(dst,(float)0.2, 80);
	CvMat *mag = cvCreateMat(uv.fx->height,uv.fx->width,uv.fx->type);
	CvMat *dst1 = cvCreateMat(uv.fx->height,uv.fx->width,uv.fx->type);
	CvMat *dst2 = cvCreateMat(uv.fx->height,uv.fx->width,uv.fx->type);
	cvMul(uv.fx,uv.fx,dst1,1);
	cvMul(uv.fy,uv.fy,dst2,1);
	cvAdd(dst1,dst2,mag,0);
	for(int i = 0; i < mag->height;i++)
		for(int j = 0;j < mag->width;j++)
			cvmSet(mag,i,j,cvSqrt(cvmGet(mag,i,j))+1E-10);
	CvMat *px = cvCreateMat(uv.fx->height,uv.fx->width,uv.fx->type);
	CvMat *py = cvCreateMat(uv.fx->height,uv.fx->width,uv.fx->type);
	cvDiv(uv.fx,mag,px,1);
	cvDiv(uv.fy,mag,py,1);
	CvMat *x1 = cvCreateMat(1,(int)(hw.width-20)/5,CV_32FC1);
	CvMat *y1 = cvCreateMat(1,(int)(hw.width-20)/5,CV_32FC1);
	CvMat *x2 = cvCreateMat((int)(hw.width-20)/5,1,CV_32FC1);
	CvMat *y2 = cvCreateMat((int)(hw.width-20)/5,1,CV_32FC1);
	for(int i = 0,j = 20;i<x1->width;i++,j = j + 5)
		cvmSet(x1,0,i,j);
	for(int i = 0;i<y1->width;i++)
		cvmSet(y1,0,i,2);
	cvT(x1,x2);
	cvT(y1,y2);
	CvMat* t = cvCreateMat(hw.height,hw.width,CV_32FC1);
	CvSize s = cvSize(t->width,t->height);
	cvNamedWindow("Test",CV_WINDOW_AUTOSIZE);
	for(int i = 1; i <= 15;i++)
	{
		xy = snakedeform(x2,y2,0.05,0,1,0.6,px,py,5);	
		for(int j = 0;j < xy.fx->height;j++)
		{
			cvCopy(dst,t,0);
			if((int)cvmGet(xy.fx,j,0)<t->width && (int)cvmGet(xy.fy,j,0)<t->height)
				cvmSet(t,(int)cvmGet(xy.fx,j,0),(int)cvmGet(xy.fy,j,0),189);
		}
		cvCopy(xy.fx,x2,0);
		cvCopy(xy.fy,y2,0);
		cvShowImage("Test",dst);
		cvWaitKey(0);
	}
	
	cvReleaseImage(&Edge);
	cvReleaseImage(&Edge);
	cvReleaseMat(&dst);
	cvReleaseMat(&uv.fx);
	cvReleaseMat(&uv.fy);
	cvReleaseMat(&xy.fx);
	cvReleaseMat(&xy.fy);
	cvReleaseMat(&mag);
	cvReleaseMat(&dst1);
	cvReleaseMat(&dst2);
	cvReleaseMat(&x1);
	cvReleaseMat(&x2);
	cvReleaseMat(&y1);
	cvReleaseMat(&y2);
	cvReleaseMat(&t);
	cvDestroyWindow("Test");
}

dmat GVF(CvMat *f,float mu,int ITER)
{
	CvSize mn;
	double *max,*min;
	double temp;
	dmat dm,uv;
	mn = cvGetSize(f);
	CvScalar cvm;
	CvMat *dst = cvCreateMat(mn.height,mn.width,f->type);
	CvMat *tempp = cvCreateMat(1,1,CV_32FC1);
	min = (double *)malloc(sizeof(double *));
	max = (double *)malloc(sizeof(double *));
	cvMinMaxLoc(f,min,max);
	temp  = *max - *min;
	cvm.val[0] = *min;
	cvSubS(f,cvm,dst,0);
	for(int i = 0;i < mn.height; i++)
		for(int j = 0;j < mn.width;j++)
			cvmSet(dst,i,j,cvmGet(dst,i,j)/temp);

	dst = BoundMirrorExpand(dst);
	dm = gradient(dst);
	uv.fx = cvCreateMat(dm.fx->height,dm.fx->width,f->type);
	uv.fy = cvCreateMat(dm.fy->height,dm.fy->width,f->type);
	cvCopy(dm.fx,uv.fx,0);
	cvCopy(dm.fy,uv.fy,0);
	temp = (double)mu*4;
	CvMat *t1 = cvCreateMat(dm.fx->height,dm.fx->width,f->type);
	CvMat *t2 = cvCreateMat(dm.fx->height,dm.fx->width,f->type);
	CvMat *tt1 = cvCreateMat(dm.fx->height,dm.fx->width,f->type);
	CvMat *tt2 = cvCreateMat(dm.fx->height,dm.fx->width,f->type);
	for(int i = 1;i <= ITER;i++)
	{
		cvDel2(uv.fx,t1);
		cvDel2(uv.fy,t2);
		for(int a = 0;a < uv.fx->height;a++)
			for(int b = 0;b <uv.fx->width;b++)
				cvmSet(uv.fx,a,b,cvmGet(uv.fx,a,b)+cvmGet(t1,a,b)*temp);
		for(int a = 0;a < uv.fy->height;a++)
			for(int b = 0;b <uv.fy->width;b++)
				cvmSet(uv.fy,a,b,cvmGet(uv.fy,a,b)+cvmGet(t2,a,b)*temp);
		fprintf(stdout,"%3d",i);
		if(i%20 == 0)
			fprintf(stdout,"\n");
	}
	fprintf(stdout,"\n");
	uv.fx = BoundMirrorShrink(uv.fx);
	uv.fy = BoundMirrorShrink(uv.fy);
	cvReleaseMat(&t1);
	cvReleaseMat(&t2);
	cvReleaseMat(&tt1);
	cvReleaseMat(&tt2);
	return uv;
}
CvMat *BoundMirrorExpand(CvMat *a)
{
	CvSize mn = cvSize(a->width,a->height);
	int m = mn.width;
	int n = mn.height;
	CvMat *b = cvCreateMat(mn.height+2,mn.width+2,a->type);
	cvSetZero(b);
	for(int x = 1;x <n+1;x++)
		for(int y = 1;y <m+1;y++)
			cvmSet(b,x,y,cvmGet(a,x-1,y-1));
	cvmSet(b,0,0,cvmGet(b,2,2));
	cvmSet(b,0,m+1,cvmGet(b,2,m-1));
	cvmSet(b,n+1,0,cvmGet(b,n-1,2));
	cvmSet(b,n+1,m+1,cvmGet(b,n-1,m-1));
	for(int y = 1;y <m+1;y++)
	{
		cvmSet(b,0,y,cvmGet(b,2,y));
		cvmSet(b,n+1,y,cvmGet(b,n-1,y));
	}
	for(int x = 1;x <n+1;x++)
	{
		cvmSet(b,x,0,cvmGet(b,x,2));
		cvmSet(b,x,m+1,cvmGet(b,x,m-1));
	}
	return b;
}
CvMat *BoundMirrorEnsure(CvMat *a)
{
	CvSize mn = cvSize(a->width,a->height);
	int m = mn.width;
	int n = mn.height;
	if(mn.height<3||mn.width<3)
		fprintf(stdout,"either the number of rows or columns is smaller than 3");
	CvMat *b = cvCreateMat(mn.height,mn.width,a->type);
	cvCopy(a,b,0);
	cvmSet(b,0,0,cvmGet(b,2,2));
	cvmSet(b,0,m-1,cvmGet(b,2,m-3));
	cvmSet(b,n-1,0,cvmGet(b,n-3,2));
	cvmSet(b,n-1,m-1,cvmGet(b,n-3,m-3));
	for(int y = 1;y <=m-3;y++)
	{
		cvmSet(b,0,y,cvmGet(b,2,y));
		cvmSet(b,n-1,y,cvmGet(b,n-3,y));
	}
	for(int x = 1;x <=n-3;x++)
	{
		cvmSet(b,x,0,cvmGet(b,x,2));
		cvmSet(b,x,m-1,cvmGet(b,x,m-3));
	}
	return b;
}
CvMat *BoundMirrorShrink(CvMat *a)
{
	CvSize mn = cvSize(a->width,a->height);
	int m = mn.width;
	int n = mn.height;
	CvMat *b = cvCreateMat(mn.height-2,mn.width-2,a->type);
	for(int x = 1;x <=n-2;x++)
		for(int y = 1;y <=m-2;y++)
			cvmSet(b,x-1,y-1,cvmGet(a,x,y));
	return b;
}
dmat gradient(CvMat *f)
{
	dmat ret;
	ret.fx=cvCreateMat(f->height,f->width,f->type);
	ret.fy=cvCreateMat(f->height,f->width,f->type);
	CvMat *x = cvCreateMat(f->height,f->width,f->type);
	CvMat *y = cvCreateMat(f->height,f->width,f->type);
	for(int i = 0;i<f->rows;i++)
		for(int j = 0;j<f->cols-1;j++)
			cvmSet(x,i,j,cvmGet(f,i,j+1)-cvmGet(f,i,j));
	for(int i = 0;i<f->rows-1;i++)
		for(int j = 0;j<f->cols;j++)
			cvmSet(y,i,j,cvmGet(f,i+1,j)-cvmGet(f,i,j));

	for(int i = 0;i<f->rows;i++)
		cvmSet(ret.fx,i,0,cvmGet(x,i,0));
	for(int i = 0;i<f->rows;i++)
		for(int j = 1;j<f->cols-1;j++)
			cvmSet(ret.fx,i,j,(cvmGet(x,i,j-1)+cvmGet(x,i,j))/2);
	for(int i = 0;i<f->rows;i++)
		cvmSet(ret.fx,i,f->cols-1,cvmGet(x,i,f->cols-1));

	for(int j = 0;j<f->cols;j++)
		cvmSet(ret.fy,0,j,cvmGet(y,0,j));
	for(int i = 1;i<f->rows-1;i++)
		for(int j = 0;j<f->cols;j++)
			cvmSet(ret.fy,i,j,(cvmGet(y,i-1,j)+cvmGet(y,i,j))/2);
	for(int j = 0;j<f->cols;j++)
		cvmSet(ret.fy,f->rows-1,j,cvmGet(y,f->rows-1,j));
	cvReleaseMat(&x);
	cvReleaseMat(&y);
	return ret;
}
void cvDel2(CvMat *src,CvMat *dst)
{
	int i,j;
 for(i=0;i<src->height;i++)
 {
  for(j=0;j<src->width;j++)
  {
   if(i==0 && j==0)
   {
    cvSetReal1D(dst,i*src->width+j,(2*cvGetReal1D(src,i*src->width+j)+cvGetReal1D(src,(i+2)*src->width+j)+cvGetReal1D(src,i*src->width+j+2))/4-(cvGetReal1D(src,(i+1)*src->width+j)+cvGetReal1D(src,i*src->width+j+1))/2);
   }
   else if(i==0 && j!=0 && j!=src->width-1)
   {
    cvSetReal1D(dst,i*src->width+j,(cvGetReal1D(src,i*src->width+j)+cvGetReal1D(src,(i+2)*src->width+j)+cvGetReal1D(src,i*src->width+j+1)+cvGetReal1D(src,i*src->width+j-1))/4-(cvGetReal1D(src,(i+1)*src->width+j)+cvGetReal1D(src,i*src->width+j))/2);
   }
   else if(i==0 && j==src->width-1)
   {
    cvSetReal1D(dst,i*src->width+j,(2*cvGetReal1D(src,i*src->width+j)+cvGetReal1D(src,(i+2)*src->width+j)+cvGetReal1D(src,i*src->width+j-2))/4-(cvGetReal1D(src,(i+1)*src->width+j)+cvGetReal1D(src,i*src->width+j-1))/2);
   }
   else if(i!=0 && i!=src->height-1 && j==src->width-1)
   {
    cvSetReal1D(dst,i*src->width+j,(cvGetReal1D(src,i*src->width+j)+cvGetReal1D(src,i*src->width+j-2)+cvGetReal1D(src,(i+1)*src->width+j)+cvGetReal1D(src,(i-1)*src->width+j))/4-(cvGetReal1D(src,i*src->width+j-1)+cvGetReal1D(src,i*src->width+j))/2);
   }
   else if(i==src->height-1 && j==src->width-1)
   {
    cvSetReal1D(dst,i*src->width+j,(2*cvGetReal1D(src,i*src->width+j)+cvGetReal1D(src,(i-2)*src->width+j)+cvGetReal1D(src,i*src->width+j-2))/4-(cvGetReal1D(src,(i-1)*src->width+j)+cvGetReal1D(src,i*src->width+j-1))/2);
   }
   else if(i==src->height-1 && j!=0 && j!=src->width-1)
   {
    cvSetReal1D(dst,i*src->width+j,(cvGetReal1D(src,i*src->width+j)+cvGetReal1D(src,(i-2)*src->width+j)+cvGetReal1D(src,i*src->width+j+1)+cvGetReal1D(src,i*src->width+j-1))/4-(cvGetReal1D(src,(i-1)*src->width+j)+cvGetReal1D(src,i*src->width+j))/2);
   }
   else if(i==src->height-1 && j==0)
   {
    cvSetReal1D(dst,i*src->width+j,(2*cvGetReal1D(src,i*src->width+j)+cvGetReal1D(src,(i-2)*src->width+j)+cvGetReal1D(src,i*src->width+j+2))/4-(cvGetReal1D(src,(i-1)*src->width+j)+cvGetReal1D(src,i*src->width+j+1))/2);
   }
   else if(i!=src->height-1 && i!=0 && j==0)
   {
    cvSetReal1D(dst,i*src->width+j,(cvGetReal1D(src,(i-1)*src->width+j)+cvGetReal1D(src,(i+1)*src->width+j)+cvGetReal1D(src,i*src->width+j)+cvGetReal1D(src,i*src->width+j+2))/4-(cvGetReal1D(src,i*src->width+j)+cvGetReal1D(src,i*src->width+j+1))/2);
   }
   else
   {
    cvSetReal1D(dst,i*src->width+j,(cvGetReal1D(src,(i-1)*src->width+j)+cvGetReal1D(src,(i+1)*src->width+j)+cvGetReal1D(src,i*src->width+j-1)+cvGetReal1D(src,i*src->width+j+1))/4-cvGetReal1D(src,i*src->width+j));
   }
  }
 }
}
dmat snakedeform(CvMat *x,CvMat *y,double alpha,double beta,double gamma,double kappa,CvMat *fx,CvMat *fy,int ITER)
{
	int N = x->height;
	double a = beta;
	double b = -alpha - 4*beta;
	double c = 2*alpha +6*beta;
	double d =b;
	double e = beta;
	CvMat *A = cvCreateMat(N,N,CV_32FC1);
	dmat ret;

	for(int i = 0, j = 2;j<N;i++,j++)//A = diag(a(1:N-2),-2) + diag(a(N-1:N),N-2);
		cvmSet(A,i,j,a);
	cvmSet(A,N-2,0,a);
	cvmSet(A,N-1,1,a);

	for(int i = 0, j = 1;j<N;i++,j++)//A = A + diag(b(1:N-1),-1) + diag(b(N), N-1);
		cvmSet(A,i,j,b);
	cvmSet(A,N-1,0,b);

	for(int i = 0, j = 0;j<N;i++,j++)//A = A + diag(c);(A + gamma * diag(ones(1,N));
		cvmSet(A,i,j,c+gamma);

	for(int i = 1, j = 0;i<N;i++,j++)//A = A + diag(d(1:N-1),1) + diag(d(N),-(N-1));
		cvmSet(A,i,j,d);
	cvmSet(A,0,N-1,d);

	for(int i = 2, j = 0;i<N;i++,j++)//A = A + diag(e(1:N-2),2) + diag(e(N-1:N),-(N-2));
		cvmSet(A,i,j,e);
	cvmSet(A,0,N-2,e);
	cvmSet(A,1,N-1,e);

	CvMat *invAI = cvCreateMat(N,N,CV_32FC1);
	CvMat *vfx = cvCreateMat(x->rows,x->cols,CV_32FC1);
	CvMat *vfy = cvCreateMat(x->rows,x->cols,CV_32FC1);
	ret.fx = cvCreateMat(x->rows,x->cols,CV_32FC1);
	ret.fy = cvCreateMat(x->rows,x->cols,CV_32FC1);
	cvInvert(A,invAI,0);
	for(int i = 1;i<=ITER;i++)
	{
		CvScalar val = cvScalarAll(0);
		vfx = interp2(fx,x,y);
        vfy = interp2(fy,x,y);
		for(i = 0;i<x->rows;i++)
			cvmSet(x,i,0,cvmGet(x,i,0)*gamma+cvmGet(vfx,i,0)*kappa);
		for(i = 0;i<y->rows;i++)
			cvmSet(y,i,0,cvmGet(y,i,0)*gamma+cvmGet(vfy,i,0)*kappa);
		cvMatMul(invAI,x,ret.fx);
		cvMatMul(invAI,y,ret.fy);
		cvCopy(ret.fx,x,0);
		cvCopy(ret.fy,y,0);
	}
	cvReleaseMat(&A);
	cvReleaseMat(&invAI);
	cvReleaseMat(&vfx);
	cvReleaseMat(&vfy);
	return ret;
}
CvMat *interp2(CvMat *fx,CvMat *x,CvMat *y)
{
	CvSize s = cvSize(fx->width,fx->height);
	CvMat *ndx = cvCreateMat(x->rows,x->cols,CV_32FC1);
	CvMat *ret = cvCreateMat(x->rows,x->cols,CV_32FC1);
	for(int i = 0;i<x->rows;i++)
		cvmSet(ndx,i,0,cvmGet(y,i,0)+(cvmGet(x,i,0)-1)*s.width);
	for(int i = 0;i<x->rows;i++)
	{
		int a = (int)cvmGet(ndx,i,0)%s.width;
		int b = cvmGet(ndx,i,0)/s.width;
		if(a<fx->height&&b<fx->width)
			cvmSet(ret,i,0,cvmGet(fx,a,b));
	}
	return ret;
}