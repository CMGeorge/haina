#ifndef BELUGAMAIN_H
#define BELUGAMAIN_H

#include <QtGui/QDialog>
#include <QtGui/QTabWidget>
#include <QtGui/QTreeWidget>
#include <QtGui/QWidget>
#include <QtGui/QPicture>
#include <QtGui/QMenuBar>
#include <QtGui/QMenu>
#include <QtGui/QAction>
#include <QtGui/QListWidget>
#include <QtGui/QTabBar>
#include <QtCore/QList>
#include "ui_belugamain.h"

#include "CContactDb.h"
#include "CGroupDb.h"
#include "CTagDb.h"
#include "CContactIterator.h"
#include "CGroupIterator.h"
#include "CTagIterator.h"
#include "CContact.h"
#include "CGroup.h"
#include "CTag.h"


class BelugaMain : public QDialog, public Ui::BelugaAppMain
{
	Q_OBJECT

public:
	BelugaMain(QWidget *parent = 0, Qt::WFlags flags = 0);
	~BelugaMain();

private:
	BOOL initBelugDb();
	BOOL loadContacts(QTreeWidgetItem* item);
	BOOL loadGroups(int nTagId);
	BOOL loadTags();
	BOOL addItemOperation(QTreeWidget * tree);
	BOOL createActions(BOOL bContact);

private slots:
	void onCurrentChanged(int nIndex);
	void onCurrentItemChanged(QTreeWidgetItem* current, QTreeWidgetItem* previous);
	void onItemExpanded(QTreeWidgetItem* item);
	void onItemClicked(QTreeWidgetItem* item, int column);
	void onItemCollapsed(QTreeWidgetItem* item);
	void onActionTriggered(QAction* action);

private:
	CContactDb	* m_pContactDb;
	CGroupDb	* m_pGroupDb;
	CTagDb		* m_pTagDb;
	
	QString		  m_qCurItemText;
	int			  m_nCurTabIndex;
	BOOL		  m_bIsCurTopItem; /* current item is top item which was group */
	
	QList<QWidget*> m_qWidgetPanelList;
	QList<QTreeWidget*> m_qTreeList;

	QTabBar		* m_qTabBar;
	QTreeWidget * m_qCurTree;
	QMenuBar	* m_qMenuBar;
	QWidget		* m_qSearListPanelWidget;
	QListWidget * m_qSearchList;

	/* contact actions */
	QMenu		* m_qMenuCall;
	QAction		* m_qActionVoiceCall;
	QAction     * m_qActionVideoCall;
	QAction		* m_qActionIpCall;
	QAction		* m_qActionMsgC;
	QAction		* m_qActionViewC;
	QAction		* m_qActionNewC;
	QAction		* m_qActionEditC;
	QAction		* m_qActionDelC;
	QAction		* m_qActionSelectC;
	QAction     * m_qActionGroupC;
	QAction	    * m_qActionSyncC;

	/* group actions */
	QAction		* m_qActionEditG;
	QAction		* m_qActionNewG;
	QMenu		* m_qMenuOrder;
	QAction		* m_qActionUpG;
	QAction		* m_qActionDownG;
	QAction     * m_qActionDelG;
	QAction		* m_qActionMsgG; /* group msg */
	QAction		* m_qActionExpandColapseG;

	/* search action */
	QAction     * m_qActionSearch;
};

#endif // BELUGAMAIN_H
