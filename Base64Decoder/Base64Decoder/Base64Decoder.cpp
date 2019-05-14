// Base64Decoder.cpp : 定義主控台應用程式的進入點。
//

#include "stdafx.h"
#include "Base64Coder.h"
#include <fstream>
#include <string>
#include <iostream>
//#include <cstring>
using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
	Base64Coder B64;
	string encryptStr = "UEGudljiZXF8TVNaPU8RPR3wO1Ag9kNkckGi4UZueDNucl5pMTUwMTsV92VxFaja4UVBPVGaXzsGT1NLOzQgdDaIU2819lNj4UVBPVGaPisfRzj4Uzr6UDVx92jyd0ATZWN19ljzeRAGclZu4VQxdWU!";
	string decryptStr = "Provider=SQLOLEDB.1;Password=getdata;Persist Security Info=True;User ID=getdata;Initial Catalog=CRCAIOII20020115;Data Source=10.148.14.90,3000";
	//CString des;

	//::GetCurrentDirectory(MAX_PATH,des.GetBuffer(MAX_PATH));  //取得專案資料夾
	//des.ReleaseBuffer();
	//des+="\\main.ini";

	std::ofstream out("out.txt");
    std::streambuf *coutbuf = std::cout.rdbuf(); //save old buf
    std::cout.rdbuf(out.rdbuf()); //redirect std::cout to out.txt!

	char info[256],buf[256], * pbuf,* obuf; 
	int len,n;
	len = encryptStr.length();

	pbuf = (char*)B64.decodeKey((char*)encryptStr.c_str(),len,&n);
	//strcpy( buf ,pbuf);
	cout << pbuf <<endl;

	obuf = (char*)B64.encodeKey((char*)decryptStr.c_str(),decryptStr.length());
	//CString Initmp = B64.encodeKey((char*)decryptStr.c_str(),decryptStr.length());
	cout << obuf <<endl; 
	//WritePrivateProfileString(L"main",L"lisence",Initmp,des);
	cin >> n;


	return 0;
}

