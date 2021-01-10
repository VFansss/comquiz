using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using comquiz.ViewModels;
using Avalonia.Media;

namespace comquiz.Views
{
    public enum FONT_OPERATION
    {
        Decrease,
        Increase
    }


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

            this.FindControl<TextBlock>("txt_fontsizeDown").Tapped += FontSizeDown_Tapped;

            this.FindControl<TextBlock>("txt_fontsizeUp").Tapped += FontSizeUp_Tapped;

        }

        private void FontSizeDown_Tapped(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            FontSizeAdjust(FONT_OPERATION.Decrease);
        }

        private void FontSizeUp_Tapped(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            FontSizeAdjust(FONT_OPERATION.Increase);
        }

        private void FontSizeAdjust(FONT_OPERATION _OPERATION)
        {
            if(_OPERATION == FONT_OPERATION.Increase)
            {
                Application.Current.Resources["FontSizeSmall"] = ((double)Application.Current.Resources["FontSizeSmall"]) + 2;
                Application.Current.Resources["FontSizeNormal"] = ((double)Application.Current.Resources["FontSizeNormal"]) + 2;
                Application.Current.Resources["FontSizeNormalPlus"] = ((double)Application.Current.Resources["FontSizeNormalPlus"]) + 2;
                Application.Current.Resources["FontSizeLarge"] = ((double)Application.Current.Resources["FontSizeLarge"]) + 2;
                Application.Current.Resources["FontSizeLarger"] = ((double)Application.Current.Resources["FontSizeLarger"]) + 2;
                Application.Current.Resources["FontSizeEnormous"] = ((double)Application.Current.Resources["FontSizeEnormous"]) + 2;
                Application.Current.Resources["FontSizeIntrusive"] = ((double)Application.Current.Resources["FontSizeIntrusive"]) + 2;
            }
            else
            {
                if (((double)Application.Current.Resources["FontSizeNormal"]) <= 8) return;

                Application.Current.Resources["FontSizeSmall"] = ((double)Application.Current.Resources["FontSizeSmall"]) - 2;
                Application.Current.Resources["FontSizeNormal"] = ((double)Application.Current.Resources["FontSizeNormal"]) - 2;
                Application.Current.Resources["FontSizeNormalPlus"] = ((double)Application.Current.Resources["FontSizeNormalPlus"]) - 2;
                Application.Current.Resources["FontSizeLarge"] = ((double)Application.Current.Resources["FontSizeLarge"]) - 2;
                Application.Current.Resources["FontSizeLarger"] = ((double)Application.Current.Resources["FontSizeLarger"]) - 2;
                Application.Current.Resources["FontSizeEnormous"] = ((double)Application.Current.Resources["FontSizeEnormous"]) - 2;
                Application.Current.Resources["FontSizeIntrusive"] = ((double)Application.Current.Resources["FontSizeIntrusive"]) - 2;
            }

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
