using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace comquiz.ViewModels
{
    class MainMenuViewModel : ViewModelBase
    {
        readonly MainWindowViewModel MainDatacontext;






        public MainMenuViewModel(MainWindowViewModel parentDataContext)
        {
            MainDatacontext = parentDataContext;
        }

        public async void PickQuizFromFile(Window parameter)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                AllowMultiple = false,
                Title = "Seleziona un file di QUIZ",
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
                    "Errore durante la lettura del quiz:"+"\n\n"+ ex.Message,
                    MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    MessageBox.Avalonia.Enums.Icon.Error);

                    _ = messageBoxStandardWindow.Show();
                }
                
                
            }           

        }

    }
}
