using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using comquiz.ViewModels;

namespace comquiz.Views
{
    public class MainMenuView : UserControl
    {
        public MainMenuView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            TextBlock ohi = this.FindControl<TextBlock>("txt_about");

            ohi.Tapped += Ohi_Tapped;

        }

        private void Ohi_Tapped(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

            TextBlock tappedTextBox = (TextBlock)sender;

            MainMenuViewModel dataContext = (MainMenuViewModel)tappedTextBox.DataContext;

            dataContext.MainDatacontext.DisplayAboutScreen();

        }
    }
}
