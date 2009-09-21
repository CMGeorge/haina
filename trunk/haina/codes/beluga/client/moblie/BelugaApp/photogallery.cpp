
#include "photogallery.h"



PhotoGallery::PhotoGallery(QWidget *parent, QString & diyIconFile, bool * bDIY, Qt::WFlags flags) 
							: m_qIconFile(diyIconFile), m_bDIY(bDIY)
{
	QTableWidgetItem * qTableItem = NULL;
	int i = 1, j = 0;
	int k = 1;
	
	this->setModal(TRUE);
	setupUi(this);
	maletable->setShowGrid(TRUE);
	maletable->setColumnCount(maletable->width()/45);
	maletable->setRowCount(maletable->height()/45);
	connect(maletable, SIGNAL(itemClicked(QTableWidgetItem*)), this, SLOT(onItemClicked(QTableWidgetItem*)));

	m_qActionOk = new QAction(tr("Ok"), this);
	connect(m_qActionOk, SIGNAL(triggered(bool)), this, SLOT(onDefaultActionTriggered(bool)));

	m_qMenuBar = new QMenuBar(this);
	m_qActionClear = new QAction(tr("Clear DIY"), this);
	m_qMenuBar->addAction(m_qActionClear);
	m_qActionCancel = new QAction(tr("Cancel"), this);
	m_qMenuBar->addAction(m_qActionCancel);

	connect(m_qMenuBar, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
	m_qMenuBar->setDefaultAction(m_qActionOk);

	/* create photo labels */
	maletable->setRowHeight(0, 45);
	maletable->setColumnWidth(0, 45);
	if (diyIconFile.isEmpty())
		qTableItem = new QTableWidgetItem(tr("DIY"));
	else
		qTableItem = new QTableWidgetItem(QIcon(diyIconFile), "");
	qTableItem->setTextAlignment(Qt::AlignHCenter | Qt::AlignVCenter);
	maletable->setItem(0, 0, qTableItem);

	while (i < maletable->rowCount())
	{
		maletable->setRowHeight(i, 45);
		while (j < maletable->columnCount())
		{
			maletable->setColumnWidth(j, 45);
			QTableWidgetItem * qTableItem = new QTableWidgetItem(QIcon(QString(":/BelugaApp/Resources/jobs/job_%1.png").arg(k)), "");
			qTableItem->setData(Qt::UserRole, QVariant(k));
			maletable->setItem(i, j, qTableItem);
			j++;
			k++;
			if (k > 16) return;
		}
		i++;
		j = 0;
	}
}

PhotoGallery::~PhotoGallery()
{

}

void PhotoGallery::onItemClicked(QTableWidgetItem * item)
{
	if (0 == item->row())
	{
		QString fileName = QFileDialog::getOpenFileName(this, tr("Open Image"), "", tr("Image Files (*.png *.bmp)"));
		if (!fileName.isNull())
		{
			item->setText("");
			item->setIcon(QIcon(fileName));
			m_qIconFile.clear();
			m_qIconFile.append(fileName);
			*m_bDIY = TRUE;
		}
	}
	else
	{
		m_qIconFile.clear();
		m_qIconFile.append(QString(":/BelugaApp/Resources/jobs/job_%1.png").arg(item->data(Qt::UserRole).toInt()));
		*m_bDIY = FALSE;
		accept();
	}
}

void PhotoGallery::onActionTriggered(QAction* action)
{
	if (m_qActionClear == action)
	{
		m_qIconFile.clear();
		maletable->item(0, 0)->setIcon(QIcon());
		maletable->item(0, 0)->setText(tr("DIY"));
		*m_bDIY = FALSE;
	}
	else
	{
		reject();
	}

}

void PhotoGallery::onDefaultActionTriggered(bool checked)
{
	accept();
}