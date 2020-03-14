using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace comquiz
{
    public class AnswerSheet
    {
        private string _answerBody;
        public string AnswerBody
        {
            get
            {
                string textToReturn = _answerBody;

                if (Preferences != null)
                {
                    if (Preferences.RandomizeAnswersOrder) textToReturn = TrimAnswerListAnnotation();
                }               

                return textToReturn;
            }

            set
            {
                _answerBody = value;
            }
        }

        public bool IsTheRightAnswer { get; set; }

        public bool HasBeenSelected { get; set; }

        public QuizPreferences Preferences { get; set; }

        public AnswerSheet(string answer, bool isRight)
        {
            AnswerBody = answer;
            IsTheRightAnswer = isRight;
            HasBeenSelected = false;
        }

        public static void CheckTheseAnswers(List<AnswerSheet> answersToSelect)
        {
            foreach(AnswerSheet singleAnswer in answersToSelect)
            {
                singleAnswer.HasBeenSelected = true;
            }
        }

        private string TrimAnswerListAnnotation()
        {
            return Regex.Replace(this._answerBody, @"^([A-z]|[0-9]){1}(\)|\.)[ ]*", "");
        }
  
    }

}
