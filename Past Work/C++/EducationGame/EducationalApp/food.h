#ifndef FOOD_H
#define FOOD_H

#include <qsfmlcanvas.h>
#include <qstring.h>
#include <QDir>


//This class Will hold information about the type of food.
class Food
{
    int calories = 0;
    int protein;
    int carbs;
    bool healthy = false;
    int sugar;
    int fat;
    sf::Sprite foodSprite;
    bool isDead;


public:

    Food();
    std::string name;
    std::string spritePath;
    sf::Texture texture;
    Food(int cals, bool healthy, int sugar, int protein, int carb, int fat, std::string name, std::string texturePath);
    void setCalories(int);
    void setHealthy(bool);
    void setSugar(int sugary);
    void setProtein(int);
    void setCarbs(int);
    void setFat(int);
    int getCalories();
    bool getHealthy();
    int getSugar();
    int getProtein();
    int getCarbs();
    int getFat();
    bool getIsDead() const;
    void setIsDead(bool value);

    sf::Sprite getSprite();

};

#endif // FOOD_H
