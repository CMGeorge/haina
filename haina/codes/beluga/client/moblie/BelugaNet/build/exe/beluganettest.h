/**
* @author north0808@gmail.com
* @version 1.0
*/
#ifndef BELUGANETTEST_H
#define BELUGANETTEST_H

#include <QtGui/QMainWindow>
#include "ui_beluganettest.h"

class BelugaNetTest : public QMainWindow
{
	Q_OBJECT

public:
	BelugaNetTest(QWidget *parent = 0, Qt::WFlags flags = 0);
	~BelugaNetTest();

private:
	Ui::BelugaNetTestClass ui;

private slots:
	void pushButtonQQSubmitClick();//查询qq在线状态
	void pushButtonWeatherSubmitClick();//查询城市在线状态
	void pushButtonCitySubmitClick();//同步本地城市数据库
};

#endif // BELUGANETTEST_H
