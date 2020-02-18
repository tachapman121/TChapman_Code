#ifndef COLLISIONOBJECT_H
#define COLLISIONOBJECT_H
#include <QString>

// since need to cast as item, using abstract function in collision detection
// static cast removes polymorphism so not working on check
class CollisionObject
{
public:
    virtual std::string getType() = 0;
};

#endif // COLLISIONOBJECT_H


