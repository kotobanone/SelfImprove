#include "DBLEForm.h"
using namespace System;
using namespace DBLinkEcrypt;

int main(array <String^>^args)
{
    Application::EnableVisualStyles() ;
    Application::SetCompatibleTextRenderingDefault(false) ;
    Application::Run(gcnew DBLEForm());
    return 0;
}

