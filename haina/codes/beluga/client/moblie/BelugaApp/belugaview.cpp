
#include "belugamain.h"
#include "belugadetail.h"
#include "belugaview.h"


BelugaView::BelugaView(QWidget *parent /* = 0 */, int nContactId, bool * bEdited, Qt::WFlags flags /* = 0 */)
						:m_nContactId(nContactId), m_bEidted(bEdited)
{
	m_pContact = NULL;
	this->setModal(TRUE);
	m_pBelugaMain = (BelugaMain*)parent;
	setupUi(this);
	contactlist->setVisible(FALSE);
	
	/* initialize fields */
	initializeFields();

	connect(contactlist, SIGNAL(currentItemChanged(QListWidgetItem*, QListWidgetItem*)), 
		this, SLOT(onCurrentItemChanged(QListWidgetItem*, QListWidgetItem*)));
	connect(contactlist, SIGNAL(itemClicked(QListWidgetItem*)), this, SLOT(onItemClicked(QListWidgetItem*)));
	connect(contactlist, SIGNAL(itemDoubleClicked(QListWidgetItem*)), this, SLOT(onItemDoubleClicked(QListWidgetItem*)));

	m_qMenuBar = new QMenuBar(this);
	m_qActionEdit = new QAction(tr("Edit"), this);

	m_qMenuCall = m_qMenuBar->addMenu(tr("Call"));
	m_qActionVoiceCall = new QAction(tr("Voice Call"), this);
	m_qMenuCall->addAction(m_qActionVoiceCall);
	connect(m_qActionVoiceCall, SIGNAL(triggered(bool)), this, SLOT(onActionVoiceCallTriggered(bool)));

	m_qActionVideoCall = new QAction(tr("Video Call"), this);
	m_qMenuCall->addAction(m_qActionVideoCall);
	connect(m_qActionVideoCall, SIGNAL(triggered(bool)), this, SLOT(onActionVideoCallTriggered(bool)));

	m_qActionIpCall = new QAction(tr("IP Call"), this);
	m_qMenuCall->addAction(m_qActionIpCall);
	connect(m_qActionIpCall, SIGNAL(triggered(bool)), this, SLOT(onActionIPCallTriggered(bool)));

	m_qActionMsgC = new QAction(tr("Message"), this);
	m_qMenuBar->addAction(m_qActionMsgC);
	connect(m_qActionMsgC, SIGNAL(triggered(bool)), this, SLOT(onActionMsgCTriggered(bool)));

	m_qActionDelC = new QAction(tr("Delete"), this);
	m_qMenuBar->addAction(m_qActionDelC);
	connect(m_qActionDelC, SIGNAL(triggered(bool)), this, SLOT(onActionDelCTriggered(bool)));

	m_qActionNote = new QAction(tr("View Note"), this);
	m_qMenuBar->addAction(m_qActionNote);
	connect(m_qActionNote, SIGNAL(triggered(bool)), this, SLOT(onActionNoteTriggered(bool)));

	m_qActionBack = new QAction(tr("Back"), this);
	m_qMenuBar->addAction(m_qActionBack);
	connect(m_qActionBack, SIGNAL(triggered(bool)), this, SLOT(onActionBackTriggered(bool)));

	m_qMenuBar->setDefaultAction(m_qActionEdit);
	connect(m_qActionEdit, SIGNAL(triggered(bool)), this, SLOT(onDefaultActionTriggered(bool)));
	contactlist->setVisible(TRUE);
}

BelugaView::~BelugaView()
{
	if (m_pContact)
		delete m_pContact;

	m_pContact = NULL;
}

BOOL BelugaView::initializeFields()
{
	QWidget * parent = this;
	QString string;
	QListWidgetItem * qItem = NULL;
	CPhoneContact * pContact = NULL;

	m_pBelugaMain->getContactDb()->GetEntityById(m_nContactId, (CDbEntity**)&pContact);
	if (pContact == NULL)
		return FALSE;
	
	m_pContact = pContact;
	/* photo */
	GString * value = NULL;
	pContact->GetFieldValue(ContactField_Photo, &value);
	photo->setIcon(QIcon(QString(value->str)));
	g_string_free(value, TRUE);

	/* name */
	pContact->GetFieldValue(ContactField_Name, &value); 
	namelabel->setText(QString(value->str));
	
	/* nickname */
	GString *nickname = NULL;
	pContact->GetFieldValue(ContactField_NickName, &nickname); 
	if (g_strcasecmp(value->str, nickname->str) != 0)
		nicknamelabel->setText(QString(nickname->str));
	g_string_free(value, TRUE);
	g_string_free(nickname, TRUE);
	
	contactlist->clear();
	/* org */
	pContact->GetFieldValue(ContactField_Org, &value); 
	if (value->len)
	{
		string = QString(tr("  Org:\t%1")).arg(QString(value->str));
		qItem = new QListWidgetItem(string);
		contactlist->addItem(qItem);
	}
	g_string_free(value, TRUE);

	/* title */
	pContact->GetFieldValue(ContactField_Title, &value); 
	if (value->len)
	{
		string = QString(tr("  Title:\t%1")).arg(QString(value->str));
		qItem = new QListWidgetItem(string);
		contactlist->addItem(qItem);
	}
	g_string_free(value, TRUE);

	
	/* Mobile Phone */
	gchar* sPhone = NULL;
	pContact->GetFieldValue(ContactField_PhonePref, &value);
	pContact->GetPhone((ECommType)(CommType_Mobile | CommType_Phone), &sPhone);
	if (sPhone != NULL && strlen(sPhone))
	{
		if (g_strcasecmp(value->str, sPhone) == 0)
			string = QString(tr("  Mobile(*):  %1")).arg(QString(sPhone));	
		else
			string = QString(tr("  Mobile:  %1")).arg(QString(sPhone));	
		qItem = new QListWidgetItem(string);
		qItem->setData(Qt::UserRole, QVariant(sPhone));
		contactlist->addItem(qItem);
	}
	if (sPhone)
		g_free(sPhone);

	/* Work Phone */
	sPhone = NULL;
	pContact->GetPhone((ECommType)(CommType_Work | CommType_Phone), &sPhone);
	if (sPhone != NULL && strlen(sPhone))
	{
		if (g_strcasecmp(value->str, sPhone) == 0)
			string = QString(tr("  Work Tel(*):  %1")).arg(QString(sPhone));	
		else
			string = QString(tr("  Work Tel:  %1")).arg(QString(sPhone));	
		qItem = new QListWidgetItem(string);
		qItem->setData(Qt::UserRole, QVariant(sPhone));
		contactlist->addItem(qItem);
	}
	if (sPhone)
		g_free(sPhone);

	/* Home Phone */
	sPhone = NULL;
	pContact->GetPhone((ECommType)(CommType_Home | CommType_Phone), &sPhone);
	if (sPhone != NULL && strlen(sPhone))
	{
		if (g_strcasecmp(value->str, sPhone) == 0)
			string = QString(tr("  Home Tel(*):  %1")).arg(QString(sPhone));	
		else
			string = QString(tr("  Home Tel:  %1")).arg(QString(sPhone));	
		qItem = new QListWidgetItem(string);
		qItem->setData(Qt::UserRole, QVariant(sPhone));
		contactlist->addItem(qItem);
	}
	if (sPhone)
		g_free(sPhone);
	g_string_free(value, TRUE);

	/* Work Email */
	gchar* sEmail = NULL;
	pContact->GetFieldValue(ContactField_EmailPref, &value);
	pContact->GetEmail((ECommType)(CommType_Work | CommType_Email), &sEmail);
	if (sEmail != NULL && strlen(sEmail))
	{
		if (g_strcasecmp(value->str, sEmail) == 0)
			string = QString(tr("  Work Email(*):  %1")).arg(QString(sEmail));	
		else
			string = QString(tr("  Work Email:  %1")).arg(QString(sEmail));	
		qItem = new QListWidgetItem(string);
		qItem->setData(Qt::UserRole, QVariant(sEmail));
		contactlist->addItem(qItem);
	}
	if (sEmail)
		g_free(sEmail);

	/* Home Email */
	sEmail = NULL;
	pContact->GetEmail((ECommType)(CommType_Home | CommType_Email), &sEmail);
	if (sEmail != NULL && strlen(sEmail))
	{
		if (g_strcasecmp(value->str, sEmail) == 0)
			string = QString(tr("  Home Email(*):  %1")).arg(QString(sEmail));	
		else
			string = QString(tr("  Home Email:  %1")).arg(QString(sEmail));	
		qItem = new QListWidgetItem(string);
		qItem->setData(Qt::UserRole, QVariant(sEmail));
		contactlist->addItem(qItem);
	}
	if (sEmail)
		g_free(sEmail);
	g_string_free(value, TRUE);

	/* QQ */ 
	gchar * im = NULL;
	pContact->GetIM((ECommType)(CommType_IM | CommType_QQ), &im);
	if (im != NULL && strlen(im))
	{
		string = QString(tr("  QQ:\t%1")).arg(QString(im));	
		qItem = new QListWidgetItem(string);
		qItem->setData(Qt::UserRole, QVariant(im));
		contactlist->addItem(qItem);
	}
	if (im)
		g_free(im);	

	/* MSN */ 
	im = NULL;
	pContact->GetIM((ECommType)(CommType_IM | CommType_MSN), &im);
	if (im != NULL && strlen(im))
	{
		string = QString(tr("  MSN:\t%1")).arg(QString(im));	
		qItem = new QListWidgetItem(string);
		qItem->setData(Qt::UserRole, QVariant(im));
		contactlist->addItem(qItem);
	}
	if (im)
		g_free(im);	

	/* URL */
	pContact->GetFieldValue(ContactField_Url, &value); 
	if (value->len)
	{
		string = QString(tr("  URL:\t%1")).arg(QString(value->str));
		qItem = new QListWidgetItem(string);
		qItem->setData(Qt::UserRole, QVariant(value->str));
		contactlist->addItem(qItem);
	}
	g_string_free(value, TRUE);

	/* Birthday */
	pContact->GetFieldValue(ContactField_Birthday, &value); 
	if (value->len)
	{
		string = QString(tr("  Birthday:\t%1")).arg(QString(value->str));
		qItem = new QListWidgetItem(string);
		contactlist->addItem(qItem);
	}
	g_string_free(value, TRUE);

	/* Work addr */
	stAddress *addr = NULL;
	pContact->GetAddress((ECommType)(CommType_Work | CommType_Address), &addr);
	if (addr != NULL && (strlen(addr->block) || strlen(addr->city) || strlen(addr->district) || strlen(addr->state) || strlen(addr->street)))
	{
		string = QString(tr("  Work Address:"));	
		string.append(QString("\t%1,%2,%3\n\t\t%4,%5,%6,%7").arg(addr->country).arg(addr->state).arg(addr->postcode).arg(addr->city).arg(addr->district).arg(addr->street).arg(addr->block));
		qItem = new QListWidgetItem(string);
		contactlist->addItem(qItem);
	}
	if (addr)
		g_free(addr);

	/* Home addr */
	addr = NULL;
	pContact->GetAddress((ECommType)(CommType_Work | CommType_Address), &addr);
	if (addr != NULL && (strlen(addr->block) || strlen(addr->city) || strlen(addr->district) || strlen(addr->state) || strlen(addr->street)))
	{
		string = QString(tr("  Home Address:"));	
		string.append(QString("\t%1,%2,%3\n\t\t%4,%5,%6,%7").arg(addr->country).arg(addr->state).arg(addr->postcode).arg(addr->city).arg(addr->district).arg(addr->street).arg(addr->block));
		qItem = new QListWidgetItem(string);
		contactlist->addItem(qItem);
	}
	if (addr)
		g_free(addr);

	return TRUE;
}

void BelugaView::onActionVoiceCallTriggered(bool checked)
{

}

void BelugaView::onActionVideoCallTriggered(bool checked)
{

}

void BelugaView::onActionIPCallTriggered(bool checked)
{

}

void BelugaView::onActionMsgCTriggered(bool checked)
{

}

void BelugaView::onActionDelCTriggered(bool checked)
{
	QMessageBox msg(QMessageBox::Question, "", tr("Sure to delete the contact?"), QMessageBox::Yes|QMessageBox::No, this);
	msg.setDefaultButton(QMessageBox::Yes);
	if (QMessageBox::Yes == msg.exec())
	{
		GString * ContactId = NULL;
		m_pContact->GetFieldValue(ContactField_Id, &ContactId);		
		int id = atoi(ContactId->str);
		g_string_free(ContactId, TRUE);

		if (0 == m_pBelugaMain->getContactDb()->DeleteEntity(id))  /* remove successfully */
		{
			*m_bEidted = TRUE;
			accept();
		}
	}
}

void BelugaView::onActionBackTriggered(bool checked)
{
	accept();
}

void BelugaView::onActionNoteTriggered(bool checked)
{

}

void BelugaView::onDefaultActionTriggered(bool checked)
{
	BelugaDetail detail(this, m_pBelugaMain);
	detail.setFieldsValue(m_pContact);
	detail.exec();
	*m_bEidted = TRUE;
	initializeFields();
}

void BelugaView::onItemDoubleClicked(QListWidgetItem * item)
{

}

void BelugaView::onCurrentItemChanged(QListWidgetItem * current, QListWidgetItem * previous)
{

}

void BelugaView::onItemClicked (QListWidgetItem * item)
{

}

void BelugaView::paintEvent(QPaintEvent *)
{
	QStyleOption opt;
	opt.init(this);
	QPainter p(this);
	style()->drawPrimitive(QStyle::PE_Widget, &opt, &p, this);
}
