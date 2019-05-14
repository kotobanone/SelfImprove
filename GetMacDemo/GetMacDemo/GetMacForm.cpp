#include "GetMacForm.h"
using namespace GetMacDemo;
[STAThread]
int main(array<System::String^>^args)
{
	Application::EnableVisualStyles();
	Application::Run(gcnew GetMacForm());
	return 0;
}

