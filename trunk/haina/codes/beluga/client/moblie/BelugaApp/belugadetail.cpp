
#include "belugamain.h"
#include "belugadetail.h"
#include "photogallery.h"
#include "addredit.h"
#include <QtGui/QFileDialog>
#include <QtGui/QInputDialog>
#include <QtGui/QMessageBox>
#include <QtCore/QTimer>
#include <QtCore/QDate>
#include "hz2py.h"
#include "CPhoneContact.h"


BelugaDetail::BelugaDetail(QWidget *parent /* = 0 */, BelugaMain* pMain, Qt::WFlags flags /* = 0 */)
						:m_pBelugaMain(pMain)
{
	m_pContact = NULL;
	memset(&m_stHomeAddr, 0, sizeof(stAddress));
	memset(&m_stWorkAddr, 0, sizeof(stAddress));
	m_nPrefTel = Row_PhoneMobile;
	m_nPrefAddr = Row_AddrWork;
	m_nPrefEmail = Row_EmailWork;
	m_bDIYPhoto = FALSE;
	m_qDIYPhotoFile = ":/BelugaApp/Resources/images/photo_contact_1.png";
	this->setModal(TRUE);
	setupUi(this);
	detailarea->setVisible(FALSE);
	detailarea->setVerticalScrollBarPolicy(Qt::ScrollBarAsNeeded);
#if 0
	contacttable->setVisible(FALSE);
	contacttable->setColumnWidth(0, 80);
	contacttable->setColumnWidth(1, 140);
#endif

	/* initialize fields */
	initializeFields();

	m_qMenuBar = new QMenuBar(this);
	m_qActionOk = new QAction(tr("Ok"), this);
	m_qActionCancel = new QAction(tr("Cancel"), this);
//	m_qMenuBar->addAction(m_qActionOk);
	m_qMenuBar->addAction(m_qActionCancel);
	m_qMenuBar->setDefaultAction(m_qActionOk);
	connect(m_qActionCancel, SIGNAL(triggered(bool)), this, SLOT(onActionCancelTriggered(bool)));
	connect(m_qActionOk, SIGNAL(triggered(bool)), this, SLOT(onDefaultActionTriggered(bool)));

#if 0
	contacttable->setVisible(TRUE);
#endif
	detailarea->setVisible(TRUE);
}

BelugaDetail::~BelugaDetail()
{

}

BOOL BelugaDetail::initializeFields()
{
	QWidget * parent = scrollAreaWidgetContents;
	QLineEdit * qEdit = NULL;
	QLabel * qLabel = NULL;
	QComboBox * qComboBox = NULL;
#if 0
	QHBoxLayout * qHLayout = NULL;
	QVBoxLayout * qVLayout = NULL;
	
	qVLayout = new QVBoxLayout(detailarea);
	qVLayout->setSizeConstraint(QLayout::SetMaximumSize);
#endif

	/* name */
	qEdit = new QLineEdit(parent);
	m_qRightWidgets.append(qEdit);
	qLabel = new QLabel(tr("Name:"), parent);
	qLabel->setBuddy(qEdit);
	qLabel->setAlignment(Qt::AlignHCenter);
	m_qLeftWidgets.append(qLabel);
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qEdit);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_Name, 20);
	contacttable->setCellWidget(Row_Name, 0, qLabel);
	contacttable->setCellWidget(Row_Name, 1, qEdit);
#endif
	
	/* nickname */
	qEdit = new QLineEdit(parent);
	m_qRightWidgets.append(qEdit);
	qLabel = new QLabel(tr("NickName:"), parent);
	qLabel->setBuddy(qEdit);
	qLabel->setAlignment(Qt::AlignHCenter);
	m_qLeftWidgets.append(qLabel);
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qEdit);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_NickName, 20);
	contacttable->setCellWidget(Row_NickName, 0, qLabel);
	contacttable->setCellWidget(Row_NickName, 1, qEdit);
#endif
	/* sex */
	qComboBox = new QComboBox(parent);
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Gender:"), parent);
	qLabel->setBuddy(qComboBox);
	qLabel->setAlignment(Qt::AlignHCenter);
	m_qLeftWidgets.append(qLabel);
	qComboBox->addItem(QIcon(":/BelugaApp/Resources/images/male.png"), tr("Male"), QVariant(0));
	qComboBox->addItem(QIcon(":/BelugaApp/Resources/images/female.png"), tr("Female"), QVariant(1));

#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qComboBox);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_Sex, 20);
	contacttable->setCellWidget(Row_Sex, 0, qLabel);
	contacttable->setCellWidget(Row_Sex, 1, qComboBox);
#endif

	/* photo */
	QPushButton * qPushButton = new QPushButton(parent);
	qPushButton->setFlat(TRUE);
	m_qRightWidgets.append(qPushButton);
	qLabel = new QLabel(tr("Photo:"), parent);
	qLabel->setBuddy(qPushButton);
	qLabel->setAlignment(Qt::AlignHCenter);
	m_qLeftWidgets.append(qLabel);
	qPushButton->setIcon(QIcon(m_qDIYPhotoFile));
	connect(qPushButton, SIGNAL(clicked(bool)), this, SLOT(onClicked(bool)));

#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qComboBox);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_Photo, 20);
	contacttable->setCellWidget(Row_Photo, 0, qLabel);
	contacttable->setCellWidget(Row_Photo, 1, qComboBox);
#endif

	/* org */
	qEdit = new QLineEdit(parent);
	m_qRightWidgets.append(qEdit);
	qLabel = new QLabel(tr("Company:"), parent);
	qLabel->setBuddy(qEdit);
	qLabel->setAlignment(Qt::AlignHCenter);
	m_qLeftWidgets.append(qLabel);
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qComboBox);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_Org, 20);
	contacttable->setCellWidget(Row_Org, 0, qLabel);
	contacttable->setCellWidget(Row_Org, 1, qComboBox);
#endif

	/* title */
	qEdit = new QLineEdit(parent);
	m_qRightWidgets.append(qEdit);
	qLabel = new QLabel(tr("Job title:"), parent);
	qLabel->setBuddy(qEdit);
	qLabel->setAlignment(Qt::AlignHCenter);
	m_qLeftWidgets.append(qLabel);
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qComboBox);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_Title, 20);
	contacttable->setCellWidget(Row_Title, 0, qLabel);
	contacttable->setCellWidget(Row_Title, 1, qComboBox);
#endif

	/* Mobile Phone */
	qEdit = new QLineEdit(parent);
	m_qRightWidgets.append(qEdit);
	qPushButton = new QPushButton(tr("Mobile tel:"), parent);
	qPushButton->setFlat(TRUE);
	m_qLeftWidgets.append(qPushButton);
	QMenu * qMenu = new QMenu();
	m_qActionMobileTel = new QAction(tr("Prefer"), this);
	qMenu->addAction(m_qActionMobileTel);
	m_qActionMobileTel->setCheckable(TRUE);
	qPushButton->setMenu(qMenu);
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qComboBox);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_PhoneMobile, 20);
	contacttable->setCellWidget(Row_PhoneMobile, 0, qLabel);
	contacttable->setCellWidget(Row_PhoneMobile, 1, qComboBox);
#endif

	/* Work Phone */
	qEdit = new QLineEdit(parent);
	m_qRightWidgets.append(qEdit);
	qPushButton = new QPushButton(tr("Work tel:"), parent);
	qPushButton->setFlat(TRUE);
	m_qLeftWidgets.append(qPushButton);
	qMenu = new QMenu();
	m_qActionWorkTel = new QAction(tr("Prefer"), this);
	qMenu->addAction(m_qActionWorkTel);
	m_qActionWorkTel->setCheckable(TRUE);
	qPushButton->setMenu(qMenu);
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qComboBox);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_PhoneWork, 20);
	contacttable->setCellWidget(Row_PhoneWork, 0, qLabel);
	contacttable->setCellWidget(Row_PhoneWork, 1, qComboBox);
#endif

	/* Home Phone */
	qEdit = new QLineEdit(parent);
	m_qRightWidgets.append(qEdit);
	qPushButton = new QPushButton(tr("Home tel:"), parent);
	qPushButton->setFlat(TRUE);
	m_qLeftWidgets.append(qPushButton);
	qMenu = new QMenu();
	m_qActionHomeTel = new QAction(tr("Prefer"), this);
	qMenu->addAction(m_qActionHomeTel);
	m_qActionHomeTel->setCheckable(TRUE);
	qPushButton->setMenu(qMenu);
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qComboBox);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_PhoneHome, 20);
	contacttable->setCellWidget(Row_PhoneHome, 0, qLabel);
	contacttable->setCellWidget(Row_PhoneHome, 1, qComboBox);
#endif

	/* Work Email */
	qEdit = new QLineEdit(parent);
	m_qRightWidgets.append(qEdit);
	qPushButton = new QPushButton(tr("Work email:"), parent);
	qPushButton->setFlat(TRUE);
	m_qLeftWidgets.append(qPushButton);
	qMenu = new QMenu();
	m_qActionWorkEmail = new QAction(tr("Prefer"), this);
	qMenu->addAction(m_qActionWorkEmail);
	m_qActionWorkEmail->setCheckable(TRUE);
	qPushButton->setMenu(qMenu);
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qComboBox);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_EmailWork, 20);
	contacttable->setCellWidget(Row_EmailWork, 0, qLabel);
	contacttable->setCellWidget(Row_EmailWork, 1, qComboBox);
#endif

	/* Home Email */
	qEdit = new QLineEdit(parent);
	m_qRightWidgets.append(qEdit);
	qPushButton = new QPushButton(tr("Home email:"), parent);
	qPushButton->setFlat(TRUE);
	m_qLeftWidgets.append(qPushButton);
	qMenu = new QMenu();
	m_qActionHomeEmail = new QAction(tr("Prefer"), this);
	qMenu->addAction(m_qActionHomeEmail);
	m_qActionHomeEmail->setCheckable(TRUE);
	qPushButton->setMenu(qMenu);
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qComboBox);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_EmailHome, 20);
	contacttable->setCellWidget(Row_EmailHome, 0, qLabel);
	contacttable->setCellWidget(Row_EmailHome, 1, qComboBox);
#endif

	/* QQ */
	qEdit = new QLineEdit(parent);
	m_qRightWidgets.append(qEdit);
	qLabel = new QLabel(tr("QQ:"), parent);
	qLabel->setBuddy(qEdit);
	qLabel->setAlignment(Qt::AlignHCenter);
	m_qLeftWidgets.append(qLabel);
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qComboBox);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_IMQQ, 20);
	contacttable->setCellWidget(Row_IMQQ, 0, qLabel);
	contacttable->setCellWidget(Row_IMQQ, 1, qComboBox);
#endif

	/* MSN */
	qEdit = new QLineEdit(parent);
	m_qRightWidgets.append(qEdit);
	qLabel = new QLabel(tr("MSN:"), parent);
	qLabel->setBuddy(qEdit);
	qLabel->setAlignment(Qt::AlignHCenter);
	m_qLeftWidgets.append(qLabel);
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qComboBox);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_IMMSN, 20);
	contacttable->setCellWidget(Row_IMMSN, 0, qLabel);
	contacttable->setCellWidget(Row_IMMSN, 1, qComboBox);
#endif

	/* Work addr */
	qComboBox = new QComboBox(parent);
	qComboBox->addItem(tr("Edit address..."));
	connect(qComboBox, SIGNAL(activated(int)), this, SLOT(onWorkAddrActivated(int)));
	m_qRightWidgets.append(qComboBox);
	qPushButton = new QPushButton(tr("Work addr:"), parent);
	qPushButton->setFlat(TRUE);
	m_qLeftWidgets.append(qPushButton);
	qMenu = new QMenu();
	m_qActionWorkAddr = new QAction(tr("Prefer"), this);
	qMenu->addAction(m_qActionWorkAddr);
	m_qActionWorkAddr->setCheckable(TRUE);
	qPushButton->setMenu(qMenu);
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qComboBox);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_AddrWork, 20);
	contacttable->setCellWidget(Row_AddrWork, 0, qLabel);
	contacttable->setCellWidget(Row_AddrWork, 1, qComboBox);
#endif

	/* Home addr */
	qComboBox = new QComboBox(parent);
	qComboBox->addItem(tr("Edit address..."));
	connect(qComboBox, SIGNAL(activated(int)), this, SLOT(onHomeAddrActivated(int)));
	m_qRightWidgets.append(qComboBox);
	qPushButton = new QPushButton(tr("Home addr:"), parent);
	qPushButton->setFlat(TRUE);
	m_qLeftWidgets.append(qPushButton);
	qMenu = new QMenu();
	m_qActionHomeAddr = new QAction(tr("Prefer"), this);
	qMenu->addAction(m_qActionHomeAddr);
	m_qActionHomeAddr->setCheckable(TRUE);
	qPushButton->setMenu(qMenu);
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qComboBox);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_AddrHome, 20);
	contacttable->setCellWidget(Row_AddrHome, 0, qLabel);
	contacttable->setCellWidget(Row_AddrHome, 1, qComboBox);
#endif

	/* URL */
	qEdit = new QLineEdit("http://", parent);
	m_qRightWidgets.append(qEdit);
	qLabel = new QLabel(tr("Web page:"), parent);
	qLabel->setBuddy(qEdit);
	qLabel->setAlignment(Qt::AlignHCenter);
	m_qLeftWidgets.append(qLabel);
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qEdit);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_Url, 20);
	contacttable->setCellWidget(Row_Url, 0, qLabel);
	contacttable->setCellWidget(Row_Url, 1, qComboBox);
#endif

	/* Ring */
	qComboBox = new QComboBox(parent);
	connect(qComboBox, SIGNAL(activated(int)), this, SLOT(onRingItemActivated(int)));
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Ring:"), parent);
	qLabel->setBuddy(qComboBox);
	qLabel->setAlignment(Qt::AlignHCenter);
	m_qLeftWidgets.append(qLabel);
	qComboBox->addItem(QIcon(":/BelugaApp/Resources/images/default_call_ring.png"), "Default Call.wav", QVariant(QString("\\My Documents\\My Music\\Default Call.wav")));
	qComboBox->addItem(QIcon(":/BelugaApp/Resources/images/default_msg_ring.png"), "Default Msg.wav", QVariant(QString("\\My Documents\\My Music\\Default Msg.wav")));
	qComboBox->addItem(tr("Another call ring..."));
	qComboBox->addItem(tr("Another msg ring..."));
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qComboBox);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_Ring, 20);
	contacttable->setCellWidget(Row_Ring, 0, qLabel);
	contacttable->setCellWidget(Row_Ring, 1, qComboBox);
#endif

	/* Birthday */
	QDateEdit * qDate = new QDateEdit(parent);
	qDate->setCalendarPopup(TRUE);
	qDate->setDisplayFormat("dd-MM-yyyy");
	qDate->setDate(QDate::currentDate());
	m_qRightWidgets.append(qDate);
	qLabel = new QLabel(tr("Birthday:"), parent);
	qLabel->setBuddy(qDate);
	qLabel->setAlignment(Qt::AlignHCenter);
	m_qLeftWidgets.append(qLabel);
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qDate);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_Birthday, 20);
	contacttable->setCellWidget(Row_Birthday, 0, qLabel);
	contacttable->setCellWidget(Row_Birthday, 1, qDate);
#endif

	/* Note */
	QPlainTextEdit * qPlainEdit = new QPlainTextEdit(parent);
	m_qRightWidgets.append(qPlainEdit);
	qLabel = new QLabel(tr("Note:"), parent);
	qLabel->setBuddy(qPlainEdit);
	qLabel->setAlignment(Qt::AlignHCenter);
	m_qLeftWidgets.append(qLabel);
#if 0
	qHLayout = new QHBoxLayout();
	qHLayout->addWidget(qLabel);
	qHLayout->addSpacing(5);
	qHLayout->addWidget(qPlainEdit);
	qVLayout->addLayout(qHLayout);
	qVLayout->addSpacing(3);
#endif
#if 0
	contacttable->setRowHeight(Row_Note, 60);
	contacttable->setCellWidget(Row_Note, 0, qLabel);
	contacttable->setCellWidget(Row_Note, 1, qPlainEdit);
#endif
	
	setWidgetRect();

	return TRUE;
}

void BelugaDetail::setWidgetRect()
{
	int i;
	int h = 3;

	for (i=0; i<m_qRightWidgets.length(); i++)
	{
		m_qLeftWidgets[i]->setGeometry(3, h, 80, 20);
		
		switch(i)
		{
		case Row_Sex:
			((QComboBox*)m_qRightWidgets[i])->setIconSize(QSize(24, 24));
			m_qRightWidgets[i]->setGeometry(85, h, 90, 26);
			h += 26;
			break;
		case Row_Photo:
			((QPushButton*)m_qRightWidgets[i])->setIconSize(QSize(42, 42));
			m_qRightWidgets[i]->setGeometry(85, h, 45, 45);
			h += 45;
			break;
		case Row_Note:
			m_qRightWidgets[i]->setGeometry(85, h, 134, 60);
			h += 60;
			break;
		default:
			m_qRightWidgets[i]->setGeometry(85, h, 134, 20);
			h += 20;
			break;
		}
		h += 2;
	}
	scrollAreaWidgetContents->setGeometry(0, 0, 240, h);
}

void BelugaDetail::onActionCancelTriggered(bool checked)
{
	reject();
}

void BelugaDetail::onDefaultActionTriggered(bool checked)
{
	gint32 ret = ECode_No_Error;
	int i = 0;
	QString group;
	QStringList items;
	int ids[20] = {0};
	bool bOK = FALSE;
	CGroupIterator * pGroupIterator = NULL;

	CGroupDb * pGroupDb = m_pBelugaMain->getGroupDb();
	ret = pGroupDb->GetAllGroupsByTag(ContactType_Phone, &pGroupIterator);
	if (ret != ECode_No_Error)
	{
		printf("Get all groups failed!\n");
		return;
	}
	
	items << QString(tr("My Contact"));
	ids[i++] = (int)GROUPID_DEFAULT;

	BOOL bSucceed = FALSE;
	do
	{
		CDbEntity * pEntity = NULL;
		ret = pGroupIterator->Current(&pEntity);	
		if (ret != ECode_No_Error)
		{
			printf("Get group instance failed!\n");
			goto _Error;
		}

		CGroup * pGroup = (CGroup*)pEntity;

		GString * groupName = NULL;
		pGroup->GetFieldValue(GroupField_Name, &groupName);
		items << QString(groupName->str);
		g_string_free(groupName, TRUE);

		GString * groupId = NULL;
		pGroup->GetFieldValue(GroupField_Id, &groupId);
		ids[i++] = atoi(groupId->str);
		g_string_free(groupId, TRUE);

		delete pGroup;
		pGroup = NULL;
	} while(0 == pGroupIterator->Next(&bSucceed) && bSucceed);

	delete pGroupIterator;
	pGroupIterator = NULL;
	
	if (m_pContact == NULL)
	{
		group = QInputDialog::getItem(this, tr("Choose Group"), tr("Add the Contact to Group:"), items, 0, FALSE, &bOK);
		if (bOK)
		{
			CPhoneContact * pContact = NULL;
			pContact = new CPhoneContact(m_pBelugaMain->getContactDb());
			if (NULL == pContact)
				return;

			getFieldsValue(pContact);
			if (0 != m_pBelugaMain->getContactDb()->SaveEntity(pContact))
			{
				QMessageBox msg(QMessageBox::Warning, tr("Error"), tr("Add Contact Failed!"), QMessageBox::Close, this);
				msg.setDefaultButton(QMessageBox::Close);
				QTimer::singleShot(2000, &msg, SLOT(accept()));
				msg.exec();
			}
			else
			{
				int nIndex = items.indexOf(group);
				if (nIndex > 0)  /* -1: invalid   0: default group */
				{
					guint32 nMaxId = 0;
					int nId = ids[nIndex];
					/* add the contact to group */
					m_pBelugaMain->getContactDb()->GetMaxId(&nMaxId);
					m_pBelugaMain->getContactDb()->CreateContactGroupRelation(nMaxId, nId);
					/* return to main view */
					m_pBelugaMain->updateGroupView(nId);
				}
				else
				{
					/* return to main view */
					m_pBelugaMain->updateGroupView((int)GROUPID_DEFAULT);
				}

				accept();
			}
		}
	}
	else
	{
		getFieldsValue(m_pContact);
		ret = m_pBelugaMain->getContactDb()->UpdateEntity(m_pContact);
		if (0 != ret)
		{
			QMessageBox msg(QMessageBox::Warning, tr("Error"), tr("Edit Contact Failed!"), QMessageBox::Close, this);
			msg.setDefaultButton(QMessageBox::Close);
			QTimer::singleShot(2000, &msg, SLOT(accept()));
			msg.exec();
			reject();
		}
		else
		{
			accept();
		}
	}


_Error:
	if (pGroupIterator != NULL)
	{
		delete pGroupIterator;
		pGroupIterator = NULL;
	}
}

void BelugaDetail::onClicked( bool checked)
{
	QString file = "";

	/* show default photo grid */
	if (m_bDIYPhoto)
		file = m_qDIYPhotoFile; 

	PhotoGallery gallery(this, file, &m_bDIYPhoto);
	if (QDialog::Accepted == gallery.exec() && !file.isEmpty())
	{
		((QPushButton*)m_qRightWidgets[Row_Photo])->setIcon(QIcon(file));
		m_qDIYPhotoFile = file;
	}
}

void BelugaDetail::onTelActionTrigggered(QAction * action)
{
	if (action == m_qActionMobileTel)
		m_nPrefTel = Row_PhoneMobile;
	else if (action == m_qActionHomeTel)
		m_nPrefTel = Row_PhoneHome;
	else
		m_nPrefTel = Row_PhoneWork;

	if (!action->isChecked())
		m_nPrefTel = Row_PhoneMobile;
}

void BelugaDetail::onEmailActionTrigggered(QAction * action)
{
	if (action == m_qActionWorkEmail)
		m_nPrefEmail = Row_EmailWork;
	else
		m_nPrefEmail = Row_EmailHome;

	if (!action->isChecked())
		m_nPrefEmail = Row_EmailWork;
}

void BelugaDetail::onAddrActionTrigggered(QAction * action)
{
	if (action == m_qActionWorkAddr)
		m_nPrefAddr = Row_AddrWork;
	else
		m_nPrefAddr = Row_AddrHome;

	if (!action->isChecked())
		m_nPrefAddr = Row_AddrWork;
}

void BelugaDetail::onHomeAddrActivated(int index)
{
	QComboBox * qHomeAddr = (QComboBox*)m_qRightWidgets[Row_AddrHome];
	
	if (qHomeAddr->count() == 2 && index == 0)
		return;

	AddrEdit addr(this, &m_stHomeAddr);
	if (QDialog::Accepted == addr.exec() && (strlen(m_stHomeAddr.city) != 0 || strlen(m_stHomeAddr.district) != 0 || strlen(m_stHomeAddr.block) != 0))
	{
		qHomeAddr->clear();
		qHomeAddr->addItem(QString("%1,%2,%3,%4").arg(m_stHomeAddr.city).arg(m_stHomeAddr.district).arg(m_stHomeAddr.street).arg(m_stHomeAddr.block));
		qHomeAddr->addItem(tr("Edit address..."));
	}
}

void BelugaDetail::onWorkAddrActivated(int index)
{
	QComboBox * qWorkAddr = (QComboBox*)m_qRightWidgets[Row_AddrWork];

	if (qWorkAddr->count() == 2 && index == 0)
		return;

	AddrEdit addr(this, &m_stWorkAddr);
	if (QDialog::Accepted == addr.exec() && (strlen(m_stWorkAddr.city) != 0 || strlen(m_stWorkAddr.district) != 0 || strlen(m_stWorkAddr.block) != 0))
	{
		qWorkAddr->clear();
		qWorkAddr->addItem(QString("%1,%2,%3,%4").arg(m_stWorkAddr.city).arg(m_stWorkAddr.district).arg(m_stHomeAddr.street).arg(m_stWorkAddr.block));
		qWorkAddr->addItem(tr("Edit address..."));
	}
}

void BelugaDetail::onRingItemActivated(int index)
{
	if (index < 2)
		return;
	
	QString ringFile;
	QString fileName = QFileDialog::getOpenFileName(this, tr("Select Ring"), "", tr("Ring Files (*.wav *.wma *.mp3 *.aac *.midi)"));
	if (!fileName.isNull())
	{
		QComboBox * qComboBox = (QComboBox*)m_qRightWidgets[Row_Ring];
		ringFile = fileName.right(fileName.length() - fileName.lastIndexOf('/') - 1);
		if (index == 2)
			qComboBox->setItemText(index - 2, ringFile);
		else if (index == 3)
			qComboBox->setItemText(index - 2, ringFile);
		
		qComboBox->setItemData(index - 2, QVariant(ringFile));
		qComboBox->setCurrentIndex(index - 2);
	}
}

void BelugaDetail::getFieldsValue(CPhoneContact * pContact)
{
	/* set contact type */
	GString * value = g_string_new("1");  /* 1: ContactType_Phone */
	pContact->SetFieldValue(ContactField_Type, value);
	g_string_free(value, TRUE);

	/* set contact uid: default uid is name */
	QString string = ((QLineEdit*)m_qRightWidgets[Row_Name])->text();
	value = g_string_new(string.toLatin1().data());
	pContact->SetFieldValue(ContactField_UserId, value);

	/* set contact name */
	pContact->SetFieldValue(ContactField_Name, value);
	g_string_free(value, TRUE);

	/* set contact name spell */
	QString spell = Chinese2PY(string);
	GString * namespell = g_string_new(spell.toLatin1().data()); 
	pContact->SetFieldValue(ContactField_NameSpell, namespell);
	g_string_free(namespell, TRUE);

	/* set contact nickname */
	string = ((QLineEdit*)m_qRightWidgets[Row_NickName])->text();
	value = g_string_new(string.toLatin1().data());
	pContact->SetFieldValue(ContactField_NickName, value);

	/* set contact nickname spell */
	spell = Chinese2PY(string);
	namespell = g_string_new(spell.toLatin1().data()); 
	pContact->SetFieldValue(ContactField_NickNameSpell, namespell);
	g_string_free(namespell, TRUE);

	/* set contact sex */
	QVariant sex = ((QComboBox*)m_qRightWidgets[Row_Sex])->itemData(((QComboBox*)m_qRightWidgets[Row_Sex])->currentIndex());
	char szSex[2] = {0};
	sprintf(szSex, "%d", sex.toInt());
	value = g_string_new(szSex); /* 0: male  1: female */
	pContact->SetFieldValue(ContactField_Sex, value);
	g_string_free(value, TRUE);

	/* set contact photo */
	value = g_string_new(m_qDIYPhotoFile.toLatin1().data());
	pContact->SetFieldValue(ContactField_Photo, value);
	g_string_free(value, TRUE);

	/* set contact signature */
	value = g_string_new(tr("Hi, Belugaer").toLatin1().data());
	pContact->SetFieldValue(ContactField_Signature, value);
	g_string_free(value, TRUE);

	/* set contact birthday */
	QDate birthday = ((QDateEdit*)m_qRightWidgets[Row_Birthday])->date();
	char szDate[10] = {0};
	sprintf(szDate, "%d-%02d-%02d", birthday.year(), birthday.month(), birthday.day());
	value = g_string_new(szDate);
	pContact->SetFieldValue(ContactField_Birthday, value);
	g_string_free(value, TRUE);

	/* set contact org */
	string = ((QLineEdit*)m_qRightWidgets[Row_Org])->text();
	value = g_string_new(string.toLatin1().data());
	pContact->SetFieldValue(ContactField_Org, value);
	g_string_free(value, TRUE);

	/* set contact Url */
	string = ((QLineEdit*)m_qRightWidgets[Row_Url])->text();
	value = g_string_new(string.toLatin1().data());
	pContact->SetFieldValue(ContactField_Url, value);
	g_string_free(value, TRUE);

	/* set contact title */
	string = ((QLineEdit*)m_qRightWidgets[Row_Title])->text();
	value = g_string_new(string.toLatin1().data());
	pContact->SetFieldValue(ContactField_Title, value);
	g_string_free(value, TRUE);

	/* set contact ring */
	QString callRing = ((QComboBox*)m_qRightWidgets[Row_Ring])->itemData(0).toString();
	QString msgRing = ((QComboBox*)m_qRightWidgets[Row_Ring])->itemData(1).toString();
	value = g_string_new(callRing.toLatin1().data()); 
	pContact->SetFieldValue(ContactField_CallRing, value);
	value = g_string_assign(value, msgRing.toLatin1().data()); 
	pContact->SetFieldValue(ContactField_MsgRing, value);
	g_string_free(value, TRUE);

	/* set contact note */
	string = ((QPlainTextEdit*)m_qRightWidgets[Row_Note])->toPlainText();
	value = g_string_new(string.toLatin1().data()); 
	pContact->SetFieldValue(ContactField_Note, value);
	g_string_free(value, TRUE);

	/* set mobile phone */ 
	QString mobilephone = ((QLineEdit*)m_qRightWidgets[Row_PhoneMobile])->text();
	pContact->SetPhone((ECommType)(CommType_Mobile | CommType_Phone), mobilephone.toLatin1().data());
	
	/* set home phone */ 
	QString homephone = ((QLineEdit*)m_qRightWidgets[Row_PhoneHome])->text();
	pContact->SetPhone((ECommType)(CommType_Home | CommType_Phone), homephone.toLatin1().data());

	/* set work phone */ 
	QString workphone = ((QLineEdit*)m_qRightWidgets[Row_PhoneWork])->text();
	pContact->SetPhone((ECommType)(CommType_Work | CommType_Phone), workphone.toLatin1().data());
	
	/* set pref phone */
	if (((QLineEdit*)m_qRightWidgets[m_nPrefTel])->text().isEmpty())
	{
		if (mobilephone.isEmpty())
			m_nPrefTel = Row_PhoneWork;
		if (workphone.isEmpty())
			m_nPrefTel = Row_PhoneHome;
	}
	value = g_string_new(((QLineEdit*)m_qRightWidgets[m_nPrefTel])->text().toLatin1().data());
	pContact->SetFieldValue(ContactField_PhonePref, value);
	g_string_free(value, TRUE);

	/* set home email */ 
	QString homeemail = ((QLineEdit*)m_qRightWidgets[Row_EmailHome])->text();
	pContact->SetEmail((ECommType)(CommType_Home | CommType_Email), homeemail.toLatin1().data());

	/* set work email */ 
	QString workemail = ((QLineEdit*)m_qRightWidgets[Row_EmailWork])->text();
	pContact->SetEmail((ECommType)(CommType_Work | CommType_Email), workemail.toLatin1().data());

	/* set pref email */
	if (((QLineEdit*)m_qRightWidgets[m_nPrefEmail])->text().isEmpty())
	{
		if (workemail.isEmpty())
			m_nPrefEmail = Row_EmailHome;
	}
	value = g_string_new(((QLineEdit*)m_qRightWidgets[m_nPrefEmail])->text().toLatin1().data());
	pContact->SetFieldValue(ContactField_EmailPref, value);
	g_string_free(value, TRUE);

	/* set home address */ 
	pContact->SetAddress((ECommType)(CommType_Home | CommType_Address), &m_stHomeAddr);

	/* set work address */ 
	pContact->SetAddress((ECommType)(CommType_Work | CommType_Address), &m_stWorkAddr);

	/* set QQ */ 
	string = ((QLineEdit*)m_qRightWidgets[Row_IMQQ])->text();
	pContact->SetIM((ECommType)(CommType_IM | CommType_QQ), string.toLatin1().data());

	/* set MSN */ 
	string = ((QLineEdit*)m_qRightWidgets[Row_IMMSN])->text();
	pContact->SetIM((ECommType)(CommType_IM | CommType_MSN), string.toLatin1().data());
}

void BelugaDetail::setFieldsValue(CPhoneContact * pContact)
{
	m_pContact = pContact;
	if(m_pContact == NULL)
		return;

	/* fill basic contact info to fields */
	/* set contact name */
	GString * value = NULL;
	pContact->GetFieldValue(ContactField_Name, &value);
	((QLineEdit*)m_qRightWidgets[Row_Name])->setText(QString(value->str));
	g_string_free(value, TRUE);

	/* set contact nickname */
	pContact->GetFieldValue(ContactField_NickName, &value);
	((QLineEdit*)m_qRightWidgets[Row_NickName])->setText(QString(value->str));
	g_string_free(value, TRUE);

	/* set contact sex */
	pContact->GetFieldValue(ContactField_Sex, &value); /* 0: male  1: female */
	int nIndex = ((QComboBox*)m_qRightWidgets[Row_Sex])->currentIndex();
	if (((QComboBox*)m_qRightWidgets[Row_Sex])->itemData(nIndex).toInt() != atoi(value->str))
		((QComboBox*)m_qRightWidgets[Row_Sex])->setCurrentIndex(1 - nIndex);
	g_string_free(value, TRUE);

	/* set contact photo */
	pContact->GetFieldValue(ContactField_Photo, &value);
	m_qDIYPhotoFile = QString(value->str);
	((QPushButton*)m_qRightWidgets[Row_Photo])->setIcon(QIcon(m_qDIYPhotoFile));
	g_string_free(value, TRUE);

	/* set contact birthday */
	pContact->GetFieldValue(ContactField_Birthday, &value);
	((QDateEdit*)m_qRightWidgets[Row_Birthday])->setDate(QDate::fromString(QString(value->str), "yyyy-MM-dd"));
	g_string_free(value, TRUE);

	/* set contact org */
	pContact->GetFieldValue(ContactField_Org, &value);
	((QLineEdit*)m_qRightWidgets[Row_Org])->setText(QString(value->str));	
	g_string_free(value, TRUE);

	/* set contact Url */
	pContact->GetFieldValue(ContactField_Url, &value);
	((QLineEdit*)m_qRightWidgets[Row_Url])->setText(QString(value->str));	
	g_string_free(value, TRUE);

	/* set contact title */
	pContact->GetFieldValue(ContactField_Title, &value);
	((QLineEdit*)m_qRightWidgets[Row_Title])->setText(QString(value->str));	
	g_string_free(value, TRUE);

	/* set contact ring */
	pContact->GetFieldValue(ContactField_CallRing, &value);
	QString fileName = QString(value->str);
	((QComboBox*)m_qRightWidgets[Row_Ring])->setItemText(0, fileName.right(fileName.length() - fileName.lastIndexOf('/') - 1));
	((QComboBox*)m_qRightWidgets[Row_Ring])->setItemData(0, QVariant(fileName));
	g_string_free(value, TRUE);

	pContact->GetFieldValue(ContactField_MsgRing, &value);
	fileName = QString(value->str);
	((QComboBox*)m_qRightWidgets[Row_Ring])->setItemText(1, fileName.right(fileName.length() - fileName.lastIndexOf('/') - 1));
	((QComboBox*)m_qRightWidgets[Row_Ring])->setItemData(1, QVariant(fileName));
	g_string_free(value, TRUE);

	/* set contact note */
	pContact->GetFieldValue(ContactField_Note, &value);
	((QPlainTextEdit*)m_qRightWidgets[Row_Note])->setPlainText(QString(value->str));
	g_string_free(value, TRUE);

	/* set mobile phone */
	pContact->GetFieldValue(ContactField_PhonePref, &value);

	gchar* sPhone = NULL;
	pContact->GetPhone((ECommType)(CommType_Mobile | CommType_Phone), &sPhone);
	if (sPhone != NULL)
	{
		((QLineEdit*)m_qRightWidgets[Row_PhoneMobile])->setText(QString(sPhone));
		if (g_strcasecmp(value->str, sPhone) == 0)
			m_qActionMobileTel->setChecked(TRUE);
		g_free(sPhone);
	}

	/* set home phone */ 
	sPhone = NULL;
	pContact->GetPhone((ECommType)(CommType_Home | CommType_Phone), &sPhone);
	if (sPhone != NULL)
	{
		((QLineEdit*)m_qRightWidgets[Row_PhoneHome])->setText(QString(sPhone));
		if (g_strcasecmp(value->str, sPhone) == 0)
			m_qActionHomeTel->setChecked(TRUE);
		g_free(sPhone);
	}

	/* set work phone */ 
	sPhone = NULL;
	pContact->GetPhone((ECommType)(CommType_Work | CommType_Phone), &sPhone);
	if (sPhone != NULL)
	{
		((QLineEdit*)m_qRightWidgets[Row_PhoneWork])->setText(QString(sPhone));
		if (g_strcasecmp(value->str, sPhone) == 0)
			m_qActionWorkTel->setChecked(TRUE);
		g_free(sPhone);
		g_string_free(value, TRUE);
	}

	/* set home email */ 
	pContact->GetFieldValue(ContactField_EmailPref, &value);

	gchar* sEmail = NULL;
	pContact->GetEmail((ECommType)(CommType_Home | CommType_Email), &sEmail);
	if (sEmail != NULL)
	{
		((QLineEdit*)m_qRightWidgets[Row_EmailHome])->setText(QString(sEmail));
		if (g_strcasecmp(value->str, sEmail) == 0)
			m_qActionHomeEmail->setChecked(TRUE);
		g_free(sEmail);
	}

	/* set work email */ 
	sEmail = NULL;
	pContact->GetPhone((ECommType)(CommType_Work | CommType_Email), &sEmail);
	if (sEmail != NULL)
	{
		((QLineEdit*)m_qRightWidgets[Row_EmailWork])->setText(QString(sEmail));
		if (g_strcasecmp(value->str, sEmail) == 0)
			m_qActionWorkEmail->setChecked(TRUE);
		g_free(sEmail);
		g_string_free(value, TRUE);
	}

	/* set home address */ 
	stAddress* addr = NULL;
	pContact->GetAddress((ECommType)(CommType_Home | CommType_Address), &addr);
	if (addr != NULL)
	{
		memcpy(&m_stHomeAddr, addr, sizeof(stAddress));
		g_free(addr);
		((QComboBox*)m_qRightWidgets[Row_AddrHome])->setItemText(0, QString("%1,%2,%3,%4").arg(m_stHomeAddr.city).arg(m_stHomeAddr.district).arg(m_stHomeAddr.street).arg(m_stHomeAddr.block));
	}

	/* set work address */ 
	pContact->GetAddress((ECommType)(CommType_Work | CommType_Address), &addr);
	if (addr != NULL)
	{
		memcpy(&m_stWorkAddr, addr, sizeof(stAddress));
		g_free(addr);
		((QComboBox*)m_qRightWidgets[Row_AddrWork])->setItemText(0, QString("%1,%2,%3,%4").arg(m_stHomeAddr.city).arg(m_stHomeAddr.district).arg(m_stHomeAddr.street).arg(m_stHomeAddr.block));
	}

	/* set QQ */ 
	gchar * im = NULL;
	pContact->GetIM((ECommType)(CommType_IM | CommType_QQ), &im);
	if (im != NULL)
	{
		((QLineEdit*)m_qRightWidgets[Row_IMQQ])->setText(QString(im));	
		g_free(im);	
	}

	/* set MSN */ 
	im = NULL;
	pContact->GetIM((ECommType)(CommType_IM | CommType_MSN), &im);
	if (im != NULL)
	{
		((QLineEdit*)m_qRightWidgets[Row_IMMSN])->setText(QString(im));	
		g_free(im);	
	}
}

void BelugaDetail::paintEvent(QPaintEvent *)
{
	QStyleOption opt;
	opt.init(this);
	QPainter p(this);
	style()->drawPrimitive(QStyle::PE_Widget, &opt, &p, this);
}
