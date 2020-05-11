//
// Created by brayden on 4/21/16.
//

#ifndef EDUCATIONALAPPSERVER_WEBS_H
#define EDUCATIONALAPPSERVER_WEBS_H

#include "mysql_connection.h"
#include <cppconn/driver.h>
#include <cppconn/exception.h>
#include <cppconn/resultset.h>
#include <cppconn/statement.h>

class Webs {
public:
    Webs();
    void Start();
    void Stop();
    bool running;
    sql::Driver * driver;
    sql::Connection * connection;
};


#endif //EDUCATIONALAPPSERVER_WEBS_H
