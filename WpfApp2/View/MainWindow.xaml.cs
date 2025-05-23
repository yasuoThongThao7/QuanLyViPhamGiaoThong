using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using WpfApp2.ViewModel;
using WpfApp2.ViewModel.Account;
using WpfApp2.ViewModel.MainWindowViewModel;

namespace WpfApp2.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isSidebarVisible = true;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = App.ViewModel;
        }
       

        private void ToggleSidebar_Click(object sender, RoutedEventArgs e)
        {
            if (isSidebarVisible)
            {
                ((Storyboard)FindResource("CollapseSidebar")).Begin();
                isSidebarVisible = false;
            }
            else
            {

                ((Storyboard)FindResource("ExpandSidebar")).Begin();
                isSidebarVisible = true;
            }

        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
          Application.Current.Shutdown();
        }

    }
}