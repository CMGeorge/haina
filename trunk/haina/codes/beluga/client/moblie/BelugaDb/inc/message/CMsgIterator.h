/*
 ============================================================================
 Name		: CMsgIterator.h
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Msg Entity Iterator
 ============================================================================
 */

#ifndef __CMSGITERATOR_H__
#define __CMSGITERATOR_H__


#include <glib.h>
#include "CDbEntityIterator.h"

class CMsgIterator : public CDbEntityIterator
{
public:
	CMsgIterator(CEntityDb * pEntityDb): 
		CDbEntityIterator(pEntityDb)
		{
		}
	
	~CMsgIterator()
		{
		}
	
	IMPORT_C gint32 Current(CDbEntity ** pEntity);
	IMPORT_C gint32 Next(gboolean * pSuccess);
	IMPORT_C gint32 Prev(gboolean * pSuccess);
};	

#endif
