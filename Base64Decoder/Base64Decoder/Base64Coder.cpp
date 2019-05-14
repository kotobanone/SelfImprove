// Base64Coder.cpp: implementation of the Base64Coder class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "Base64Coder.h"
#include <time.h>

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

char Base64Coder::ch64[] = {
	'A','B','C','D','E','F','G','H','I','J','K','L','M','N',
		'O','P','Q','R','S','T','U','V','W','X','Y','Z',
		'a','b','c','d','e','f','g','h','i','j','k','l','m','n',
		'o','p','q','r','s','t','u','v','w','x','y','z',
		'0','1','2','3','4','5','6','7','8','9','+','/','='
};

char Base64Coder::ch64Key[] = {
	'p','A','0','B','a','C','D','E','F','G','H','L','M','N',
		'O','4','P','Q','R','T','U','V','W','X','Y','Z',
		'b','c','9','d','e','f','I','g','h','J','i','j','l','K',
		'm','n','o','q','r','s','t','u','v','w','x','y',
		'z','1','2','k','3','5','S','6','7','8','-','_','!'
};


Base64Coder::Base64Coder()
{
        buf = NULL;
        size = 0 ;
}

Base64Coder::~Base64Coder()
{
        if ( buf )
                free(buf);
}

void Base64Coder::allocMem(int NewSize)
{
        if ( buf )
                buf = (char*)realloc(buf,NewSize);
        else
                buf = (char*)malloc(NewSize);
        memset(buf,0,NewSize);
}

//char* Base64Coder::encode(CString& buffer)
//{
//        //return encode(buffer.c_str(),buffer.length());
//	//const char *p='a';
//	//char *retStr = (LPCTSTR)buffer;
//	const size_t newsizea = buffer.GetLength()+1;
//	char *nstringa = new char[newsizea];
//	strcpy_s(nstringa, newsizea,*buffer);
//	return nstringa;
//}

char* Base64Coder::encodeKey(char* buffer,int buflen)
{
	encode(buffer,buflen);
	
	int i,j;
	for(i=0; i<KeyLen; i++)
	{		
		for(j=0; j< 65; j++)
		{
			if( buf[i] == ch64[j])
			{
				buf[i] = ch64Key[j];
				break;
			}
		}
	}

	return buf;
}

char* Base64Coder::decodeKey(char* buffer,int Length,int *n)
{
	int length = Length;
	if ( length%4 != 0 )
		return NULL;
	
	char * pbuf = NULL;
	pbuf = (char*)malloc(length) ;
	memset(pbuf,0x00,length);
	
	int i,j;
	for(i=0; i<length; i++)
	{		
		for(j=0; j< 65; j++)
		{
			if( buffer[i] == ch64Key[j] )
			{
				pbuf[i] = ch64[j];
				break;
			}
		}
	}

	decode(pbuf,length,n);
	
	free(pbuf);
	pbuf = NULL;

	return buf;
}

char* Base64Coder::encode(char* buffer,int buflen)
{
        int nLeft =  3 - buflen%3 ;
        //誹BASE64衡猭????Θ?4/3
        //┮?だ皌∽length*4/31??才?(0)
		KeyLen = ( buflen + nLeft )*4/3;
        allocMem(( buflen + nLeft )*4/3+1);
		
        //???秖
        char ch[4];
        int i ,j;
        for ( i = 0 ,j = 0; i < ( buflen - buflen%3 );  i += 3,j+= 4 )
        {
                ch[0] = (char)((buffer[i]&0xFC) >> 2 );
                ch[1] = (char)((buffer[i]&0x03) << 4 | (buffer[i+1]&0xF0) >> 4 );
                ch[2] = (char)((buffer[i+1]&0x0F) << 2 | (buffer[i+2]&0xC0) >> 6 );
                ch[3] = (char)((buffer[i+2]&0x3F));
                //琩????????才
                buf[j] = ch64[ch[0]];
                buf[j+1] = ch64[ch[1]];
                buf[j+2] = ch64[ch[2]];
                buf[j+3] = ch64[ch[3]];
        }
        
        if ( nLeft == 2 )
        {
                ch[0] = (char)((buffer[i]&0xFC) >> 2);
                ch[1] = (char)((buffer[i]&0x3) << 4 );
                ch[2] = 64;
                ch[3] = 64;

                //琩????????才
                buf[j] = ch64[ch[0]];
                buf[j+1] = ch64[ch[1]];
                buf[j+2] = ch64[ch[2]];
                buf[j+3] = ch64[ch[3]];
        }
        else if ( nLeft == 1 )
        {
                ch[0] = (char)((buffer[i]&0xFC) >> 2 );
                ch[1] = (char)((buffer[i]&0x03) << 4 | (buffer[i+1]&0xF0) >> 4 );
                ch[2] = (char)((buffer[i+1]&0x0F) << 2 );
                ch[3] = 64;

                //琩????????才
                buf[j] = ch64[ch[0]];
                buf[j+1] = ch64[ch[1]];
                buf[j+2] = ch64[ch[2]];
                buf[j+3] = ch64[ch[3]];
        }

		
        return buf;
}

char* Base64Coder::decode(char* buffer,int Length,int *n)
{
        int length = Length;
        if ( length%4 != 0 )
                return NULL;
		
		
        allocMem(length*3/4 + 1);

        char p;
        char ch[4];
        int i , j ;
        for ( i = 0 , j = 0 ; i < length ; i += 4 , j+= 3)
        {
                for ( int z = 0 ; z < 4 ; z++)
                {
                        //ノ2だ猭琩т
                        p = (char)BinSearch(buffer[i+z]);
                        if ( p == -1 )
                                return NULL;
                        ch[z] = p;
                }

                buf[j] = (char)((ch[0]&0x3F) << 2 | (ch[1]&0x30) >> 4 );
                buf[j+1] = (char)((ch[1]&0xF) <<4 | (ch[2]&0x3C) >>2 );
                buf[j+2] = (char)((ch[2]&0x03) << 6 | (ch[3]&0x3F));
        }
		
		*n=j+2;
        return buf;
		
}

static inline bool is_base64(unsigned char c)
{
	return (isalnum(c)||(c=='-')||(c=='_'));
}

//ノだ猭琩тpch64??い竚狦тぃ?-1
int Base64Coder::BinSearch(char p)
{
        if ( p >= 'A' && p <= 'Z' )
                return (p - 'A');
        else if ( p >= 'a' && p <= 'z' )
                return (p - 'a' + 26);
        else if ( p >= '0' && p <= '9' )
                return (p - '0' + 26 + 26);
        else if ( p == '+' )
                return 62;
        else if ( p == '/' )
                return 63;
        else if ( p == '=' )
                return 64;
        return -1;
}

int Base64Coder::AllSearch(char p)
{
	if ( p == '#' )
		return -1;

	int index = 0;

	for(index = 0; index <64; index++)
	{
		if(p==ch64Key[index])
		{
			break;
		}
	}
	if(index==64)
	{
		return -1;
	}

    return index;
}

CStringA Base64Coder::encode64(const char* bytes_to_encode,int in_len)
{
	CStringA ret;
		int i=0;
		int j=0;
		unsigned char char_array_3[3]={0};
		unsigned char char_array_4[4]={0};

		while(in_len--)
		{
			char_array_3[i++] = *(bytes_to_encode++);
			if(i==3){
				char_array_4[0] = (char_array_3[0] & 0xfc)>>2;
				char_array_4[1] = ((char_array_3[0] & 0x03)<<4)+((char_array_3[1]&0xf0)>>4);
				char_array_4[2] = ((char_array_3[1] & 0x0f)<<2)+((char_array_3[2]&0xc0)>>6);
				char_array_4[3] = (char_array_3[2] & 0x3f);
				for(i=0; i<4; i++){
					ret+=ch64Key[char_array_4[i]];
				}
				i=0;
			}
			
		}
		if(i)
		{
			for(j=i;j<3;j++)
			{
				char_array_3[j]='\0';
			}
			char_array_4[0] = (char_array_3[0] & 0xfc) >> 2;
			char_array_4[1] = ((char_array_3[0] & 0x03) << 4) + ((char_array_3[1] & 0xf0) >> 4);
			char_array_4[2] = ((char_array_3[1] & 0x0f) << 2) + ((char_array_3[2] & 0xc0) >> 6);
			char_array_4[3] = (char_array_3[2] & 0x3f);
			for(j=0;j<i+1;j++)
			{
				ret+=ch64Key[char_array_4[j]];
			}
			while(i++ < 3)//ぃìよ干'#'
			{
				ret+='#';
			}
		}

		return ret;
}

CStringA Base64Coder::decode64(const char* buffer,int Length)
{
    int in_len = Length;
	int i=0;
	int j=0;
	int in_ =0;
	unsigned char char_array_3[3]={0};
	unsigned char char_array_4[4]={0};
	CStringA ret;
	char p;
	while(in_len-- && (buffer[in_]!='=')&&is_base64(buffer[in_]))
	{
		char_array_4[i++] = (unsigned char)(buffer[in_++]);  //*(bytes_to_encode++)
		
		if(i==4){
			for(i=0; i<4; i++)
			{
				//char_array_4[i] = base64_char.find(char_array_4[i]);
				 p = (char)AllSearch(buffer[(in_/4-1)*4+i]);
                        if ( p == -1 ){
							printf("BinSearch Error!");
						}
                        char_array_4[i] = p;
			}
			char_array_3[0] = (char_array_4[0]<<2)+((char_array_4[1] & 0x30)>>4);
			char_array_3[1] = ((char_array_4[1] & 0xf) << 4)+((char_array_4[2] & 0x3c)>>2);
			char_array_3[2] = ((char_array_4[2] & 0x3) << 6)+char_array_4[3];

			for(i=0; i<3; i++)
			{
				ret += char_array_3[i];
			}
			i=0;
		}
	}
	if(i){
		for(j=i;j<4;j++)
		{
			char_array_4[j]=0;
		}
		for(j=0;j<4;j++)
		{
			//char_array_4[j] = base64_char.find(char_array_4[j]);
			p = (char)AllSearch(buffer[(in_/4)*4+j]);
                        if ( p == -1 ){
							char_array_4[j]=(unsigned char)0;
						}
						else
						{
                        char_array_4[j] = p;
						}
		}
		char_array_3[0] = (char_array_4[0]<<2)+((char_array_4[1] & 0x30)>>4);
		char_array_3[1] = ((char_array_4[1] & 0xf) << 4)+((char_array_4[2] & 0x3c)>>2);
		char_array_3[2] = ((char_array_4[2] & 0x3) << 6)+char_array_4[3];
		for(j=0;j<i-1; j++)
		{
			ret += char_array_3[j];
		}
	}
	return ret;
}
