#pragma once
#include "afxwin.h"
#include <iostream> 
using namespace std;
#include <beluga_manage.h>

// beluga_demo dialog

class beluga_demo : public CDialog
{
	DECLARE_DYNAMIC(beluga_demo)

public:
	beluga_demo(CWnd* pParent = NULL);   // standard constructor
	virtual ~beluga_demo();

// Dialog Data
	enum { IDD = IDD_DIALOG1 };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedBtnQq();
	afx_msg void OnBnClickedButtonRegister();
	afx_msg void OnBnClickedButtonLogin();
	afx_msg void OnBnClickedButtonLogout();
	afx_msg void OnBnClickedButtonWeather();
	afx_msg void OnBnClickedButtonWeather7();
	CString m_strQQ_id;
	CString m_strUserName;
	CString m_pwd;
	CString m_mobileID;
	CString m_cityCode;
	CComboBox m_weathers;
	virtual BOOL OnInitDialog();
private:
	beluga_manage	*m_bm;
};
