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
#include <QtGui/QLayout>
#include <QtGui/QVBoxLayout>
#include <QtGui/QHBoxLayout>
#include <QtGui/QPushButton>
#include <QtGui/QToolButton>
#include <QtGui/QStylePainter>
#include <QtGui/QPainter>
#include "ui_belugadetail.h"
#include "CContact.h"
#include "CPhoneContact.h"

enum eDetailRow
{
	Row_Name = 0,
	Row_NickName,
	Row_Sex,
	Row_Photo,
	Row_Org,
	Row_Title,
	Row_PhoneMobile,
	Row_PhoneWork,
	Row_PhoneHome,
	Row_EmailWork,
	Row_EmailHome,
	Row_IMQQ,
	Row_IMMSN,
	Row_AddrWork,
	Row_AddrHome,
	Row_Url,
	Row_Ring,
	Row_Birthday,
	Row_Note
};

class BelugaMain;

class BelugaDetail : public QDialog, public Ui::belugadetail
{
	Q_OBJECT

public:
	BelugaDetail(QWidget *parent = 0, BelugaMain* pMain = NULL, Qt::WFlags flags = 0);
	~BelugaDetail();
	
	void setFieldsValue(CPhoneContact * pContact = NULL);

private:
	BOOL initializeFields();
	void setWidgetRect();
	void getFieldsValue(CPhoneContact * pContact);
	void paintEvent(QPaintEvent * event);

private slots:
	void onActionCancelTriggered(bool checked = false);
	void onDefaultActionTriggered(bool checked = false);
	void onClicked( bool checked = false);
	void onTelActionTrigggered(QAction * action);
	void onEmailActionTrigggered(QAction * action);
	void onAddrActionTrigggered(QAction * action);
	void onHomeAddrActivated(int index);
	void onWorkAddrActivated(int index);
	void onRingItemActivated(int index);

private:
	QList<QWidget*> m_qLeftWidgets;
	QList<QWidget*> m_qRightWidgets;

	QMenuBar	* m_qMenuBar;
	QAction		* m_qActionOk;
	QAction     * m_qActionCancel;

	QAction		* m_qActionHomeTel;
	QAction		* m_qActionWorkTel;
	QAction		* m_qActionMobileTel;
	QAction		* m_qActionHomeEmail;
	QAction		* m_qActionWorkEmail;
	QAction		* m_qActionHomeAddr;
	QAction		* m_qActionWorkAddr;

	BelugaMain  * m_pBelugaMain;

	bool		  m_bDIYPhoto;
	QString		  m_qDIYPhotoFile;

	int			  m_nPrefTel;
	int			  m_nPrefEmail;
	int		      m_nPrefAddr;

	stAddress	  m_stHomeAddr;
	stAddress	  m_stWorkAddr;

	CPhoneContact * m_pContact;
};

#endif // BELUGADETAIL_H

