#ifndef QUESTION_H
#define QUESTION_H

#include "qsfmlcanvas.h"

class Question
{
    QString text;
    QString answer;
    QStringList answers;

public:
    Question();


    QString getQuestion();
    QString getAnswer();

    //setters;
    void addAnswer(std::string);
    void setAnswer(std::string);
    void setQuestion(std::string);
    QStringList getAnswers();
};

#endif // QUESTION_H
