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
#include "CConfigDb.h"
#include "CConfig.h"
#include "glib.h"


enum eActionId 
{
	CLOSE_SEARCH_ACTION = 0,
	/* contact action */
	CONTACT_VIEW_ACTION,
	CONTACT_VOICECALL_ACTION,
	CONTACT_IPCALL_ACTION,
	CONTACT_VIDEOCALL_ACTION,
	CONTACT_MSG_ACTION,
	CONTACT_NEW_ACTION,
	CONTACT_EDIT_ACTION,
	CONTACT_DEL_ACTION,
	CONTACT_GROUP_ACTION,
	CONTACT_SYNC_ACTION,

	/* group action */
	GROUP_EXPAND_COLLAPSE_ACTION,
	GROUP_NEW_ACTION,
	GROUP_EDIT_ACTION,
	GROUP_DEL_ACTION,
	GROUP_UP_ACTION,
	GROUP_DOWN_ACTION,
	GROUP_MSG_ACTION,

	ACTION_NUM
};

typedef struct MenuBarStatus 
{
	int nDefaultAction;
	int nMenuType; 
} MenuBarStatus;


#define  PHONECONTACT_MENU		0
#define  IMCONTACT_MENU			1
#define  GROUP_MENU				2


class BelugaMain : public QDialog, public Ui::BelugaAppMain
{
	Q_OBJECT

public:
	BelugaMain(QWidget *parent = 0, Qt::WFlags flags = 0);
	~BelugaMain();

	CContactDb * getContactDb();
	CGroupDb * getGroupDb();
	QTreeWidget * getGroupTree(int nTabIndex);
	int getCurrentTab();

private:
	BOOL initBelugDb();
	BOOL loadContacts(QTreeWidget* tree, QTreeWidgetItem* item, CContactIterator * pContactIterator, bool isTree = TRUE);
	BOOL loadGroups(int nTagId);
	BOOL loadTags();
	BOOL addItemOperation(QTreeWidget * tree);
	BOOL createContactActions(int nContactType);
	BOOL createGroupActions();
	BOOL saveMenuBar(int nTabId, int nActionId, int nMenuType);
	BOOL restoreMenuBar(int nTabId);
	BOOL searchContacts(const char* text);
	QTreeWidget * createTreeWidget(const char* name);
	BOOL loadSelfContacts();
	gboolean importContacts();

private slots:
	void onCurrentChanged(int nIndex);
	void onCurrentItemChanged(QTreeWidgetItem* current, QTreeWidgetItem* previous);
	void onItemExpanded(QTreeWidgetItem* item);
	void onItemClicked(QTreeWidgetItem* item, int column);
	void onItemCollapsed(QTreeWidgetItem* item);
	void onActionTriggered(QAction* action);
	void onDefaultActionTriggered(bool checked = false);
	void onTextChanged(const QString & text);
	void onEditingFinished();

private:
	CContactDb	* m_pContactDb;
	CGroupDb	* m_pGroupDb;
	CTagDb		* m_pTagDb;
	CConfigDb   * m_pConfigDb;
	
	QString		  m_qCurItemText;
	int			  m_nCurTabIndex;
	int			  m_nCurDefaultAction;
	BOOL		  m_bIsCurTopItem; /* current item is top item which was group */
	BOOL		  m_bSelfContactLoaded;
	
	QList<QWidget*> m_qWidgetPanelList;
	QList<QTreeWidget*> m_qTreeList;
	QAction		* m_qActions[ACTION_NUM];

	QTabBar		* m_qTabBar;
	QTreeWidget * m_qCurTree;
	QMenuBar	* m_qMenuBar;

	QList<MenuBarStatus> m_stMenuStatus;

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

	/* close search result action */
	QAction     * m_qActionCloseSearch;
};

#endif // BELUGAMAIN_H
