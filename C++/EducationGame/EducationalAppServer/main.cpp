#include <iostream>
#include <signal.h>
#include <stdlib.h>
#include <stdio.h>
#include <unistd.h>
#include <sstream>
#include "mysql_connection.h"
#include <cppconn/driver.h>
#include <cppconn/exception.h>
#include <cppconn/resultset.h>
#include <cppconn/statement.h>
#include <cppconn/prepared_statement.h>
#include <SFML/Network.hpp>
#include <future>

#include "webs.h"

using namespace std;

bool listening;

void onSignalCaught(int sig) {
    cout << "Close signal caught. Cleaning up..." << endl;
    listening = false;
    exit(1);
}

void handleClient(sf::TcpSocket * client) {
    cout << "Client connected! (" << client->getRemoteAddress().toString() << ")" << endl;

    sf::Packet packet;

    if (client->receive(packet) == sf::Socket::Done) {

        /* Receive data via the packets */
        std::string msg;
        packet >> msg;

        /* split the recieved string by spaces into an array */
        string arr[4];
        int i = 0;
        stringstream ssin(msg);
        while(ssin.good() && i < 4){
            ssin >> arr[i];
            ++i;
        }

        /* the client is requesting a login */
        if(arr[0] == "LOGIN") {
            /* check the database for user creds */
            try {
                sql::Driver *driver;
                sql::Connection *con;
                /* Create a connection */
                driver = get_driver_instance();
                con = driver->connect("tcp://yo.octetship.com:3306", "yobruh", "dolladollabillyall");
                /* Connect to the MySQL healthy_running_game database */
                con->setSchema("healthy_running_game");
                /* query the database */
                auto username = arr[1];
                auto password = arr[2];

                auto stmt = con->prepareStatement("SELECT * FROM `users` WHERE `username` = ? AND `password` = ? AND `is_admin` = 1");
                stmt->setString(1, username);
                stmt->setString(2, password);
                auto res = stmt->executeQuery();

                /* send authentication response */
                sf::Packet outPacket;
                if (res->rowsCount() == 0) {
                    outPacket << "FAILURE";
                    client->send(outPacket);
                }
                else {
                    res->next();
                    outPacket << "SUCCESS " << res->getString("user_id");
                    client->send(outPacket);
                }
                delete res;
                delete stmt;
                delete con;
            }
            catch(sql::SQLException &e) {
                cout << "# ERR: SQLException in " << __FILE__;
                cout << "(" << __FUNCTION__ << ") on line " << __LINE__ << endl;
                cout << "# ERR: " << e.what();
                cout << " (MySQL error code: " << e.getErrorCode();
                cout << ", SQLState: " << e.getSQLState() << " )" << endl;
            }
        }

        /* client has sent a score */
        if(arr[0] == "SCORE"){
            try {
                /* store the score in the db */
                sql::Driver *driver;
                sql::Connection *con;
                /* Create a connection */
                driver = get_driver_instance();
                con = driver->connect("tcp://yo.octetship.com:3306", "yobruh", "dolladollabillyall");
                /* Connect to the MySQL healthy_running_game database */
                con->setSchema("healthy_running_game");
                /* query the databse */
                sql::Statement *stmt;
                sql::ResultSet *res;
                stmt = con->createStatement();

                res = stmt->executeQuery("INSERT INTO scores (user_id, score, difficulty) VALUES (" + arr[1] + ", " + arr[2] + ", " + arr[3] + ")");

                delete res;
                delete stmt;
                delete con;
                 } catch(sql::SQLException &e) {
                cout << "# ERR: SQLException in " << __FILE__;
                cout << "(" << __FUNCTION__ << ") on line " << __LINE__ << endl;
                cout << "# ERR: " << e.what();
                cout << " (MySQL error code: " << e.getErrorCode();
                cout << ", SQLState: " << e.getSQLState() << " )" << endl;
            }

        }

    }
}

int main() {
    Webs * webs = new Webs;

    thread([webs](){
        webs->Start();
    }).detach();

    signal(SIGINT, onSignalCaught);

    cout << "Starting EducationalAppServer on port : 50055" << endl;

    sf::TcpListener listener;
    if (listener.listen(50055) != sf::Socket::Done) {
        cout << "There was an error trying to listen on port: 50055" << endl;
        return -1;
    }

    listening = true;
    while(listening) {
        sf::TcpSocket * client = new sf::TcpSocket;
        if (listener.accept(*client) != sf::Socket::Done) {
            cout << "Error accepting client" << endl;
            continue;
        }

        thread(handleClient, client).detach();
    }

    webs->Stop();
    delete webs;
}