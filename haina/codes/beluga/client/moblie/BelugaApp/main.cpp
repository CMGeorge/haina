#include "belugaapp.h"
#include "belugastyle.h"
#include <QtGui/QApplication>

int main(int argc, char *argv[])
{
	QApplication a(argc, argv);
	QApplication::setStyle(new BelugaStyle);

	BelugaApp w;
	return a.exec();
}
