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
using WpfApp2.ViewModel.User;

namespace WpfApp2.View.Citizen
{
    /// <summary>
    /// Interaction logic for Individual.xaml
    /// </summary>
    public partial class Individual : UserControl
    {
        public Individual()
        {
            InitializeComponent();
            this.DataContext = App.IndividualVmd;
        }
    }
}
