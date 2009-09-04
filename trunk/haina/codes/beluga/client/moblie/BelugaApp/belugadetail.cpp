
#include "belugamain.h"
#include "belugadetail.h"


BelugaDetail::BelugaDetail(QWidget *parent /* = 0 */, Qt::WFlags flags /* = 0 */)
{
	this->setModal(TRUE);
	m_pBelugaMain = (BelugaMain*)parent;
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
	connect(m_qMenuBar, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
	m_qActionOk = new QAction(tr("Ok"), this);
	m_qActionCancel = new QAction(tr("Cancel"), this);;
	m_qMenuBar->addAction(m_qActionOk);
	m_qMenuBar->addAction(m_qActionCancel);
	m_qMenuBar->setDefaultAction(m_qActionOk);
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
	m_qLeftWidgets.append(qLabel);
	qComboBox->addItem(QIcon(":/BelugaApp/Resources/images/male.png"), tr("Male"), QVariant(1));
	qComboBox->addItem(QIcon(":/BelugaApp/Resources/images/female.png"), tr("Female"), QVariant(0));

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
	qComboBox = new QComboBox(parent);
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Photo:"), parent);
	qLabel->setBuddy(qComboBox);
	m_qLeftWidgets.append(qLabel);
	qComboBox->addItem(QIcon(":/BelugaApp/Resources/images/male.png"), tr(""), QVariant(1));
	qComboBox->addItem(QIcon(":/BelugaApp/Resources/images/female.png"), tr(""), QVariant(0));
	qComboBox->addItem(QIcon(":/BelugaApp/Resources/images/male.png"), tr(""), QVariant(1));
	qComboBox->addItem(QIcon(":/BelugaApp/Resources/images/female.png"), tr(""), QVariant(0));
	qComboBox->addItem(QIcon(":/BelugaApp/Resources/images/male.png"), tr(""), QVariant(1));
	qComboBox->addItem(QIcon(":/BelugaApp/Resources/images/female.png"), tr(""), QVariant(0));
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
	qComboBox = new QComboBox(parent);
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Company:"), parent);
	qLabel->setBuddy(qComboBox);
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
	qComboBox = new QComboBox(parent);
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Job title:"), parent);
	qLabel->setBuddy(qComboBox);
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
	qComboBox = new QComboBox(parent);
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Mobile tel:"), parent);
	qLabel->setBuddy(qComboBox);
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
	contacttable->setRowHeight(Row_PhoneMobile, 20);
	contacttable->setCellWidget(Row_PhoneMobile, 0, qLabel);
	contacttable->setCellWidget(Row_PhoneMobile, 1, qComboBox);
#endif

	/* Work Phone */
	qComboBox = new QComboBox(parent);
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Work tel:"), parent);
	qLabel->setBuddy(qComboBox);
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
	contacttable->setRowHeight(Row_PhoneWork, 20);
	contacttable->setCellWidget(Row_PhoneWork, 0, qLabel);
	contacttable->setCellWidget(Row_PhoneWork, 1, qComboBox);
#endif

	/* Home Phone */
	qComboBox = new QComboBox(parent);
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Home tel:"), parent);
	qLabel->setBuddy(qComboBox);
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
	contacttable->setRowHeight(Row_PhoneHome, 20);
	contacttable->setCellWidget(Row_PhoneHome, 0, qLabel);
	contacttable->setCellWidget(Row_PhoneHome, 1, qComboBox);
#endif

	/* Work Email */
	qComboBox = new QComboBox(parent);
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Work email:"), parent);
	qLabel->setBuddy(qComboBox);
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
	contacttable->setRowHeight(Row_EmailWork, 20);
	contacttable->setCellWidget(Row_EmailWork, 0, qLabel);
	contacttable->setCellWidget(Row_EmailWork, 1, qComboBox);
#endif

	/* Home Email */
	qComboBox = new QComboBox(parent);
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Home email:"), parent);
	qLabel->setBuddy(qComboBox);
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
	contacttable->setRowHeight(Row_EmailHome, 20);
	contacttable->setCellWidget(Row_EmailHome, 0, qLabel);
	contacttable->setCellWidget(Row_EmailHome, 1, qComboBox);
#endif

	/* QQ */
	qComboBox = new QComboBox(parent);
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("QQ:"), parent);
	qLabel->setBuddy(qComboBox);
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
	qComboBox = new QComboBox(parent);
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("MSN:"), parent);
	qLabel->setBuddy(qComboBox);
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

	/* URL */
	qEdit = new QLineEdit("http://", parent);
	m_qRightWidgets.append(qEdit);
	qLabel = new QLabel(tr("Web page:"), parent);
	qLabel->setBuddy(qEdit);
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
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Ring:"), parent);
	qLabel->setBuddy(qComboBox);
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
	contacttable->setRowHeight(Row_Ring, 20);
	contacttable->setCellWidget(Row_Ring, 0, qLabel);
	contacttable->setCellWidget(Row_Ring, 1, qComboBox);
#endif

	/* Birthday */
	QDateEdit * qDate = new QDateEdit(parent);
	qDate->setCalendarPopup(TRUE);
	m_qRightWidgets.append(qDate);
	qLabel = new QLabel(tr("Birthday:"), parent);
	qLabel->setBuddy(qDate);
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

	/* Work addr */
	qComboBox = new QComboBox(parent);
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Work addr:"), parent);
	qLabel->setBuddy(qComboBox);
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
	contacttable->setRowHeight(Row_AddrWork, 20);
	contacttable->setCellWidget(Row_AddrWork, 0, qLabel);
	contacttable->setCellWidget(Row_AddrWork, 1, qComboBox);
#endif

	/* Home addr */
	qComboBox = new QComboBox(parent);
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Home addr:"), parent);
	qLabel->setBuddy(qComboBox);
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
	contacttable->setRowHeight(Row_AddrHome, 20);
	contacttable->setCellWidget(Row_AddrHome, 0, qLabel);
	contacttable->setCellWidget(Row_AddrHome, 1, qComboBox);
#endif
	
	/* Note */
	QPlainTextEdit * qPlainEdit = new QPlainTextEdit(parent);
	m_qRightWidgets.append(qPlainEdit);
	qLabel = new QLabel(tr("Note:"), parent);
	qLabel->setBuddy(qPlainEdit);
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
		m_qLeftWidgets[i]->setAlignment(Qt::AlignHCenter);
		m_qLeftWidgets[i]->setBuddy(m_qRightWidgets[i]);
		
		switch(i)
		{
		case Row_Sex:
			((QComboBox*)m_qRightWidgets[i])->setIconSize(QSize(25, 25));
			m_qRightWidgets[i]->setGeometry(85, h, 100, 27);
			h += 27;
			break;
		case Row_Photo:
			((QComboBox*)m_qRightWidgets[i])->setIconSize(QSize(45, 45));
			m_qRightWidgets[i]->setGeometry(85, h, 100, 47);
			h += 47;
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

void BelugaDetail::onActionTriggered(QAction* action)
{
	if (action == m_qActionOk)
	{

	}
	else if (action == m_qActionCancel)
	{

	}
}

void BelugaDetail::onDefaultActionTriggered(bool checked)
{
	
}