#include "question.h"

Question::Question()
{

}

QString Question::getQuestion()
{
    return text;
}

void Question::setQuestion(std::string question)
{
    this->text = QString::fromStdString(question);
}

void Question::setAnswer(std::string a)
{
    answer = QString::fromStdString(a);
}

QString Question::getAnswer()
{
    return this->answer;
}

QStringList Question::getAnswers()
{
    return this->answers;
}

void Question::addAnswer(std::string s)
{
    answers.append(QString::fromStdString(s));
}

