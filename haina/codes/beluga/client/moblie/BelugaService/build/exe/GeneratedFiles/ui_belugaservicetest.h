/********************************************************************************
** Form generated from reading ui file 'belugaservicetest.ui'
**
** Created: Sat Oct 17 17:14:38 2009
**      by: Qt User Interface Compiler version 4.5.2
**
** WARNING! All changes made in this file will be lost when recompiling ui file!
********************************************************************************/

#ifndef UI_BELUGASERVICETEST_H
#define UI_BELUGASERVICETEST_H

#include <QtCore/QVariant>
#include <QtGui/QAction>
#include <QtGui/QApplication>
#include <QtGui/QButtonGroup>
#include <QtGui/QHeaderView>
#include <QtGui/QLabel>
#include <QtGui/QLineEdit>
#include <QtGui/QMainWindow>
#include <QtGui/QMenu>
#include <QtGui/QMenuBar>
#include <QtGui/QPushButton>
#include <QtGui/QWidget>

QT_BEGIN_NAMESPACE

class Ui_CBelugaServiceTestClass
{
public:
    QAction *actionExit;
    QWidget *centralWidget;
    QLineEdit *lineEditQQ;
    QPushButton *pushButtonSubmit;
    QLabel *labelStatus;
    QLabel *labelTitle;
    QMenuBar *menuBar;
    QMenu *menu_File;

    void setupUi(QMainWindow *CBelugaServiceTestClass)
    {
        if (CBelugaServiceTestClass->objectName().isEmpty())
            CBelugaServiceTestClass->setObjectName(QString::fromUtf8("CBelugaServiceTestClass"));
        CBelugaServiceTestClass->resize(600, 400);
        actionExit = new QAction(CBelugaServiceTestClass);
        actionExit->setObjectName(QString::fromUtf8("actionExit"));
        centralWidget = new QWidget(CBelugaServiceTestClass);
        centralWidget->setObjectName(QString::fromUtf8("centralWidget"));
        lineEditQQ = new QLineEdit(centralWidget);
        lineEditQQ->setObjectName(QString::fromUtf8("lineEditQQ"));
        lineEditQQ->setGeometry(QRect(20, 60, 121, 20));
        pushButtonSubmit = new QPushButton(centralWidget);
        pushButtonSubmit->setObjectName(QString::fromUtf8("pushButtonSubmit"));
        pushButtonSubmit->setGeometry(QRect(20, 100, 101, 23));
        labelStatus = new QLabel(centralWidget);
        labelStatus->setObjectName(QString::fromUtf8("labelStatus"));
        labelStatus->setGeometry(QRect(150, 60, 121, 16));
        labelTitle = new QLabel(centralWidget);
        labelTitle->setObjectName(QString::fromUtf8("labelTitle"));
        labelTitle->setGeometry(QRect(20, 30, 91, 16));
        CBelugaServiceTestClass->setCentralWidget(centralWidget);
        menuBar = new QMenuBar(CBelugaServiceTestClass);
        menuBar->setObjectName(QString::fromUtf8("menuBar"));
        menuBar->setGeometry(QRect(0, 0, 600, 20));
        menu_File = new QMenu(menuBar);
        menu_File->setObjectName(QString::fromUtf8("menu_File"));
        CBelugaServiceTestClass->setMenuBar(menuBar);

        menuBar->addAction(menu_File->menuAction());
        menu_File->addAction(actionExit);

        retranslateUi(CBelugaServiceTestClass);
        QObject::connect(actionExit, SIGNAL(triggered()), CBelugaServiceTestClass, SLOT(close()));

        QMetaObject::connectSlotsByName(CBelugaServiceTestClass);
    } // setupUi

    void retranslateUi(QMainWindow *CBelugaServiceTestClass)
    {
        CBelugaServiceTestClass->setWindowTitle(QApplication::translate("CBelugaServiceTestClass", "\350\257\273\345\217\226QQ\345\234\250\347\272\277\347\212\266\346\200\201", 0, QApplication::UnicodeUTF8));
        actionExit->setText(QApplication::translate("CBelugaServiceTestClass", "E&xit", 0, QApplication::UnicodeUTF8));
        lineEditQQ->setText(QString());
        pushButtonSubmit->setText(QApplication::translate("CBelugaServiceTestClass", "\346\237\245\350\257\242\345\234\250\347\272\277\347\212\266\346\200\201", 0, QApplication::UnicodeUTF8));
        labelStatus->setText(QString());
        labelTitle->setText(QApplication::translate("CBelugaServiceTestClass", "\350\257\267\350\276\223\345\205\245QQ\345\217\267\347\240\201\357\274\232", 0, QApplication::UnicodeUTF8));
        menu_File->setTitle(QApplication::translate("CBelugaServiceTestClass", "&File", 0, QApplication::UnicodeUTF8));
    } // retranslateUi

};

namespace Ui {
    class CBelugaServiceTestClass: public Ui_CBelugaServiceTestClass {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_BELUGASERVICETEST_H
