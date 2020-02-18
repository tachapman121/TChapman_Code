#-------------------------------------------------
#
# Project created by QtCreator 2016-04-12T20:13:50
#
#-------------------------------------------------

QT       += core gui
CONFIG  += c++11
QT += widgets
QT += multimedia

TARGET = EducationalApp
TEMPLATE = app


SOURCES += main.cpp\
    player.cpp \
    qsfmlcanvas.cpp \
    gamecanvas.cpp \
    demo.cpp \
    logindialog.cpp \
    network.cpp \
    food.cpp \
    foodmenu.cpp \
    contactlistener.cpp \
    foodspawner.cpp \
    question.cpp \
    test.cpp

HEADERS  += \
    player.h \
    qsfmlcanvas.h \
    gamecanvas.h \
    demo.h \
    logindialog.h \
    network.h \
    food.h \
    foodmenu.h \
    collisionobject.h \
    contactlistener.h \
    foodspawner.h \
    question.h \
    test.h

FORMS    += mainwindow.ui \
    logindialog.ui

mac {
    CONFIG -= app_bundle
}

win32: LIBS += -L$$PWD/../libs/win/Box2D/
macx: LIBS += -L$$PWD/../libs/mac/Box2D/

LIBS += -lBox2D

win32: INCLUDEPATH += $$PWD/../libs/win/Box2D
win32: DEPENDPATH += $$PWD/../libs/win/Box2D
macx: INCLUDEPATH += $$PWD/../libs/mac/Box2D
macx: DEPENDPATH += $$PWD/../libs/mac/Box2D

win32: LIBS += -L$$PWD/../libs/win/SFML/lib/
macx: LIBS += -L$$PWD/../libs/mac/SFML/lib/

LIBS += -lsfml-audio -lsfml-graphics -lsfml-network -lsfml-window -lsfml-system

win32: INCLUDEPATH += $$PWD/../libs/win/SFML/include
win32: DEPENDPATH += $$PWD/../libs/win/SFML/include
macx: INCLUDEPATH += $$PWD/../libs/mac/SFML/include
macx: DEPENDPATH += $$PWD/../libs/mac/SFML/include

# If need to run on CADE Lab

#unix:!macx: LIBS += -L$$PWD/../libs/linux/lib/ -lBox2D -lGLEW -lopenal -lsfml-audio  -lsfml-graphics -lsfml-network -lsfml-system -lsfml-window

#INCLUDEPATH += $$PWD/../libs/linux/include
#DEPENDPATH += $$PWD/../libs/linux/include

#unix:!macx: PRE_TARGETDEPS += $$PWD/../libs/linux/lib/libBox2D.a
#unix: !macx: QMAKE_RPATHDIR += $$PWD/../libs/linux/include/
