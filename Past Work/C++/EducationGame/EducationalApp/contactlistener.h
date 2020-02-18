#ifndef CONTACTLISTENER_H
#define CONTACTLISTENER_H

#include"Box2D/Box2D.h"
#include <QObject>


class ContactListener : public b2ContactListener
{
public:
    ContactListener();
    virtual void BeginContact(b2Contact*);
    virtual void EndContact(b2Contact*);
};

#endif // CONTACTLISTENER_H
