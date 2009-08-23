#ifndef BELUGADETAIL_H
#define BELUGADETAIL_H

#include <QtGui/QDialog>
#include <QtGui/QTableWidget>
#include <QtGui/QWidget>
#include <QtGui/QPicture>
#include <QtGui/QMenuBar>
#include <QtGui/QAction>
#include <QtCore/QList>
#include "ui_belugadetail.h"

#include "CContactDb.h"
#include "CGroupDb.h"
#include "CContactIterator.h"
#include "CGroupIterator.h"
#include "CContact.h"
#include "CGroup.h"

class BelugaDetail : public QDialog, public Ui::belugadetail
{
	Q_OBJECT

public:
	BelugaDetail(QWidget *parent = 0, Qt::WFlags flags = 0);
	~BelugaDetail();

private:

private slots:

private:
	CContactDb	* m_pContactDb;
	CGroupDb	* m_pGroupDb;

#if 0
	QString		  m_qCurItemText;
	int			  m_nCurTabIndex;
	int			  m_nCurDefaultAction;
	BOOL		  m_bIsCurTopItem; /* current item is top item which was group */

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
#endif
};

#endif // BELUGADETAIL_H

