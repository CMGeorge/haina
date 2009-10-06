#include "belugaservicetest.h"
#include <QtGui/QApplication>

int main(int argc, char *argv[])
{
	QApplication a(argc, argv);
	CBelugaServiceTest w;
	w.showMaximized();
	return a.exec();
}
