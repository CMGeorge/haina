
#include "belugamain.h"
#include "belugadetail.h"


BelugaDetail::BelugaDetail(QWidget *parent /* = 0 */, Qt::WFlags flags /* = 0 */)
{
	this->setModal(TRUE);
	m_pBelugaMain = (BelugaMain*)parent;
	setupUi(this);
	contacttable->setVisible(FALSE);
	contacttable->setColumnWidth(0, 80);
	contacttable->setColumnWidth(1, 160);
	
	/* initialize fields */
	initializeFields();

	contacttable->setVisible(TRUE);
}

BelugaDetail::~BelugaDetail()
{

}

BOOL BelugaDetail::initializeFields()
{
	QLineEdit * qEdit = NULL;
	QLabel * qLabel = NULL;
	QComboBox * qComboBox = NULL;
	
	/* name */
	qEdit = new QLineEdit();
	m_qRightWidgets.append(qEdit);
	qLabel = new QLabel(tr("Name:"));
	qLabel->setBuddy(qEdit);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_Name, 20);
	contacttable->setCellWidget(Row_Name, 0, qLabel);
	contacttable->setCellWidget(Row_Name, 1, qEdit);
	
	/* nickname */
	qEdit = new QLineEdit();
	m_qRightWidgets.append(qEdit);
	qLabel = new QLabel(tr("NickName:"));
	qLabel->setBuddy(qEdit);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_NickName, 20);
	contacttable->setCellWidget(Row_NickName, 0, qLabel);
	contacttable->setCellWidget(Row_NickName, 1, qEdit);

	/* sex */
	qComboBox = new QComboBox();
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Gender:"));
	qLabel->setBuddy(qComboBox);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_Sex, 20);
	contacttable->setCellWidget(Row_Sex, 0, qLabel);
	contacttable->setCellWidget(Row_Sex, 1, qComboBox);

	/* photo */
	qComboBox = new QComboBox();
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Photo:"));
	qLabel->setBuddy(qComboBox);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_Photo, 20);
	contacttable->setCellWidget(Row_Photo, 0, qLabel);
	contacttable->setCellWidget(Row_Photo, 1, qComboBox);

	/* org */
	qComboBox = new QComboBox();
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Company:"));
	qLabel->setBuddy(qComboBox);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_Org, 20);
	contacttable->setCellWidget(Row_Org, 0, qLabel);
	contacttable->setCellWidget(Row_Org, 1, qComboBox);

	/* title */
	qComboBox = new QComboBox();
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Job title:"));
	qLabel->setBuddy(qComboBox);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_Title, 20);
	contacttable->setCellWidget(Row_Title, 0, qLabel);
	contacttable->setCellWidget(Row_Title, 1, qComboBox);

	/* Mobile Phone */
	qComboBox = new QComboBox();
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Mobile tel:"));
	qLabel->setBuddy(qComboBox);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_PhoneMobile, 20);
	contacttable->setCellWidget(Row_PhoneMobile, 0, qLabel);
	contacttable->setCellWidget(Row_PhoneMobile, 1, qComboBox);

	/* Work Phone */
	qComboBox = new QComboBox();
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Work tel:"));
	qLabel->setBuddy(qComboBox);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_PhoneWork, 20);
	contacttable->setCellWidget(Row_PhoneWork, 0, qLabel);
	contacttable->setCellWidget(Row_PhoneWork, 1, qComboBox);

	/* Home Phone */
	qComboBox = new QComboBox();
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Home tel:"));
	qLabel->setBuddy(qComboBox);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_PhoneHome, 20);
	contacttable->setCellWidget(Row_PhoneHome, 0, qLabel);
	contacttable->setCellWidget(Row_PhoneHome, 1, qComboBox);

	/* Work Email */
	qComboBox = new QComboBox();
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Work email:"));
	qLabel->setBuddy(qComboBox);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_EmailWork, 20);
	contacttable->setCellWidget(Row_EmailWork, 0, qLabel);
	contacttable->setCellWidget(Row_EmailWork, 1, qComboBox);

	/* Home Email */
	qComboBox = new QComboBox();
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Home email:"));
	qLabel->setBuddy(qComboBox);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_EmailHome, 20);
	contacttable->setCellWidget(Row_EmailHome, 0, qLabel);
	contacttable->setCellWidget(Row_EmailHome, 1, qComboBox);

	/* QQ */
	qComboBox = new QComboBox();
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("QQ:"));
	qLabel->setBuddy(qComboBox);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_IMQQ, 20);
	contacttable->setCellWidget(Row_IMQQ, 0, qLabel);
	contacttable->setCellWidget(Row_IMQQ, 1, qComboBox);

	/* MSN */
	qComboBox = new QComboBox();
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("MSN:"));
	qLabel->setBuddy(qComboBox);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_IMMSN, 20);
	contacttable->setCellWidget(Row_IMMSN, 0, qLabel);
	contacttable->setCellWidget(Row_IMMSN, 1, qComboBox);

	/* URL */
	qEdit = new QLineEdit("http://");
	m_qRightWidgets.append(qEdit);
	qLabel = new QLabel(tr("Web page:"));
	qLabel->setBuddy(qEdit);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_Url, 20);
	contacttable->setCellWidget(Row_Url, 0, qLabel);
	contacttable->setCellWidget(Row_Url, 1, qComboBox);

	/* Ring */
	qComboBox = new QComboBox();
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Ring:"));
	qLabel->setBuddy(qComboBox);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_Ring, 20);
	contacttable->setCellWidget(Row_Ring, 0, qLabel);
	contacttable->setCellWidget(Row_Ring, 1, qComboBox);

	/* Birthday */
	QDateEdit * qDate = new QDateEdit();
	qDate->setCalendarPopup(TRUE);
	m_qRightWidgets.append(qDate);
	qLabel = new QLabel(tr("Birthday:"));
	qLabel->setBuddy(qDate);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_Birthday, 20);
	contacttable->setCellWidget(Row_Birthday, 0, qLabel);
	contacttable->setCellWidget(Row_Birthday, 1, qDate);

	/* Work addr */
	qComboBox = new QComboBox();
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Work addr:"));
	qLabel->setBuddy(qComboBox);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_Ring, 20);
	contacttable->setCellWidget(Row_Ring, 0, qLabel);
	contacttable->setCellWidget(Row_Ring, 1, qComboBox);

	/* Home addr */
	qComboBox = new QComboBox();
	m_qRightWidgets.append(qComboBox);
	qLabel = new QLabel(tr("Home addr:"));
	qLabel->setBuddy(qComboBox);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_Ring, 20);
	contacttable->setCellWidget(Row_Ring, 0, qLabel);
	contacttable->setCellWidget(Row_Ring, 1, qComboBox);
	
	/* Note */
	QPlainTextEdit * qPlainEdit = new QPlainTextEdit();
	m_qRightWidgets.append(qPlainEdit);
	qLabel = new QLabel(tr("Note:"));
	qLabel->setBuddy(qPlainEdit);
	m_qLeftWidgets.append(qLabel);
	contacttable->setRowHeight(Row_Ring, 20);
	contacttable->setCellWidget(Row_Ring, 0, qLabel);
	contacttable->setCellWidget(Row_Ring, 1, qPlainEdit);

	return TRUE;
}