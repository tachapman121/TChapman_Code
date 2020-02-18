#ifndef LOGINDIALOG_H
#define LOGINDIALOG_H

#include <QDialog>
#include <QString>
#include <QRegularExpression>
#include <functional>

namespace Ui {
class LoginDialog;
}

class LoginDialog : public QDialog
{
    Q_OBJECT

public:
    explicit LoginDialog(std::function<void (void)> onSuccess, QWidget *parent = 0);
    ~LoginDialog();
    int userId;
signals:
    void loginButtonClicked(QString username, QString password);
private slots:
    void onLoginButtonClicked();
private:
    Ui::LoginDialog *ui;
    std::function<void (void)> onSuccess;

};

#endif // LOGINDIALOG_H
