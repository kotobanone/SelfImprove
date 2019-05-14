// Base64Str2ByteArray.cpp : 定義主控台應用程式的進入點。
//

#include "stdafx.h"
#include "base64.h"
#include <iostream>
using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
	const string s = 
    "René Nyffenegger\n"
    "http://www.renenyffenegger.ch\n"
    "passion for data\n";

  string encoded = base64_encode(reinterpret_cast<const unsigned char*>(s.c_str()), s.length());
  string decoded = base64_decode(encoded);

  cout << "encoded: " << endl << encoded << endl << endl;
  cout << "decoded: " << endl << decoded << endl;


  // Test all possibilites of fill bytes (none, one =, two ==)
  // References calculated with: https://www.base64encode.org/

  string rest0_original = "abc";
  string rest0_reference = "YWJj";

  string rest0_encoded = base64_encode(reinterpret_cast<const unsigned char*>(rest0_original.c_str()),
    rest0_original.length());
  string rest0_decoded = base64_decode(rest0_encoded);

  cout << "encoded:   " << rest0_encoded << endl;
  cout << "reference: " << rest0_reference << endl;
  cout << "decoded:   " << rest0_decoded << endl << endl;


  string rest1_original = "01050006FF006C3B";
  //string rest1_reference = "YWJjZA==";

  string rest1_encoded = base64_encode(reinterpret_cast<const unsigned char*>(rest1_original.c_str()),
    rest1_original.length());
  string rest1_decoded = base64_decode(rest1_encoded);

  cout << "encoded:   " << rest1_encoded << endl;
  //cout << "reference: " << rest1_reference << endl;
  cout << "decoded:   " << rest1_decoded << endl << endl;
	vector<unsigned char> tmpBytes = HexToBytes(rest1_decoded);
	int len = tmpBytes.size(); 
  for(int i=0;i<len;i++){
		cout << (unsigned int)tmpBytes[i] << endl;
	}

  string rest2_original = "abcde";
  string rest2_reference = "YWJjZGU=";

  string rest2_encoded = base64_encode(reinterpret_cast<const unsigned char*>(rest2_original.c_str()),
    rest2_original.length());
  string rest2_decoded = base64_decode(rest2_encoded);

  cout << "encoded:   " << rest2_encoded << endl;
  cout << "reference: " << rest2_reference << endl;
  cout << "decoded:   " << rest2_decoded << endl << endl;
    int c;
  cin>>c;
  return 0;
}



