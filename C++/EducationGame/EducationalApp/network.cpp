#include "network.h"
#include <SFML/Network.hpp>
#include <iostream>
#include <memory>
#include <QDebug>

Network::Network()
{
    socket = new sf::TcpSocket;
}

Network::~Network() {
    if (socket != nullptr)
        delete socket;
}

bool Network::connect() {
    return this->socket->connect("yo.octetship.com", 50055) == sf::Socket::Done;
}


void Network::send(std::string content) {
    if (this->socket == nullptr) return;

    sf::Packet outPacket;
    outPacket << content;

    if (this->socket->send(outPacket) != sf::Socket::Done) {
        qDebug() << "Error sending packet to server";
    }
}
