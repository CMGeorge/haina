
#ifndef BELUGASTYLE_H
#define BELUGASTYLE_H

#include<QtGui/QWindowsMobileStyle>
#include <QtGui/QPainter>

class BelugaStyle : public QWindowsMobileStyle
{
	Q_OBJECT;

public:
	BelugaStyle() {}

	void drawPrimitive(PrimitiveElement which,
		const QStyleOption *option,
		QPainter *painter,
		const QWidget *widget) const
	{
		switch (which) {
			case PE_IndicatorBranch:
				break;
			default:
				QWindowsMobileStyle::drawPrimitive(which, option, painter, widget);
		}
	}

};

#endif
