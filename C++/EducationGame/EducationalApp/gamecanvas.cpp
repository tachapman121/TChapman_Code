#include "gamecanvas.h"
#include <iostream>
#include <string>
#include <QDir>
#include <QDebug>

GameCanvas::GameCanvas(QWidget* Parent, const QPoint& Position, const QSize& Size) : QSFMLCanvas(Parent, Position, Size), demo(*this)
{

}

void GameCanvas::OnInit()
{
    // reset clock
    gameClock.restart();

}
void GameCanvas::OnUpdate()
{
    // First, clear screen
    RenderWindow::clear(sf::Color(0,0,0, 0));
    demo.userId = this->userId;
    demo.Run();

    // Reset
    gameClock.restart();
}
