using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace comquiz.ViewModels
{
    class MainMenuViewModel : ViewModelBase
    {
        readonly public MainWindowViewModel MainDatacontext;

        public string _version = "v200321";
        public string Version
        {
            get => _version;
        }


        public MainMenuViewModel(MainWindowViewModel parentDataContext)
        {
            MainDatacontext = parentDataContext;

        }

        public async void PickQuizFromFile(Window parameter)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                AllowMultiple = false,
                Title = Properties.strings.mainMenu_chooseFileDialogTitle,
                Filters = new List<FileDialogFilter> { new FileDialogFilter() { Name = "Text", Extensions = { "txt" } } }
            };

            string[] file = await dialog.ShowAsync(parameter);

            if (file.Length > 0)
            {
                string messageError = null;

                try
                {
                    QuizSheet selectedQuiz = new QuizSheet(file.GetValue(0).ToString(), QUIZPART.Entire, QUIZPARTIAL.First);

                    MainDatacontext.IsVisible = !MainDatacontext.IsVisible;

                    MainDatacontext.CurrentQuiz = selectedQuiz;

                }
                catch (Exception ex)
                {
                    MessageBox.Avalonia.BaseWindows.MsBoxStandardWindow messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                    "( ⚆ _ ⚆ )",
                    Properties.strings.mainMenu_quizParsingErrorString + ":\n\n"+ ex.Message,
                    MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    MessageBox.Avalonia.Enums.Icon.Error);

                    _ = messageBoxStandardWindow.Show();
                }
                
                
            }           

        }

    }
}
