
#include "belugamain.h"
#include "personstatus.h"


PersonStatus::PersonStatus(QWidget *parent /* = 0 */, Qt::WFlags flags /* = 0 */)
{
	m_pContact = NULL;
	m_nContactId = -1;
	m_pBelugaMain = (BelugaMain*)parent;
	setupUi(this);
	
	//connect(contactlist, SIGNAL(itemClicked(QListWidgetItem*)), this, SLOT(onItemClicked(QListWidgetItem*)));
	//connect(contactlist, SIGNAL(itemDoubleClicked(QListWidgetItem*)), this, SLOT(onItemDoubleClicked(QListWidgetItem*)));
}

PersonStatus::~PersonStatus()
{
	if (m_pContact)
		delete m_pContact;

	m_pContact = NULL;
}

void PersonStatus::setContact(int nContactId)
{
	m_nContactId = nContactId;
}

void PersonStatus::paintEvent(QPaintEvent *)
{
	QStyleOption opt;
	opt.init(this);
	QPainter p(this);
	style()->drawPrimitive(QStyle::PE_Widget, &opt, &p, this);
}

void PersonStatus::onFun1Clicked( bool checked)
{

}

void PersonStatus::onFun2Clicked( bool checked)
{

}

void PersonStatus::onFun3Clicked( bool checked)
{

}

void PersonStatus::onFun4Clicked( bool checked)
{

}

void PersonStatus::onFun5Clicked( bool checked)
{

}

void PersonStatus::onFun6Clicked( bool checked)
{

}

void PersonStatus::onWeatherClicked( bool checked)
{

}