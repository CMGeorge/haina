/*
 ============================================================================
 Name		: CMsgFaceIterator.h
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : MsgFace Entity Iterator
 ============================================================================
 */

#ifndef __CMSGFACEITERATOR_H__
#define __CMSGFACEITERATOR_H__

#include <glib.h>
#include "CDbEntityIterator.h"

class CMsgFaceIterator : public CDbEntityIterator
{
public:
	IMPORT_C CMsgFaceIterator(CEntityDb * pEntityDb): 
		CDbEntityIterator(pEntityDb)
		{
		}
	
	IMPORT_C ~CMsgFaceIterator()
		{
		}
	
	IMPORT_C gint32 Current(CDbEntity ** pEntity);
	IMPORT_C gint32 Next(gboolean * pSuccess);
	IMPORT_C gint32 Prev(gboolean * pSuccess);
};	

#endif
