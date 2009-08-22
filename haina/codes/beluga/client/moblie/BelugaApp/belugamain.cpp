#include "belugamain.h"
#include "glib.h"


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

    setupUi(this);
	m_qTabBar = new QTabBar(this);
	m_qTabBar->setGeometry(QRect(55, 25, 182, 25));
	m_qTabBar->setShape(QTabBar::RoundedNorth);
	m_qTabBar->setExpanding(FALSE);
	m_qTabBar->setVisible(FALSE);
	/* set signals and slots */
	connect(m_qTabBar, SIGNAL(currentChanged(int)), this, SLOT(onCurrentChanged(int)));

	/* set search edit signals and slots */
	connect(search, SIGNAL(editingFinished()), this, SLOT(onEditingFinished()));
	connect(search, SIGNAL(textEdited(const QString&)), this, SLOT(onTextChanged(const QString&)));

	/* search icon label */
	searchlabel->setBuddy(search);

	/* create close search action */
	m_qActionCloseSearch = new QAction(tr("Close"), this);
	m_qActions[CLOSE_SEARCH_ACTION] = m_qActionCloseSearch;
	connect(m_qActionCloseSearch, SIGNAL(triggered(bool)), this, SLOT(onDefaultActionTriggered(bool)));

	/* create menu */ 
	m_qMenuBar = new QMenuBar();
	createGroupActions();

	/* init db engine and load main ui data */
	if (FALSE == initBelugDb())
	{
		printf("Init BelugaDb Failed!\n");
		return;
	}
	if (FALSE == loadTags() || FALSE == loadGroups(ContactType_Phone))
	{
		printf("Load BelugaDb data Failed!\n");
		return;
	}
	
	contactlogo->setPixmap(QPixmap(":/BelugaApp/Resources/images/contact_default.png"));
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

	m_pContactDb->InitEntityDb("\\Program Files\\Beluga\\beluga.db");
	m_pGroupDb->InitEntityDb("\\Program Files\\Beluga\\beluga.db");
	m_pTagDb->InitEntityDb("\\Program Files\\Beluga\\beluga.db");

	return TRUE;
}

BOOL BelugaMain::loadContacts(QTreeWidgetItem* item)
{
	gint32 ret = ECode_No_Error;
	CContactIterator * pContactIterator = NULL;
	int nGroupId = item->data(0, Qt::UserRole).toInt();

	ret = m_pContactDb->GetAllContactsByGroup(nGroupId, TRUE, &pContactIterator);
	if (ret != ECode_No_Error)
	{
		printf("Get all contacts failed!\n");
		return FALSE;
	}

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
			qLogo.addFile(contactLogo->str);
		else
			qLogo.addFile(":/BelugaApp/Resources/images/contact_default.png");
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
		delete pGroup;
		pGroup = NULL;
	} while(0 == pGroupIterator->Next(&bSucceed) && bSucceed);
	
	delete pGroupIterator;
	pGroupIterator = NULL;

	/* insert default group which is make up of all contact without group */ 
	QTreeWidgetItem * qDefaultGroupItem = new QTreeWidgetItem();

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
			MenuBarStatus status;
			status.nDefaultAction = GROUP_EXPAND_COLLAPSE_ACTION;
			status.nMenuType = GROUP_MENU;
			m_stMenuStatus.append(status);

			GString * tagOrder = NULL;
			pTag->GetFieldValue(TagField_TagOrder, &tagOrder);
			nTagIndex = atoi(tagOrder->str);
			g_string_free(tagOrder, TRUE);
								
			/* get Tag Name and Logo */
			GString * tagName = NULL;
			pTag->GetFieldValue(TagField_Name, &tagName);
			GString * tagLogo = NULL;
			pTag->GetFieldValue(TagField_Logo, &tagLogo);
			
			QWidget * pWidget = new QWidget(this);
			pWidget->setGeometry(QRect(0, 50, 240, 270));
			pWidget->setObjectName(tr(tagName->str));
			pWidget->setVisible(FALSE);
			m_qWidgetPanelList.append(pWidget);

			QTreeWidget * pTagList = NULL;
			pTagList = new QTreeWidget(pWidget);
			pTagList->setGeometry(QRect(0, 0, 240, 220));
			pTagList->header()->setVisible(FALSE);
			pTagList->setObjectName(tr(tagName->str));
			pTagList->setColumnCount(3);
			pTagList->setColumnWidth(0, 150);
			pTagList->setColumnWidth(1, 50);
			pTagList->setColumnWidth(2, 5);
			pTagList->setUniformRowHeights(FALSE);
			pTagList->setIndentation(1);
			pTagList->setWordWrap(TRUE);
			pTagList->setFrameShadow(QFrame::Plain);
			pTagList->setFrameShape(QFrame::NoFrame);
			pTagList->setLineWidth(1);
			m_qTreeList.append(pTagList);
			addItemOperation(pTagList);
		
			m_qTabBar->insertTab(nTagIndex, QIcon(QString(tagLogo->str)), QString());
			m_qTabBar->setTabData(nTagIndex, QVariant(tr(tagName->str)));
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
	MenuBarStatus status;
	status.nDefaultAction = GROUP_EXPAND_COLLAPSE_ACTION;
	status.nMenuType = GROUP_MENU;
	m_stMenuStatus.append(status);

	QWidget * pWidget = new QWidget(this);
	pWidget->setGeometry(QRect(0, 50, 240, 270));
	pWidget->setObjectName(tr("Recent Contact"));
	pWidget->setVisible(FALSE);
	m_qWidgetPanelList.append(pWidget);

	QTreeWidget * pTagList = NULL;
	pTagList = new QTreeWidget(pWidget);
	pTagList->setGeometry(QRect(0, 0, 240, 240));
	pTagList->header()->setVisible(FALSE);
	pTagList->setObjectName(tr("Recent Contact"));
	pTagList->setColumnCount(3);
	pTagList->setColumnWidth(0, 150);
	pTagList->setColumnWidth(1, 50);
	pTagList->setColumnWidth(2, 5);
	pTagList->setUniformRowHeights(FALSE);
	pTagList->setIndentation(1);
	pTagList->setWordWrap(TRUE);
	pTagList->expandAll();
	pTagList->setFrameShadow(QFrame::Plain);
	pTagList->setFrameShape(QFrame::NoFrame);
	pTagList->setLineWidth(1);
	m_qTreeList.append(pTagList);
	addItemOperation(pTagList);
	
	m_qTabBar->insertTab(nTagIndex + 1, QIcon(":/BelugaApp/Resources/images/recent.png"), QString());
	m_qTabBar->setTabData(nTagIndex + 1, QVariant(tr("Recent Contact")));

	/* search list parent widget */
	status.nDefaultAction = GROUP_EXPAND_COLLAPSE_ACTION;
	status.nMenuType = GROUP_MENU;
	m_stMenuStatus.append(status);

	pWidget = new QWidget(this);
	pWidget->setGeometry(QRect(0, 50, 240, 270));
	pWidget->setObjectName(tr("Search Result"));
	pWidget->setVisible(FALSE);
	m_qWidgetPanelList.append(pWidget);

	pTagList = new QTreeWidget(pWidget);
	pTagList->setGeometry(QRect(0, 0, 240, 240));
	pTagList->header()->setVisible(FALSE);
	pTagList->setObjectName(tr("Search Result"));
	pTagList->setColumnCount(3);
	pTagList->setColumnWidth(0, 150);
	pTagList->setColumnWidth(1, 50);
	pTagList->setColumnWidth(2, 5);
	pTagList->setUniformRowHeights(FALSE);
	pTagList->setIndentation(1);
	pTagList->setWordWrap(TRUE);
	pTagList->expandAll();
	pTagList->setFrameShadow(QFrame::Plain);
	pTagList->setFrameShape(QFrame::NoFrame);
	pTagList->setLineWidth(1);
	m_qTreeList.append(pTagList);
	addItemOperation(pTagList);

	return TRUE;

_Error:
	if (pTagIterator != NULL)
	{
		delete pTagIterator;
		pTagIterator = NULL;
	}
	
	return FALSE;
}

void BelugaMain::onCurrentChanged(int nIndex)
{
	saveMenuBar(m_nCurTabIndex, m_nCurDefaultAction, 
		m_bIsCurTopItem ? GROUP_MENU : (m_nCurTabIndex + 1 == ContactType_Phone ? PHONECONTACT_MENU : IMCONTACT_MENU));
	
	m_qTabBar->setTabText(m_nCurTabIndex, QString(""));
	m_qWidgetPanelList.at(m_nCurTabIndex)->setVisible(FALSE);
	m_nCurTabIndex = nIndex;
	m_qCurTree = m_qTreeList.at(m_nCurTabIndex);
	m_qTabBar->setTabText(m_nCurTabIndex, m_qTabBar->tabData(m_nCurTabIndex).toString());
	m_qWidgetPanelList.at(m_nCurTabIndex)->setVisible(TRUE);

	restoreMenuBar(nIndex);
}

BOOL BelugaMain::addItemOperation(QTreeWidget * tree)
{
	connect(tree, SIGNAL(currentItemChanged(QTreeWidgetItem*, QTreeWidgetItem*)), 
				this, SLOT(onCurrentItemChanged(QTreeWidgetItem*, QTreeWidgetItem*)));
	connect(tree, SIGNAL(itemExpanded(QTreeWidgetItem*)), this, SLOT(onItemExpanded(QTreeWidgetItem*)));
	connect(tree, SIGNAL(itemCollapsed(QTreeWidgetItem*)), this, SLOT(onItemCollapsed(QTreeWidgetItem*)));
	connect(tree, SIGNAL(itemClicked(QTreeWidgetItem*, int)), this, SLOT(onItemClicked(QTreeWidgetItem*, int)));

	return TRUE;
}

void BelugaMain::onCurrentItemChanged(QTreeWidgetItem* current, QTreeWidgetItem* previous)
{
	int prevIndex;
	int curIndex;
	
	if (NULL == current)
		return;

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

			/* change current item large icon size */
			current->setSizeHint(0, QSize(current->sizeHint(0).width(), 32));
			QLabel * iconlabel = new QLabel();
			iconlabel->setObjectName(QString::fromUtf8("iconlabel"));
			iconlabel->setGeometry(QRect(3, 3, 200, 26));
			iconlabel->setPixmap(QPixmap(QString::fromUtf8(":/BelugaApp/Resources/images/contact_default.png")).scaled(QSize(26, 26)));

			m_qCurItemText.clear();
			m_qCurItemText.append(current->text(0));
			current->setText(0, QString("  %1\n  %2").arg(m_qCurItemText, current->data(1, Qt::UserRole).toString()));
			m_qCurTree->setItemWidget(current, 0, iconlabel);
		}
		else if (-1 != prevIndex && -1 == curIndex)  /* prev item is group but current item is contact */
		{
			m_bIsCurTopItem = FALSE;

			/* change current item large icon size */
			current->setSizeHint(0, QSize(current->sizeHint(0).width(), 32));
			QLabel * iconlabel = new QLabel();
			iconlabel->setObjectName(QString::fromUtf8("iconlabel"));
			iconlabel->setGeometry(QRect(3, 3, 200, 26));
			iconlabel->setPixmap(QPixmap(QString::fromUtf8(":/BelugaApp/Resources/images/contact_default.png")).scaled(QSize(26, 26)));

			m_qCurItemText.clear();
			m_qCurItemText.append(current->text(0));
			current->setText(0, QString("  %1\n  %2").arg(m_qCurItemText, current->data(1, Qt::UserRole).toString()));
			m_qCurTree->setItemWidget(current, 0, iconlabel);

			/* change menu actions to contact actions */
			createContactActions(m_nCurTabIndex + 1);
		}
		else if (-1 == prevIndex && -1 != curIndex) /* prev item is contact but current item is group */
		{
			m_bIsCurTopItem = TRUE;

			/* change prev item normal icon size */
			m_qCurTree->removeItemWidget(previous, 0);
			previous->setSizeHint(0, current->sizeHint(0));
			previous->setText(0, m_qCurItemText);

			/* change menu actions to group actions */
			createGroupActions();
		}
	}
	else
	{
		onItemClicked(current, 0);
	}
	
}

void BelugaMain::onItemExpanded(QTreeWidgetItem* item)
{
	m_qActionExpandColapseG->setText(tr("Collapse"));
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

		QIcon icon;
		if (item->isExpanded()) /* collapse group */
		{
			/* change to group icon */
			icon.addFile(":/BelugaApp/Resources/images/right.png");
			item->setIcon(0, icon);
			m_qCurTree->collapseItem(item);
		}
		else /* expand group to show contacts */
		{
			/* change to group expanded icon */
			icon.addFile(":/BelugaApp/Resources/images/down.png");
			item->setIcon(0, icon);

			if (0 == item->childCount()) /* load contact from the group */
			{
				loadContacts(item);
			}
			m_qCurTree->expandItem(item);
		}
	}
	else /* second level is contact item */
	{
		/* change contact item height */
		item->setSizeHint(0, QSize(item->sizeHint(0).width(), 32));
		
		if (NULL != m_qCurTree->itemWidget(item, 0)) /* item widget exists */
			return;

		if (m_bIsCurTopItem)  /* prev item is group */
		{
			createContactActions(m_nCurTabIndex + 1);
			m_bIsCurTopItem = FALSE;
		}

		/* change the icon size large */ 
		QLabel * iconlabel = new QLabel();
		iconlabel->setObjectName(QString::fromUtf8("iconlabel"));
		iconlabel->setGeometry(QRect(3, 3, 200, 26));
		iconlabel->setPixmap(QPixmap(QString::fromUtf8(":/BelugaApp/Resources/images/contact_default.png")).scaled(QSize(26, 26)));
		/* adjust text position */
		m_qCurItemText.clear();
		m_qCurItemText.append(item->text(0));
		item->setText(0, QString("  %1\n  %2").arg(m_qCurItemText, item->data(1, Qt::UserRole).toString()));
		m_qCurTree->setItemWidget(item, 0, iconlabel);
	}
}

void BelugaMain::onItemCollapsed(QTreeWidgetItem* item)
{
	m_qActionExpandColapseG->setText(tr("Expand"));
}

BOOL BelugaMain::createContactActions(int nContactType)
{
	m_qMenuBar->clear();
	
	if (ContactType_Phone == nContactType)
	{
		m_qActionViewC = new QAction(tr("View"), this);
		m_qActions[CONTACT_VIEW_ACTION] = m_qActionViewC;
		connect(m_qActionViewC, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
		m_qMenuBar->addAction(m_qActionViewC);

		m_qMenuCall = m_qMenuBar->addMenu(tr("Call"));		

		m_qActionVoiceCall = new QAction(tr("Voice"), this);
		m_qActions[CONTACT_VOICECALL_ACTION] = m_qActionVoiceCall;
		connect(m_qActionVoiceCall, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
		m_qMenuCall->addAction(m_qActionVoiceCall);

		m_qActionIpCall = new QAction(tr("IP"), this);
		m_qActions[CONTACT_IPCALL_ACTION] = m_qActionIpCall;
		connect(m_qActionIpCall, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
		m_qMenuCall->addAction(m_qActionIpCall);

		m_qActionVideoCall = new QAction(tr("Video"), this);
		m_qActions[CONTACT_VIDEOCALL_ACTION] = m_qActionVideoCall;
		connect(m_qActionVideoCall, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
		m_qMenuCall->addAction(m_qActionVideoCall);

		m_qActionMsgC = new QAction(tr("Message"), this);
		m_qActions[CONTACT_MSG_ACTION] = m_qActionMsgC;
		connect(m_qActionMsgC, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
		m_qMenuBar->addAction(m_qActionMsgC);

		m_qActionNewC = new QAction(tr("New"), this);
		m_qActions[CONTACT_NEW_ACTION] = m_qActionNewC;
		connect(m_qActionNewC, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
		connect(m_qActionNewC, SIGNAL(triggered(bool)), this, SLOT(onDefaultActionTriggered(bool)));
		m_qMenuBar->addAction(m_qActionNewC);

		m_qActionEditC = new QAction(tr("Edit"), this);
		m_qActions[CONTACT_EDIT_ACTION] = m_qActionEditC;
		connect(m_qActionEditC, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
		m_qMenuBar->addAction(m_qActionEditC);

		m_qActionDelC = new QAction(tr("Delete"), this);
		m_qActions[CONTACT_DEL_ACTION] = m_qActionDelC;
		connect(m_qActionDelC, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
		m_qMenuBar->addAction(m_qActionDelC);

		m_qActionGroupC = new QAction(tr("Group"), this);
		m_qActions[CONTACT_GROUP_ACTION] = m_qActionGroupC;
		connect(m_qActionGroupC, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
		m_qMenuBar->addAction(m_qActionGroupC);

		m_qActionSyncC = new QAction(tr("Sync"), this);
		m_qActions[CONTACT_MSG_ACTION] = m_qActionSyncC;
		connect(m_qActionSyncC, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
		m_qMenuBar->addAction(m_qActionSyncC);
		
		m_qMenuBar->setDefaultAction(m_qActionNewC);
		m_nCurDefaultAction = CONTACT_NEW_ACTION;
	}
	else
	{

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
	connect(m_qActionExpandColapseG, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
	connect(m_qActionExpandColapseG, SIGNAL(triggered(bool)), this, SLOT(onDefaultActionTriggered(bool)));

	m_qActionEditG = new QAction(tr("Edit"), this);
	m_qActions[GROUP_EDIT_ACTION] = m_qActionEditG;
	connect(m_qActionEditG, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
	m_qMenuBar->addAction(m_qActionEditG);

	m_qActionNewG = new QAction(tr("New"), this);
	m_qActions[GROUP_NEW_ACTION] = m_qActionNewG;
	connect(m_qActionNewG, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
	m_qMenuBar->addAction(m_qActionNewG);

	m_qMenuOrder = m_qMenuBar->addMenu(tr("Order"));	

	m_qActionUpG = new QAction(tr("Up"), this);
	m_qActions[GROUP_UP_ACTION] = m_qActionUpG;
	connect(m_qActionUpG, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
	m_qMenuOrder->addAction(m_qActionUpG);

	m_qActionDownG = new QAction(tr("Down"), this);
	m_qActions[GROUP_DOWN_ACTION] = m_qActionDownG;
	connect(m_qActionDownG, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
	m_qMenuOrder->addAction(m_qActionDownG);

	m_qActionDelG = new QAction(tr("Delete"), this);
	m_qActions[GROUP_DEL_ACTION] = m_qActionDelG;
	connect(m_qActionDelG, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
	m_qMenuBar->addAction(m_qActionDelG);

	m_qActionMsgG = new QAction(tr("Message"), this);
	m_qActions[GROUP_MSG_ACTION] = m_qActionMsgG;
	connect(m_qActionMsgG, SIGNAL(triggered(QAction*)), this, SLOT(onActionTriggered(QAction*)));
	m_qMenuBar->addAction(m_qActionMsgG);

	m_qMenuBar->setDefaultAction(m_qActionExpandColapseG);
	m_nCurDefaultAction = GROUP_EXPAND_COLLAPSE_ACTION;

	return TRUE;
}

void BelugaMain::onActionTriggered(QAction* action)
{
	int i;
	for (i=0; i<ACTION_NUM; i++)
		if (action == m_qActions[i])
			break;

	switch(i)
	{
	/* contact actions */
	case CONTACT_VIEW_ACTION:
		break;
	case CONTACT_VOICECALL_ACTION:
		break;
	case CONTACT_IPCALL_ACTION:
		break;
	case CONTACT_VIDEOCALL_ACTION:
		break;
	case CONTACT_NEW_ACTION:
		break;
	case CONTACT_EDIT_ACTION:
		break;
	case CONTACT_DEL_ACTION:
		break;
	case CONTACT_MSG_ACTION:
		break;
	case CONTACT_GROUP_ACTION:
		break;
	case CONTACT_SYNC_ACTION:
		break;

	/* group actions */
	case GROUP_EXPAND_COLLAPSE_ACTION:
		break;
	case GROUP_EDIT_ACTION:
		break;
	case GROUP_NEW_ACTION:
		break;
	case GROUP_MSG_ACTION:
		break;
	case GROUP_UP_ACTION:
		break;
	case GROUP_DOWN_ACTION:
		break;
	case GROUP_DEL_ACTION:
		break;

	default:
		break;
	}
}

void BelugaMain::onEditingFinished()
{

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
		searchContacts(text.toLatin1().data()); /* search */
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

BOOL BelugaMain::searchContacts(const char* text)
{
	gint32 ret = ECode_No_Error;
	CContactIterator * pContactIterator = NULL;

	ret = m_pContactDb->SearchContactsByName(m_nCurTabIndex + 1, (gchar*)text, TRUE, &pContactIterator);
	if (ret != ECode_No_Error)
	{
		printf("search contacts failed!\n");
		return FALSE;
	}

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
			qLogo.addFile(contactLogo->str);
		else
			qLogo.addFile(":/BelugaApp/Resources/images/contact_default.png");
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

		m_qTreeList.last()->addTopLevelItem(qContactItem);

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

void BelugaMain::onDefaultActionTriggered(bool checked)
{
	QTreeWidgetItem * item;
	switch(m_nCurDefaultAction)
	{
		/* contact actions */
	case CONTACT_NEW_ACTION:
		break;

		/* group actions */
	case GROUP_EXPAND_COLLAPSE_ACTION:
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