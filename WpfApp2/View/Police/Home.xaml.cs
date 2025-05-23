
using System.Windows.Controls;
using WpfApp2.ViewModel.MainWindowViewModel.ChildrentsMainViewModel;

namespace WpfApp2.View
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
            this.DataContext = new HomeViewModel();
        }
        // Hàm lazy load dữ liệu khi scroll 
        private void DataGridBienBan_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // Kiểm tra nếu scroll đã gần chạm cuối

            if (e.VerticalOffset + e.ViewportHeight >= e.ExtentHeight - 1)
            {
                var vm = DataContext as HomeViewModel;
                vm?.LoadNextPageAsync();
            }

        }

    }
}
