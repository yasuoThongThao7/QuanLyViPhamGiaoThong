using System.Windows.Controls;
using WpfApp2.ViewModel.MainWindowViewModel.ChildrentsMainViewModel;

namespace WpfApp2.View
{
    /// <summary>
    /// Interaction logic for ViolationSearch.xaml
    /// </summary>
    public partial class ViolationSearch : UserControl
    {
        public ViolationSearch()
        {
            InitializeComponent();
            this.DataContext = new ViolationSearchViewModel();
        }

    }
}
