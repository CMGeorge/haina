#ifndef BELUGASERVICETEST_H
#define BELUGASERVICETEST_H

#include <QtGui/QMainWindow>
#include "ui_belugaservicetest.h"

class CBelugaServiceTest : public QMainWindow
{
	Q_OBJECT

public:
	CBelugaServiceTest(QWidget *parent = 0, Qt::WFlags flags = 0);
	~CBelugaServiceTest();

private:
	Ui::CBelugaServiceTestClass ui;

private slots:
	void pushButtonSubmitClick();//²éÑ¯qqÔÚÏß×´Ì¬
};

#endif // BELUGASERVICETEST_H
