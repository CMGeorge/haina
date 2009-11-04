
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
			countryedit->setText(QString::fromUtf8(m_pAddr->country));
		if (strlen(m_pAddr->state))
			stateedit->setText(QString::fromUtf8(m_pAddr->state));
		if (strlen(m_pAddr->city))
			cityedit->setText(QString::fromUtf8(m_pAddr->city));
		if (strlen(m_pAddr->district))
			districtedit->setText(QString::fromUtf8(m_pAddr->district));
		if (strlen(m_pAddr->street))
			streetedit->setText(QString::fromUtf8(m_pAddr->street));
		if (strlen(m_pAddr->block))
			blockedit->setText(QString::fromUtf8(m_pAddr->block));
		if (strlen(m_pAddr->postcode))
			postcodeedit->setText(QString::fromUtf8(m_pAddr->postcode));
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
	strcpy(m_pAddr->country, countryedit->text().toUtf8().data());
	strcpy(m_pAddr->state, stateedit->text().toUtf8().data());
	strcpy(m_pAddr->city, cityedit->text().toUtf8().data());
	strcpy(m_pAddr->district, districtedit->text().toUtf8().data());
	strcpy(m_pAddr->block, blockedit->text().toUtf8().data());
	strcpy(m_pAddr->postcode, postcodeedit->text().toUtf8().data());
	strcpy(m_pAddr->street, streetedit->text().toUtf8().data());

	QDialog::accept();
}

void AddrEdit::paintEvent(QPaintEvent *)
{
	QStyleOption opt;
	opt.init(this);
	QPainter p(this);
	style()->drawPrimitive(QStyle::PE_Widget, &opt, &p, this);
}
