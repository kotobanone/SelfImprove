// Encrypt_CAA.h : Encrypt_CAA DLL 的主要標頭檔
//

#pragma once

#ifndef __AFXWIN_H__
	#error "對 PCH 包含此檔案前先包含 'stdafx.h'"
#endif

#include "resource.h"		// 主要符號


// CEncrypt_CAAApp
// 這個類別的實作請參閱 Encrypt_CAA.cpp
//

class CEncrypt_CAAApp : public CWinApp
{
public:
	CEncrypt_CAAApp();

// 覆寫
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
