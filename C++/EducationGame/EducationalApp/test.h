#ifndef TEST_H
#define TEST_H

#include "question.h"
#include <QVector>

class Test
{
    QVector<Question> test;

public:
    Test();
    void loadFile();
    Question* getQuestion();
};

#endif // TEST_H
