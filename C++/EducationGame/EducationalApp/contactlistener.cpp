#include "contactlistener.h"
#include "player.h"
#include "food.h"
#include <QString>
#include <qdebug.h>

ContactListener::ContactListener()
{

}

void ContactListener::BeginContact(b2Contact* contact)
{
    b2Fixture* first = contact->GetFixtureA();
    b2Fixture* second = contact->GetFixtureB();

    // Collision between player 0x0002 and Food 0x0003
    if (first->GetFilterData().categoryBits == 0x0002 && second->GetFilterData().categoryBits == 0x0003)
    {
        Player* player = static_cast<Player*>(first->GetUserData());
        Food* food = static_cast<Food*>(second->GetUserData());
        player->eatFood(food);
    }

    // Collision between food 0x0003 and player 0x0002
    else if (first->GetFilterData().categoryBits == 0x0003 && second->GetFilterData().categoryBits == 0x0002)
    {
        Player* player = static_cast<Player*>(second->GetUserData());
        Food* food = static_cast<Food*>(first->GetUserData());
        player->eatFood(food);
    }

    // Collision between food 0x0003 and ground 0x0004
    else if(first->GetFilterData().categoryBits == 0x0003 && second->GetFilterData().categoryBits == 0x0004)
    {
        Food* food = static_cast<Food*>(first->GetUserData());
        food->setIsDead(true); // delete if hits ground
    }

    // Collision between ground 0x0004 and food 0x0003
    else if(first->GetFilterData().categoryBits == 0x0004 && second->GetFilterData().categoryBits == 0x0003)
    {
        Food* food = static_cast<Food*>(second->GetUserData());
        food->setIsDead(true); // delete if hits ground
    }

//    else
//        qDebug() << "Something else collided, that's weird: " << first->GetFilterData().categoryBits <<
//                    " " << second->GetFilterData().categoryBits;
}

// Likely not needed since will either end game or eat food on contact but leave for now
void ContactListener::EndContact(b2Contact* contact)
{

}
