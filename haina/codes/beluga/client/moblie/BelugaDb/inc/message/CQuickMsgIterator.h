/*
 ============================================================================
 Name		: CQuickMsgIterator.h
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : QuickMsg Entity Iterator
 ============================================================================
 */

#ifndef __CQUICKMSGITERATOR_H__
#define __CQUICKMSGITERATOR_H__

#include <glib.h>
#include "CDbEntityIterator.h"

class CQuickMsgIterator : public CDbEntityIterator
{
public:
	IMPORT_C CQuickMsgIterator(CEntityDb * pEntityDb): 
		CDbEntityIterator(pEntityDb)
		{
		}
	
	IMPORT_C ~CQuickMsgIterator()
		{
		}
	
	IMPORT_C gint32 Current(CDbEntity ** pEntity);
	IMPORT_C gint32 Next(gboolean * pSuccess);
	IMPORT_C gint32 Prev(gboolean * pSuccess);
};	

#endif
