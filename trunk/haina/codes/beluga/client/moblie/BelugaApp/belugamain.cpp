#include "belugamain.h"
#include "belugadetail.h"
#include "belugamobile.h"
#include "belugaview.h"
#include "newmessage.h"

#include <QtGui/QMessageBox>
#include <QtGui/QProgressDialog>
#include <QtGui/QInputDialog>
#include "Pimstore.h"
#include "hz2py.h"


BelugaMain::BelugaMain(QWidget *parent, Qt::WFlags flags)
	: QDialog(parent, flags)
{
	m_pContactDb = NULL;
	m_pGroupDb = NULL;
	m_pTagDb = NULL;
	m_qTabBar = NULL;
	m_qCurTree = NULL;
	m_qMenuBar = NULL;
	m_nCurTabIndex = 0;
	m_nCurDefaultAction = 0;
	m_bIsCurTopItem = FALSE;
	m_bSelfContactLoaded = FALSE;
	m_QStatusWidget = NULL;

    setupUi(this);
	
	/* set beluga css */
	QFile file(":/BelugaApp/beluga.qss");
	file.open(QIODevice::ReadOnly);
	QString stylesheet = QString(file.readAll());
	qApp->setStyleSheet(stylesheet);
	search->setStyleSheet("QComboBox::drop-down{border:0px;image:url(:/BelugaApp/Resources/images/search.png);	subcontrol-position: center right;}");
	connect(qApp, SIGNAL(focusChanged(QWidget*, QWidget*)), this, SLOT(onFocusChanged(QWidget*, QWidget*)));

	/* show person status timer */
	m_qShowTimer = new QTimer(this);
	m_qShowTimer->setSingleShot(true);
	connect(m_qShowTimer, SIGNAL(timeout()), this, SLOT(showPersonStatus()));
	
	m_qCloseTimer = new QTimer(this);
	m_qCloseTimer->setSingleShot(true);
	connect(m_qCloseTimer, SIGNAL(timeout()), this, SLOT(hidePersonStatus()));

	/* person status widget */
	createPersonStatus();

	/* create tab bar */
	m_qTabBar = new QTabBar(this);
	m_qTabBar->setGeometry(QRect(55, 25, 182, 25));
	m_qTabBar->setShape(QTabBar::RoundedNorth);
	m_qTabBar->setExpanding(FALSE);
	m_qTabBar->setVisible(FALSE);
	/* set signals and slots */
	connect(m_qTabBar, SIGNAL(currentChanged(int)), this, SLOT(onCurrentChanged(int)));

	/* set search edit signals and slots */
	connect(search, SIGNAL(editTextChanged(const QString&)), this, SLOT(onTextChanged(const QString&)));

	/* create close search action */
	m_qActionCloseSearch = new QAction(tr("Close"), this);
	m_qActions[CLOSE_SEARCH_ACTION] = m_qActionCloseSearch;
	connect(m_qActionCloseSearch, SIGNAL(triggered(bool)), this, SLOT(onDefaultActionTriggered(bool)));

	/* create menu */ 
	m_qMenuBar = new QMenuBar(this);
	createGroupActions();

	/* init db engine and load main ui data */
	if (FALSE == initBelugDb())
	{
		printf("Init BelugaDb Failed!\n");
		return;
	}
	if (FALSE == loadTags()/* || FALSE == loadGroups(ContactType_Phone)*/)
	{
		printf("Load BelugaDb data Failed!\n");
		return;
	}
	
	contactphoto->setIconSize(QSize(42, 42));
	contactphoto->setIcon(QIcon(":/BelugaApp/Resources/images/contact_default.png"));
	m_qTabBar->setCurrentIndex(m_nCurTabIndex);
	m_qTabBar->setTabText(m_nCurTabIndex, m_qTabBar->tabData(m_nCurTabIndex).toString());
	m_qTabBar->setVisible(TRUE);
	m_qWidgetPanelList.at(m_nCurTabIndex)->setVisible(TRUE);
}

BelugaMain::~BelugaMain()
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
	if (m_pTagDb == NULL)
	{
		delete m_pTagDb;
		m_pTagDb = NULL;
	}
	if (m_pConfigDb == NULL)
	{
		delete m_pConfigDb;
		m_pConfigDb = NULL;
	}

	delete m_qShowTimer;
	delete m_qCloseTimer;
}

BOOL BelugaMain::initBelugDb()
{
	m_pContactDb = new CContactDb();
	if (NULL == m_pContactDb)
	{
		printf("Create contact db instance error.\n");
		return FALSE;
	}
	m_pGroupDb = new CGroupDb();
	if (NULL == m_pGroupDb)
	{
		printf("Create group db instance error.\n");
		return FALSE;
	}
	m_pTagDb = new CTagDb();
	if (NULL == m_pTagDb)
	{
		printf("Create tag db instance error.\n");
		return FALSE;
	}
	m_pConfigDb = new CConfigDb();
	if (NULL == m_pConfigDb)
	{
		printf("Create config db instance error.\n");
		return FALSE;
	}

	m_pContactDb->InitEntityDb("\\Program Files\\Beluga\\beluga.db");
	m_pGroupDb->InitEntityDb("\\Program Files\\Beluga\\beluga.db");
	m_pTagDb->InitEntityDb("\\Program Files\\Beluga\\beluga.db");
	m_pConfigDb->InitEntityDb("\\Program Files\\Beluga\\beluga.db");

	return TRUE;
}

BOOL BelugaMain::loadContacts(QTreeWidget* tree, QTreeWidgetItem* item, CContactIterator * pContactIterator, bool isTree)
{
	gint32 ret = ECode_No_Error;
	BOOL bSucceed = FALSE;

	do
	{
		CDbEntity * pEntity = NULL;
		ret = pContactIterator->Current(&pEntity);	
		if (ret != ECode_No_Error)
		{
			printf("Get contact instance failed!\n");
			goto _Error;
		}

		CContact * pContact = (CContact*)pEntity;
		QTreeWidgetItem * qContactItem = new QTreeWidgetItem();

		/* set contact id as item data */
		GString * ContactId = NULL;
		pContact->GetFieldValue(ContactField_Id, &ContactId);
		qContactItem->setData(0, Qt::UserRole, QVariant(ContactId->str));
		g_string_free(ContactId, TRUE);

		/* set contact Logo */
		GString * contactLogo = NULL;
		pContact->GetFieldValue(ContactField_Photo, &contactLogo);
		QIcon qLogo;
		if (contactLogo->len != 0)
		{
			qLogo.addFile(contactLogo->str);
			qContactItem->setData(3, Qt::UserRole, QVariant(QString(contactLogo->str)));
		}
		else
		{
			qLogo.addFile(":/BelugaApp/Resources/images/contact_default.png");
			qContactItem->setData(3, Qt::UserRole, QVariant(QString(":/BelugaApp/Resources/images/contact_default.png")));
		}
		qContactItem->setIcon(0, qLogo);
		g_string_free(contactLogo, TRUE);			

		/* set contact name */
		GString * contactName = NULL;
		pContact->GetFieldValue(ContactField_Name, &contactName);
		qContactItem->setText(0, tr(contactName->str));
		g_string_free(contactName, TRUE);

		/* set contact name spell as sort condition */
		GString * spellName = NULL;
		pContact->GetFieldValue(ContactField_NameSpell, &spellName);
		qContactItem->setData(2, Qt::UserRole, QVariant(spellName->str));
		g_string_free(spellName, TRUE);

		/* get contact type */
		GString * contactType = NULL;
		pContact->GetFieldValue(ContactField_Type, &contactType);
		if (ContactType_Phone == atoi(contactType->str))
		{
			/* get phone contact pref phone */
			GString * pref = NULL;
			pContact->GetFieldValue(ContactField_PhonePref, &pref);
			qContactItem->setData(1, Qt::UserRole, tr(pref->str));
			g_string_free(pref, TRUE);
		}
		else
		{
			/* get im contact user id */
			GString * userId = NULL;
			pContact->GetFieldValue(ContactField_UserId, &userId);
			qContactItem->setData(1, Qt::UserRole, tr(userId->str));
			g_string_free(userId, TRUE);
		}
		g_string_free(contactType, TRUE);

		if (isTree)
			tree->addTopLevelItem(qContactItem);
		else
			item->addChild(qContactItem);

		delete pContact;
		pContact = NULL;
	} while(0 == pContactIterator->Next(&bSucceed) && bSucceed);

	delete pContactIterator;
	pContactIterator = NULL;

	return TRUE;

_Error:
	if (pContactIterator != NULL)
	{
		delete pContactIterator;
		pContactIterator = NULL;
	}

	return FALSE;
}

BOOL BelugaMain::loadGroups(int nTagId)
{
	gint32 ret = ECode_No_Error;
	CGroupIterator * pGroupIterator = NULL;
	QIcon defaultIcon;
	
	ret = m_pGroupDb->GetAllGroupsByTag(nTagId, &pGroupIterator);
	if (ret != ECode_No_Error)
	{
		printf("Get all groups failed!\n");
		return FALSE;
	}
	
	if (m_qCurTree == NULL)
	{
		m_qCurTree = m_qTreeList.at(m_nCurTabIndex);
	}

	BOOL bSucceed = FALSE;
	do
	{
		CDbEntity * pEntity = NULL;
		ret = pGroupIterator->Current(&pEntity);	
		if (ret != ECode_No_Error)
		{
			printf("Get group instance failed!\n");
			goto _Error;
		}
				
		CGroup * pGroup = (CGroup*)pEntity;
		QTreeWidgetItem * qGroupItem = new QTreeWidgetItem();

#if 0
		GString * groupOrder = NULL;
		int nGroupIndex = 0;
		pGroup->GetFieldValue(GroupField_GroupOrder, &groupOrder);
		nGroupIndex = atoi(groupOrder->str);
		g_string_free(groupOrder, TRUE);
#endif

		/* set group id as item data */
		GString * groupId = NULL;
		pGroup->GetFieldValue(GroupField_Id, &groupId);
		qGroupItem->setData(0, Qt::UserRole, QVariant(groupId->str)); 
		g_string_free(groupId, TRUE);

		/* set group Logo */
		//GString * groupLogo = NULL;
		//pGroup->GetFieldValue(GroupField_Logo, &groupLogo);
		QIcon qLogo;
		//qLogo.addFile(groupLogo->str, QSize(32, 32));
		qLogo.addFile(":/BelugaApp/Resources/images/right.png");
		qGroupItem->setIcon(0, qLogo);
		//g_string_free(groupLogo, TRUE);			

		/* set group name */
		GString * groupName = NULL;
		pGroup->GetFieldValue(GroupField_Name, &groupName);
		qGroupItem->setText(0, tr(groupName->str));
		g_string_free(groupName, TRUE);
		
		/* set group name spell as sort condition */
		GString * spellName = NULL;
		pGroup->GetFieldValue(GroupField_NameSpell, &spellName);
		qGroupItem->setData(2, Qt::UserRole, QVariant(spellName->str));
		g_string_free(spellName, TRUE);
		
		m_qCurTree->addTopLevelItem(qGroupItem);
		//m_qCurTree->insertTopLevelItem(nGroupIndex, qGroupItem);
		delete pGroup;
		pGroup = NULL;
	} while(0 == pGroupIterator->Next(&bSucceed) && bSucceed);
	
	delete pGroupIterator;
	pGroupIterator = NULL;

	/* insert default group which is make up of all contact without group */ 
	QTreeWidgetItem * qDefaultGroupItem = new QTreeWidgetItem();
	
	qDefaultGroupItem->setData(0, Qt::UserRole, GROUPID_DEFAULT); 
	defaultIcon.addFile(":/BelugaApp/Resources/images/right.png");
	qDefaultGroupItem->setIcon(0, defaultIcon);
	if (ContactType_Phone == nTagId)
		qDefaultGroupItem->setText(0, tr("My Contact"));
	else
		qDefaultGroupItem->setText(0, tr("My Friend"));
	m_qCurTree->insertTopLevelItem(0, qDefaultGroupItem);

	return TRUE;

_Error:
	if (pGroupIterator != NULL)
	{
		delete pGroupIterator;
		pGroupIterator = NULL;
	}

	return FALSE;
}

BOOL BelugaMain::loadTags()
{
	gint32 ret = ECode_No_Error;
	int nTagIndex = 0;
	CTagIterator * pTagIterator = NULL;

	ret = m_pTagDb->GetAllTags(&pTagIterator);
	if (ret != ECode_No_Error)
	{
		printf("Get all tags failed!\n");
		return FALSE;
	}
	
	BOOL bSucceed = FALSE;
	do
	{
		CDbEntity * pEntity = NULL;
		ret = pTagIterator->Current(&pEntity);	
		if (ret != ECode_No_Error)
		{
			printf("Get tag instance failed!\n");
			goto _Error;
		}
		
		CTag * pTag = (CTag*)pEntity;
		
		GString * tagDeleteFlag = NULL;
		pTag->GetFieldValue(TagField_DeleteFlag, &tagDeleteFlag);
		if (FALSE == atoi(tagDeleteFlag->str))  /* the Tag has been setting to deleted */
		{
			GString * tagOrder = NULL;
			pTag->GetFieldValue(TagField_TagOrder, &tagOrder);
			nTagIndex = atoi(tagOrder->str);
			g_string_free(tagOrder, TRUE);
								
			/* get Tag Name and Logo */
			GString * tagName = NULL;
			pTag->GetFieldValue(TagField_Name, &tagName);
			GString * tagLogo = NULL;
			pTag->GetFieldValue(TagField_Logo, &tagLogo);
			createTreeWidget(tagName->str);		
			m_qTabBar->insertTab(nTagIndex, QIcon(QString(tagLogo->str)), QString());
			m_qTabBar->setTabToolTip(nTagIndex, tr(tagName->str));
			g_string_free(tagName, TRUE);
			g_string_free(tagLogo, TRUE);
		}
		g_string_free(tagDeleteFlag, TRUE);

		delete pTag;
		pTag = NULL;
	} while(0 == pTagIterator->Next(&bSucceed) && bSucceed);
	
	delete pTagIterator;
	pTagIterator = NULL;
	
	/* insert last tab for recent contact */ 
	createTreeWidget("Recent Contact");
	
	m_qTabBar->insertTab(nTagIndex + 1, QIcon(":/BelugaApp/Resources/images/recent.png"), QString());
	m_qTabBar->setTabToolTip(nTagIndex + 1, tr("Recent Contact"));
	m_nRencentContactTab = nTagIndex + 1;

	/* search list parent widget */
	createTreeWidget("Search Result");

	return TRUE;

_Error:
	if (pTagIterator != NULL)
	{
		delete pTagIterator;
		pTagIterator = NULL;
	}
	
	return FALSE;
}

QTreeWidget * BelugaMain::createTreeWidget(const char* name)
{
	MenuBarStatus status;
	status.nDefaultAction = GROUP_EXPAND_COLLAPSE_ACTION;
	status.nMenuType = GROUP_MENU;
	m_stMenuStatus.append(status);

	QWidget * pWidget = new QWidget(this);
	pWidget->setGeometry(QRect(0, 50, 240, 220));
	pWidget->setObjectName(tr(name));
	pWidget->setVisible(FALSE);
	m_qWidgetPanelList.append(pWidget);

	QTreeWidget * pTagList = NULL;
	pTagList = new QTreeWidget(pWidget);
	pTagList->setGeometry(QRect(0, 0, 240, 217));
	pTagList->header()->setVisible(FALSE);
	pTagList->setObjectName(tr(name));
	pTagList->setColumnCount(4);
	pTagList->setColumnWidth(0, 150);
	pTagList->setColumnWidth(1, 50);
	pTagList->setColumnWidth(2, 3);
	pTagList->setColumnWidth(3, 2);
	pTagList->setUniformRowHeights(FALSE);
	pTagList->setIndentation(1);
	pTagList->setWordWrap(TRUE);
	pTagList->expandAll();
	pTagList->setFrameShadow(QFrame::Plain);
	pTagList->setFrameShape(QFrame::NoFrame);
	pTagList->setLineWidth(1);
	m_qTreeList.append(pTagList);
	addItemOperation(pTagList);

	return pTagList;
}

void BelugaMain::onCurrentChanged(int nIndex)
{
	if (m_qWidgetPanelList.last()->isVisible())
	{
		search->clear();
/*		item = m_qCurTree->currentItem();

		if (NULL != item && NULL != m_qCurTree->itemWidget(item, 0))
		{
		m_qCurTree->removeItemWidget(item, 0);
		item->setSizeHint(0, item->sizeHint(0));
		item->setText(0, m_qCurItemText);
		}	
*/
		m_qCurTree->clear();
		m_qWidgetPanelList.last()->setVisible(FALSE);
	}
	else
	{
		saveMenuBar(m_nCurTabIndex, m_nCurDefaultAction, 
			m_bIsCurTopItem ? GROUP_MENU : (m_nCurTabIndex + 1 == ContactType_Phone ? PHONECONTACT_MENU : IMCONTACT_MENU));
	}

	m_qTabBar->setTabText(m_nCurTabIndex, QString(""));
	m_qWidgetPanelList.at(m_nCurTabIndex)->setVisible(FALSE);
	m_nCurTabIndex = nIndex;
	m_qCurTree = m_qTreeList.at(m_nCurTabIndex);
	m_qTabBar->setTabText(m_nCurTabIndex, m_qTabBar->tabData(m_nCurTabIndex).toString());
	m_qWidgetPanelList.at(m_nCurTabIndex)->setVisible(TRUE);
	
	if (0 == m_qCurTree->topLevelItemCount())
		loadGroups(m_nCurTabIndex + 1);
	
	if (m_nRencentContactTab == m_nCurTabIndex)  /* recent contact tab */
		loadRecentContact();

	restoreMenuBar(m_nCurTabIndex);
}

BOOL BelugaMain::addItemOperation(QTreeWidget * tree)
{
	connect(tree, SIGNAL(currentItemChanged(QTreeWidgetItem*, QTreeWidgetItem*)), 
				this, SLOT(onCurrentItemChanged(QTreeWidgetItem*, QTreeWidgetItem*)));
	connect(tree, SIGNAL(itemExpanded(QTreeWidgetItem*)), this, SLOT(onItemExpanded(QTreeWidgetItem*)));
	connect(tree, SIGNAL(itemCollapsed(QTreeWidgetItem*)), this, SLOT(onItemCollapsed(QTreeWidgetItem*)));
	connect(tree, SIGNAL(itemClicked(QTreeWidgetItem*, int)), this, SLOT(onItemClicked(QTreeWidgetItem*, int)));
	connect(tree, SIGNAL(itemDoubleClicked(QTreeWidgetItem*, int)), this, SLOT(onItemDoubleClicked(QTreeWidgetItem*, int)));

	return TRUE;
}

void BelugaMain::onCurrentItemChanged(QTreeWidgetItem* current, QTreeWidgetItem* previous)
{
	int prevIndex;
	int curIndex;
	
	if (NULL == current)
		return;

	if (m_nCurTabIndex == m_nRencentContactTab)  /* recent contact tab */
	{
		int contactType = current->data(3, Qt::UserRole).toInt();
		if (contactType == ContactType_IM)
			m_qMenuCall->setEnabled(FALSE);
		else
			m_qMenuCall->setEnabled(FALSE);
	}

	if (m_qCurTree == m_qTreeList.last())  /* the tree list is search result list */
	{
		prevIndex = curIndex = -1;  /* means that them are contact items */
	}
	else
	{
		prevIndex = m_qCurTree->indexOfTopLevelItem(previous);
		curIndex = m_qCurTree->indexOfTopLevelItem(current);
	}

	if (NULL != previous)
	{
		if (-1 == prevIndex && -1 == curIndex)  /* prev and current item all are contact */
		{
			m_bIsCurTopItem = FALSE;

			/* change prev item normal icon size */
			m_qCurTree->removeItemWidget(previous, 0);
			previous->setSizeHint(0, current->sizeHint(0));
			previous->setText(0, m_qCurItemText);
			previous->setIcon(0, QIcon(previous->data(3, Qt::UserRole).toString()));

			/* change current item large icon size */
			current->setSizeHint(0, QSize(current->sizeHint(0).width(), 32));
			QLabel * iconlabel = new QLabel();
			iconlabel->setObjectName(QString::fromUtf8("iconlabel"));
			iconlabel->setGeometry(QRect(3, 3, 26, 26));
			// iconlabel->setPixmap(QPixmap(QString::fromUtf8(":/BelugaApp/Resources/images/contact_default.png")).scaled(QSize(26, 26)));
			iconlabel->setPixmap(QPixmap(current->data(3, Qt::UserRole).toString()).scaled(QSize(26, 26)));

			m_qCurItemText.clear();
			m_qCurItemText.append(current->text(0));
			current->setIcon(0, QIcon());
			current->setText(0, QString("        %1\n        %2").arg(m_qCurItemText, current->data(1, Qt::UserRole).toString()));
			m_qCurTree->setItemWidget(current, 0, iconlabel);

			hidePersonStatus();
			m_qShowTimer->start(2000);
		}
		else if (-1 != prevIndex && -1 == curIndex)  /* prev item is group but current item is contact */
		{
			m_bIsCurTopItem = FALSE;

			/* change current item large icon size */
			current->setSizeHint(0, QSize(current->sizeHint(0).width(), 32));
			QLabel * iconlabel = new QLabel();
			iconlabel->setObjectName(QString::fromUtf8("iconlabel"));
			iconlabel->setGeometry(QRect(3, 3, 26, 26));
			//iconlabel->setPixmap(QPixmap(QString::fromUtf8(":/BelugaApp/Resources/images/contact_default.png")).scaled(QSize(26, 26)));
			iconlabel->setPixmap(QPixmap(current->data(3, Qt::UserRole).toString()).scaled(QSize(26, 26)));

			m_qCurItemText.clear();
			m_qCurItemText.append(current->text(0));
			current->setIcon(0, QIcon());
			current->setText(0, QString("        %1\n        %2").arg(m_qCurItemText, current->data(1, Qt::UserRole).toString()));
			m_qCurTree->setItemWidget(current, 0, iconlabel);

			/* change menu actions to contact actions */
			createContactActions(m_nCurTabIndex + 1);
			
			hidePersonStatus();
			m_qShowTimer->start(2000);
		}
		else if (-1 == prevIndex && -1 != curIndex) /* prev item is contact but current item is group */
		{
			m_bIsCurTopItem = TRUE;

			/* change prev item normal icon size */
			m_qCurTree->removeItemWidget(previous, 0);
			previous->setSizeHint(0, current->sizeHint(0));
			previous->setText(0, m_qCurItemText);
			previous->setIcon(0, QIcon(previous->data(3, Qt::UserRole).toString()));

			/* change menu actions to group actions */
			createGroupActions();
		}
	}
	else
	{
//		onItemClicked(current, 0);
	}
	
}

void BelugaMain::onItemClicked(QTreeWidgetItem* item, int column)
{
	int index;

	if (m_qWidgetPanelList.last()->isVisible()) /* search result panel is visible */
		index = -1; /* means that this is contact item */
	else
		index = m_qCurTree->indexOfTopLevelItem(item);
	
	if (-1 != index)  /* top level is group item */
	{
		if (!m_bIsCurTopItem)  /* prev item is not group */
		{
			createGroupActions();
			m_bIsCurTopItem = TRUE;
		}

		if (item->isExpanded()) /* collapse group */
		{
			m_qCurTree->collapseItem(item);
		}
		else /* expand group to show contacts */
		{
			m_qCurTree->expandItem(item);
		}
	}
	else /* second level is contact item */
	{
		if (NULL != m_qCurTree->itemWidget(item, 0)) /* item widget exists */
			return;

		if (m_bIsCurTopItem)  /* prev item is group */
		{
			createContactActions(m_nCurTabIndex + 1);
			m_bIsCurTopItem = FALSE;
		}

		/* change contact item height */
		item->setSizeHint(0, QSize(item->sizeHint(0).width(), 32));
		/* change the icon size large */ 
		QLabel * iconlabel = new QLabel();
		iconlabel->setObjectName(QString::fromUtf8("iconlabel"));
		iconlabel->setGeometry(QRect(3, 3, 26, 26));
		iconlabel->setPixmap(QPixmap(item->data(3, Qt::UserRole).toString()).scaled(QSize(26, 26)));
		/* adjust text position */
		m_qCurItemText.clear();
		m_qCurItemText.append(item->text(0));
		item->setIcon(0, QIcon());
		item->setText(0, QString("        %1\n        %2").arg(m_qCurItemText, item->data(1, Qt::UserRole).toString()));
		m_qCurTree->setItemWidget(item, 0, iconlabel);
		
		hidePersonStatus();
		m_qShowTimer->start(2000);
	}
}

void BelugaMain::onItemCollapsed(QTreeWidgetItem* item)
{
	QIcon icon;
	/* change to group icon */
	icon.addFile(":/BelugaApp/Resources/images/right.png");
	item->setIcon(0, icon);
	m_qActionExpandColapseG->setText(tr("Expand"));
}

void BelugaMain::onItemExpanded(QTreeWidgetItem* item)
{
	QIcon icon;
	/* change to group expanded icon */
	icon.addFile(":/BelugaApp/Resources/images/down.png");
	item->setIcon(0, icon);

	if (0 == item->childCount()) /* load contact from the group */
	{
		/* check config, if mobile platform contact database exists some contacts, import into beluga database */
		loadSelfContacts();

		/* load contact from the group */
		int nGroupId = item->data(0, Qt::UserRole).toInt();
		gint32 ret = ECode_No_Error;
		CContactIterator * pContactIterator = NULL;
		
		if (GROUPID_DEFAULT != nGroupId)
			ret = m_pContactDb->GetAllContactsByGroup(nGroupId, TRUE, &pContactIterator);
		else
			ret = m_pContactDb->GetAllContactsNotInGroupByTag(m_nCurTabIndex + 1, TRUE, &pContactIterator);
		if (ret != ECode_No_Error)
		{
			printf("Get contacts failed!\n");
			return;
		}

		loadContacts(NULL, item, pContactIterator, FALSE);
	}
	m_qActionExpandColapseG->setText(tr("Collapse"));
}

BOOL BelugaMain::createContactActions(int nContactType)
{
	m_qMenuBar->clear();
	
	if (ContactType_Phone == nContactType)
	{
		m_qActionViewC = new QAction(tr("View"), this);
		m_qActions[CONTACT_VIEW_ACTION] = m_qActionViewC;
		m_qMenuBar->addAction(m_qActionViewC);
		connect(m_qActionViewC, SIGNAL(triggered(bool)), this, SLOT(onActionViewCTriggered(bool)));

		m_qMenuCall = m_qMenuBar->addMenu(tr("Call"));		

		m_qActionVoiceCall = new QAction(tr("Voice"), this);
		m_qActions[CONTACT_VOICECALL_ACTION] = m_qActionVoiceCall;
		m_qMenuCall->addAction(m_qActionVoiceCall);
		connect(m_qActionVoiceCall, SIGNAL(triggered(bool)), this, SLOT(onActionVoiceCallTriggered(bool)));

		m_qActionIpCall = new QAction(tr("IP"), this);
		m_qActions[CONTACT_IPCALL_ACTION] = m_qActionIpCall;
		m_qMenuCall->addAction(m_qActionIpCall);
		connect(m_qActionIpCall, SIGNAL(triggered(bool)), this, SLOT(onActionIPCallTriggered(bool)));

		m_qActionVideoCall = new QAction(tr("Video"), this);
		m_qActions[CONTACT_VIDEOCALL_ACTION] = m_qActionVideoCall;
		m_qMenuCall->addAction(m_qActionVideoCall);
		connect(m_qActionVideoCall, SIGNAL(triggered(bool)), this, SLOT(onActionVideoCallTriggered(bool)));

		m_qActionMsgC = new QAction(tr("Message"), this);
		m_qActions[CONTACT_MSG_ACTION] = m_qActionMsgC;
		m_qMenuBar->addAction(m_qActionMsgC);
		connect(m_qActionMsgC, SIGNAL(triggered(bool)), this, SLOT(onActionMsgCTriggered(bool)));

		m_qActionNewC = new QAction(tr("New"), this);
		m_qActions[CONTACT_NEW_ACTION] = m_qActionNewC;
		connect(m_qActionNewC, SIGNAL(triggered(bool)), this, SLOT(onDefaultActionTriggered(bool)));
//		m_qMenuBar->addAction(m_qActionNewC);

		m_qActionEditC = new QAction(tr("Edit"), this);
		m_qActions[CONTACT_EDIT_ACTION] = m_qActionEditC;
		m_qMenuBar->addAction(m_qActionEditC);
		connect(m_qActionEditC, SIGNAL(triggered(bool)), this, SLOT(onActionEditCTriggered(bool)));

		m_qActionDelC = new QAction(tr("Delete"), this);
		m_qActions[CONTACT_DEL_ACTION] = m_qActionDelC;
		m_qMenuBar->addAction(m_qActionDelC);
		connect(m_qActionDelC, SIGNAL(triggered(bool)), this, SLOT(onActionDelCTriggered(bool)));

		m_qActionGroupC = new QAction(tr("Group"), this);
		m_qActions[CONTACT_GROUP_ACTION] = m_qActionGroupC;
		m_qMenuBar->addAction(m_qActionGroupC);
		connect(m_qActionGroupC, SIGNAL(triggered(bool)), this, SLOT(onActionGroupCTriggered(bool)));

		m_qActionSyncC = new QAction(tr("Sync"), this);
		m_qActions[CONTACT_SYNC_ACTION] = m_qActionSyncC;
		m_qMenuBar->addAction(m_qActionSyncC);
	    connect(m_qActionSyncC, SIGNAL(triggered(bool)), this, SLOT(onActionSyncTriggered(bool)));
		
		m_qActionSetting = new QAction(tr("Setting"), this);
		m_qActions[SETTING_ACTION] = m_qActionSetting;
		m_qMenuBar->addAction(m_qActionSetting);
		connect(m_qActionSetting, SIGNAL(triggered(bool)), this, SLOT(onActionSettingTriggered(bool)));

		m_qMenuBar->setDefaultAction(m_qActionNewC);
		m_nCurDefaultAction = CONTACT_NEW_ACTION;
	}
	else
	{
		m_qActionViewC = new QAction(tr("View"), this);
		m_qActions[CONTACT_VIEW_ACTION] = m_qActionViewC;
		m_qMenuBar->addAction(m_qActionViewC);
		connect(m_qActionViewC, SIGNAL(triggered(bool)), this, SLOT(onActionViewCTriggered(bool)));

		m_qActionMsgC = new QAction(tr("Message"), this);
		m_qActions[CONTACT_MSG_ACTION] = m_qActionMsgC;
		m_qMenuBar->addAction(m_qActionMsgC);
		connect(m_qActionMsgC, SIGNAL(triggered(bool)), this, SLOT(onActionMsgCTriggered(bool)));

		m_qActionNewC = new QAction(tr("New"), this);
		m_qActions[CONTACT_NEW_ACTION] = m_qActionNewC;
		connect(m_qActionNewC, SIGNAL(triggered(bool)), this, SLOT(onDefaultActionTriggered(bool)));
//		m_qMenuBar->addAction(m_qActionNewC);

		m_qActionEditC = new QAction(tr("Edit"), this);
		m_qActions[CONTACT_EDIT_ACTION] = m_qActionEditC;
		m_qMenuBar->addAction(m_qActionEditC);
		connect(m_qActionEditC, SIGNAL(triggered(bool)), this, SLOT(onActionEditCTriggered(bool)));

		m_qActionDelC = new QAction(tr("Delete"), this);
		m_qActions[CONTACT_DEL_ACTION] = m_qActionDelC;
		m_qMenuBar->addAction(m_qActionDelC);
		connect(m_qActionDelC, SIGNAL(triggered(bool)), this, SLOT(onActionDelCTriggered(bool)));

		m_qActionGroupC = new QAction(tr("Group"), this);
		m_qActions[CONTACT_GROUP_ACTION] = m_qActionGroupC;
		m_qMenuBar->addAction(m_qActionGroupC);
		connect(m_qActionGroupC, SIGNAL(triggered(bool)), this, SLOT(onActionGroupCTriggered(bool)));

		m_qActionSetting = new QAction(tr("Setting"), this);
		m_qActions[SETTING_ACTION] = m_qActionSetting;
		m_qMenuBar->addAction(m_qActionSetting);
		connect(m_qActionSetting, SIGNAL(triggered(bool)), this, SLOT(onActionSettingTriggered(bool)));

		m_qMenuBar->setDefaultAction(m_qActionNewC);
		m_nCurDefaultAction = CONTACT_NEW_ACTION;
	}
	
	return TRUE;
}

BOOL BelugaMain::createGroupActions()
{
	m_qMenuBar->clear();

	if (m_qCurTree && m_qCurTree->currentItem() && m_qCurTree->currentItem()->isExpanded())
		m_qActionExpandColapseG = new QAction(tr("Collapse"), this);
	else
		m_qActionExpandColapseG = new QAction(tr("Expand"), this);
	m_qActions[GROUP_EXPAND_COLLAPSE_ACTION] = m_qActionExpandColapseG;
	connect(m_qActionExpandColapseG, SIGNAL(triggered(bool)), this, SLOT(onDefaultActionTriggered(bool)));

	m_qActionNewC = new QAction(tr("New Contact"), this);
	m_qActions[CONTACT_NEW_ACTION] = m_qActionNewC;
	connect(m_qActionNewC, SIGNAL(triggered(bool)), this, SLOT(onActionNewCTriggered(bool)));
	m_qMenuBar->addAction(m_qActionNewC);

	m_qActionNewG = new QAction(tr("New Group"), this);
	m_qActions[GROUP_NEW_ACTION] = m_qActionNewG;
	m_qMenuBar->addAction(m_qActionNewG);
	connect(m_qActionNewG, SIGNAL(triggered(bool)), this, SLOT(onActionNewGTriggered(bool)));

	m_qActionMsgG = new QAction(tr("Message"), this);
	m_qActions[GROUP_MSG_ACTION] = m_qActionMsgG;
	m_qMenuBar->addAction(m_qActionMsgG);
	connect(m_qActionMsgG, SIGNAL(triggered(bool)), this, SLOT(onActionMsgGTriggered(bool)));

	m_qActionEditG = new QAction(tr("Edit"), this);
	m_qActions[GROUP_EDIT_ACTION] = m_qActionEditG;
	m_qMenuBar->addAction(m_qActionEditG);
	connect(m_qActionEditG, SIGNAL(triggered(bool)), this, SLOT(onActionEditGTriggered(bool)));

#if 0
	m_qMenuOrder = m_qMenuBar->addMenu(tr("Order"));	

	m_qActionUpG = new QAction(tr("Up"), this);
	m_qActions[GROUP_UP_ACTION] = m_qActionUpG;
	m_qMenuOrder->addAction(m_qActionUpG);
	connect(m_qActionUpG, SIGNAL(triggered(bool)), this, SLOT(onActionUpGTriggered(bool)));

	m_qActionDownG = new QAction(tr("Down"), this);
	m_qActions[GROUP_DOWN_ACTION] = m_qActionDownG;
	m_qMenuOrder->addAction(m_qActionDownG);
	connect(m_qActionDownG, SIGNAL(triggered(bool)), this, SLOT(onActionDownGTriggered(bool)));
#endif

	m_qActionDelG = new QAction(tr("Delete"), this);
	m_qActions[GROUP_DEL_ACTION] = m_qActionDelG;
	m_qMenuBar->addAction(m_qActionDelG);
	connect(m_qActionDelG, SIGNAL(triggered(bool)), this, SLOT(onActionDelGTriggered(bool)));

	m_qActionSetting = new QAction(tr("Setting"), this);
	m_qActions[SETTING_ACTION] = m_qActionSetting;
	m_qMenuBar->addAction(m_qActionSetting);
	connect(m_qActionSetting, SIGNAL(triggered(bool)), this, SLOT(onActionSettingTriggered(bool)));

	m_qMenuBar->setDefaultAction(m_qActionExpandColapseG);
	m_nCurDefaultAction = GROUP_EXPAND_COLLAPSE_ACTION;

	return TRUE;
}

void BelugaMain::onActionTriggered(int actionRow)
{
	QTreeWidgetItem * item =m_qCurTree->currentItem();
	hidePersonStatus();
	switch(actionRow)
	{
	/* contact actions */
	case CONTACT_VIEW_ACTION:
		{
			int nContactId = item->data(0, Qt::UserRole).toInt();
			int nGroupId = item->parent()->data(0, Qt::UserRole).toInt();
			/* show contact view */
			bool bEdited = FALSE;
			BelugaView view(this, nContactId, &bEdited);
			view.exec();

			if (bEdited)
				updateGroupView(nGroupId);
		}
		break;
	case CONTACT_VOICECALL_ACTION:
		break;
	case CONTACT_IPCALL_ACTION:
		break;
	case CONTACT_VIDEOCALL_ACTION:
		break;
	case CONTACT_EDIT_ACTION:
		{
			CContact * pContact = NULL;
			m_pContactDb->GetEntityById(item->data(0, Qt::UserRole).toInt(), (CDbEntity**)&pContact);
			if (pContact == NULL)
				return;

			BelugaDetail detail(this, this);
			detail.setFieldsValue((CPhoneContact*)pContact);
			if (QDialog::Accepted == detail.exec())
				updateGroupView(item->parent()->data(0, Qt::UserRole).toInt());
		}
		break;
	case CONTACT_DEL_ACTION:
		{
			QMessageBox msg(QMessageBox::Question, "", tr("Sure to delete the contact?"), QMessageBox::Yes|QMessageBox::No, this);
			msg.setDefaultButton(QMessageBox::Yes);
			if (QMessageBox::Yes == msg.exec())
			{
				if (0 == m_pContactDb->DeleteEntity(item->data(0, Qt::UserRole).toInt()))  /* remove successfully */
				{
					QTreeWidgetItem * group = item->parent();
					m_qCurTree->setCurrentItem(group);
					group->removeChild(item);
				}
			}
		}
		break;
	case CONTACT_MSG_ACTION:
		{
			CContact * pContact = NULL;
			m_pContactDb->GetEntityById(item->data(0, Qt::UserRole).toInt(), (CDbEntity**)&pContact);
			if (pContact == NULL)
				return;
			
			GString * phone = NULL;
			GString * name = NULL;
			pContact->GetFieldValue(ContactField_PhonePref, &phone);
			pContact->GetFieldValue(ContactField_Name, &name);
			NewMessage message(this, phone->str, name->str);
			message.exec();

			g_string_free(phone, TRUE);
			g_string_free(name, TRUE);
			delete pContact;
		}
		break;
	case CONTACT_GROUP_ACTION:
		{
			gint32 ret = ECode_No_Error;
			int i = 0;
			QString group;
			QStringList items;
			int ids[20] = {0};
			bool bOK = FALSE;
			CGroupIterator * pGroupIterator = NULL;

			ret = m_pGroupDb->GetAllGroupsByTag(m_nCurTabIndex + 1, &pGroupIterator);
			if (ret != ECode_No_Error)
			{
				printf("Get all groups failed!\n");
				break;
			}

			if (m_nCurTabIndex + 1 == ContactType_Phone)
				items << QString(tr("My Contact"));
			else
				items << QString(tr("My Friend"));

			ids[i++] = (int)GROUPID_DEFAULT;

			BOOL bSucceed = FALSE;
			do
			{
				CDbEntity * pEntity = NULL;
				ret = pGroupIterator->Current(&pEntity);	
				if (ret != ECode_No_Error)
				{
					printf("Get group instance failed!\n");
					goto _Error;
				}

				CGroup * pGroup = (CGroup*)pEntity;

				GString * groupName = NULL;
				pGroup->GetFieldValue(GroupField_Name, &groupName);
				items << QString(groupName->str);
				g_string_free(groupName, TRUE);

				GString * groupId = NULL;
				pGroup->GetFieldValue(GroupField_Id, &groupId);
				ids[i++] = atoi(groupId->str);
				g_string_free(groupId, TRUE);

				delete pGroup;
				pGroup = NULL;
			} while(0 == pGroupIterator->Next(&bSucceed) && bSucceed);

			delete pGroupIterator;
			pGroupIterator = NULL;

			group = QInputDialog::getItem(this, tr("Change Group"), tr("Change the Contact to Group:"), items, 0, FALSE, &bOK);
			if (bOK)
			{
				int nIndex = items.indexOf(group);
				int nGroupId = ids[nIndex];
				int nContactId = m_qCurTree->currentItem()->data(0, Qt::UserRole).toInt();

				if (nIndex != -1) /* release prev relation */ 
				{
					QTreeWidgetItem * prevGroup = m_qCurTree->currentItem()->parent();
					prevGroup->removeChild(m_qCurTree->currentItem());
					m_qCurTree->setCurrentItem(prevGroup);
					int nPrevGroupId = prevGroup->data(0, Qt::UserRole).toInt();
					/* release prev relation */
					m_pContactDb->ReleaseContactGroupRelation(nContactId, nPrevGroupId);
				}

				if (nIndex > 0)  /* -1: invalid   0: default group */
				{
					/* add the contact to group */
					m_pContactDb->CreateContactGroupRelation(nContactId, nGroupId);
					updateGroupView(nGroupId);
				}
				else
				{
					updateGroupView((int)GROUPID_DEFAULT);
				}
			}
_Error:
			if (pGroupIterator != NULL)
			{
				delete pGroupIterator;
				pGroupIterator = NULL;
			}
		}
		break;
	case CONTACT_SYNC_ACTION:
		break;

	/* group actions */
	case GROUP_EXPAND_COLLAPSE_ACTION:
		break;
	case GROUP_EDIT_ACTION:
		{
			bool bOK = FALSE;
			CGroup * pGroup = NULL;

			int groupId = m_qCurTree->currentItem()->data(0, Qt::UserRole).toInt();
			if (groupId == (int)GROUPID_DEFAULT)
				return;

			m_pGroupDb->GetEntityById(groupId, (CDbEntity**)&pGroup);

			GString * name = NULL;
			pGroup->GetFieldValue(GroupField_Name, &name);
			QString group = QInputDialog::getText(this, tr("Edit group"), tr("Edit the group:"), QLineEdit::Normal, QString(name->str), &bOK);
			g_string_free(name, TRUE);

			if (bOK && !group.isEmpty())
			{
				/* set group name  */
				GString * groupName = g_string_new(group.toLatin1().data());
				pGroup->SetFieldValue(GroupField_Name, groupName);
				g_string_free(groupName, TRUE);

				/* set group namespell  */
				QString spell = Chinese2PY(group);
				GString * namespell = g_string_new(spell.toLatin1().data()); 
				pGroup->SetFieldValue(GroupField_NameSpell, namespell);
				g_string_free(namespell, TRUE);

				m_pGroupDb->UpdateEntity(pGroup);
				m_qCurTree->clear();
				loadGroups(m_nCurTabIndex + 1);
			}
		}
		break;
	case GROUP_NEW_ACTION:
		{
			bool bOK = FALSE;
			QString group = QInputDialog::getText(this, tr("Create group"), tr("Create a new group:"), QLineEdit::Normal, QString(), &bOK);
			if (bOK && !group.isEmpty())
			{
				CGroup * pGroup = new CGroup(m_pGroupDb);

				/* set tag id which group belong to */
				char szTagId[3] = {0};
				sprintf(szTagId, "%d", m_nCurTabIndex + 1);
				GString * tagId = g_string_new(szTagId);
				pGroup->SetFieldValue(GroupField_TagId, tagId);
				g_string_free(tagId, TRUE);

				/* set group name  */
				GString * groupName = g_string_new(group.toLatin1().data());
				pGroup->SetFieldValue(GroupField_Name, groupName);
				g_string_free(groupName, TRUE);

				/* set group namespell  */
				QString spell = Chinese2PY(group);
				GString * namespell = g_string_new(spell.toLatin1().data()); 
				pGroup->SetFieldValue(GroupField_NameSpell, namespell);
				g_string_free(namespell, TRUE);

				m_pGroupDb->SaveEntity(pGroup);
				m_qCurTree->clear();
				loadGroups(m_nCurTabIndex + 1);
			}
		}
		break;
	case GROUP_MSG_ACTION:
		break;
#if 0
	case GROUP_UP_ACTION:
		break;
	case GROUP_DOWN_ACTION:
#endif
		break;
	case GROUP_DEL_ACTION:
		{
			int groupId = item->data(0, Qt::UserRole).toInt();
			if (groupId == (int)GROUPID_DEFAULT)
				return;

			QMessageBox msg(QMessageBox::Question, "", tr("Sure to delete the group?"), QMessageBox::Yes|QMessageBox::No, this);
			msg.setDefaultButton(QMessageBox::Yes);
			if (QMessageBox::Yes == msg.exec())
			{
				if (0 == m_pGroupDb->DeleteEntity(groupId))  /* remove successfully */
				{
					int index = m_qCurTree->indexOfTopLevelItem(item);
					if (index != -1)
					{
						QTreeWidgetItem * del = m_qCurTree->takeTopLevelItem(index);
						delete del;
					}
				}
			}	
		}
		break;
	
	case SETTING_ACTION:
		{

		}
		break;
	default:
		break;
	}
}

void BelugaMain::onActionVoiceCallTriggered(bool checked)
{

}

void BelugaMain::onActionVideoCallTriggered(bool checked)
{

}

void BelugaMain::onActionIPCallTriggered(bool checked)
{

}

void BelugaMain::onActionMsgCTriggered(bool checked)
{
	onActionTriggered(CONTACT_MSG_ACTION);
}

void BelugaMain::onActionViewCTriggered(bool checked)
{
	onActionTriggered(CONTACT_VIEW_ACTION);
}

void BelugaMain::onActionNewCTriggered(bool checked)
{
	hidePersonStatus();
	BelugaDetail detail(this, this);
	detail.exec();
}

void BelugaMain::onActionEditCTriggered(bool checked)
{
	onActionTriggered(CONTACT_EDIT_ACTION);
}

void BelugaMain::onActionDelCTriggered(bool checked)
{
	onActionTriggered(CONTACT_DEL_ACTION);
}

void BelugaMain::onActionGroupCTriggered(bool checked)
{
	onActionTriggered(CONTACT_GROUP_ACTION);
}

void BelugaMain::onActionSyncTriggered(bool checked)
{

}


void BelugaMain::onActionEditGTriggered(bool checked)
{
	onActionTriggered(GROUP_EDIT_ACTION);
}

void BelugaMain::onActionNewGTriggered(bool checked)
{
	onActionTriggered(GROUP_NEW_ACTION);
}

#if 0
void BelugaMain::onActionUpGTriggered(bool checked)
{
	onActionTriggered(GROUP_UP_ACTION);
}

void BelugaMain::onActionDownGTriggered(bool checked)
{
	onActionTriggered(GROUP_DOWN_ACTION);
}
#endif

void BelugaMain::onActionDelGTriggered(bool checked)
{
	onActionTriggered(GROUP_DEL_ACTION);
}

void BelugaMain::onActionMsgGTriggered(bool checked)
{
	onActionTriggered(GROUP_MSG_ACTION);
}

void BelugaMain::onActionExpandColapseGTriggered(bool checked)
{

}

void BelugaMain::onActionSettingTriggered(bool checked)
{
	onActionTriggered(SETTING_ACTION);
}


void BelugaMain::onTextChanged(const QString & text)
{	
	if (FALSE == m_qWidgetPanelList.last()->isVisible())
	{
		if (0 == text.length())
			return;

		saveMenuBar(m_nCurTabIndex, m_nCurDefaultAction, 
			m_bIsCurTopItem ? GROUP_MENU : (m_nCurTabIndex + 1 == ContactType_Phone ? PHONECONTACT_MENU : IMCONTACT_MENU));

		/* show search panel menu */
		if (m_nCurTabIndex + 1 == ContactType_Phone)
			createContactActions(ContactType_Phone);
		else
			createContactActions(ContactType_IM);
		m_qMenuBar->setDefaultAction(m_qActionCloseSearch);
		m_nCurDefaultAction = CLOSE_SEARCH_ACTION;

		m_qWidgetPanelList.at(m_nCurTabIndex)->setVisible(FALSE);
		m_qCurTree = m_qTreeList.last();
		m_qWidgetPanelList.last()->setVisible(TRUE);
	}
	
/*	QTreeWidgetItem * item = m_qCurTree->currentItem();
	if (NULL != item && NULL != m_qCurTree->itemWidget(item, 0))
	{
		m_qCurTree->removeItemWidget(item, 0);
		item->setSizeHint(0, item->sizeHint(0));
		item->setText(0, m_qCurItemText);
	}	
*/
	m_qCurTree->clear();
	if (text.length())
	{
		gint32 ret = ECode_No_Error;
		CContactIterator * pContactIterator = NULL;

		ret = m_pContactDb->SearchContactsByName(m_nCurTabIndex + 1, (gchar*)text.toLatin1().data(), TRUE, &pContactIterator);
		if (ret != ECode_No_Error)
		{
			printf("search contacts failed!\n");
			return;
		}

		loadContacts(m_qCurTree, NULL, pContactIterator, TRUE); /* load searched result */
	}
	else
	{
		m_qWidgetPanelList.last()->setVisible(FALSE);
		m_qWidgetPanelList.at(m_nCurTabIndex)->setVisible(TRUE);
		m_qCurTree = m_qTreeList.at(m_nCurTabIndex);
		restoreMenuBar(m_nCurTabIndex);
	}
		
}

BOOL BelugaMain::saveMenuBar(int nTabId, int nDefaultActionId, int nMenuType)
{
	MenuBarStatus status = m_stMenuStatus.at(nTabId);
	status.nDefaultAction = nDefaultActionId;
	status.nMenuType = nMenuType;

	return TRUE;
}

BOOL BelugaMain::restoreMenuBar(int nTabId)
{
	m_qMenuBar->clear();
	
	if (m_stMenuStatus.at(nTabId).nMenuType == PHONECONTACT_MENU)
		createContactActions(ContactType_Phone);
	else if (m_stMenuStatus.at(nTabId).nMenuType == IMCONTACT_MENU)
		createContactActions(ContactType_IM);
	else
		createGroupActions();

	m_qMenuBar->setDefaultAction(m_qActions[m_stMenuStatus.at(nTabId).nDefaultAction]);
	m_nCurDefaultAction = m_stMenuStatus.at(nTabId).nDefaultAction;
	
	return TRUE;
}

void BelugaMain::onDefaultActionTriggered(bool checked)
{
	QTreeWidgetItem * item;
	switch(m_nCurDefaultAction)
	{
		/* contact actions */
	case CONTACT_NEW_ACTION:
		{
			BelugaDetail detail(this, this);
			detail.exec();
		}
		break;

		/* group actions */
	case GROUP_EXPAND_COLLAPSE_ACTION:
		item = m_qCurTree->currentItem();
		if (NULL == item)
		{
			item = m_qCurTree->topLevelItem(0);
			m_qCurTree->setCurrentItem(item);
		}
		else
		{
			if (item->isExpanded())
				m_qCurTree->collapseItem(item);
			else
				m_qCurTree->expandItem(item);
		}
		break;

		/* close search result */
	case CLOSE_SEARCH_ACTION:
		search->clear();
/*		item = m_qCurTree->currentItem();

		if (NULL != item && NULL != m_qCurTree->itemWidget(item, 0))
		{
			m_qCurTree->removeItemWidget(item, 0);
			item->setSizeHint(0, item->sizeHint(0));
			item->setText(0, m_qCurItemText);
		}	
*/
		m_qCurTree->clear();
		m_qWidgetPanelList.last()->setVisible(FALSE);
		m_qWidgetPanelList.at(m_nCurTabIndex)->setVisible(TRUE);
		m_qCurTree = m_qTreeList.at(m_nCurTabIndex);
		restoreMenuBar(m_nCurTabIndex);
		break;

	default:
		break;
	}
}

CContactDb * BelugaMain::getContactDb()
{
	return m_pContactDb;
}

CGroupDb * BelugaMain::getGroupDb()
{
	return m_pGroupDb;
}

QTreeWidget * BelugaMain::getGroupTree(int nTabIndex)
{
	return m_qTreeList.at(nTabIndex);
}

int BelugaMain::getCurrentTab()
{
	return m_nCurTabIndex;
}

BOOL BelugaMain::loadSelfContacts()
{
	gint32 ret; 
	CConfig * pConfig = NULL;
	
	if (m_bSelfContactLoaded)
		return TRUE;

	ret = m_pConfigDb->GetConfigByName("loadselfcontact", &pConfig);
	if (ret != ECode_No_Error)
	{
		printf("get config failed!");
		return FALSE;
	}
	
	GString * value = NULL;
	pConfig->GetFieldValue(ConfigField_Value, &value);
	if (atoi(value->str) == 0) /*  not loaded config value */
	{
		QMessageBox msg(QMessageBox::Question, "", tr("Load the native contacts?"), QMessageBox::Yes|QMessageBox::No, this);
		msg.setDefaultButton(QMessageBox::Yes);
		if (QMessageBox::No == msg.exec())
		{
			delete pConfig;
			return FALSE;
		}

		importContacts();

		/* set loaded config */
		g_string_assign(value, "1");
		pConfig->SetFieldValue(ConfigField_Value, value);
		m_pConfigDb->UpdateEntity(pConfig);
		g_string_free(value, TRUE);
		m_bSelfContactLoaded = TRUE;
	}

	return TRUE;
}

gboolean BelugaMain::importContacts()
{
	GList * pContactList = NULL;
	GList * pCurContact = NULL;
	GList * pLastContact = NULL;
	IContact * pIContact = NULL;
	int i = 0;

	BelugaWinMobile::Contact * pContact = new BelugaWinMobile::Contact();
	pContact->getContacts(&pContactList, 0, pContact->getContactCount());
	pCurContact = g_list_first(pContactList);
	pLastContact = g_list_last(pContactList);

	QProgressDialog progress(tr("Importing contacts..."), tr("Cancel"), 0, pContact->getContactCount(), this);
	progress.setMinimumDuration(1000);
	progress.setWindowModality(Qt::WindowModal);
	do 
	{
		if (pCurContact == NULL)
			break;

		progress.setValue(i++);
		if (progress.wasCanceled())
			break;

		CPhoneContact* pContact = new CPhoneContact(m_pContactDb);

		/* set contact type */
		GString * value = g_string_new("1");  /* 1: ContactType_Phone */
		pContact->SetFieldValue(ContactField_Type, value);
		g_string_free(value, TRUE);

		pIContact = (IContact*)(pCurContact->data);
		value = g_string_new("");

		BSTR bstr;
		const wchar_t* wstr;
		char* cstr;

		/* set contact name */
		pIContact->get_FirstName(&bstr);
		wstr = (const wchar_t*)bstr;
		cstr = (char*)malloc(2 * wcslen(wstr));
		wcstombs(cstr, wstr, wcslen(wstr));
		g_string_sprintf(value, "%s", cstr);
		SysFreeString(bstr);
		free(cstr);

		pIContact->get_MiddleName(&bstr);
		wstr = (const wchar_t*)bstr;
		cstr = (char*)malloc(2 * wcslen(wstr));
		wcstombs(cstr, wstr, wcslen(wstr));
		g_string_sprintfa(value, "%s", cstr);
		SysFreeString(bstr);
		free(cstr);

		pIContact->get_LastName(&bstr);
		wstr = (const wchar_t*)bstr;
		cstr = (char*)malloc(2 * wcslen(wstr));
		wcstombs(cstr, wstr, wcslen(wstr));
		g_string_sprintfa(value, " %s", cstr);
		SysFreeString(bstr);
		free(cstr);

		pContact->SetFieldValue(ContactField_Name, value);
		/* nickname */	
		pContact->SetFieldValue(ContactField_NickName, value);

		/* set name spell */
		QString spell = Chinese2PY(QString(value->str));
		g_string_free(value, TRUE);
		GString * namespell = g_string_new(spell.toLatin1().data()); 
		pContact->SetFieldValue(ContactField_NameSpell, namespell);
		/* set nickname spell */
		pContact->SetFieldValue(ContactField_NickNameSpell, namespell);
		g_string_free(namespell, TRUE);

		/* set contact sex, mobile contact has not sex field, default set male */
		value = g_string_new("0"); /* 0: male */
		pContact->SetFieldValue(ContactField_Sex, value);
		g_string_free(value, TRUE);


		/* nickname spell */

		/*
		ContactField_Photo,
		ContactField_Signature,
		ContactField_PhonePref,
		ContactField_EmailPref,
		ContactField_IMPref,
		ContactField_Birthday,
		ContactField_Org,
		ContactField_Url,
		ContactField_Ring,
		ContactField_Title,
		ContactField_Note,
		*/

		/* set mobile phone */ 
		pIContact->get_MobileTelephoneNumber(&bstr);
		wstr = (const wchar_t*)bstr;
		cstr = (char*)malloc(2 * wcslen(wstr));
		wcstombs(cstr, wstr, wcslen(wstr));
		value = g_string_new(cstr);
		SysFreeString(bstr);
		free(cstr);

		pContact->SetFieldValue(ContactField_PhonePref, value);
		g_string_free(value, TRUE);

		/* set work email */
		pIContact->get_Email1Address(&bstr);
		wstr = (const wchar_t*)bstr;
		cstr = (char*)malloc(2 * wcslen(wstr));
		wcstombs(cstr, wstr, wcslen(wstr));
		value = g_string_new(cstr);
		SysFreeString(bstr);
		free(cstr);

		pContact->SetFieldValue(ContactField_EmailPref, value);
		g_string_free(value, TRUE);

		/*
		imtable = NULL;
		addrarray = NULL;
		guint32 commkey[2] = {0x45,0x46};

		imtable = g_hash_table_new_full(g_int_hash, g_int_equal, NULL, g_free);
		g_hash_table_insert(imtable, &commkey[0], g_strdup("82010953"));  //QQ
		g_hash_table_insert(imtable, &commkey[1], g_strdup("sherry.co@163.com")); //MSN

		addrarray = g_ptr_array_new();
		stAddress * pAddr = (stAddress*)g_malloc0(sizeof(stAddress));
		pAddr->aid = 1;
		pAddr->atype = CommType_Address | CommType_Home;
		g_stpcpy(pAddr->block, "1508号");  // 中文可能需要先做utf8转换
		g_stpcpy(pAddr->street, "梅家浜路");
		g_stpcpy(pAddr->district, "松江");
		g_stpcpy(pAddr->city, "上海");
		g_stpcpy(pAddr->state, "上海");
		g_stpcpy(pAddr->country, "中国");
		g_stpcpy(pAddr->postcode, "200233");
		g_ptr_array_add(addrarray, pAddr);

		pContact->SetIMs(imtable);
		pContact->SetAddresses(addrarray);

		g_hash_table_destroy(imtable);
		freeAddressArray(addrarray);
		*/
		// 保存到数据库
		m_pContactDb->SaveEntity((CDbEntity*)pContact);
		pIContact->Release();

		pCurContact = g_list_next(pCurContact);
	} while(pCurContact != pLastContact);
	
	progress.setValue(pContact->getContactCount());
	delete pContact;

	return TRUE;
}

int BelugaMain::updateGroupView(int nGroupId)
{
	QTreeWidgetItem * item = NULL;
	int i = 0;
	bool bOk = FALSE;

	item = m_qCurTree->currentItem();
	if (NULL == item || nGroupId != item->parent()->data(0, Qt::UserRole).toInt())
	{
		for(i=0; i<m_qCurTree->topLevelItemCount(); i++)
		{
			item = m_qCurTree->topLevelItem(i);
			if (nGroupId == item->data(0, Qt::UserRole).toInt())
			{
				bOk = TRUE;
				break;
			}
		}
	}

	if (!bOk)
		return -1;

	QList<QTreeWidgetItem*> list = item->takeChildren();
	for (i=0; i<list.count(); i++)
		delete list[i];
	list.clear();

	/* reload contact list */
	gint32 ret = ECode_No_Error;
	CContactIterator * pContactIterator = NULL;

	if (GROUPID_DEFAULT != nGroupId)
		ret = m_pContactDb->GetAllContactsByGroup(nGroupId, TRUE, &pContactIterator);
	else
		ret = m_pContactDb->GetAllContactsNotInGroupByTag(m_nCurTabIndex + 1, TRUE, &pContactIterator);
	if (ret != ECode_No_Error)
	{
		printf("Get contacts failed!\n");
		return -1;
	}

	loadContacts(NULL, item, pContactIterator, FALSE);

	return 0;
}

void BelugaMain::onItemDoubleClicked(QTreeWidgetItem* item, int column)
{
	if (-1 != m_qCurTree->indexOfTopLevelItem(item))   /* group item */
		return;

	hidePersonStatus();

	if (m_nCurTabIndex == 0) /* phone contact */
	{
		int nContactId = item->data(0, Qt::UserRole).toInt();
		int nGroupId = item->parent()->data(0, Qt::UserRole).toInt();
		/* show contact view */
		bool bEdited = FALSE;
		BelugaView view(this, nContactId, &bEdited);
		view.exec();

		if (bEdited)
			updateGroupView(nGroupId);
	}
	else  /* im contact */
	{
		/* msg to contact */
	}
	
	return;
}

void BelugaMain::paintEvent(QPaintEvent *)
{
	QStyleOption opt;
	opt.init(this);
	QPainter p(this);
	style()->drawPrimitive(QStyle::PE_Widget, &opt, &p, this);
}

void BelugaMain::showPersonStatus()
{
	QTreeWidgetItem * item =m_qCurTree->currentItem();
	if (-1 != m_qCurTree->indexOfTopLevelItem(item))   /* group item */
		return;

//	if (m_nCurTabIndex == 0) /* phone contact */
	{	
		GString * string = NULL;
		CContact * pContact = NULL;
		int nContactId = item->data(0, Qt::UserRole).toInt();
		if (0!= m_pContactDb->GetEntityById(nContactId, (CDbEntity**)&pContact))
			return;

		/* set contact photo */
		QIcon qLogo;
		pContact->GetFieldValue(ContactField_Photo, &string);
		if (string->len != 0)
			qLogo.addFile(string->str);
		else
			qLogo.addFile(":/BelugaApp/Resources/images/contact_default.png");
		photoshow->setIcon(qLogo);
		g_string_free(string, TRUE);
		string = NULL;

		/* set contact signature */
		pContact->GetFieldValue(ContactField_Signature, &string);
		signature->setPlainText(string->str);
		g_string_free(string, TRUE);
		string = NULL;
		
		/* set contact name */
		pContact->GetFieldValue(ContactField_Name, &string);
		name->setText(string->str);
		g_string_free(string, TRUE);
		string = NULL;

		/*set location */ 
		stPhoneDistrict disctrict;
		pContact->GetFieldValue(ContactField_PhonePref, &string);
		if (0 == m_pContactDb->GetPhoneDistrict(string->str, &disctrict))
			location->setText(QString("%1 %2").arg(disctrict.ownerState).arg(disctrict.districtName));
		
		g_string_free(string, TRUE);
		string = NULL;
		delete pContact;
		pContact = NULL;

		/*set weather */ 
		weather->setIcon(QIcon(":/BelugaApp/Resources/weather/rain.png"));

		/* set sysmsg */
		tm time, now; 
		if (m_pContactDb->IsInRecentContact(nContactId)) /* the contact is a recent contact */
		{
			time = m_pContactDb->GetAarliestContactTime(nContactId);
		}
		else
		{
			time = m_pContactDb->GetAarliestContactTime();
		}
		
		GetLocalTime(&now);
		if (now.tm_mday == time.tm_mday && now.tm_hour == time.tm_hour && now.tm_min == time.tm_min && now.tm_sec == time.tm_sec)
			sysmsg->setText(tr("Long time we aren't in touch!"));

		int days = now.tm_mday - time.tm_mday;
		if (days > 7)
			sysmsg->setText(tr("At least a week we aren't in touch!"));
		else if (days > 3)
			sysmsg->setText(tr("At least 3 days we aren't in touch!"));
		else
			sysmsg->setText(tr("How are you, my friend!"));
		
		/* hide some widget  */
		m_qTabBar->setVisible(FALSE);
		contactphoto->setVisible(FALSE);
		search->setVisible(FALSE);

		m_qWidgetPanelList.at(m_nCurTabIndex)->setGeometry(0, 135, 240, 136);
		m_qTreeList.at(m_nCurTabIndex)->setGeometry(0, 0, 240, 133);

		/* show contact status */
		m_QStatusWidget->setVisible(TRUE);
	}
// 	else  /* im contact */
// 	{
// 		/* msg to contact */
// 	}
	
	m_statusVisible = TRUE;
	m_qCloseTimer->start(5000);
}

void BelugaMain::hidePersonStatus()
{
	if (!m_statusVisible)
		return;

	m_qShowTimer->stop();
	m_qWidgetPanelList.at(m_nCurTabIndex)->setGeometry(0, 50, 240, 220);
	m_qTreeList.at(m_nCurTabIndex)->setGeometry(0, 0, 240, 217);
	
	m_qTabBar->setVisible(TRUE);
	contactphoto->setVisible(TRUE);
	search->setVisible(TRUE);

	/* hide contact view */
	m_QStatusWidget->setVisible(FALSE);
	m_statusVisible = FALSE;
}

void BelugaMain::createPersonStatus()
{
	QPushButton *fun1;
	QPushButton *fun2;
	QPushButton *fun3;
	QPushButton *fun4;
	QPushButton *fun5;
	QPushButton *fun6;
	QPushButton *fun7;
	QPushButton *fun8;

	m_QStatusWidget = new QWidget(this);
	m_QStatusWidget->setGeometry(0, 0, 240, 140);
	m_QStatusWidget->setVisible(FALSE);

	photoshow = new QPushButton(m_QStatusWidget);
	photoshow->setObjectName(QString::fromUtf8("photoshow"));
	photoshow->setGeometry(QRect(5, 3, 80, 90));
	photoshow->setIconSize(QSize(80, 90));
	photoshow->setFlat(true);

	name = new QLabel(m_QStatusWidget);
	name->setObjectName(QString::fromUtf8("name"));
	name->setGeometry(QRect(90, 3, 145, 16));

	signature = new QPlainTextEdit(m_QStatusWidget);
	signature->setObjectName(QString::fromUtf8("signature"));
	signature->setGeometry(QRect(90, 20, 145, 38));
	signature->setReadOnly(TRUE);

	location = new QLabel(m_QStatusWidget);
	location->setObjectName(QString::fromUtf8("location"));
	location->setGeometry(QRect(100, 60, 85, 32));

	weather = new QPushButton(m_QStatusWidget);
	weather->setObjectName(QString::fromUtf8("weather"));
	weather->setGeometry(QRect(190, 60, 32, 32));
	weather->setIconSize(QSize(32, 32));
	weather->setFlat(true);

	sysmsg = new QLabel(m_QStatusWidget);
	sysmsg->setObjectName(QString::fromUtf8("sysmsg"));
	sysmsg->setGeometry(QRect(5, 93, 230, 16));

	fun1 = new QPushButton(m_QStatusWidget);
	fun1->setObjectName(QString::fromUtf8("fun1"));
	fun1->setGeometry(QRect(5, 110, 24, 24));
	fun1->setFlat(true);
	fun1->setIconSize(QSize(24, 24));
	fun1->setIcon(QIcon(":/BelugaApp/Resources/status/call.png"));

	fun2 = new QPushButton(m_QStatusWidget);
	fun2->setObjectName(QString::fromUtf8("fun2"));
	fun2->setGeometry(QRect(32, 110, 24, 24));
	fun2->setFlat(true);
	fun2->setIconSize(QSize(24, 24));
	fun2->setIcon(QIcon(":/BelugaApp/Resources/status/msg.png"));

	fun3 = new QPushButton(m_QStatusWidget);
	fun3->setObjectName(QString::fromUtf8("fun3"));
	fun3->setGeometry(QRect(59, 110, 24, 24));
	fun3->setFlat(true);
	fun3->setIconSize(QSize(24, 24));
	fun3->setIcon(QIcon(":/BelugaApp/Resources/status/qq.png"));
	qq = fun3;

	fun4 = new QPushButton(m_QStatusWidget);
	fun4->setObjectName(QString::fromUtf8("fun4"));
	fun4->setGeometry(QRect(86, 110, 24, 24));
	fun4->setFlat(true);
	fun4->setIconSize(QSize(24, 24));
	fun4->setIcon(QIcon(":/BelugaApp/Resources/status/msn.png"));
	msn = fun4;

	fun5 = new QPushButton(m_QStatusWidget);
	fun5->setObjectName(QString::fromUtf8("fun5"));
	fun5->setGeometry(QRect(113, 110, 24, 24));
	fun5->setFlat(true);
	fun5->setIconSize(QSize(24, 24));
	fun5->setIcon(QIcon(":/BelugaApp/Resources/status/friend.png"));

	fun6 = new QPushButton(m_QStatusWidget);
	fun6->setObjectName(QString::fromUtf8("fun6"));
	fun6->setGeometry(QRect(140, 110, 24, 24));
	fun6->setFlat(true);
	fun6->setIconSize(QSize(24, 24));
	fun6->setIcon(QIcon(":/BelugaApp/Resources/status/music.png"));

	fun7 = new QPushButton(m_QStatusWidget);
	fun7->setObjectName(QString::fromUtf8("fun7"));
	fun7->setGeometry(QRect(167, 110, 24, 24));
	fun7->setFlat(true);
	fun7->setIconSize(QSize(24, 24));
	fun7->setIcon(QIcon(":/BelugaApp/Resources/status/tv.png"));

	fun8 = new QPushButton(m_QStatusWidget);
	fun8->setObjectName(QString::fromUtf8("fun8"));
	fun8->setGeometry(QRect(194, 110, 24, 24));
	fun8->setFlat(true);
	fun8->setIconSize(QSize(24, 24));
	fun8->setIcon(QIcon(":/BelugaApp/Resources/status/game.png"));

#if 0
	m_QStatusWidget = new QWidget();
	m_QStatusWidget->setGeometry(0, 0, 240, 100);
	m_QStatusWidget->setVisible(FALSE);

	photoshow = new QPushButton(m_QStatusWidget);
	photoshow->setObjectName(QString::fromUtf8("photoshow"));
	photoshow->setGeometry(QRect(1, 1, 48, 64));
	photoshow->setIconSize(QSize(48, 64));
	photoshow->setFlat(true);

	name = new QLabel(m_QStatusWidget);
	name->setObjectName(QString::fromUtf8("name"));
	name->setGeometry(QRect(50, 2, 180, 16));

	signature = new QPlainTextEdit(m_QStatusWidget);
	signature->setObjectName(QString::fromUtf8("signature"));
	signature->setGeometry(QRect(50, 18, 180, 45));
	signature->setReadOnly(TRUE);

	sysmsg = new QLabel(m_QStatusWidget);
	sysmsg->setObjectName(QString::fromUtf8("sysmsg"));
	sysmsg->setGeometry(QRect(1, 66, 150, 16));

	location = new QLabel(m_QStatusWidget);
	location->setObjectName(QString::fromUtf8("location"));
	location->setGeometry(QRect(151, 66, 88, 16));

	QWidget * widget = new QWidget(m_QStatusWidget);
	widget->setGeometry(0, 83, 240, 16);

	fun1 = new QPushButton(widget);
	fun1->setObjectName(QString::fromUtf8("fun1"));
	fun1->setGeometry(QRect(2, 0, 16, 16));
	fun1->setFlat(true);
	fun1->setIconSize(QSize(16, 16));
	fun1->setIcon(QIcon(":/BelugaApp/Resources/status/call.png"));

	fun2 = new QPushButton(widget);
	fun2->setObjectName(QString::fromUtf8("fun2"));
	fun2->setGeometry(QRect(20, 0, 16, 16));
	fun2->setFlat(true);
	fun2->setIconSize(QSize(16, 16));
	fun2->setIcon(QIcon(":/BelugaApp/Resources/status/msg.png"));

	fun3 = new QPushButton(widget);
	fun3->setObjectName(QString::fromUtf8("fun3"));
	fun3->setGeometry(QRect(38, 0, 16, 16));
	fun3->setFlat(true);
	fun3->setIconSize(QSize(16, 16));
	fun3->setIcon(QIcon(":/BelugaApp/Resources/status/qq.png"));
	qq = fun3;

	fun4 = new QPushButton(widget);
	fun4->setObjectName(QString::fromUtf8("fun4"));
	fun4->setGeometry(QRect(56, 0, 16, 16));
	fun4->setFlat(true);
	fun4->setIconSize(QSize(16, 16));
	fun4->setIcon(QIcon(":/BelugaApp/Resources/status/msn.png"));
	msn = fun4;

	fun5 = new QPushButton(widget);
	fun5->setObjectName(QString::fromUtf8("fun5"));
	fun5->setGeometry(QRect(74, 0, 16, 16));
	fun5->setFlat(true);
	fun5->setIconSize(QSize(16, 16));
	fun5->setIcon(QIcon(":/BelugaApp/Resources/status/friend.png"));

	fun6 = new QPushButton(widget);
	fun6->setObjectName(QString::fromUtf8("fun6"));
	fun6->setGeometry(QRect(92, 0, 16, 16));
	fun6->setFlat(true);
	fun6->setIconSize(QSize(16, 16));
	fun6->setIcon(QIcon(":/BelugaApp/Resources/status/music.png"));

	fun7 = new QPushButton(widget);
	fun7->setObjectName(QString::fromUtf8("fun7"));
	fun7->setGeometry(QRect(110, 0, 16, 16));
	fun7->setFlat(true);
	fun7->setIconSize(QSize(16, 16));
	fun7->setIcon(QIcon(":/BelugaApp/Resources/status/tv.png"));

	fun8 = new QPushButton(widget);
	fun8->setObjectName(QString::fromUtf8("fun8"));
	fun8->setGeometry(QRect(128, 0, 16, 16));
	fun8->setFlat(true);
	fun8->setIconSize(QSize(16, 16));
	fun8->setIcon(QIcon(":/BelugaApp/Resources/status/game.png"));

	weather = new QPushButton(widget);
	weather->setObjectName(QString::fromUtf8("weather"));
	weather->setGeometry(QRect(146, 0, 16, 16));
	weather->setFlat(true);
	weather->setIconSize(QSize(16, 16));
#endif
}

void BelugaMain::onFocusChanged(QWidget * old, QWidget * now)
{
	QWidget * statusFocus = m_QStatusWidget->focusWidget();
	QWidget * appFocus = qApp->focusWidget();
	if (statusFocus == appFocus)
	{
		m_qCloseTimer->stop();
	}
	else
	{
		hidePersonStatus();
	}
}

void BelugaMain::loadRecentContact()
{
	GPtrArray * pContactArray = NULL;
	
	if ( 0 == m_pContactDb->GetRecentContacts(&pContactArray))
	{
		for (guint i=0; i<pContactArray->len; i++)
		{
			stRecentContact * pRecentContact = (stRecentContact*)g_ptr_array_index(pContactArray, i);
			QTreeWidgetItem * qItem = new QTreeWidgetItem();


			/* set contact id as item data */
			qItem->setData(0, Qt::UserRole, QVariant(pRecentContact->nContactId));
			
			/* set contact event */
			qItem->setData(1, Qt::UserRole, QVariant(pRecentContact->event));

			/* set recent event picture */
			QIcon qLogo;
			switch(pRecentContact->event)
			{
			case ContactEvent_SMS:
				{
					qLogo.addFile(":/BelugaApp/Resources/status/msg.png");
					break;
				}
			case ContactEvent_Email:
				{
					qLogo.addFile(":/BelugaApp/Resources/status/msg.png");
					break;
				}
			case ContactEvent_MMS:
				{
					qLogo.addFile(":/BelugaApp/Resources/status/msg.png");
					break;
				}
			case ContactEvent_AnsweredCall:
				{
					qLogo.addFile(":/BelugaApp/Resources/status/call.png");
					break;
				}
			case ContactEvent_RejectedCall:
				{
					qLogo.addFile(":/BelugaApp/Resources/status/call.png");
					break;
				}
			case ContactEvent_OutgoCall:
				{
					qLogo.addFile(":/BelugaApp/Resources/status/call.png");
					break;
				}
			case ContactEvent_QQ:
				{
					qLogo.addFile(":/BelugaApp/Resources/status/qq.png");
					break;
				}
			case ContactEvent_MSN:
				{
					qLogo.addFile(":/BelugaApp/Resources/status/msn.png");
					break;
				}
			}
			qItem->setIcon(0, qLogo);
			
			/* set contact event comminfo */
			qItem->setData(2, Qt::UserRole, QVariant(pRecentContact->eventCommInfo));

			/* set contact name */
			GString * contactName = NULL;
			CContact * pContact = NULL;
			m_pContactDb->GetEntityById(pRecentContact->nContactId, (CDbEntity**)&pContact);
			pContact->GetFieldValue(ContactField_Name, &contactName);
			QString recentContact = QString("%1(%2)").arg(tr(contactName->str), tr(pRecentContact->eventCommInfo));
			g_string_free(contactName, TRUE);
			qItem->setText(0, recentContact);

			/* set contact type */
			GString * contactType = NULL;
			pContact->GetFieldValue(ContactField_Type, &contactType);
			qItem->setData(3, Qt::UserRole, QVariant(contactType->str));
			g_string_free(contactType, TRUE);
			delete pContact;
			pContact = NULL;

			if (m_qCurTree == NULL)
			{
				m_qCurTree = m_qTreeList.at(m_nCurTabIndex);
			}
			m_qCurTree->addTopLevelItem(qItem);
		}
		
		createContactActions(ContactType_Phone);
		freeRecentContactArray(pContactArray);
	}
}