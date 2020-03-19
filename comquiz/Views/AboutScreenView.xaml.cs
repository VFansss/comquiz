using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace comquiz.Views
{
    public class AboutScreenView : UserControl
    {
        public AboutScreenView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

        }

    }
}
