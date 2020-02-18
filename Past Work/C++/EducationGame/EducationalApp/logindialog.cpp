#include "logindialog.h"
#include "ui_logindialog.h"
#include "network.h"
#include "qdebug.h"

extern bool isSpecialMenu;

LoginDialog::LoginDialog(std::function<void (void)> onSuccess, QWidget *parent) :
    QDialog(parent),    ui(new Ui::LoginDialog), onSuccess(onSuccess)
{
    ui->setupUi(this);
    ui->lbInvalidMess->hide();
    ui->logo->setStyleSheet("background-image: url(../resources/demo/logo_bgrm.png);");
    connect(ui->btnLogin, SIGNAL(clicked(bool)), this, SLOT(onLoginButtonClicked()));
}

LoginDialog::~LoginDialog()
{
    delete ui;
}

void LoginDialog::onLoginButtonClicked()
{
    QString username = ui->inputUsername->text();
    QString password = ui->inputPass->text();

    QRegularExpression namePattern("^[a-zA-z0-9]+$");
    QRegularExpression passPattern("^.{4,}$");

    //client-side validation. Normalization will be added if needed.
    if(!namePattern.match(username).hasMatch() || !passPattern.match(password).hasMatch())
    {
        ui->lbInvalidMess->show();
    }
    else
    {
        ui->lbInvalidMess->hide();
        //signal
        emit loginButtonClicked(username, password);
        // for now handle onSuccess here
        onSuccess();

        isSpecialMenu = false;

        //convert qstrings to strings because sfml and qt hate eachother
        std::string usernameClean = username.toUtf8().constData();
        std::string passwordClean = password.toUtf8().constData();

        // send user creds to server through the socket
        Network *network = new Network();
        network->connect();
        network->send("LOGIN " + usernameClean + " " + passwordClean);

        // wait for a response from the server
        network->socket->setBlocking(true);
        sf::Packet packet;
        sf::Socket::Status status = network->socket->receive(packet);
        // protect the game from being used without authentication
        if(status == sf::Socket::Done){
            std::string response;
            packet >> response;

            QString qResponse = QString::fromStdString(response);
            QStringList params = qResponse.split(QRegExp("\\s"));

            if(params.value(0) == "SUCCESS"){
                this->userId = params.value(1).toInt();
                this->close();
            }
            else if(params.value(0) == "FAILURE") {
                ui->lbInvalidMess->show();
            }
            network->socket->disconnect();
        }
        else {
            network->socket->disconnect();
            this->close();
        }
    }
}

