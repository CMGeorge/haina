#include "belugaapp.h"

BelugaApp::BelugaApp()
{
	m_appMain = new BelugaMain();
	m_appMain->showMaximized();
}

BelugaApp::~BelugaApp()
{
	delete m_appMain;
	m_appMain = NULL;
}
