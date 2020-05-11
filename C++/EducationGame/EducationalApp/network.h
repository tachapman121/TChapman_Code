#ifndef NETWORK_H
#define NETWORK_H

#include <SFML/Network.hpp>
#include <QString>


class Network
{
public:
    Network();
    ~Network();
    bool connect();
    int login(QString, QString);
    void send(std::string content);
    sf::TcpSocket * socket;

};

#endif // NETWORK_H
