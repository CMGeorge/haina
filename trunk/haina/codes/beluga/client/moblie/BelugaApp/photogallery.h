#ifndef PHOTOGALLERY_H
#define PHOTOGALLERY_H

#include <QtGui/QDialog>
#include <QtGui/QTableWidget>
#include <QtGui/QWidget>
#include <QtGui/QPushButton>
#include <QtGui/QLabel>
#include <QtGui/QIcon>
#include <QtGui/QMenuBar>
#include <QtGui/QFileDialog>
#include <QtGui/QStylePainter>
#include <QtGui/QPainter>
#include "ui_photogallery.h"


class PhotoGallery : public QDialog, public Ui::photogallery
{
	Q_OBJECT

public:
	PhotoGallery(QWidget *parent = 0, QString & diyIconFile = QString(), bool * bDIY = NULL, Qt::WFlags flags = 0);
	~PhotoGallery();

private:
	inline void paintEvent(QPaintEvent * event);

private slots:
	void onItemClicked(QTableWidgetItem * item);
	void onActionTriggered(QAction* action);
	void onDefaultActionTriggered(bool checked = false);

private:	
	QAction		* m_qActionOk;
	QAction		* m_qActionClear;
	QAction		* m_qActionCancel;
	QMenuBar	* m_qMenuBar;
	QString		& m_qIconFile;
	bool		* m_bDIY;
};

#endif // PHOTOGALLERY_H

