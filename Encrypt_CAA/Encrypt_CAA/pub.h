#include "cryptopp/rsa.h"
#include "cryptopp/aes.h"
#include "cryptopp/des.h"
#include "cryptopp/Hex.h"
#include "cryptopp/filters.h"
#include "cryptopp/modes.h"
#include "cryptopp/base64.h"
#include "cryptopp/config.h"
#include "cryptopp/stdcpp.h"
#include <stdio.h>
#include <iostream>
#include <fstream>
#include <sstream>

#pragma comment(lib,"cryptlib.lib")

using namespace std;
using namespace CryptoPP;
//const  unsigned char InitKey[16]={'F','1','O','2','X','3','C','4','O','5','N','6','N','7','k','8'};
byte b_Key[16] = {70,49,58,50,88,51,67,52,48,53,78,54,78,55,107,56};
byte i_IV[16] = {4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0};

void encrypt(char *plainText, char *outStr)
{
	string cipherText;
	CBC_Mode<CryptoPP::AES>::Encryption encryption(b_Key, sizeof(b_Key),(byte *)i_IV);
	StringSource encryptor(plainText, true,
		new StreamTransformationFilter(encryption,
		new CryptoPP::Base64Encoder(new StringSink(cipherText),
		BlockPaddingSchemeDef::BlockPaddingScheme::DEFAULT_PADDING,false)));
	memcpy(outStr,cipherText.c_str(),cipherText.length());
	outStr[cipherText.length()]='\0';
}

void decrypt(char *incipherText, char *outStr)
{
	string cipherText(incipherText);
	string decryptedText;
	CBC_Mode<CryptoPP::AES>::Decryption decryption(b_Key,sizeof(b_Key),(byte *)i_IV);
	StringSource decryptor(incipherText,true,
		new Base64Decoder(
		new StreamTransformationFilter(decryption, new StringSink(decryptedText),
		BlockPaddingSchemeDef::BlockPaddingScheme::DEFAULT_PADDING,false)));
	memcpy(outStr,decryptedText.c_str(),decryptedText.length());
	outStr[decryptedText.length()]='\0';
}
