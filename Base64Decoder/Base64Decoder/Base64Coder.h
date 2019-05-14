// Base64Coder.h: interface for the Base64Coder class.
//
//////////////////////////////////////////////////////////////////////
#include <atlstr.h>

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class Base64Coder  
{

private :
	
	char* buf;
	int size ;
	static int BinSearch(char p);
	static int AllSearch(char p);
	void allocMem(int NewSize);
	
public :
	Base64Coder();
	~Base64Coder();
	static char ch64[];
	static char ch64Key[];

	char* encode(CString& message);
	char* encode(char* buffer,int buflen);
	char* decode(char* buffer,int Length,int *n);
	CStringA encode64(const char* bytes_to_encode,int in_len);
	CStringA decode64(const char* buffer,int Length);
	int KeyLen;
	char* encodeKey(char* buffer,int buflen);
	char* decodeKey(char* buffer,int Length,int *n);
	

};

