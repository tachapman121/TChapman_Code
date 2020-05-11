#include "food.h"
#include <qdebug.h>




Food::Food(){}

Food::Food(int cals, bool healthy, int sugar, int protein, int carb, int fat, std::string name, std::string texturePath)
{
    setCalories(cals);
    setHealthy(healthy);
    setSugar(sugar);
    setProtein(protein);
    setCarbs(carb);
    setFat(fat);
    setIsDead(false);
    this->name=name;
    this->spritePath = texturePath;

    QDir directory;
    directory.cdUp();

    if(!texture.loadFromFile(std::string(directory.path().toUtf8().constData()) + spritePath))
            qDebug() << "Texture load error " << QString::fromStdString(std::string(directory.path().toUtf8().constData()) + spritePath);

    foodSprite.setTexture(texture);
}

sf::Sprite Food::getSprite()
{
    return foodSprite;
}

//set calories in food.
void Food::setCalories(int cals)
{
    calories = cals;
}

//It's a healthy food, or is it?
void Food::setHealthy(bool healthy)
{
    this->healthy = healthy;
}

//It's full of sugar, or not.
void Food::setSugar(int sugar)
{
    this->sugar = sugar;
}


//We have the meats.  Maybe.
void Food::setProtein(int protein)
{
    this->protein = protein;
}

//This food could be some type of bread, cracker, cornthing, or other grain based goodness.
void Food::setCarbs(int carbs)
{
    this->carbs = carbs;
}

//Let's face it, you're just licking butter now, aren't you?
void Food::setFat(int fatty)
{
    this->fat = fatty;
}

//Calories in, calories out amirite?
int Food::getCalories()
{
    return calories;
}


//Time for a SUGAR RUSH!!!
int Food::getSugar()
{
    return sugar;
}

//Bob Harper stopped being a vegetarian so he could make more gains.
//Let's eat some meat.
int Food::getProtein()
{
    return protein;
}

//The whiter the bread the sooner you're dead.
int Food::getCarbs()
{
    return carbs;
}

//If it causes diabetes or cancer, it's probably not healthy.
bool Food::getHealthy()
{
    return healthy;
}

//Extra butter on that popcorn?  Yes PLEASE!
int Food::getFat()
{
    return fat;
}

// added to determine if food should be deleted
bool Food::getIsDead() const
{
    return isDead;
}

void Food::setIsDead(bool value)
{
    isDead = value;
}
