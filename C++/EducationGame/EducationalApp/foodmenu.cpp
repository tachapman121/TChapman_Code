#include "foodmenu.h"
#include <QDir>
#include <QDebug>
#include <QVector>
#include <time.h>

extern bool isSpecialMenu;

FoodMenu::FoodMenu()
{
//Food::Food(cals, healthy, sugar, protein, carb, fat, std::string name, std::string texturePath)
    //fruit
    hot = Food(105, true, 14, 1, 27, 0, std::string("Hot Hot Hot"), std::string("/sprites/figures/vegetables/spicy.png"));
    banana = Food(105, true, 14, 1, 27, 0, std::string("Medium Banana"), std::string("/sprites/figures/fruits/banana.png"));
    cherry = Food(97, true, 20, 2, 25, 0, std::string("Cup of Cherries"), std::string("/sprites/figures/fruits/cherry.png"));
    grape = Food(104, true, 23, 1, 27, 0, std::string("Cup of Grapes"), std::string("/sprites/figures/fruits/grape.png"));
    greenApple = Food(103, true, 17, 1, 24, 0, std::string("Green Apple"), std::string("/sprites/figures/fruits/greenapple.png"));
    kiwi = Food(110, true, 16, 2, 26, 1, std::string("Cup of Kiwi Fruit"), std::string("/sprites/figures/fruits/kiwi.png"));
    pear = Food(103, true, 17, 1, 27, 1, std::string("Pear"), std::string("/sprites/figures/fruits/pear.png"));
    pumpkin = Food(49, true, 3, 2, 12, 2, std::string("Cup of boiled Pumpkin (MMMmm)"), std::string("/sprites/figures/fruits/pumpkin.png"));
    redApple = Food(119, true, 21, 1, 28, 0, std::string("Red Delicious Apple"), std::string("/sprites/figures/fruits/redapple.png"));

    ourFoods.append(&cherry);
    ourFoods.append(&grape);
    ourFoods.append(&greenApple);
    ourFoods.append(&kiwi);
    ourFoods.append(&pear);
    ourFoods.append(&pumpkin);
    ourFoods.append(&redApple);

    //cals, healthy, sugar, protein, carb, fat, std::string name, std::string texturePath)

    eggPlant = Food(35, true, 3, 1, 8, 0, std::string("Cup of Boiled Eggplant (Delish!)"), std::string("/sprites/figures/vegetables/eggplant.png"));
    lettuce = Food(8, true, 1, 1, 2, 0, std::string("Cup of Lettuce"), std::string("/sprites/figures/vegetables/lettuce.png"));
    pepper = Food(24, true, 4, 1, 6, 2, std::string("Cup of Sliced Red Peppers"), std::string("/sprites/figures/vegetables/pepper.png"));
    tomato = Food(32, true, 5, 2, 7, 0, std::string("Cup of Diced Tomatos"), std::string("/sprites/figures/vegetables/tomato.png"));

    ourFoods.append(&eggPlant);
    ourFoods.append(&lettuce);
    ourFoods.append(&pepper);
    ourFoods.append(&tomato);

    //cals, healthy, sugar, protein, carb, fat, std::string name, std::string texturePath)
    //Carbs
    candyCane = Food(60, false, 14, 0, 14, 0, std::string("Candy Cane"), std::string("/sprites/figures/carbs/candycane.png"));
    coffee = Food(136, false, 10, 7, 11, 7, std::string("Caffe Latte, Whole Milk"), std::string("/sprites/figures/carbs/coffee1.png"));
    donut = Food(190, false, 10, 2, 21, 11, std::string("Glazed Donut"), std::string("/sprites/figures/carbs/donut1.png"));
    iceCream = Food(160, false, 20, 4, 27, 4, std::string("Fast Food Ice Cream Cone"), std::string("/sprites/figures/carbs/icecream_cone.png"));
    lollipop = Food(51, false, 10, 0, 13, 0, std::string("Two Lollipops"), std::string("/sprites/figures/carbs/lolipop.png"));
    fries = Food(190, false, 10, 2, 21, 11, std::string("French Fries"), std::string("/sprites/figures/carbs/fries.png"));
    hamburger= Food(190, false, 10, 2, 21, 11, std::string("McDouble"), std::string("/sprites/figures/carbs/hamburger.png"));
    softdrink= Food(190, false, 10, 2, 21, 11, std::string("Soft Drink"), std::string("/sprites/figures/carbs/softdrink.png"));

    ourFoods.append(&candyCane);
    ourFoods.append(&coffee);
    ourFoods.append(&donut);
    ourFoods.append(&iceCream);
    ourFoods.append(&lollipop);
    ourFoods.append(&fries);
    ourFoods.append(&hamburger);
    ourFoods.append(&softdrink);
    //cals, healthy, sugar, protein, carb, fat, std::string name, std::string texturePath)
    //grains
    grains = Food(216, true, 1, 5, 45, 2, std::string("Cup of Long Grain Brown Rice"), std::string("/sprites/figures/grains/grains.png"));

    steak = Food(207, true, 0, 35, 0, 7, std::string("4 oz Sirloin Steak"), std::string("/sprites/figures/meat/meat.png"));

    ourFoods.append(&steak);
    ourFoods.append(&grains);
}

Food * FoodMenu::getRandomFood()
{
    srand (time(NULL)); //seed random with time
    int index = rand() % ourFoods.count();
    return isSpecialMenu ? &hot : ourFoods[index];
}
