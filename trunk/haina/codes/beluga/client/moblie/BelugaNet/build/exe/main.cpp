/**
* @author north0808@gmail.com
* @version 1.0
*/
#include "beluganettest.h"
#include <QtGui/QApplication>
#include <QtCore/QTextCodec>

int main(int argc, char *argv[])
{
	const char *encode = "utf-8";
	QTextCodec::setCodecForLocale(QTextCodec::codecForName(encode));
	QTextCodec::setCodecForCStrings(QTextCodec::codecForName(encode));
	QTextCodec::setCodecForTr(QTextCodec::codecForName(encode));
	QApplication a(argc, argv);
	BelugaNetTest w;
	w.showMaximized();
	return a.exec();
}
