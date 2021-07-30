using System.Windows;
using System.Windows.Controls;

namespace GCodePlayer3D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private void GCodeItems_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var lb = ((ListBox)sender);
            lb.ScrollIntoView(lb.SelectedItem);
        }
    }
}
