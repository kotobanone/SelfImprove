#include <string>
#include <fstream>
using namespace std;
class SelfDefineLog
{
public:
	SelfDefineLog(void);
	~SelfDefineLog(void);
	static SelfDefineLog *Log();
	void Message(const string &sFileName, const string &sFunc, const long &ILine, const string &sMessage);
	void WRITELOG(const string &sFile, const string &sFunc, const long &ILine, const string &sMessage);

	
private:
	static SelfDefineLog *m_pInstance;
	
	void GetNowTime();
	ofstream m_FileOut;
	string m_sFilePath;
	string m_sNowTime;
	string m_sFileSavePath;
	string m_LastDate;

};