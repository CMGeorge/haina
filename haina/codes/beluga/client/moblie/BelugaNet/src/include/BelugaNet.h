/**
* @author north0808@gmail.com
* @version 1.0
*/
#pragma once

// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the BELUGANET_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// BELUGANET_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef BELUGANET_EXPORTS
#define BELUGANET_API __declspec(dllexport)
#else
#define BELUGANET_API __declspec(dllimport)
#endif

// This class is exported from the BelugaNet.dll
class BELUGANET_API CBelugaNet {
public:
	CBelugaNet(void);
	// TODO: add your methods here.
};

extern BELUGANET_API int nBelugaNet;

BELUGANET_API int fnBelugaNet(void);
