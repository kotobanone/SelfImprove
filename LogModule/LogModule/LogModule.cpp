// LogModule.cpp : 定義主控台應用程式的進入點。
//

#include "stdafx.h"
#include "SD_Log.h"
//#include <windows.h>
#include <afx.h>
#include <iostream>

#define MAX_PATH 260

SelfDefineLog myLog;
void FuncB()
{
	myLog.WRITELOG(__FILE__,__FUNCTION__,__LINE__,"It is in FuncB");
}

int _tmain(int argc, _TCHAR* argv[])
{	
	CString des;
	cout << "This is a Test of Log Module!"<<endl;
	::GetCurrentDirectory(MAX_PATH,des.GetBuffer(MAX_PATH));  //取得專案資料夾
	des.ReleaseBuffer();
	des+="\\Logs\\";
	cout << "PATH:" << des.GetBuffer(0) <<endl;
	//bool iResult = myLog.LogPath(des.GetBuffer(0));
	/*cout << "result is "<<iResult<<endl;*/
	int i=0;
	while(i<10)
	{
		cout<<"Write..."<<endl;
		myLog.WRITELOG(__FILE__,__FUNCTION__,__LINE__,"This is a Test");
		FuncB();
		Sleep(1000);
		i++;
	}
	return 0;
}

