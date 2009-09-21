#ifndef ADDREDIT_H
#define ADDREDIT_H

#include <QtGui/QDialog>
#include <QtGui/QTableWidget>
#include <QtGui/QWidget>
#include <QtGui/QPushButton>
#include <QtGui/QLabel>
#include <QtGui/QIcon>
#include <QtGui/QMenuBar>
#include <QtGui/QFileDialog>
#include "ui_addredit.h"
#include "CContact.h"


class AddrEdit : public QDialog, public Ui::addredit
{
	Q_OBJECT

public:
	AddrEdit(QWidget *parent = 0, stAddress * addr = NULL, Qt::WFlags flags = 0);
	~AddrEdit();

	void accept();

private slots:
	void onAccepted();

private:	
	stAddress	* m_pAddr;
};

#endif // ADDREDIT_H

