// TimeHashTest.cpp : 定義主控台應用程式的進入點。
// 將時間戳記與mac地址做xor的加解密

#include "stdafx.h"
#include <atlstr.h>
#include <nb30.h>
#include <string.h>
#include <windows.h>
#include <time.h>
#include <atltime.h>
#include <winsock2.h>
#include <iphlpapi.h>
#pragma comment(lib,"IPHLPAPI.lib")

#pragma comment(lib,"Netapi32.lib")

typedef struct _ASTAT_ 
{ 
	ADAPTER_STATUS adapt; 
	NAME_BUFFER NameBuff [30]; 
}ASTAT, * PASTAT;

char ch64normal[] = {
	'A','B','C','D','E','F','G','H','I','J','K','L','M','N',
		'O','P','Q','R','S','T','U','V','W','X','Y','Z',
		'a','b','c','d','e','f','g','h','i','j','k','l','m','n',
		'o','p','q','r','s','t','u','v','w','x','y','z',
		'0','1','2','3','4','5','6','7','8','9','+','/' //,'='
};

char ch64[] = {
	'p','A','0','B','a','C','D','E','F','G','H','L','M','N',
		'O','4','P','Q','R','T','U','V','W','X','Y','Z',
		'b','c','9','d','e','f','I','g','h','J','i','j','l','K',
		'm','n','o','q','r','s','t','u','v','w','x','y',
		'z','1','2','k','3','5','S','6','7','8','-','_' //,'!'
};
char* buf;
int KeyLen;
void allocMem(int NewSize)
{
        if ( buf )
                buf = (char*)realloc(buf,NewSize);
        else
                buf = (char*)malloc(NewSize);
        memset(buf,0,NewSize);
}
//尋找對應的字元在ch64[]中的位置。如果找不到返回-1
int BinSearch(char p)
{
	if ( p == '#' )
		return -1;

	int index = 0;

	for(index = 0; index <64; index++)
	{
		if(p==ch64[index])
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

CStringA encode64(const char* bytes_to_encode,int in_len)
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
					ret+=ch64[char_array_4[i]];
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
				ret+=ch64[char_array_4[j]];
			}
			while(i++ < 3)//不足的地方補'='
			{
				ret+='#';
			}
		}

		return ret;
}
static inline bool is_base64(unsigned char c)
{
	return (isalnum(c)||(c=='-')||(c=='_'));
}

CStringA decode64(const char* buffer,int Length)
{
    int in_len = Length;  //16
	int i=0;
	int j=0;
	int in_ =0;
	unsigned char char_array_3[3]={0};
	unsigned char char_array_4[4]={0};
	CStringA ret;
	char p;
	while(in_len-- && (buffer[in_]!='=')&&is_base64(buffer[in_]))
	{
		printf("in_len==%d \n",in_len);
		char_array_4[i++] = (unsigned char)(buffer[in_++]);  //*(bytes_to_encode++)
		
		if(i==4){
			for(i=0; i<4; i++)
			{
				//char_array_4[i] = base64_char.find(char_array_4[i]);
				 p = (char)BinSearch(buffer[(in_/4-1)*4+i]);
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
				//printf("%d  ",char_array_3[i]);
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
			p = (char)BinSearch(buffer[(in_/4)*4+j]);
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
			printf("%d  ",char_array_3[j]);
		}
	}
	printf("\n");
	return ret;
}

unsigned char dyas[12] = {31,28,31,30,31,30,31,31,30,31,30,31};
unsigned char dyasleap[12] = {31,29,31,30,31,30,31,31,30,31,30,31};
int GetDays(int nYear,int nMonth,int nDate)
{
	int days=0,leap,i;
	leap = (nYear % 4 && nYear %100 !=0) || (nYear % 400 == 0) ;
	for(i=0; i<nMonth-1; i++)
	{
		if(leap)
			days += dyasleap[i];
		else
			days += dyas[i];
	}
	days += nDate;
	
	
	int n,m,k,daytem=0;
	n = (nYear-1) / 400;
	daytem = n*400*365 + 97*n;
	
	m = (nYear-1) % 400;
	k = m / 4 - m / 100;
	daytem += m* 365 + k;
	
	days += daytem;
	
	return days;
}

int InvolidDate(char* date)
{
	printf("InvolidDate Str = %s\n",date);
	int days=0,ndays,mdays;	
	int   nYear,nMonth,nDate; 
	sscanf_s(date,"%d-%d%-%d ",&nYear,&nMonth,&nDate);	
	printf("Target Year=%d\n",nYear);
	CTime t1=CTime::GetCurrentTime();
	int   mYear,   mMonth,   mDate;
	
	mYear = t1.GetYear() ;
	mMonth = t1.GetMonth() ;
	mDate = t1.GetDay() ;
	
	ndays = GetDays(nYear,nMonth,nDate);
	mdays = GetDays(mYear,mMonth,mDate);
	days = ndays - mdays;
	return days;
	
}

void GetMac(unsigned char *macAddress)
{
	char buf[1024];
	memset(buf,0x00,sizeof(buf));
	DWORD len = 0;
	//len = ReadLicenseFile(buf);
	//if(len == 0 || len == 0xFFFF) return 0x0;		
	UCHAR uRetCode;
	NCB ncb; LANA_ENUM lana_enum; BYTE MacAddress[6];
	CString macstr;
	memset( &ncb, 0, sizeof(ncb) ); ncb.ncb_command = NCBENUM; ncb.ncb_buffer = (unsigned char *) &lana_enum; ncb.ncb_length = sizeof(lana_enum);
	if ( Netbios( &ncb ) == 0 ) {
		for ( int i=0; i< lana_enum.length; ++i) {
			NCB ncb; ASTAT Adapter; 
			memset( &ncb, 0, sizeof(ncb) ); 
			ncb.ncb_command = NCBRESET; 
			ncb.ncb_lana_num = lana_enum.lana[i];  
			uRetCode = Netbios( &ncb ); 

			memset( &ncb, 0, sizeof(ncb) ); 
			ncb.ncb_command = NCBASTAT; 
			ncb.ncb_lana_num = lana_enum.lana[i];  
			strcpy((char *)ncb.ncb_callname ,"*               "); 
			ncb.ncb_buffer = (unsigned char *) &Adapter;
			ncb.ncb_length = sizeof(Adapter);

			if ( Netbios( &ncb )==0 ) {
				for(int i=0;i<5;i++){
				macAddress[i]= Adapter.adapt.adapter_address[i];
				}
				//memcpy(macAddress,Adapter.adapt.adapter_address,6); 
				break;
			}
		} 
	}
	//return macstr;
}

bool GetMacByGetAdaptersInfo(unsigned char *macAddress)
{
	bool ret = false;
	ULONG ulOutBufLen = sizeof(IP_ADAPTER_INFO);
	PIP_ADAPTER_INFO pAdapterInfo = (IP_ADAPTER_INFO*)malloc(sizeof(IP_ADAPTER_INFO));
	if(pAdapterInfo==NULL)
		return false;
	//Make an Initial call to GetAdaptersInfo to get the necessary size into the ulOutBufLen variable
	if(GetAdaptersInfo(pAdapterInfo, &ulOutBufLen)==ERROR_BUFFER_OVERFLOW)
	{
		free(pAdapterInfo);
		pAdapterInfo = (IP_ADAPTER_INFO*)malloc(ulOutBufLen);
		if(pAdapterInfo==NULL)
			return false;
	}
	if(GetAdaptersInfo(pAdapterInfo, &ulOutBufLen)==NO_ERROR)
	{
		for(PIP_ADAPTER_INFO pAdapter = pAdapterInfo; pAdapter!=NULL;pAdapter=pAdapter->Next)
		{
			//Be sure that is ethernet
			if(pAdapter->Type != MIB_IF_TYPE_ETHERNET)
				continue;
			//Be sure length of MAC address is 00-00-00-00-00-00
			if(pAdapter->AddressLength!=6)
				continue;

			for(int i=0;i<6;i++){
				macAddress[i]= pAdapter->Address[i];
			}
			ret = true;
		}
	}
	free(pAdapterInfo);
	return ret;
}

void encoder(unsigned char* src,int lengthofsrc ,unsigned char* macKey)
{
	for(int i=0;i<lengthofsrc; i++)
	{
		src[i] = src[i]^macKey[5]^macKey[4]^macKey[3]^macKey[2]^macKey[1]^macKey[0];
	}
}

void decoder(unsigned char* src,int lengthofsrc ,unsigned char* macKey)
{
	for(int i=0;i<lengthofsrc; i++)
	{
		src[i] = src[i]^macKey[5]^macKey[4]^macKey[3]^macKey[2]^macKey[1]^macKey[0];
	}
}

int _tmain(int argc, _TCHAR* argv[])
{
	int a = 0;
	CString des = "";
	::GetCurrentDirectory(MAX_PATH,des.GetBuffer(MAX_PATH));  //取得專案資料夾
	des.ReleaseBuffer();
	des+="\\config.ini";

	//=======================
	unsigned char MacAddress[6] = {0};
	//Change CTime To Cstring
	CTime EndTime;
	EndTime=CTime(2018,8,1,0,0,0);
	//printf("EndTime:%04d-%02d-%02d\n",EndTime.GetYear(),EndTime.GetMonth(), EndTime.GetDay());

	//Change String To CTime
	CStringA StartTime("2017-11-01");
	const size_t newsizea = StartTime.GetLength()+1;
	char *nstringa = new char[newsizea];
	strcpy_s(nstringa, newsizea,StartTime);
	int year=0,month=0,day=0;
	sscanf(nstringa,"%d-%d-%d",&year,&month,&day);



	printf("StartTime:%04d/%02d/%02d\n",year,month,day);
	CTime tStartTime = CTime(year,month,day,0,0,0);

	//Get Mac Address
	//GetMac(MacAddress);
	bool isGetMac = GetMacByGetAdaptersInfo(MacAddress);
	printf("Get Mac Ret=%d \n",isGetMac);
	printf("%02X-%02X-%02X-%02X-%02X-%02X\n",MacAddress[0],MacAddress[1],MacAddress[2],MacAddress[3],MacAddress[4],MacAddress[5]);	
	//De / Encode Test
	encoder((unsigned char*)nstringa,newsizea-1,MacAddress);
	printf("Encoded:%s\n",nstringa);
	//加密後的長度為原本的4/3倍
	int tmplen=0;
	if((newsizea-1)%3>0)
	{
		tmplen=((newsizea-1)/3+1)*4+1;
	}
	else
	{
		tmplen=(newsizea-1)/3*4+1;
	}
	printf("TmpLeng==%d \n",tmplen);
	char *tmp = new char[tmplen];
	strcpy_s(tmp, tmplen, encode64(nstringa,newsizea-1));
	tmp[tmplen-1]='\0';

	
	printf("After 64Encode:%s\n",tmp);
	char info[128]={0};

	//===寫ini文件
	//CStringA tmpWriteStr(tmp);
	//LPCWSTR writeStr = tmpWriteStr.AllocSysString();
	//GetPrivateProfileString(LPCWSTR("main"),LPCWSTR("lesence"),NULL,LPWSTR(info),128,des);
	WritePrivateProfileString("main","lesence",tmp,des);
	//寫end

	//===讀ini文件
	//CStringA tmpReadStr(info);
	//LPWSTR readStr = tmpReadStr.AllocSysString();	
	//Sleep(200);
	//GetPrivateProfileString("main","lesence",NULL,readStr,128,des);
	Sleep(200);

	//讀end
	int de64len=(tmplen/4)*3+1;
	char *detmp = new char[de64len];
	char buffer[128]={0};
	//wcstombs(buffer,readStr,128);  //把LPWPSTR轉成char
	CString tmpStr(tmp);
	strcpy_s(detmp, de64len, decode64(tmp,tmplen-1)); //把讀出來的內容經base64解碼後，放進detmp中
	detmp[de64len-1]='\0';
	printf("After 64Decode:%s\n",detmp);
	
	decoder((unsigned char*)detmp,newsizea-1,MacAddress);  //將detmp再經過xor解碼
	printf("Decoded:%s\n",detmp);
	//===TestEnd
		
	int overdays=0;
	overdays = InvolidDate(detmp);
	printf("Left Days=%d\n",overdays);

	//用new生成的空間就一定要記得delete
	delete [] tmp;
	delete [] nstringa;
	delete [] detmp;

	scanf_s("%d",&a);
	return 0;
}




