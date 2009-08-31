// Author JackyHo
// Create Date:2009.8.30
// Description:读取mobile6.0通讯录
// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the DLL_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// DLL_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef DLL_EXPORTS
#define DLL_API __declspec(dllexport)
#else
#define DLL_API __declspec(dllimport)
#endif

#include <glib.h>

// This class is exported from the dll.dll
class DLL_API CContactDll {
public:
	CContactDll(void);
	// TODO: add your methods here.
	//设备上的总联系人数量
	gint totalCount;
	//初始化并设置当前实例对象中的总联系人数量
	gboolean init(void);
	// 读取设备上存储的联系人
	gboolean getContact(GList **pGListContact, guint offset, guint len) const;
};

extern DLL_API int ndll;

DLL_API int fndll(void);
