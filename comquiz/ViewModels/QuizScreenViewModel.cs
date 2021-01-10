using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
            set
            {
                this.RaiseAndSetIfChanged(ref _currentQuestion, value);

                WarningForNotEnoughAnswer = false;

            }
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

        bool _warningForNotEnoughAnswer = false;
        public bool WarningForNotEnoughAnswer
        {
            get => _warningForNotEnoughAnswer;
            set => this.RaiseAndSetIfChanged(ref _warningForNotEnoughAnswer, value);
        }

        bool QuizIsOver = false;

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

            if (CurrentQuestion.NumberOfRightAnswers == SelectedAnswers?.Count)
            {
                MarkChoosedAnswers();
            }

            ANSWERED questionStatus = CurrentQuestion.GetQuestionStatus();

            if (questionStatus == ANSWERED.NotYet)
            {
                ShowNotEnoughAnswersAlert();
            }
            else
            {
                SpoilRightAnswer = false;

                QuestionSheet nextQuestion = CurrentQuiz.GetUnansweredQuestion();

                if (nextQuestion is null)
                {
                    // QUIZ FINISHED!

                    if(!QuizIsOver) DisplayQuizResult();

                    QuizIsOver = true;

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

            CultureInfo culture = CultureInfo.CurrentCulture;

            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
            "( ⚆ _ ⚆ )",

            String.Format(
                culture, "{0}:\n\n{1} : {2}, {3} {4} ({5}%)",
                Properties.strings.quizScreen_quizCompleted_1,
                Properties.strings.quizScreen_quizCompleted_2,
                myStats.RightQuestions,
                Properties.strings.quizScreen_quizCompleted_3,
                myStats.TotalQuestions,
                myStats.CompletePercentage),

            MessageBox.Avalonia.Enums.ButtonEnum.Ok,
            MessageBox.Avalonia.Enums.Icon.Info);

            messageBoxStandardWindow.Show();

            AnsweringEnabled = false;
        }

        public void ShowNotEnoughAnswersAlert()
        {
            WarningForNotEnoughAnswer = true;
        }


    }



}
