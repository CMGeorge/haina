
#include "addredit.h"



AddrEdit::AddrEdit(QWidget *parent, stAddress * addr, Qt::WFlags flags) 
							: m_pAddr(addr)
{
	this->setModal(TRUE);
	setupUi(this);
	connect(buttonBox, SIGNAL(accepted()), this, SLOT(onAccepted()));

	if (m_pAddr != NULL)
	{
		if (strlen(m_pAddr->country))
			countryedit->setText(QString(m_pAddr->country));
		if (strlen(m_pAddr->state))
			stateedit->setText(QString(m_pAddr->state));
		if (strlen(m_pAddr->city))
			cityedit->setText(QString(m_pAddr->city));
		if (strlen(m_pAddr->district))
			districtedit->setText(QString(m_pAddr->district));
		if (strlen(m_pAddr->street))
			streetedit->setText(QString(m_pAddr->street));
		if (strlen(m_pAddr->block))
			blockedit->setText(QString(m_pAddr->block));
		if (strlen(m_pAddr->postcode))
			postcodeedit->setText(QString(m_pAddr->postcode));
	}
}

AddrEdit::~AddrEdit()
{

}

void AddrEdit::accept()
{
	onAccepted();
}

void AddrEdit::onAccepted()
{
	strcpy(m_pAddr->country, countryedit->text().toLatin1().data());
	strcpy(m_pAddr->state, stateedit->text().toLatin1().data());
	strcpy(m_pAddr->city, cityedit->text().toLatin1().data());
	strcpy(m_pAddr->district, districtedit->text().toLatin1().data());
	strcpy(m_pAddr->block, blockedit->text().toLatin1().data());
	strcpy(m_pAddr->postcode, postcodeedit->text().toLatin1().data());
	strcpy(m_pAddr->street, streetedit->text().toLatin1().data());

	QDialog::accept();
}

void AddrEdit::paintEvent(QPaintEvent *)
{
	QStyleOption opt;
	opt.init(this);
	QPainter p(this);
	style()->drawPrimitive(QStyle::PE_Widget, &opt, &p, this);
}
