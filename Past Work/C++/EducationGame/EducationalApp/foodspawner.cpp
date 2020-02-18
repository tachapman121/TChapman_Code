#include "foodspawner.h"
#include <QDebug>

FoodSpawner::FoodSpawner()
{
    dropSpeed = 0.1;
}

void FoodSpawner::SpawnFood(b2World* World, int X, int Y, float SCALE, float speed)
{
    // qDebug() << "Starting add food to world";

    b2BodyDef BodyDef;
    BodyDef.position = b2Vec2(X , Y);
    BodyDef.type = b2_dynamicBody;
    BodyDef.gravityScale = 0;
    b2Body* Body = World->CreateBody(&BodyDef);

    // Adjust to change fall speed
    Body->SetGravityScale(0);
    Body->SetLinearVelocity(b2Vec2(0, speed));
    b2PolygonShape Shape;
    Shape.SetAsBox((128.f/2)/SCALE, (128.f/2)/SCALE);
    b2FixtureDef FixtureDef;

    // Used for determining what it is
    FixtureDef.filter.categoryBits = 0x0003; //it's food
    FixtureDef.density = 0.01;
    FixtureDef.friction = 0.7f;
    FixtureDef.shape = &Shape;
    Body->CreateFixture(&FixtureDef);

    // test to put player in body's fixture
    Body->GetFixtureList()->SetUserData(ourFoods.getRandomFood());

    // qDebug() << "Exiting Food";
}

void FoodSpawner::IncreaseSpeed()
{
    if (dropSpeed < .2)
        dropSpeed += 0.02;
}

void FoodSpawner::ResetSpeed()
{
    dropSpeed = 0.1;
}
