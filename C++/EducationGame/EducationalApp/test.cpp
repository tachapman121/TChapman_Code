#include "test.h"
#include <fstream>
#include <QDir>
#include <QDebug>

Test::Test()
{
    //loadFile(fileName);//TODO: Load quiz file.


    //pracice Question
    Question testQ; //we will want to load in from file probably
    testQ.setQuestion("Pick the healthy option: ");

    testQ.addAnswer("Pizza");
    testQ.addAnswer("Broccoli");
    testQ.addAnswer("Ice Cream");
    testQ.addAnswer("None of the above");
    testQ.setAnswer("Broccoli");

    test.append(testQ);
    loadFile();
}

void Test::loadFile()
{
    QDir directory;
    directory.cdUp();
    std::string line;
    std::ifstream myfile (std::string(directory.path().toUtf8().constData()) + "/quizzes.txt");
    if (myfile.is_open())
    {
      Question* newQ = new Question();
      while ( getline (myfile,line) )
      {
          if(line.substr(0,3) == ":Q:")
          {
              newQ->setQuestion(line.substr(3,line.length()));
              //qDebug() << newQ.getQuestion();
          } else
          if(line.substr(0,3) == ":A:")
          {
              newQ->addAnswer(line.substr(3,line.length()));
              newQ->setAnswer(line.substr(3,line.length()));
              //qDebug() << "Right Here";
              //qDebug() << newQ->getAnswer();
          } else
          if(line.substr(0,3) == ":F:")
          {
              newQ->addAnswer(line.substr(3,line.length()));
          }
          if(line == ":end:")
          {
              test.append(*newQ);
              newQ = new Question();
          }
      }

      myfile.close();
    }

    else qDebug() << "Unable to open file" << QString::fromStdString(std::string(directory.path().toUtf8().constData()) + "/quizzes.txt");
}

Question *Test::getQuestion()
{
    srand (time(NULL)); //seed random with time
    int index = rand() % test.count();
    return &test[index];
}
