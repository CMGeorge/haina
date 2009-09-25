#ifndef BELUGAVIEW_H
#define BELUGAVIEW_H

#include <QtGui/QDialog>
#include <QtGui/QWidget>
#include <QtGui/QPixmap>
#include <QtGui/QMenuBar>
#include <QtGui/QAction>
#include <QtGui/QListWidget>
#include <QtGui/QListWidgetItem>
#include <QtGui/QLabel>
#include <QtGui/QStylePainter>
#include <QtGui/QPainter>
#include <QtGui/QMessageBox>
#include "ui_belugaview.h"
#include "CContact.h"
#include "CPhoneContact.h"

class BelugaMain;

class BelugaView : public QDialog, public Ui::belugaview
{
	Q_OBJECT

public:
	BelugaView(QWidget *parent = 0, int nContactId = 0, bool * bEdited = FALSE, Qt::WFlags flags = 0);
	~BelugaView();

private:
	BOOL initializeFields();
	void paintEvent(QPaintEvent * event);

private slots:
	void onActionVoiceCallTriggered(bool checked = false);
	void onActionVideoCallTriggered(bool checked = false);
	void onActionIPCallTriggered(bool checked = false);
	void onActionMsgCTriggered(bool checked = false);
	void onActionDelCTriggered(bool checked = false);
	void onActionNoteTriggered(bool checked = false);
	void onActionBackTriggered(bool checked = false);

	void onDefaultActionTriggered(bool checked = false);
	void onItemDoubleClicked(QListWidgetItem * item);
	void onCurrentItemChanged(QListWidgetItem * current, QListWidgetItem * previous);
	void onItemClicked (QListWidgetItem * item);

private:
	QListWidget * m_qList;
	QMenuBar	* m_qMenuBar;
	QAction		* m_qActionEdit;
	QAction     * m_qActionBack;

	QMenu		* m_qMenuCall;
	QAction		* m_qActionVoiceCall;
	QAction     * m_qActionVideoCall;
	QAction		* m_qActionIpCall;
	QAction		* m_qActionMsgC;
	QAction		* m_qActionDelC;
	QAction     * m_qActionNote;

	BelugaMain		* m_pBelugaMain;
	int				m_nContactId;
	CPhoneContact	* m_pContact;
	bool			* m_bEidted;
};

#endif // BELUGAVIEW_H

