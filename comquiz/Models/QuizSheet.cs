﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static comquiz.QuestionSheet;

namespace comquiz
{
    public class QuizSheet
    {
        private string Title { get; set; }

        public bool IsQuizWellFormed { get; private set; }

        private List<QuestionSheet> OriginalQuestionsList { get; set; } = new List<QuestionSheet>();

        private List<QuestionSheet> PersonalizedQuestionsList { get; set; } = new List<QuestionSheet>();

        QuizPreferences _preferences;
        public QuizPreferences Preferences
        {
            get => _preferences;

            set
            {
                _preferences = value;

                foreach (QuestionSheet singleQuestion in OriginalQuestionsList)
                {
                    singleQuestion.Preferences = value;
                }
            }
        }

        public int CompletedQuestionsNumber
        {
            get
            {
                int count = 0;

                if (PersonalizedQuestionsList != null)
                {

                    foreach (QuestionSheet singleQuestion in PersonalizedQuestionsList)
                    {
                        if (singleQuestion.GetQuestionStatus() != ANSWERED.NotYet) count++;
                    }

                }

                return count;

            }

        }






        public QuizSheet(string pathToQuiz)
        {
            try
            {
                Title = Path.GetFileNameWithoutExtension(pathToQuiz);

                string fileContent = File.ReadAllText(pathToQuiz, Encoding.UTF8);

                if (!fileContent.StartsWith("|||||", StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new Exception(Properties.strings.quizSheet_parsingError_fileIsNotQuiz);
                }

                string[] questionsFound = fileContent.Split(new string[] { "|||||" }, StringSplitOptions.RemoveEmptyEntries);

                IsQuizWellFormed = false;

                // Fill the questions

                OriginalQuestionsList = new List<QuestionSheet>();

                foreach (string singleQuestion in questionsFound)
                {
                    QuestionSheet newQuestion = new QuestionSheet();

                    if (string.IsNullOrWhiteSpace(singleQuestion))
                    {
                        throw new Exception(Properties.strings.quizSheet_parsingError_badOrInvalidQuestion_1);
                    }

                    string[] tokens = Regex.Split(singleQuestion.Trim(), @"(?=\|\||\|-|\|\+)");


                    if (tokens.Length <= 2)
                    {
                        throw new Exception(Properties.strings.quizSheet_parsingError_badOrInvalidQuestion_2);
                    }

                    // A question, and least 2 answers 

                    foreach (string singleToken in tokens)
                    {

                        string token = singleToken.Trim();

                        if (String.IsNullOrEmpty(token))
                        {
                            // Empty
                            continue;
                        }

                        else if (singleToken.Substring(0, 2).Equals("||", StringComparison.InvariantCultureIgnoreCase))
                        {
                            // Is the question body
                            newQuestion.QuestionBody = singleToken[2..].Trim();
                        }

                        else if (singleToken.Substring(0, 2).Equals("|+", StringComparison.InvariantCultureIgnoreCase))
                        {
                            // Is a right answer

                            newQuestion.OriginalAnswersList.Add(new AnswerSheet(singleToken[2..].Trim(), true));
                        }

                        else if (singleToken.Substring(0, 2).Equals("|-", StringComparison.InvariantCultureIgnoreCase))
                        {
                            // Is a wrong answer
                            newQuestion.OriginalAnswersList.Add(new AnswerSheet(singleToken[2..].Trim(), false));
                        }

                        else
                        {
                            // Strange token
                            throw new Exception("Unrecognized token:\n\nIncriminated row: " + token);
                        }

                    } // End foreach token

                    OriginalQuestionsList.Add(newQuestion);

                } // End foreach question

                IsQuizWellFormed = true;

            }

            catch
            {
                throw;
            }

        }
        // End constructor

        public static List<QuestionSheet> SplitTheQuiz(List<QuestionSheet> originalList, QUIZPART quizQuarter, QUIZPARTIAL quizPart)
        {
            if (originalList == null) return null;

            int splitEvery;
            int equalPieces;
            bool wantTheLastPiece = false;

            if (quizQuarter == QUIZPART.Half)
            {
                equalPieces = 2;

                if (quizPart == QUIZPARTIAL.Second)
                {
                    wantTheLastPiece = true;
                }

            }
            else if (quizQuarter == QUIZPART.Third)
            {
                equalPieces = 3;

                if (quizPart == QUIZPARTIAL.Third)
                {
                    wantTheLastPiece = true;
                }

            }
            else
            {
                // Sixth
                equalPieces = 6;

                if (quizPart == QUIZPARTIAL.Sixth)
                {
                    wantTheLastPiece = true;
                }

            }

            // Split original quiz in in 2/3/6 equal pieces
            splitEvery = originalList.Count / equalPieces;

            if (splitEvery > 0)
            {
                // Mean that total questions are enough to be splitted

                List<List<QuestionSheet>> splittedLists = SplitList(originalList, splitEvery);

                // If I want the last part of a quiz, and the last piece is longer than an equal piece
                // merge the remaining list

                List<QuestionSheet> listToReturn = new List<QuestionSheet>();

                if (splittedLists.Count > equalPieces && wantTheLastPiece)
                {
                    // Merge the last 2 lists

                    listToReturn.AddRange(splittedLists[splittedLists.Count - 2]);
                    listToReturn.AddRange(splittedLists[splittedLists.Count - 1]);

                }
                else
                {
                    listToReturn = splittedLists[(int)quizPart - 1];
                }

                return listToReturn;

            }
            else
            {
                return originalList;
            }


        }

        private static List<List<QuestionSheet>> SplitList(List<QuestionSheet> locations, int nSize = 30)
        {
            var list = new List<List<QuestionSheet>>();

            for (int i = 0; i < locations.Count; i += nSize)
            {
                list.Add(locations.GetRange(i, Math.Min(nSize, locations.Count - i)));
            }

            return list;
        }

        public QuestionSheet GetUnansweredQuestion()
        {
            QuestionSheet nextQuestion = null;

            foreach (QuestionSheet singleQuestion in PersonalizedQuestionsList)
            {
                if (singleQuestion.GetQuestionStatus() == ANSWERED.NotYet)
                {
                    return singleQuestion;
                }
            }

            return nextQuestion;
        }

        public QuizStats GetQuizStats()
        {
            short rightQuestions = 0;

            foreach (QuestionSheet singleQuestion in PersonalizedQuestionsList)
            {
                if (singleQuestion.GetQuestionStatus() == ANSWERED.Correctly)
                {
                    rightQuestions++;
                }
            }

            short totalQuestions = (short)PersonalizedQuestionsList.Count;

            short percentComplete = (short)Math.Round((double)(100 * rightQuestions) / totalQuestions);

            return new QuizStats(rightQuestions, totalQuestions, percentComplete);

        }

        /*public void HalveTheQuiz()
        {
            List<QuestionSheet> halvedQuiz = new List<QuestionSheet>();

            int newQuizQuestionNumber = QuestionsNumber / 2;

            for (int i = 0; i< newQuizQuestionNumber; i++)
            {
                halvedQuiz.Add(PersonalizedQuestionsList[i]);
            }

            // The end!

            PersonalizedQuestionsList = halvedQuiz;

        }*/

        public void ResetQuizAnswers()
        {
            foreach (QuestionSheet singleQuestion in OriginalQuestionsList)
            {
                foreach (AnswerSheet singleAnswer in singleQuestion.OriginalAnswersList)
                {
                    singleAnswer.HasBeenSelected = false;
                }
            }
        }

        public void GeneratePersonalizedQuiz()
        {
            List<QuestionSheet> generatedQuestionList = new List<QuestionSheet>(OriginalQuestionsList);

            if (Preferences != null)
            {
                if (Preferences.QuizPart != QUIZPART.Entire)
                {
                    generatedQuestionList = SplitTheQuiz(generatedQuestionList, Preferences.QuizPart, Preferences.QuizPartial);
                }

                if (Preferences.RandomizeQuestionsOrder)
                {
                    generatedQuestionList.Shuffle<QuestionSheet>().ToList<QuestionSheet>();
                }

            }

            PersonalizedQuestionsList = generatedQuestionList;
        }

    }

    public class QuizStats
    {
        public short RightQuestions { get; private set; }
        public short TotalQuestions { get; private set; }
        public short CompletePercentage { get; private set; }

        public QuizStats(short rightQuestions, short totalQuestions, short completePercentage)
        {
            RightQuestions = rightQuestions;
            TotalQuestions = totalQuestions;
            CompletePercentage = completePercentage;
        }

    }

}