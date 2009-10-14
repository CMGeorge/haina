#ifndef NEWMESSAGE_H
#define NEWMESSAGE_H

#include <QtGui/QDialog>
#include <QtGui/QWidget>
#include <QtGui/QPicture>
#include <QtGui/QMenuBar>
#include <QtGui/QAction>
#include <QtCore/QList>
#include <QtGui/QComboBox>
#include <QtGui/QPlainTextEdit>
#include <QtGui/QLabel>
#include <QtGui/QPushButton>
#include <QtGui/QToolButton>
#include <QtGui/QStylePainter>
#include <QtGui/QPainter>
#include "ui_newmessage.h"
#include "CMsgDb.h"
#include "CMsg.h"


class BelugaMain;

class NewMessage : public QDialog, public Ui::newmessage
{
	Q_OBJECT

public:
	NewMessage(QWidget *parent = 0, gchar * phoneNumber = NULL, gchar * contactName = NULL, Qt::WFlags flags = 0);
	~NewMessage();

private:
	void paintEvent(QPaintEvent * event);
	void addListContact(int cnt);
	void delListContact(int cnt);
	void setSmsEmailPanelVisible(bool visible);

	void addToListContact(int cnt);
	void delToListContact(int cnt);
	void addCcListContact(int cnt);
	void delCcListContact(int cnt);
	void addBccListContact(int cnt);
	void delBccListContact(int cnt);

private slots:
	void onDefaultActionTriggered(bool checked = false);
	void onSendActionTriggered(bool checked = false);
	void onMoreActionTriggered(bool checked = false);
	void onTextChanged();

private:
	QMenuBar	*	m_qMenuBar;
	QAction     *	m_qActionCancel;

	QAction		*	m_qActionSend;
	QAction		*	m_qActionMore;

	BelugaMain  *	m_pBelugaMain;

	int				m_nMode; /* 0:sms  1:mms  2:email */
	int				m_nRows;

	QList<QString>  m_qPhoneNumberList;
	QList<QString>  m_qContactNameList;

	QList<QString>  m_qtoListNumberList;
	QList<QString>  m_qtoListNameList;
	QList<QString>  m_qccListNumberList;
	QList<QString>  m_qccListNameList;
	QList<QString>  m_qbCcListNumberList;
	QList<QString>  m_qbCcListNameList;
};

#endif // NEWMESSAGE_H

