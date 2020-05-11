#ifndef DEMO_H
#define DEMO_H

#include <qsfmlcanvas.h>
#include <SFML/Window.hpp>
#include <Box2D/Box2D.h>
#include <QObject>
#include "foodmenu.h"
#include "contactlistener.h"
#include "foodspawner.h"
#include "player.h"
#include "test.h"
// this is a test file created to test the box2d / sfml setup

class Demo : public QObject
{ 
    sf::Texture* playerTexture;
    Q_OBJECT

public:
    Demo(QSFMLCanvas& sfmlview);
    void CreateFood();
    void CreateBase(b2World *World, float X, float Y);
    FoodMenu * allFoods;
    FoodSpawner spawnFood;
    int userId;
    bool isFirstTimeDeathLoop;
    void Run();
    bool isStarted;
    Player* player;
    Test test;
    sf::View * view;
    sf::Shader shader;
    sf::Shader * activeShader = NULL;
    void Quiz();
    void HandleMovement(b2Body*);
    void DrawBase(b2Body*);

public slots:
    void CreatePlayer(b2World *World, float X, float Y);
    void updatePlayer();
    void displayGameOver();

private:
    QSFMLCanvas& SFMLView;
    b2Vec2 Gravity;
    b2World* World;
    ContactListener listener;
    QTimer* foodTimer;
    QTimer* pTimer;

    // textures
    sf::Texture BaseTexture;
    sf::Texture FoodTexture;
    sf::Texture CharacterTexture;
    int spriteNum = 1;
    sf::Texture foodTexture;
    sf::Texture backTexture;

    // sprites
    sf::Sprite foodSprite;
    sf::Sprite backSprite;

    // text
    sf::Text EnergyText;
    sf::Text HealthText;
    sf::Text StatusText;
    sf::Text GameOver;
    sf::Text TryAgainButton;
    sf::Text QuizText;
    sf::Text AAnswerText;
    sf::Text BAnswerText;
    sf::Text CAnswerText;
    sf::Text DAnswerText;

    // misc
    sf::Font font;
};

#endif // DEMO_H
