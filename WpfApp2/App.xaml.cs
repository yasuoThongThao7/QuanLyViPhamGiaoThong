
using System.Windows;
using WpfApp2.View;
using WpfApp2.ViewModel;
using WpfApp2.ViewModel.MainWindowViewModel.ChildrentsMainViewModel;
using WpfApp2.ViewModel.User;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    
        public static IndividualViewModel? IndividualVmd { get; set; }
        public static MainViewModel? ViewModel { get; set; }
        private void Application_Startup(Object sender, StartupEventArgs e)
        {

            InitializeViewModels();
            var mainWindow = new MainWindow();
            mainWindow.Hide();
            var loginWindow = new SignUp();
            bool? result = loginWindow.ShowDialog();
            if (result == true)
            {   
                ViewModel!.InterfaceSetup();
                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }
        // Hàm khởi tạo các ViewModel
        private void InitializeViewModels()
        {
          
            ViewModel = new MainViewModel();
            IndividualVmd = new IndividualViewModel();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Lỗi: {e.Exception.Message}\n\nChi tiết: {e.Exception.StackTrace}",
                            "Ứng dụng bị lỗi",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);

            // Đánh dấu lỗi này đã được xử lý để ứng dụng không bị crash.
            e.Handled = true;
        }
    }

}
