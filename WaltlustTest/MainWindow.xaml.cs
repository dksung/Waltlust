using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WaltlustTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var listView = sender as System.Windows.Controls.ListView;
            if (listView == null)
                return;

            if(listView.SelectedItem is string item)
            {
                System.Windows.Clipboard.SetText(item);
            }
        }
    }
}