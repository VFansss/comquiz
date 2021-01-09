using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using comquiz.ViewModels;
using Avalonia.Media;

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

            this.FindControl<TextBlock>("txt_about").Tapped += About_Tapped;

            this.FindControl<TextBlock>("txt_font").Tapped += Font_Tapped;

            this.FindControl<TextBlock>("txt_controls").Tapped += Controls_Tapped;

        }

        private void Font_Tapped(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

            FontFamily currentFont = (FontFamily)Application.Current.Resources["UIFont"];

            if (currentFont.Name.Contains("Roboto",System.StringComparison.InvariantCultureIgnoreCase))
            {
                // Set OpenDyslexic

                var newFont = Application.Current.Resources["Font_OpenDyslexic"];

                Application.Current.Resources["UIFont"] = newFont;

            }
            else
            {
                // Set Roboto

                var newFont = Application.Current.Resources["Font_Roboto"];

                Application.Current.Resources["UIFont"] = newFont;

            }            

            // Rebound main menu to force render of new font

            TextBlock tappedTextBox = (TextBlock)sender;

            MainMenuViewModel dataContext = (MainMenuViewModel)tappedTextBox.DataContext;

            dataContext.MainDatacontext.DisplayMainMenu();

        }

        private void About_Tapped(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            TextBlock tappedTextBox = (TextBlock)sender;

            MainMenuViewModel dataContext = (MainMenuViewModel)tappedTextBox.DataContext;

            dataContext.MainDatacontext.DisplayAboutScreen();

        }

        private void Controls_Tapped(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            TextBlock tappedTextBox = (TextBlock)sender;

            MainMenuViewModel dataContext = (MainMenuViewModel)tappedTextBox.DataContext;

            dataContext.MainDatacontext.DisplayControlsScreen();
        }

    }
}
