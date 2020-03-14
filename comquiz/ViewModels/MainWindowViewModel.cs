using Avalonia;
using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace comquiz.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        ViewModelBase _mainWindowContent;
        public ViewModelBase MainWindowContent
        {
            get => _mainWindowContent;
            private set => this.RaiseAndSetIfChanged(ref _mainWindowContent, value);
        }

        Boolean _isVisible = true;
        public Boolean IsVisible
        {
            get => _isVisible;
            set => this.RaiseAndSetIfChanged(ref _isVisible, value);
        }

        QuizSheet _currentQuiz = null;
        public QuizSheet CurrentQuiz
        {
            get => _currentQuiz;
            set => this.RaiseAndSetIfChanged(ref _currentQuiz, value);
        }

        QuizPreferences _chosenPreferences = new QuizPreferences();
        public QuizPreferences ChosenPreferences
        {
            get => _chosenPreferences;
            set => this.RaiseAndSetIfChanged(ref _chosenPreferences, value);
        }





        public MainWindowViewModel()
        {
            DisplayMainMenu();
        }

        public void DisplayMainMenu()
        {
            MainWindowContent = new MainMenuViewModel(this);
        }

        public void DisplayQuizScreen()
        {
            CurrentQuiz.Preferences = ChosenPreferences;

            CurrentQuiz.GeneratePersonalizedQuiz();

            MainWindowContent = new QuizScreenViewModel(this);
        }

    }
}
