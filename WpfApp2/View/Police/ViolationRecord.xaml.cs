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
using MaterialDesignThemes.Wpf;
using WpfApp2.ViewModel.MainWindowViewModel.ChildrentsMainViewModel;

namespace WpfApp2.View
{
    public partial class ViolationRecord : UserControl
    {

        public ViolationRecord()
        {
            InitializeComponent();
            //VehicleDialog.Children.Add(new ReportSearch());
            this.DataContext = new ViolationRecordViewModel();

        }
        // Ngăn việc dán chữ
        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!text.All(char.IsDigit))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
        // Ngăn việc viết chữ
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextNumeric(e.Text);
        }
        //Kiểm tra textbox có phải là chữ số hay không
        private bool IsTextNumeric(string text)
        {
            return text.All(char.IsDigit);
        }

    }
}
