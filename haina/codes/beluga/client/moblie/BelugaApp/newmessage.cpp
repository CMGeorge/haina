
#include "belugamain.h"
#include "newmessage.h"
#include <QtGui/QFileDialog>


NewMessage::NewMessage(QWidget *parent /* = 0 */, gchar* phoneNumber, gchar* contactName, Qt::WFlags flags /* = 0 */)
{
	m_pBelugaMain =(BelugaMain*)parent;
	m_nRows = 1;
	m_nMode = 0;

	this->setModal(TRUE);
	setupUi(this);
	setSmsEmailPanelVisible(FALSE);

	m_qMenuBar = new QMenuBar(this);
	m_qActionCancel = new QAction(tr("Cancel"), this);
	m_qActionSend = new QAction(tr("Send"), this);
	m_qActionMore = new QAction(tr("More"), this);

	m_qMenuBar->addAction(m_qActionSend);
	m_qMenuBar->addAction(m_qActionMore);
	m_qMenuBar->setDefaultAction(m_qActionCancel);
	send->setDefaultAction(m_qActionSend);
	more->setDefaultAction(m_qActionMore);

	connect(m_qActionCancel, SIGNAL(triggered(bool)), this, SLOT(onActionCancelTriggered(bool)));
	connect(m_qActionSend, SIGNAL(triggered(bool)), this, SLOT(onSendActionTriggered(bool)));
	connect(m_qActionMore, SIGNAL(triggered(bool)), this, SLOT(onMoreActionTriggered(bool)));

	connect(contactlist, SIGNAL(textChanged()), this, SLOT(onTextChanged()));
	connect(toList, SIGNAL(textChanged()), this, SLOT(onTextChanged()));
	connect(ccList, SIGNAL(textChanged()), this, SLOT(onTextChanged()));
	connect(bccList, SIGNAL(textChanged()), this, SLOT(onTextChanged()));
	
	/* set first contact */
	if (phoneNumber)
	{
		m_qPhoneNumberList.append(QString::fromUtf8(phoneNumber));
		if (contactName)	
			m_qContactNameList.append(QString::fromUtf8(contactName));
		else
			m_qContactNameList.append(QString());
	}
	
	QString contact = QString("%1(%2)").arg(m_qPhoneNumberList.at(0), m_qContactNameList.at(0));
	contactlist->setPlainText(contact);
}

NewMessage::~NewMessage()
{

}

void NewMessage::addListContact(int cnt)
{
	int h = 20;

	QRect toMsg_old = toMsg->geometry();
	toMsg->setGeometry(toMsg_old.x(), toMsg_old.y(), toMsg_old.width(), toMsg_old.height() + h * cnt);
	
	QRect content_old = content->geometry();
	content->setGeometry(content_old.x(), content_old.y() + h * cnt, content_old.width(), content_old.height());

	m_nRows += cnt;
}

void NewMessage::delListContact(int cnt)
{
	int h = 20;

	m_nRows -= cnt;

	QRect toMsg_old = toMsg->geometry();
	toMsg->setGeometry(toMsg_old.x(), toMsg_old.y(), toMsg_old.width(), toMsg_old.height() - h * cnt);

	QRect content_old = content->geometry();
	content->setGeometry(content_old.x(), content_old.y() - h * cnt, content_old.width(), content_old.height());
}

void NewMessage::addToListContact(int cnt)
{
	int h = 20;

	QRect toList_old = toList->geometry();
	toList->setGeometry(toList_old.x(), toList_old.y(), toList_old.width(), toList_old.height() + h * cnt);

	QRect CcList_old = ccList->geometry();
	ccList->setGeometry(CcList_old.x(), CcList_old.y() + h * cnt, CcList_old.width(), CcList_old.height());

	QRect BccList_old = bccList->geometry();
	bccList->setGeometry(BccList_old.x(), BccList_old.y() + h * cnt, BccList_old.width(), BccList_old.height());

	QRect content_old = content->geometry();
	content->setGeometry(content_old.x(), content_old.y() + h * cnt, content_old.width(), content_old.height());

	m_nRows += cnt;
}

void NewMessage::delToListContact(int cnt)
{
	int h = 20;

	m_nRows -= cnt;
	QRect toList_old = toList->geometry();
	toList->setGeometry(toList_old.x(), toList_old.y(), toList_old.width(), toList_old.height() - h * cnt);

	QRect CcList_old = ccList->geometry();
	ccList->setGeometry(CcList_old.x(), CcList_old.y() - h * cnt, CcList_old.width(), CcList_old.height());

	QRect BccList_old = bccList->geometry();
	bccList->setGeometry(BccList_old.x(), BccList_old.y() - h * cnt, BccList_old.width(), BccList_old.height());

	QRect content_old = content->geometry();
	content->setGeometry(content_old.x(), content_old.y() - h * cnt, content_old.width(), content_old.height());
}

void NewMessage::addCcListContact(int cnt)
{
	int h = 20;

	QRect CcList_old = ccList->geometry();
	ccList->setGeometry(CcList_old.x(), CcList_old.y(), CcList_old.width(), CcList_old.height() + h * cnt);

	QRect BccList_old = bccList->geometry();
	bccList->setGeometry(BccList_old.x(), BccList_old.y() + h * cnt, BccList_old.width(), BccList_old.height());

	QRect content_old = content->geometry();
	content->setGeometry(content_old.x(), content_old.y() + h * cnt, content_old.width(), content_old.height());

	m_nRows += cnt;
}

void NewMessage::delCcListContact(int cnt)
{
	int h = 20;

	m_nRows -= cnt;
	QRect CcList_old = ccList->geometry();
	ccList->setGeometry(CcList_old.x(), CcList_old.y(), CcList_old.width(), CcList_old.height() - h * cnt);

	QRect BccList_old = bccList->geometry();
	bccList->setGeometry(BccList_old.x(), BccList_old.y() - h * cnt, BccList_old.width(), BccList_old.height());

	QRect content_old = content->geometry();
	content->setGeometry(content_old.x(), content_old.y() - h * cnt, content_old.width(), content_old.height());
}

void NewMessage::addBccListContact(int cnt)
{
	int h = 20;

	QRect BccList_old = bccList->geometry();
	bccList->setGeometry(BccList_old.x(), BccList_old.y(), BccList_old.width(), BccList_old.height() + h * cnt);
	
	QRect content_old = content->geometry();
	content->setGeometry(content_old.x(), content_old.y() + h * cnt, content_old.width(), content_old.height());

	m_nRows += cnt;
}

void NewMessage::delBccListContact(int cnt)
{
	int h = 20;

	m_nRows -= cnt;
	QRect BccList_old = bccList->geometry();
	bccList->setGeometry(BccList_old.x(), BccList_old.y(), BccList_old.width(), BccList_old.height() - h * cnt);

	QRect content_old = content->geometry();
	content->setGeometry(content_old.x(), content_old.y() - h * cnt, content_old.width(), content_old.height());
}

void NewMessage::setSmsEmailPanelVisible(bool visible)
{
	if (visible)
	{
		toMsg->setVisible(FALSE);
		count->setVisible(FALSE);
		value->setVisible(FALSE);

		toEmail->setVisible(TRUE);
		mmsEmail->setVisible(TRUE);
		
		toList->setVerticalScrollBarPolicy(Qt::ScrollBarAlwaysOff);
		ccList->setVerticalScrollBarPolicy(Qt::ScrollBarAlwaysOff);
		bccList->setVerticalScrollBarPolicy(Qt::ScrollBarAlwaysOff);
		toEmail->setGeometry(0, 0, 240, 110);
		content->setGeometry(0, 110, 240, 130);
		msgContent->setGeometry(0, 60, 235, 65);
#if 0
		send->setGeometry(10, 132, 40, 24);
		quickMsg->setGeometry(55, 132, 40, 24);
		msgFace->setGeometry(100, 132, 40, 24);
		signature->setGeometry(130, 132, 40, 24);
		more->setGeometry(175, 132, 40, 24);
#endif
	}
	else
	{
		toEmail->setVisible(FALSE);
		mmsEmail->setVisible(FALSE);

		toMsg->setVisible(TRUE);
		contactlist->setVerticalScrollBarPolicy(Qt::ScrollBarAlwaysOff);
		content->setGeometry(0, 40, 240, 200);
		count->setGeometry(10, 0, 40, 16);
		value->setGeometry(55, 0, 30, 16);
		msgContent->setGeometry(0, 20, 235, 175);
#if 0
		send->setGeometry(10, 202, 40, 24);
		quickMsg->setGeometry(55, 202, 40, 24);
		msgFace->setGeometry(100, 202, 40, 24);
		signature->setGeometry(130, 202, 40, 24);
		more->setGeometry(175, 202, 40, 24);
#endif
	}
}

void NewMessage::onDefaultActionTriggered(bool checked)
{
	
}


void NewMessage::paintEvent(QPaintEvent *)
{
	QStyleOption opt;
	opt.init(this);
	QPainter p(this);
	style()->drawPrimitive(QStyle::PE_Widget, &opt, &p, this);
}

void NewMessage::onSendActionTriggered(bool checked)
{

}

void NewMessage::onMoreActionTriggered(bool checked)
{
	if (m_nMode == 0) /*sms*/
	{
		setSmsEmailPanelVisible(TRUE);
		QString contact = QString("%1(%2)").arg(m_qPhoneNumberList.at(0), m_qContactNameList.at(0));
		toList->setPlainText(contact);
		file->setEnabled(FALSE);
		file->setVisible(FALSE);
		m_qActionMore->setText(tr("Email"));
		m_nMode = 1;
	}
	else if (m_nMode == 1) /* mms*/
	{
		file->setEnabled(TRUE);
		file->setVisible(TRUE);
		m_qActionMore->setText(tr("MMS"));
		m_nMode = 2;
	}
	else /*email*/
	{
		file->setEnabled(FALSE);
		file->setVisible(FALSE);
		m_qActionMore->setText(tr("Email"));
		m_nMode = 1;
	}

}

void NewMessage::onTextChanged()
{

}
