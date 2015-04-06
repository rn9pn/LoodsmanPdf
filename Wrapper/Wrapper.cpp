//#define _WIN32_WINNT 0x0502
#include "stdafx.h"
#include <windows.h>
#include <string>
#include <sstream>
#include <stdio.h>
#include <exception>


#import "..\Common\Loodsman.tlb" no_namespace, named_guids //, raw_interfaces_only
#import "..\Common\LoodsmanPdf.tlb" no_namespace, named_guids , raw_interfaces_only

struct MenuItem
{
	char stMenu[255];
	char stFunction[255];
};

HINSTANCE dllModuleInstance;

__declspec(dllexport) int __stdcall InitUserDLLCom(void * value)
{
	if (value)
	{
		MenuItem * items = static_cast<MenuItem *>(value);
		strcpy_s(items[0].stMenu, "MI_TOOLS#Создать вторичное представление");
		strcpy_s(items[0].stFunction, "Function1");
	}
	//Количество пунктов меню
	return 1;
}

__declspec(dllexport) int __stdcall PgiCheckMenuItemCom(char const * stFunction, IPluginCall * IPC)
{
	//TODO Реализовать проверку на Изделие
	if (strcmp(stFunction, "Function1") == 0)
		return 1;

	return 0;
}

__declspec(dllexport) int __stdcall GetPluginInfo(int Param, void* value)
{
	return 0;
}


__declspec(dllexport) void __stdcall Function1(IPluginCall * IPC)
{
	CoInitialize(NULL);
	ILoodsmanPdf *plugin;

	HRESULT hr = CoCreateInstance(CLSID_Main, NULL, CLSCTX_INPROC_SERVER, IID_ILoodsmanPdf, (LPVOID*)&plugin);

	if (SUCCEEDED(hr))
	{	
		// Command1
		plugin->Command1(IPC);	
	}
	else
	{
		std::stringstream ss;
		ss << "Ошибка создания COM-объекта. Error code = 0x" << std::hex << hr;

		DWORD LastError = GetLastError();
		OLE_HANDLE hndl = 0;//IPC->ClientHandle;
		HWND wnd = HWND(hndl);
		TCHAR lpBuffer[256] = _T("?");

		if (LastError != 0)
		{
			FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, NULL, LastError, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), lpBuffer, 255, NULL);

			//"Ошибка создания com-объекта: "   "Ошибка создания COM-объекта" + 	
		}

		MessageBoxA(wnd, ss.str().c_str(), "Ошибка", 0);
	}

	CoUninitialize();
}
