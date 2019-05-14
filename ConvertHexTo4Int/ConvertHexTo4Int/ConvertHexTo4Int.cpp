// ConvertHexTo4Int.cpp : 定義主控台應用程式的進入點。
//

#include "stdafx.h"
#include <string>
#include <iostream>
using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
	printf("請輸入Soft_NO(Hex):\n");
	char inputArr[64];
	cin >> inputArr;
	string SoftID = string(inputArr);
	
	string str1 = SoftID.substr(0,2);
	string str2 = SoftID.substr(2,2);
	string str3 = SoftID.substr(4,2);
	string str4 = SoftID.substr(6,2);
	int num1 = std::stoi(str1, 0, 16);
	int num2 = std::stoi(str2, 0, 16);
	int num3 = std::stoi(str3, 0, 16);
	int num4 = std::stoi(str4, 0, 16);
	printf("%d,%d,%d,%d",num1,num2,num3,num4);
	int block=0;
	scanf_s("%d",block);
	return 0;
}

