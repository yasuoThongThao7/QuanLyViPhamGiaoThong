using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.ViewModel.MainWindowViewModel.ChildrentsMainViewModel;

namespace WpfApp2.View.Police
{
    /// <summary>
    /// Interaction logic for OfficerProfile.xaml
    /// </summary>
    public partial class OfficerProfile : UserControl
    {
        public OfficerProfile()
        {
            InitializeComponent();
            this.DataContext = new OfficerProfileViewModel();
        }
    }
}
