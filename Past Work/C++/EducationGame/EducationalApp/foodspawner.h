#ifndef FOODSPAWNER_H
#define FOODSPAWNER_H

#include "food.h"
#include "foodmenu.h"
#include "Box2D/Box2D.h"


//The goal here is to take in a location and spawn a food item there.
class FoodSpawner
{
    FoodMenu ourFoods;
    float dropSpeed;

public:
    FoodSpawner();
    void SpawnFood(b2World* world, int X, int Y, float SCALE, float speed);
    void IncreaseSpeed();
    void ResetSpeed();
    Food* getFood();
};

#endif // FOODSPAWNER_H
