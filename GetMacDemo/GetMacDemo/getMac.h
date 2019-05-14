#include <nb30.h>
#include <iostream>
#include <windows.h>

typedef struct _ASTAT_ 
{ 
	ADAPTER_STATUS adapt; 
	NAME_BUFFER    NameBuff [30]; 
}ASTAT, * PASTAT;

std::string GetMac()
{
	char buf[1024];
	memset(buf,0x00,sizeof(buf));
	DWORD len = 0;
	//len = ReadLicenseFile(buf);
	//if(len == 0 || len == 0xFFFF) return 0x0;		
	
	NCB ncb; 
	LANA_ENUM lana_enum; 
	BYTE MacAddress[6];
	char macstr[18];
	memset( &ncb, 0, sizeof(ncb) ); ncb.ncb_command = NCBENUM; ncb.ncb_buffer = (unsigned char *) &lana_enum; ncb.ncb_length = sizeof(lana_enum);
	if ( Netbios( &ncb ) == 0 ) {
		for ( int i=0; i< lana_enum.length; ++i) {
			NCB ncb; ASTAT Adapter; 
			memset( &ncb, 0, sizeof(ncb) ); 
			ncb.ncb_command = NCBRESET; 
			ncb.ncb_lana_num = lana_enum.lana[i];  
			Netbios( &ncb ); 
			
			memset( &ncb, 0, sizeof(ncb) ); 
			ncb.ncb_command = NCBASTAT; 
			ncb.ncb_lana_num = lana_enum.lana[i];  
			strcpy( (char *)ncb.ncb_callname,"*               " ); ncb.ncb_buffer = (unsigned char *) &Adapter;ncb.ncb_length = sizeof(Adapter);
			if ( Netbios( &ncb )==0 ) {
				memcpy(MacAddress,Adapter.adapt.adapter_address,6); 
				sprintf(macstr,"%02X-%02X-%02X-%02X-%02X-%02X",MacAddress[0],MacAddress[1],MacAddress[2],MacAddress[3],MacAddress[4],MacAddress[5]);
				macstr[17]='\0';
				//macstr.Format("%02X-%02X-%02X-%02X-%02X-%02X",MacAddress[0],MacAddress[1],MacAddress[2],MacAddress[3],MacAddress[4],MacAddress[5]);
				//if ( FindStr(buf,len,(LPTSTR)(LPCTSTR)macstr,12) ) return 1;
				
			}
		} 
	}
	return macstr;
}
