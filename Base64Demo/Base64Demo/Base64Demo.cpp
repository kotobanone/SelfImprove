#include "Base64Demo.h"
#include "base64.h"
using namespace Base64DemoN;

[STAThread]
int main(array<System::String^>^args)
{
	Application::EnableVisualStyles();
	Application::Run(gcnew Base64Demo());	
	return 0;
}

