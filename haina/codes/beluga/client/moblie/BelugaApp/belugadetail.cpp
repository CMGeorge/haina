
#include "belugadetail.h"

BelugaDetail::BelugaDetail(QWidget *parent /* = 0 */, Qt::WFlags flags /* = 0 */)
{
	this->setModal(TRUE);
	setupUi(this);
}

BelugaDetail::~BelugaDetail()
{
	if (m_pContactDb == NULL)
	{
		delete m_pContactDb;
		m_pContactDb = NULL;
	}
	if (m_pGroupDb == NULL)
	{
		delete m_pGroupDb;
		m_pGroupDb = NULL;
	}
}