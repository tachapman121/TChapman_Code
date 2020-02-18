#ifndef PLAYER_H
#define PLAYER_H
#include <QString>
#include <QObject>
#include "food.h"
#include "collisionobject.h"
#include "Box2D/Box2D.h"
#include <SFML/Audio.hpp>
#include "logindialog.h"

class Player
{
private:
    int energy;
    QString currentSprite;
    int sugarRushCycles;
    int fatCycles;

    int maxHealth = 0;
    int maxLevel = 1;
    int initialEnergy = 200;

    // textures
    sf::Texture playerTextures[6];
    sf::Texture playerLeftTextures[6];

    // sound buffers
    sf::SoundBuffer walkSound;
    sf::SoundBuffer eatSound[3];
    sf::SoundBuffer yuckSound;
    sf::SoundBuffer congratulateSound;
    sf::SoundBuffer win1;
    sf::SoundBuffer win2;
    sf::SoundBuffer handclap;
    sf::SoundBuffer sadSound;
    sf::Sound sound;

    int textureID = 0;
    bool dead = false;
    int healthLevel = 0;
    bool isMovingRight = true;

public:
    Player();
    void eatFood(Food *);
    void updateStats();
    bool isDead() const;
    void setIsDead(bool);
    int getEnergy() const;
    int getHealth() const;
    int getFatCycles() const;
    QString getCurrentSprite() const;
    int getSugarRushCycles() const;
    sf::Texture getTexture();
    void nextTexture();
    void setMovingRight(bool);
    void resetPlayer();

    int userId;

    void bonusHealth(int);
    bool isCrying;


    //now we can cheat. Yay!
    void giveEnergy(int extra)
    {
        energy += extra;
    }
    int getMaxHealth() const;
    int getMaxLevel() const;
};

#endif // PLAYER_H
