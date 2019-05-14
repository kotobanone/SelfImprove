#include "stdafx.h"
#include "SD_Log.h"
#include <time.h>
#include <afx.h>

SelfDefineLog *SelfDefineLog::m_pInstance = new SelfDefineLog();

SelfDefineLog::SelfDefineLog(void)
{

}

SelfDefineLog::~SelfDefineLog(void)
{
	if(NULL!=m_pInstance)
	{
		//delete m_pInstance;
	}
	m_FileOut.close();
}

//獲得單例實體
SelfDefineLog *SelfDefineLog::Log()
{
	return m_pInstance;
}


//將訊息寫入log文件
void SelfDefineLog::Message(const string &sFile, const string &sFunc, const long &ILine, const string &sMessage)
{
	CTime CurrentTime = CTime::GetCurrentTime();
	CString strTime;
	strTime.Format("%04d-%02d-%02d.txt",CurrentTime.GetYear(),CurrentTime.GetMonth(),CurrentTime.GetDay());
	char FileName[260];
	GetModuleFileName(NULL,   FileName,   260);
	lstrcpy(_tcsrchr(FileName,_T('\\'))+1,_T("Log\\"));
	CreateDirectory(FileName,NULL);
	lstrcpy(_tcsrchr(FileName,_T('\\'))+1,_T(strTime));

	CString LogStr;
	string sFileName = sFile;
	sFileName = sFileName.substr(sFileName.find_last_of("\\")+1, sFileName.length() - sFileName.find_first_of("\\")-1);
	CString LogTime = CurrentTime.Format("%Y-%m-%d %H:%M:%S");
	LogStr.Format("%s  FILE: %s<%s>->%ld  $MES: %s\n",LogTime,sFileName.c_str(),sFunc.c_str(),ILine,sMessage.c_str()); //    
	CFile file;
	file.Open(FileName,CFile::modeNoTruncate|CFile::modeWrite|CFile::modeCreate);
	file.SeekToEnd();  //file.Seek(0,CFile::end)
	file.Write((unsigned char *)(LogStr.GetBuffer(0)),LogStr.GetLength());
	file.Flush();
	file.Close();	
}

void SelfDefineLog::WRITELOG(const string &sFile, const string &sFunc, const long &ILine, const string &sMessage)
{
	SelfDefineLog::Log()->Message(sFile,sFunc,ILine,sMessage);
}