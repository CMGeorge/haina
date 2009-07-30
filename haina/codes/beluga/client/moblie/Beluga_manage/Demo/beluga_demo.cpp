// beluga_demo.cpp : implementation file
//

#include "stdafx.h"

#include "Demo.h"
#include "beluga_demo.h"


// beluga_demo dialog

IMPLEMENT_DYNAMIC(beluga_demo, CDialog)

beluga_demo::beluga_demo(CWnd* pParent /*=NULL*/)
	: CDialog(beluga_demo::IDD, pParent)
	, m_strQQ_id(_T(""))
	, m_strUserName(_T(""))
	, m_pwd(_T(""))
	, m_mobileID(_T(""))
	, m_cityCode(_T(""))
{
	m_bm=NULL;
}

beluga_demo::~beluga_demo()
{
}

void beluga_demo::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_EDIT_QQSTATUS, m_strQQ_id);
	DDX_Text(pDX, IDC_EDIT_USERNAME, m_strUserName);
	DDX_Text(pDX, IDC_EDIT_PASSWORD, m_pwd);
	DDX_Text(pDX, IDC_EDIT_MOBILEID, m_mobileID);
	DDX_Text(pDX, IDC_EDIT_CITY, m_cityCode);
	DDX_Control(pDX, IDC_COMBO_WEATHER, m_weathers);
}


BEGIN_MESSAGE_MAP(beluga_demo, CDialog)
	ON_BN_CLICKED(IDC_BTN_QQ, &beluga_demo::OnBnClickedBtnQq)
	ON_BN_CLICKED(IDC_BUTTON_REGISTER, &beluga_demo::OnBnClickedButtonRegister)
	ON_BN_CLICKED(IDC_BUTTON_LOGIN, &beluga_demo::OnBnClickedButtonLogin)
	ON_BN_CLICKED(IDC_BUTTON_LOGOUT, &beluga_demo::OnBnClickedButtonLogout)
	ON_BN_CLICKED(IDC_BUTTON_WEATHER, &beluga_demo::OnBnClickedButtonWeather)
	ON_BN_CLICKED(IDC_BUTTON_WEATHER7, &beluga_demo::OnBnClickedButtonWeather7)
END_MESSAGE_MAP()


// beluga_demo message handlers
std::string static CoverCstr2stdstr(CStringW str)
{
	CStringA strTmp(str.GetBuffer(0));
	std::string tmp(strTmp.GetBuffer(0));
	strTmp.ReleaseBuffer();
	return tmp;
}
void beluga_demo::OnBnClickedBtnQq()
{
	// TODO: Add your control notification handler code here
	UpdateData();
	std::string tmp;
	tmp=m_bm->GetQQStatusByID(CoverCstr2stdstr(m_strQQ_id));
	m_strQQ_id.Format(L"%s", tmp.c_str());
	MessageBox(m_strQQ_id);
	//UpdateData(false);
}

void beluga_demo::OnBnClickedButtonRegister()
{
	// TODO: Add your control notification handler code here
	UpdateData();
	bool bRe=false;
	bRe=m_bm->Register(CoverCstr2stdstr(m_strUserName),CoverCstr2stdstr(m_pwd),CoverCstr2stdstr(m_mobileID));
	m_strUserName=bRe?_T("congratulations!"):_T("failed!");
	MessageBox(m_strUserName);
}

void beluga_demo::OnBnClickedButtonLogin()
{
	// TODO: Add your control notification handler code here
	UpdateData();
	bool bRe=false;
	bRe=m_bm->Login(CoverCstr2stdstr(m_strUserName),CoverCstr2stdstr(m_pwd));
	m_strUserName=bRe?_T("Login!"):_T("failed!");
	MessageBox(m_strUserName);
}

void beluga_demo::OnBnClickedButtonLogout()
{
	// TODO: Add your control notification handler code here
	UpdateData();
	bool bRe=false;
	bRe=m_bm->Logout();
	m_strUserName=bRe?_T("Logout!"):_T("failed!");
	MessageBox(m_strUserName);
}

void beluga_demo::OnBnClickedButtonWeather()
{
	// TODO: Add your control notification handler code here
	UpdateData();
	WeatherInfo wif={0};
	wif=m_bm->GetLiveWeatherByCityCode(CoverCstr2stdstr(m_cityCode));
	//m_weathers.InsertString()
}

void beluga_demo::OnBnClickedButtonWeather7()
{
	// TODO: Add your control notification handler code here
}
#define KHostName _T("58.34.186.171") 
#define KHostNamePort 8079
BOOL beluga_demo::OnInitDialog()
{
	CDialog::OnInitDialog();
	m_bm=new beluga_manage();
	bool bRe=m_bm->setNetHost(KHostName,KHostNamePort);
	MessageBox(bRe?_T("setNetHost ok"):_T("setNetHost failed"));
	// TODO:  Add extra initialization here

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}
