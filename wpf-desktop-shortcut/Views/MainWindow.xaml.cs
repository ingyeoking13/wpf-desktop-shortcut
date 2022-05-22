using System.Windows;
using wpf_desktop_shortcut.ViewModels;

namespace wpf_desktop_shortcut.Views
{
    public partial class MainWindow : Window
    {
        MainViewModel vm = new MainViewModel(App.Repo);
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
