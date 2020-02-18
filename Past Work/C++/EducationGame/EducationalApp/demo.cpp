#include "demo.h"
#include "foodmenu.h"
#include <QDebug>
#include <QString>
#include <QDir>
#include <QMessageBox>
#include "collisionobject.h"
#include "foodspawner.h"
#include <QMediaPlayer>

#include "network.h"

#include "question.h"
#include <QInputDialog>


// for conversion between real world units and pixel units
static const float SCALE = 30.f;

extern bool isSpecialMenu;

Demo::Demo(QSFMLCanvas& sfmlview) : SFMLView(sfmlview)//, Gravity(0.f, 9.8f), World(Gravity)
{
    this->view = new sf::View;
    sfmlview.setView(*this->view);
    // initially a rotating banana is diaplayed
    if (!foodTexture.loadFromFile(std::string(QDir::currentPath().toUtf8().constData()) + "/../resources/foods/b.png")) {
        qDebug() << "Error loading /../resources/foods/b.png";
    }

    // Setup the food sprite
    foodSprite.setTexture(foodTexture);
    foodSprite.setPosition(400, 400);

    // background
    backTexture.loadFromFile(std::string(QDir::currentPath().toUtf8().constData()) + "/../resources/demo/bg.png");
    backSprite.setTexture(backTexture);
    backSprite.setPosition(0,0);

    b2Vec2 gravity(0.f, 4.9f);
    World = new b2World(gravity);

    // set contact listener
    World->SetContactListener(&listener);
    QDir directory;
    directory.setCurrent(directory.currentPath());
    qDebug() << directory.cdUp() << directory;
    CreateBase(World, 400.f, 755.f);

    if(!BaseTexture.loadFromFile(std::string(directory.path().toUtf8().constData()) + "/resources/demo/ground.png"))
            qDebug() << "Texture load errorDEMO base";
    else qDebug() << QString::fromStdString(std::string(directory.path().toUtf8().constData()) + "/resources/demo/ground.png");

    if(!FoodTexture.loadFromFile(std::string(directory.currentPath().toUtf8().constData()) + "/../resources/demo/apple.png"))
        qDebug()  << "Texture load errorDEMO";

    if(!CharacterTexture.loadFromFile(std::string(directory.currentPath().toUtf8().constData()) + "/../sprites/figures/character/cat1.png"))
        qDebug() << "Failed to load character";

    CreatePlayer(World, 10.f/SCALE, 10.f/SCALE);

    // Set up timer
    foodTimer = new QTimer(this);
    foodTimer->setSingleShot(false);
    QObject::connect(foodTimer, &QTimer::timeout, this, &Demo::CreateFood);
    foodTimer->start(2000);

    // Randomize seed based on current time
    std::srand(std::time(NULL));

    // first time death loop
    isFirstTimeDeathLoop = true;

    //playertexture timer
    pTimer = new QTimer(this);
    pTimer->setSingleShot(false);
    QObject::connect(pTimer, &QTimer::timeout, this, &Demo::updatePlayer );
    pTimer->start(100);

    // Set up text
    if(!font.loadFromFile(std::string(directory.path().toUtf8().constData()) + "/resources/fonts/bubblegum.ttf"))
        qDebug() << "Font load error";
    EnergyText.setFont(font);
    EnergyText.setCharacterSize(28);
    EnergyText.setColor(sf::Color(89,72,255));
    EnergyText.setPosition(25, 25);
    EnergyText.setString("Energy : ");

    HealthText.setPosition(25, 60);
    HealthText.setFont(font);
    HealthText.setCharacterSize(28);
    HealthText.setColor(sf::Color(89,72,255));
    HealthText.setString("Healthiness : ");

    StatusText.setPosition(25, 95);
    StatusText.setFont(font);
    StatusText.setCharacterSize(28);
    StatusText.setColor(sf::Color(89,72,255));
    StatusText.setString("");

    GameOver.setPosition(235, 200);
    GameOver.setFont(font);
    GameOver.setCharacterSize(65);
    GameOver.setColor(sf::Color::Red);
    GameOver.setString("Game Over");

    TryAgainButton.setPosition(275, 300);
    TryAgainButton.setFont(font);
    TryAgainButton.setCharacterSize(45);
    TryAgainButton.setColor(sf::Color::Red);
    TryAgainButton.setString("Try Again!");


    QuizText.setPosition(275, 300);
    QuizText.setFont(font);
    QuizText.setCharacterSize(45);
    QuizText.setColor(sf::Color::Red);

    AAnswerText.setPosition(275, 300);
    AAnswerText.setFont(font);
    AAnswerText.setCharacterSize(45);
    AAnswerText.setColor(sf::Color::Red);

    BAnswerText.setPosition(275, 300);
    BAnswerText.setFont(font);
    BAnswerText.setCharacterSize(45);
    BAnswerText.setColor(sf::Color::Red);

    CAnswerText.setPosition(275, 300);
    CAnswerText.setFont(font);
    CAnswerText.setCharacterSize(45);
    CAnswerText.setColor(sf::Color::Red);

    DAnswerText.setPosition(275, 300);
    DAnswerText.setFont(font);
    DAnswerText.setCharacterSize(45);
    DAnswerText.setColor(sf::Color::Red);

    if (!sf::Shader::isAvailable())
    {
        qDebug() << "You need to upgrade son";
    }

    if (!shader.loadFromFile("../resources/shaders/grayscale.frag", sf::Shader::Fragment))
    {
        qDebug() << "Can't find grayscale.frag shader. Sad day.";
    }

}

void Demo::Run()
{
    sf::Shader::bind(activeShader);
    SFMLView.draw(backSprite);

    QDir directory;
    directory.setCurrent(directory.currentPath());
    if (SFMLView.isOpen())
    {
        if (this->player->isCrying) {
            activeShader = &shader;
        } else {
            activeShader = NULL;
        }

        if(!player->isDead())
        {
            World->Step(1/30.f, 1, 1);

            for (b2Body* BodyIterator = World->GetBodyList();
                BodyIterator != 0;
                BodyIterator = BodyIterator->GetNext())
            {
                sf::Sprite Sprite;
                if (BodyIterator->GetType() == b2_dynamicBody)
                {

                    // food
                    if(BodyIterator->GetFixtureList()->GetFilterData().categoryBits == 0x0003)
                    {
                        Food * food = static_cast<Food*>(BodyIterator->GetFixtureList()->GetUserData());
                        if (food->getIsDead() || SCALE * BodyIterator->GetPosition().y > 700)
                        {
                            food->setIsDead(false);
                            World->DestroyBody(BodyIterator);
                            continue;
                        }
                        else
                        {
                            Sprite.setTexture(food->texture);
                        }
                    }

                    // player
                    else if (BodyIterator->GetFixtureList()->GetFilterData().categoryBits == 0x0002)
                    {
                        Sprite.setTexture(CharacterTexture);

                        /*********QUIZ AREA********/
                        if(player->getHealth() != 0 && player->getHealth() % 10 == 0)
                        {
                            Quiz();
                        }

                        HandleMovement(BodyIterator);
                    }

                    Sprite.setOrigin(10.f, 10.f);
                    Sprite.setPosition(SCALE * BodyIterator->GetPosition().x, SCALE * BodyIterator->GetPosition().y);
                }

                // base
                else
                {
                    Sprite.setTexture(BaseTexture);
                    Sprite.setOrigin(400.f, 2.f);
                    // Lower Y to match body,
                    Sprite.setPosition(BodyIterator->GetPosition().x * SCALE, (BodyIterator->GetPosition().y-1) * SCALE);
                }

                SFMLView.draw(Sprite);
            }
        }

        else
        {
            displayGameOver();
        }

        SFMLView.draw(EnergyText);
        SFMLView.draw(HealthText);
        SFMLView.draw(StatusText);
        sf::Shader::bind(NULL);
    }
}

// Spawns food
void Demo::CreateFood()
{
    int X = (std::rand() + std::rand()) % SFMLView.width();
    int Y = 1;
    spawnFood.SpawnFood(World, X/SCALE, Y/SCALE, SCALE, .1 + .01*player->getHealth() / 10);
}

// Creates bottom platform
void Demo::CreateBase(b2World* World, float X, float Y)
{
    b2BodyDef BodyDef;
    BodyDef.position = b2Vec2(X/SCALE, (Y)/SCALE);
    BodyDef.type = b2_staticBody;
    b2Body* Body = World->CreateBody(&BodyDef);
    b2PolygonShape Shape;
    Shape.SetAsBox((800.f/2), 80/SCALE);
    b2FixtureDef FixtureDef;
    FixtureDef.density = 0.f;
    FixtureDef.filter.categoryBits = 0x0004; // set as base
    FixtureDef.shape = &Shape;
    Body->CreateFixture(&FixtureDef);
}

// Intital creation of player
void Demo::CreatePlayer(b2World *World, float X, float Y)
{
    player = new Player();

    // Moved from creating in Player since would not update correctly
    b2BodyDef BodyDef;
    BodyDef.position = b2Vec2(X/SCALE, 600/SCALE);
    BodyDef.type = b2_dynamicBody;
    b2Body* Body = World->CreateBody(&BodyDef);
    b2PolygonShape Shape;
    Shape.SetAsBox((32.f/2)/SCALE, (32.f/2)/SCALE);
    b2FixtureDef FixtureDef;

    // Used for determining what it is
    FixtureDef.filter.categoryBits = 0x0002;
    FixtureDef.density = 1.f;
    FixtureDef.friction = 0.7f;
    FixtureDef.shape = &Shape;
    Body->CreateFixture(&FixtureDef);
    Body->SetFixedRotation(true);
    Body->GetFixtureList()->SetUserData(player);
}

void Demo::updatePlayer()
{
    player->updateStats();
    player->nextTexture();
    CharacterTexture = player->getTexture();
    EnergyText.setString("Energy: " + std::to_string(player->getEnergy()));
    HealthText.setString("Healthiness: " + std::to_string(player->getHealth()));

    std::string status = "";
    if (player->getFatCycles() > 0)
        status += "Overweight\n";
    else if (player->getSugarRushCycles() > 0)
        status += "Sugar Rush!";
    else if(player->isDead())
        status += "Too Tired To Move!";

    StatusText.setString(status);
}

void Demo::displayGameOver()
{
    // only hit this code the first time we hit this display loop (I hate myself for doing it this way)
    if(isFirstTimeDeathLoop){
        //save the user score
        Network *network = new Network();
        network->connect();
        network->socket->setBlocking(true);
        network->send("SCORE " + std::to_string(this->userId) + " " + std::to_string(player->getMaxHealth()) + " " + std::to_string(player->getMaxLevel()));
        network->socket->disconnect();
        isFirstTimeDeathLoop = false;
    }

    pTimer->stop();
    foodTimer->stop();
    SFMLView.draw(GameOver);
    SFMLView.draw(TryAgainButton);
    //get mouse position(relatively to sfml window)
    sf::Vector2i mousePos = sf::Mouse::getPosition(SFMLView);
    //get text position and bounding rect
    sf::Vector2f buttonPos = TryAgainButton.getPosition();
    sf::FloatRect buttonRect = TryAgainButton.getLocalBounds();

    //if mouse is within text
    if(mousePos.x > buttonPos.x && mousePos.y > buttonPos.y
            && mousePos.x < buttonPos.x + buttonRect.width && mousePos.y < buttonPos.y + buttonRect.height)
    {
        TryAgainButton.setColor(sf::Color::Magenta);
        if(sf::Mouse::isButtonPressed(sf::Mouse::Left))
        {
            player->resetPlayer();
            spawnFood.ResetSpeed();
            foodTimer->setInterval(2000);
            foodTimer->start(2000);
            pTimer->start(100);
            isFirstTimeDeathLoop = true;
        }
    }
    else
    {
        TryAgainButton.setColor(sf::Color::Red);
    }

}

void Demo::Quiz()
{
    //Quiz time
    pTimer->stop();
    foodTimer->stop();

    Question * question = test.getQuestion();
    QStringList quizlet = question->getAnswers();
    QString answer = QInputDialog::getItem(NULL, "Quiz Time", question->getQuestion(), question->getAnswers(), 0, false);
    if(answer == question->getAnswer())
    {
       // qDebug() << answer;// << question->getAnswer();
        player->bonusHealth(1);
        spawnFood.IncreaseSpeed();
        if (foodTimer->interval() > 1000)
            foodTimer->setInterval(foodTimer->interval() - 100);

        pTimer->start();
        foodTimer->start();
    } else
    {
        player->bonusHealth(-9);
        pTimer->start();
        foodTimer->start();
    }
}

void Demo::HandleMovement(b2Body* playerBody)
{
    // Move player left or right if key pressed
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::Left))
    {
        player->setMovingRight(false);
        if (player->getFatCycles() > 0)
            playerBody->SetLinearVelocity(b2Vec2(-.1, 0));
        else
            playerBody->SetLinearVelocity(b2Vec2(-.2, 0));
    }

    else if (sf::Keyboard::isKeyPressed(sf::Keyboard::Right))
    {
        player->setMovingRight(true);
        if (player->getFatCycles() > 0)
            playerBody->SetLinearVelocity(b2Vec2(.1, 0));
        else
            playerBody->SetLinearVelocity(b2Vec2(.2, 0));
    }

    else if (sf::Keyboard::isKeyPressed(sf::Keyboard::Up))
   {
       if (player->getFatCycles() == 0 && playerBody->GetPosition().y > 21.5) // only jump when not fat
           playerBody->ApplyForceToCenter(b2Vec2(0, -20), false);
   }

    else
        playerBody->SetLinearVelocity(b2Vec2(0,0));

    // Keep from moving out of bounds
    if(playerBody->GetPosition().x < 0)
        playerBody->SetLinearVelocity(b2Vec2(.1,0));
    else if( playerBody->GetPosition().x > 755/SCALE)
        playerBody->SetLinearVelocity(b2Vec2(-.1,0));
}
