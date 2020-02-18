#ifndef FOODMENU_H
#define FOODMENU_H

#include "food.h"
#include <QVector>

class FoodMenu
{

public:
    QVector<Food*> ourFoods;
    FoodMenu();
    Food* getRandomFood();

    Food hot;

    Food banana;//(105, false, false, false, true, true, true, false, std::string("Banana"), std::string("/../sprites/figures/banana.png"));
    Food cherry;//(77, false, false, false, true, true, true, false, std::string("Banana"), std::string("/../sprites/figures/cherry.png"));
    Food grape;//(62, false, false, false, true, true, true, false, std::string("Banana"), std::string("/../sprites/figures/grape.png"));
    Food greenApple;//(80, false, false, false, true, true, true, false, std::string("Banana"), std::string("/../sprites/figures/greenApple.png"));
    Food kiwi;//(42, false, false, false, true, true, true, false, std::string("Banana"), std::string("/../sprites/figures/kiwi.png"));
    Food pear;//(102, false, false, false, true, true, true, false, std::string("Banana"), std::string("/../sprites/figures/pear.png"));
    Food pumpkin;//(30, true, false, false, true, false, false, false, std::string("Banana"), std::string("/../sprites/figures/pumpkin.png"));
    Food redApple;

    //Veggies
    Food eggPlant;
    Food lettuce;
    Food pepper;
    Food tomato;

    //Sweets
    Food candyCane;
    Food coffee;
    Food donut;
    Food iceCream;
    Food lollipop;

    //Grains
    Food grains;

    //meat
    Food steak;

    //fast food
    Food fries;
    Food hamburger;
    Food softdrink;
};

#endif // FOODMENU_H
