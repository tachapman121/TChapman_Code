#include "player.h"
#include <qdebug.h>
#include <qdir.h>
#include "food.h"
#include <QMediaPlayer>
#include "network.h"

extern bool isSpecialMenu;

int Player::getEnergy() const
{
    return energy;
}

QString Player::getCurrentSprite() const
{
    return currentSprite;
}

int Player::getSugarRushCycles() const
{
    return sugarRushCycles;
}

int Player::getFatCycles() const
{
    return fatCycles;
}

int Player::getHealth() const
{
    return healthLevel;
}

bool Player::isDead() const
{
    return dead;
}

void Player::setIsDead(bool isDead)
{
    dead = isDead;
}

int Player::getMaxHealth() const
{
    return maxHealth;
}

int Player::getMaxLevel() const
{
    return maxLevel;
}

Player::Player()
{
    QDir directory;
    directory.cdUp();

    //load all textures
    for(int spriteNum = 1; spriteNum <= 6; ++spriteNum)
    {
        playerTextures[spriteNum-1].loadFromFile(std::string(directory.currentPath().toUtf8().constData()) + "/../sprites/figures/character/cat" + std::to_string(spriteNum) + ".png");
        playerLeftTextures[spriteNum-1].loadFromFile(std::string(directory.currentPath().toUtf8().constData()) + "/../sprites/figures/character/catleft" + std::to_string(spriteNum) + ".png");
        qDebug() << "Sprite " << spriteNum << " Loaded";
    }

    energy = initialEnergy;
    sugarRushCycles = 0;
    fatCycles = 0;

    // set up sounds
    for(int i = 1; i < 4; i++)
    {
        eatSound[i-1].loadFromFile(std::string(directory.currentPath().toUtf8().constData()) + "/../sound/eating/eating" + std::to_string(i) + ".wav");
        qDebug() << "Eat " << i << " loaded";
    }
    if(!yuckSound.loadFromFile(std::string(directory.currentPath().toUtf8().constData()) + "/../sound/eating/yuck.wav"))
        qDebug() << "Error: yuck.wav not loaded";
    if(!walkSound.loadFromFile(std::string(directory.currentPath().toUtf8().constData()) + "/../sound/eating/walking.wav"))
        qDebug() << "Error: walking.wav sound not loaded";
    if(!congratulateSound.loadFromFile(std::string(directory.currentPath().toUtf8().constData()) + "/../sound/eating/congratz.wav"))
            qDebug() << "Error: congratulateSound.wav sound not loaded";
    if(!win1.loadFromFile(std::string(directory.currentPath().toUtf8().constData()) + "/../sound/eating/win_1.wav"))
            qDebug() << "Error: win_1.wav sound not loaded";
    if(!win2.loadFromFile(std::string(directory.currentPath().toUtf8().constData()) + "/../sound/eating/win_2.wav"))
            qDebug() << "Error: win_2.wav sound not loaded";
    if(!handclap.loadFromFile(std::string(directory.currentPath().toUtf8().constData()) + "/../sound/eating/hand_clap.wav"))
            qDebug() << "Error: hand_clap.wav sound not loaded";
    if(!sadSound.loadFromFile(std::string(directory.currentPath().toUtf8().constData()) + "/../sound/special/river.ogg"))
            qDebug() << "Error: river.ogg sound not loaded";

}

// eat food adding value to character //
void Player::eatFood(Food* food)
{
    energy += food->getCalories() * .7;

    // mechanics, can adjust as needed
    if (food->getHealthy())
    {
        healthLevel++;
        if (healthLevel > maxHealth)
            maxHealth = healthLevel;

        sound.setBuffer(eatSound[std::rand() % 3]);
        sound.play();
    }

    else
    {
        sound.setBuffer(yuckSound);
        sound.play();
        healthLevel--;
    }

    if (!food->getHealthy())
    {
        fatCycles = 50;
    }

    if (food->getSugar() > 20)
    {
        sugarRushCycles = 15;
    }

    if (healthLevel == 5)
        {
            sound.setBuffer(congratulateSound);
            sound.play();
        }

    if (healthLevel == 10)
    {
        sound.setBuffer(win1);
        sound.play();
    }

    if (healthLevel == 15)
    {
        sound.setBuffer(win2);
        sound.play();
    }

    if (healthLevel == 20)
    {
        sound.setBuffer(handclap);
        sound.play();
    }

    if (food->getCarbs() > 1000)
    {
        // TODO:
    }

    if (food->getProtein() > 1000)
    {
        // TODO:
    }

    // set to destroy food
    food->setIsDead(true);

    if (isSpecialMenu) {
        this->isCrying = true;
        sound.setBuffer(sadSound);
        sound.play();
    }
}

void Player::updateStats()
{
    // adjust as needed
    energy -= 2;

    if(sugarRushCycles > 0)
    {
        energy -= 2;
        sugarRushCycles--;
    }

    if (fatCycles > 0)
        fatCycles--;

    if (energy <= 0)
    {
        dead = true;
        maxLevel = (maxHealth / 10) + 1;
        qDebug() << "Max level: " << maxLevel;
        // qDebug() << "Ending Game" << endl;
    }

    if (isCrying && sound.getStatus() == sf::Sound::Status::Stopped)
        isCrying = false;
}

void Player::nextTexture()
{
    textureID++;
    if(textureID > 5) textureID = 0;
}

void Player::setMovingRight(bool movingRight)
{
    isMovingRight = movingRight;
}

sf::Texture Player::getTexture()
{
    if(isMovingRight)
        return playerTextures[textureID];
    else
        return playerLeftTextures[textureID];
}

void Player::bonusHealth(int i)
{
    healthLevel += i;
}

void Player::resetPlayer()
{
    dead = false;
    energy = initialEnergy;
    sugarRushCycles = 0;
    fatCycles = 0;
    healthLevel = 0;
}
