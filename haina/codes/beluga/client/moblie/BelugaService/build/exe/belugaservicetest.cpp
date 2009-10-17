#include "belugaservicetest.h"
//user add
#include <BelugaService.h>
#include <hessian/HttpConnection.h>
#include <pubService/IPubServiceProxy.h>

#include <windows.h>//cursor

using namespace std;
using namespace hessian;
using namespace pubService;

CBelugaServiceTest::CBelugaServiceTest(QWidget *parent, Qt::WFlags flags)
: QMainWindow(parent, flags)
{
	ui.setupUi(this);
	QObject::connect(ui.pushButtonSubmit,SIGNAL(clicked()),this,SLOT(pushButtonSubmitClick()));
}

CBelugaServiceTest::~CBelugaServiceTest()
{

}

void CBelugaServiceTest::pushButtonSubmitClick()
{
	SetCursor(LoadCursor(NULL,IDC_WAIT)); //设定鼠标忙碌状态
	ui.labelStatus->setText("");
	string qq(ui.lineEditQQ->text().toAscii());
	HttpConnection connection;
	IPubServiceProxy *service = new IPubServiceProxy(connection);
	try
	{
		string ret = service->getQQStatus(qq);//默认30秒超时
		/*
		* 状态包括：
		* 1.成功 statusCode=0
		* 2.QQ参数错误 statusCode = 10002
		* 3.网络错误 statusCode= 10003 
		* 
		* value值为：
		* 1.在线 value=10000
		* 2.不在线 value=10001
		*/
		if(ret.compare("10000")==0)
		{
			//在线
			ui.labelStatus->setText("online");
		}
		else if(ret.compare("-1")==0)
		{
			//在线
			ui.labelStatus->setText("server busy");
		}
		else
		{
			//不在线
			ui.labelStatus->setText("offline");
		}
	}
	catch(...)
	{
		//30秒超时，网络问题
		ui.labelStatus->setText("net problem");
	}
	SetCursor(NULL);//恢复鼠标状态	
}
