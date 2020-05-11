// QT
#include <QApplication>
#include <QFrame>
#include <QDebug>
#include <QPushButton>
#include <QVBoxLayout>
#include <iostream>
#include <QMediaPlayer>
#include <qdebug.h>

// external libs
#include "Box2D/Box2D.h"

// SFML
#include <SFML/Audio.hpp>
#include <SFML/Graphics.hpp>
#include <SFML/Network.hpp>

// custom
#include "gamecanvas.h"
#include "logindialog.h"
#include <QMessageBox>
#include "network.h"

bool shouldContinue = false;

// Don't hit me, its extern //
bool isSpecialMenu;

int main(int argc, char *argv[])
{
    QApplication App(argc, argv);

    LoginDialog loginDialog([&] {
        shouldContinue = true;
    });

    loginDialog.setWindowTitle("Login | Yo");
    loginDialog.exec();

    if (!shouldContinue) {
        return 0;
    }

    // Add Game Canvas
    GameCanvas* SFMLView = new GameCanvas(0, QPoint(20, 20), QSize(800, 800));
    SFMLView->userId = loginDialog.userId;
    SFMLView->setWindowTitle("The Game | Yo");
    SFMLView->show();

    return App.exec();
}
