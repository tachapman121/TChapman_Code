#include "webs.h"

#include <stdlib.h>
#include <signal.h>
#include <unistd.h>
#include <sstream>
#include "mongoose-cpp/mongoose/Server.h"
#include "mongoose-cpp/mongoose/WebController.h"
#include "mysql_connection.h"
#include <cppconn/driver.h>
#include <cppconn/exception.h>
#include <cppconn/resultset.h>
#include <cppconn/prepared_statement.h>

using namespace Mongoose;

class MainController : public WebController
{
public:

    Webs * webs;

    void login(Request &request, StreamResponse &response)
    {
        response.setHeader("Content-Type", "application/json");

        auto username = request.get("username", "");
        auto password = request.get("password", "");
        if (username == "" || password == "") {
            response << "{\"success\": " << false << "}" << endl;
            return;
        }

        cout << "Attempt login for " << username << endl;

        auto stmt = webs->connection->prepareStatement("SELECT * FROM `users` WHERE `username` = ? AND `password` = ? AND `is_admin` = 1");
        stmt->setString(1, username);
        stmt->setString(2, password);
        auto res = stmt->executeQuery();

        response << "{\"success\": " << (res->rowsCount() == 1)  << "}" << endl;
    }

    void users(Request &request, StreamResponse &response)
    {
        response.setHeader("Content-Type", "application/json");
        stringstream ss;

        auto state = webs->connection->createStatement();
        auto res = state->executeQuery("SELECT * FROM users");

        cout << "Running query: SELECT * FROM users" << endl;

        while (res->next()) {
            ss << "{\"name\": \"" << res->getString("username") << "\", \"id\": " << res->getInt("user_id") << ", \"is_admin\": " << res->getBoolean("is_admin") << "}";

            if (!res->isLast()) {
                ss << ",";
            }
        }

        response << "[" << ss.str() << "]" << endl;
    }

    void scores(Request &request, StreamResponse &response)
    {
      response.setHeader("Content-Type", "application/json");

      auto state = webs->connection->createStatement();
      auto res = state->executeQuery("SELECT scores.score, scores.difficulty, users.username FROM scores INNER JOIN users on users.user_id = scores.user_id ORDER BY score DESC");

      stringstream ss;

      while (res->next()) {
          ss << "{\"name\": \"" << res->getString("username") << "\", \"score\": " << res->getInt("score") << ", \"difficulty\": " << res->getBoolean("difficulty") << "}";

          if (!res->isLast()) {
              ss << ",";
          }
      }

      response << "[" << ss.str() << "]" << endl;
    }

    void deleteUser(Request &request, StreamResponse &response) {
        response.setHeader("Content-Type", "application/json");

        auto userId = request.get("id", "-1");
        cout << "Deleting: " << userId << endl;
        if (userId == "") {
            response << "{\"success\": " << false << "}" << endl;
            return;
        }

        auto stmt = webs->connection->prepareStatement("DELETE FROM `scores` WHERE `user_id` = ?");
        stmt->setInt(1, atoi(userId.c_str()));
        stmt->execute();

        auto stmt2 = webs->connection->prepareStatement("DELETE FROM `users` WHERE `user_id` = ?");
        stmt2->setInt(1, atoi(userId.c_str()));
        stmt2->execute();

        response << "{\"success\": " << true << "}" << endl;
    }

    void addUser(Request &request, StreamResponse &response) {

        response.setHeader("Content-Type", "application/json");

        auto username = request.get("username", "");
        auto password = request.get("password", "");
        if (username == "" || password == "") {
            response << "{\"success\": " << false << "}" << endl;
            return;
        }

        auto stmt = webs->connection->prepareStatement("INSERT INTO `users` (username, password, is_admin) VALUES (?, ?, 0)");
        stmt->setString(1, username);
        stmt->setString(2, password);
        stmt->execute();

        response << "{\"success\": " << true << "}" << endl;
    }

    void setUserPass(Request &request, StreamResponse &response) {
        response.setHeader("Content-Type", "application/json");

        auto userId = request.get("user_id", "");
        auto password = request.get("password", "");
        if (userId == "" || password == "") {
            response << "{\"success\": " << false << "}" << endl;
            return;
        }

        auto stmt = webs->connection->prepareStatement("UPDATE `users` SET `password`=? WHERE `user_id`=?");
        stmt->setString(1, password);
        stmt->setInt(2, atoi(userId.c_str()));
        stmt->execute();

        response << "{\"success\": " << true << "}" << endl;
    }

    void setup()
    {
        addRoute("POST", "/login", MainController, login);
        addRoute("GET", "/login", MainController, login);

        addRoute("GET", "/users", MainController, users);
        addRoute("GET", "/user/delete", MainController, deleteUser);
        addRoute("POST", "/user/add", MainController, addUser);
        addRoute("POST", "/user/setpass", MainController, setUserPass);

        addRoute("GET", "/scores", MainController, scores);
    }

    MainController(Webs * webs) {
        this->webs = webs;
    }
};


void Webs::Stop() {
    this->running = false;
}

/**
 * Starts the webserver
 *
 * Warning: is blocking
 */
void Webs::Start() {
    MainController mainController(this);
    Mongoose::Server server(8080);
    server.registerController(&mainController);
    server.start();

    cout << "Starting webserver on port 8080" << endl;

    while (this->running) {
        sleep(10);
    }
}

Webs::Webs() {
    this->running = true;

    try {
        this->driver = get_driver_instance();
        this->connection = driver->connect("tcp://yo.octetship.com:3306", "yobruh", "dolladollabillyall");
        this->connection->setSchema("healthy_running_game");

    } catch (sql::SQLException &e) {
        cout << "# ERR: SQLException in " << __FILE__;
        cout << "(" << __FUNCTION__ << ") on line " << __LINE__ << endl;
        cout << "# ERR: " << e.what();
        cout << " (MySQL error code: " << e.getErrorCode();
        cout << ", SQLState: " << e.getSQLState() << " )" << endl;
    }
}
