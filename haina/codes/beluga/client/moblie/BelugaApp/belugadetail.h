#ifndef BELUGADETAIL_H
#define BELUGADETAIL_H

#include <QtGui/QDialog>
#include <QtGui/QTableWidget>
#include <QtGui/QWidget>
#include <QtGui/QPicture>
#include <QtGui/QMenuBar>
#include <QtGui/QAction>
#include <QtCore/QList>
#include <QtGui/QComboBox>
#include <QtGui/QDateEdit>
#include <QtGui/QPlainTextEdit>
#include <QtGui/QLabel>
#include "ui_belugadetail.h"


enum eDetailRow
{
	Row_Name = 0,
	Row_NickName,
	Row_Sex,
	Row_Photo,
	Row_PhoneMobile,
	Row_PhoneWork,
	Row_PhoneHome,
	Row_EmailWork,
	Row_EmailHome,
	Row_IMQQ,
	Row_IMMSN,
	Row_AddrWork,
	Row_AddrHome,
	Row_Birthday,
	Row_Org,
	Row_Url,
	Row_Ring,
	Row_Title,
	Row_Note
};

class BelugaMain;

class BelugaDetail : public QDialog, public Ui::belugadetail
{
	Q_OBJECT

public:
	BelugaDetail(QWidget *parent = 0, Qt::WFlags flags = 0);
	~BelugaDetail();

private:
	BOOL initializeFields();

private slots:

private:
	QList<QLabel*> m_qLeftWidgets;
	QList<QWidget*> m_qRightWidgets;

	QMenuBar	* m_qMenuBar;
	QAction		* m_qActionOk;
	QAction     * m_qActionCancel;

	BelugaMain  * m_pBelugaMain;
};

#endif // BELUGADETAIL_H

