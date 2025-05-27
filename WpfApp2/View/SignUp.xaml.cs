using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp2.ViewModel.AccountAccess;

namespace WpfApp2.View
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        public static new bool IsLoaded = false;
        public SignUp()
        {
            InitializeComponent();
            DataContext = new SignUpViewModel();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel viewModel)
            {
                viewModel.Password = PasswordBox.Password;
            }
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel viewModel)
            {
                viewModel.NewPassword = NewPassword.Password;
            }
        }

        private void ConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel viewModel)
            {
                viewModel.ConfirmPassword = ConfirmPassword.Password;
            }
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextNumeric(e.Text);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.Tab || e.Key == Key.Left || e.Key == Key.Right)
            {
                e.Handled = false;
                return;
            }

            if (!(e.Key >= Key.D0 && e.Key <= Key.D9) &&
                !(e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                e.Handled = true;
            }
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextNumeric(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsTextNumeric(string text)
        {
            return text.All(char.IsDigit);
        }

    }
}
