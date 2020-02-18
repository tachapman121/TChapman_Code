#ifndef GAMECANVAS_H
#define GAMECANVAS_H

#include "qsfmlcanvas.h"
#include <SFML/Graphics.hpp>
#include "demo.h"

// This seems to be equivalent of the MainWindow class in Qt Widget Application.
class GameCanvas : public QSFMLCanvas
{
public :
    GameCanvas(QWidget* Parent, const QPoint& Position, const QSize& Size);
    void OnInit();
    void OnUpdate();
    int userId;

private :
    Demo demo;
    sf::Clock gameClock;

};
#endif // GAMECANVAS_H
