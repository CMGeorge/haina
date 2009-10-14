/*
 ============================================================================
 Name		: CSignatureIterator.h
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Signature Entity Iterator
 ============================================================================
 */

#ifndef __CSIGNATUREITERATOR_H__
#define __CSIGNATUREITERATOR_H__


#include <glib.h>
#include "CDbEntityIterator.h"

class CSignatureIterator : public CDbEntityIterator
{
public:
	IMPORT_C CSignatureIterator(CEntityDb * pEntityDb): 
		CDbEntityIterator(pEntityDb)
		{
		}
	
	IMPORT_C ~CSignatureIterator()
		{
		}
	
	IMPORT_C gint32 Current(CDbEntity ** pEntity);
	IMPORT_C gint32 Next(gboolean * pSuccess);
	IMPORT_C gint32 Prev(gboolean * pSuccess);
};	

#endif
