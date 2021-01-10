using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace comquiz.Views
{
    public class ControlsView : UserControl
    {
        public ControlsView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

        }

    }
}
