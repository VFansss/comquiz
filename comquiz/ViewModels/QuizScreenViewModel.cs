using Avalonia.Collections;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using static comquiz.QuestionSheet;

namespace comquiz.ViewModels
{
    class QuizScreenViewModel : ViewModelBase
    {
        readonly MainWindowViewModel MainDatacontext;

        QuizSheet _currentQuiz;
        public QuizSheet CurrentQuiz
        {
            get => _currentQuiz;
            set => this.RaiseAndSetIfChanged(ref _currentQuiz, value);
        }

        QuestionSheet _currentQuestion;
        public QuestionSheet CurrentQuestion
        {
            get => _currentQuestion;
            set => this.RaiseAndSetIfChanged(ref _currentQuestion, value);
        }

        short _currentQuestionNumber;
        public short CurrentQuestionNumber
        {
            get => _currentQuestionNumber;
            set => this.RaiseAndSetIfChanged(ref _currentQuestionNumber, value);
        }

        bool _answeringEnabled = true;
        public bool AnsweringEnabled
        {
            get => _answeringEnabled;
            set => this.RaiseAndSetIfChanged(ref _answeringEnabled, value);
        }

        IList _selectedAnswers;
        public IList SelectedAnswers
        {
            get => _selectedAnswers;
            set => this.RaiseAndSetIfChanged(ref _selectedAnswers, value);
        }

        bool _spoilRightAnswer = false;
        public bool SpoilRightAnswer
        {
            get => _spoilRightAnswer;
            set => this.RaiseAndSetIfChanged(ref _spoilRightAnswer, value);
        }





        public QuizScreenViewModel(MainWindowViewModel parentDataContext)
        {
            MainDatacontext = parentDataContext;

            CurrentQuiz = MainDatacontext.CurrentQuiz;

            CurrentQuiz.ResetQuizAnswers(); // Useful if quiz has been already taken previously

            CurrentQuestion = CurrentQuiz.GetUnansweredQuestion();

            CurrentQuestionNumber = 1;

        }

        // TOP BUTTONS!

        public void TerminateQuiz()
        {

            if (CurrentQuestionNumber != CurrentQuiz.CompletedQuestionsNumber)
            {
                // Quiz terminated early

                DisplayQuizResult();

            }

            MainDatacontext.DisplayMainMenu();

        }

        // BOTTOM BUTTONS!

        public void ShowAnswers()
        {
            SpoilRightAnswer = true;
        }

        public void NextQuestion()
        {
            MarkChoosedAnswers();

            ANSWERED questionStatus = CurrentQuestion.GetQuestionStatus();

            if (questionStatus == ANSWERED.NotYet)
            {
                ShowNotEnoughAnswersAlert();
            }
            else
            {
                SpoilRightAnswer = false;

                QuestionSheet nextQuestion = CurrentQuiz.GetUnansweredQuestion();

                if(nextQuestion is null)
                {
                    // QUIZ FINISHED!

                    DisplayQuizResult();

                }
                else
                {
                    CurrentQuestion = nextQuestion;
                    SelectedAnswers.Clear();

                    CurrentQuestionNumber++;
                }
              
            }
 
        }

        // AIDING METHODS

        public void MarkChoosedAnswers()
        {
            List<AnswerSheet> choosedAnswers = new List<AnswerSheet>();

            if (SelectedAnswers != null)
            {
                foreach (AnswerSheet singleAnswer in SelectedAnswers)
                {
                    choosedAnswers.Add(singleAnswer);
                }

                AnswerSheet.CheckTheseAnswers(choosedAnswers);

            }
            
        }

        public void DisplayQuizResult()
        {
            QuizStats myStats = CurrentQuiz.GetQuizStats();

            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
            "( ⚆ _ ⚆ )",
            "Quiz completato. Risultato:" + "\n\n" +
            String.Format("Domande Esatte : {0}, su un totale di {1} ({2}%)", myStats.RightQuestions, myStats.TotalQuestions, myStats.CompletePercentage),
            MessageBox.Avalonia.Enums.ButtonEnum.Ok,
            MessageBox.Avalonia.Enums.Icon.Info);

            messageBoxStandardWindow.Show();

            AnsweringEnabled = false;
        }

        public void ShowNotEnoughAnswersAlert()
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                "( ⚆ _ ⚆ )",
                "Hai selezionato meno risposte di quanto dovresti. Mettici una pezza.",
                MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                MessageBox.Avalonia.Enums.Icon.Info);

            messageBoxStandardWindow.Show();
        }


    }



}
