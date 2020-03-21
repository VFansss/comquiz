using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using comquiz.ViewModels;

namespace comquiz.Views
{
    public class QuizScreenView : UserControl
    {
        public QuizScreenView()
        {
            this.InitializeComponent();

            ListBox ohi = this.FindControl<ListBox>("lst_answers");

            ohi.Tapped += Ohi_Tapped;

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Ohi_Tapped(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ListBox answersListbox = (ListBox)sender;

            QuizScreenViewModel dataContext = (QuizScreenViewModel)answersListbox.DataContext;

            int maxAllowedAnswers = dataContext.CurrentQuestion.NumberOfRightAnswers;
            int doneAnswers = answersListbox.SelectedItems.Count;

            dataContext.SelectedAnswers = answersListbox.SelectedItems;

            if (doneAnswers > maxAllowedAnswers)
            {
                answersListbox.SelectedItems.RemoveAt(0);
            }
        }

    }
}
