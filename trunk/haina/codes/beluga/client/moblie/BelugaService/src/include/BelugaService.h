/**
* Author:JackyHo.dev@gmail.com
* Date:2009.9.30
*/
#pragma once
// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the BELUGASERVICE_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// BELUGASERVICE_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef BELUGASERVICE_EXPORTS
#define BELUGASERVICE_API __declspec(dllexport)
#else
#define BELUGASERVICE_API __declspec(dllimport)
#endif

// This class is exported from the BelugaService.dll
class BELUGASERVICE_API CBelugaService {
public:
	CBelugaService(void);
	// TODO: add your methods here.
};

extern BELUGASERVICE_API int nBelugaService;

BELUGASERVICE_API int fnBelugaService(void);
