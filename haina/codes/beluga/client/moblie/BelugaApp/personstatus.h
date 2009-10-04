#ifndef PERSONSTATUS_H
#define PERSONSTATUS_H

#include <QtGui/QDialog>
#include <QtGui/QWidget>
#include <QtGui/QPixmap>
#include <QtGui/QPushButton>
#include <QtGui/QLabel>
#include <QtGui/QStylePainter>
#include <QtGui/QPainter>
#include "ui_personstatus.h"
#include "CContact.h"
#include "CPhoneContact.h"

class BelugaMain;

class PersonStatus : public QWidget, public Ui::personstatus
{
	Q_OBJECT

public:
	PersonStatus(QWidget *parent = 0, Qt::WFlags flags = 0);
	~PersonStatus();

public:
	void setContact(int nContactId);

private:
	void paintEvent(QPaintEvent * event);

private slots:
	void onFun1Clicked( bool checked = false);
	void onFun2Clicked( bool checked = false);
	void onFun3Clicked( bool checked = false);
	void onFun4Clicked( bool checked = false);
	void onFun5Clicked( bool checked = false);
	void onFun6Clicked( bool checked = false);
	void onWeatherClicked( bool checked = false);
	

private:
	BelugaMain		* m_pBelugaMain;
	int				m_nContactId;
	CPhoneContact	* m_pContact;
};

#endif // PERSONSTATUS_H

