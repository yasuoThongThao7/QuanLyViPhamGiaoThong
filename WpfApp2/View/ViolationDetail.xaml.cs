using System;
using System.Windows.Controls;
using WpfApp2.ViewModel.MainWindowViewModel.ChildrentsMainViewModel;

namespace WpfApp2.View
{
    /// <summary>
    /// Interaction logic for Chitiet.xaml
    /// </summary>
    public partial class ViolationDetail : UserControl
    {
        public ViolationDetail()
        {
            InitializeComponent();
            DataContext = new ViolationDetailViewModel();
        }
    }
}
