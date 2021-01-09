using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using comquiz.ViewModels;
using System.Collections.Generic;

namespace comquiz.Views
{
    public class QuizScreenView : UserControl
    {
        public QuizScreenView()
        {
            this.InitializeComponent();

            ListBox lst_answers = this.FindControl<ListBox>("lst_answers");

            lst_answers.Tapped += Lst_answers_Tapped;

        }

        protected override void OnAttachedToVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
        {
            UserControl quizScreenUserControl = this.FindControl<UserControl>("quizScreenUserControl");

            quizScreenUserControl.KeyUp += QuizScreenUserControl_KeyUp;

            quizScreenUserControl.Focus();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void QuizScreenUserControl_KeyUp(object sender, Avalonia.Input.KeyEventArgs e)
        {
            e.Route = Avalonia.Interactivity.RoutingStrategies.Tunnel;

            UserControl quizUserControl = (UserControl)sender;

            QuizScreenViewModel dataContext = (QuizScreenViewModel)quizUserControl.DataContext;

            Avalonia.Input.Key pressedKey = e.Key;

            int pressedKeyIntCode = (int)pressedKey;

            if (pressedKey == Avalonia.Input.Key.Space || pressedKey == Avalonia.Input.Key.NumPad0)
            {
                dataContext.ShowAnswers();

            }
            else if (pressedKey == Avalonia.Input.Key.Right || pressedKey == Avalonia.Input.Key.Enter)
            {
                dataContext.NextQuestion();

            }
            else if (pressedKey == Avalonia.Input.Key.Escape)
            {
                dataContext.TerminateQuiz();
            }
            else
            {
                // Select answer based on what number I pressed on the keyboard

                // CHECK: Is the quiz ended?
                if (dataContext.CurrentQuiz.GetUnansweredQuestion() is null) return;

                // 35 - "1" on keyboard, 43 - "9" on keyboard
                bool keyboardNumberPressed = (pressedKeyIntCode >= 35 && pressedKeyIntCode <= 43);
                // 75 - "1" on numeric keypad, 83 - "9" on numeric keypad
                bool numpadNumberPressed = (pressedKeyIntCode >= 75 && pressedKeyIntCode <= 83);

                if(keyboardNumberPressed || numpadNumberPressed)
                {
                    // 
                    int choosedAnswerNumber = keyboardNumberPressed ? pressedKeyIntCode - 34 : pressedKeyIntCode - 74;

                    ListBox answersListbox = this.FindControl<ListBox>("lst_answers");
                    List<AnswerSheet> answerList = (List<AnswerSheet>)answersListbox.Items;

                    if (!(choosedAnswerNumber > answerList.Count))
                    {
                        int choosedAnswerListIndex = choosedAnswerNumber - 1;

                        AnswerSheet choosedAnswerSheet = answerList[choosedAnswerListIndex];

                        if(answersListbox.SelectedItems.Contains(choosedAnswerSheet)){

                            answersListbox.SelectedItems.Remove(choosedAnswerSheet);

                        }
                        else
                        {
                            answersListbox.SelectedItems.Add(choosedAnswerSheet);

                            UncheckOverreplies();
                        }

                        dataContext.SelectedAnswers = answersListbox.SelectedItems;

                    }

                }

            }

            // Got focus again on the whole UserControl

            UserControl quizScreenUserControl = this.FindControl<UserControl>("quizScreenUserControl");

            quizScreenUserControl.Focus();

            e.Handled = true;

        }

        private void Lst_answers_Tapped(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            UncheckOverreplies();
        }

        /*
         * Aiding methods
         * 
         */

        private void UncheckOverreplies()
        {
            ListBox answersListbox = this.FindControl<ListBox>("lst_answers");

            QuizScreenViewModel dataContext = (QuizScreenViewModel)answersListbox.DataContext;

            int maxAllowedAnswers = dataContext.CurrentQuestion.NumberOfRightAnswers;
            int selectedAnswers = answersListbox.SelectedItems.Count;

            dataContext.SelectedAnswers = answersListbox.SelectedItems;

            if (selectedAnswers > maxAllowedAnswers)
            {
                answersListbox.SelectedItems.RemoveAt(0);
            }
        }
    }
}
