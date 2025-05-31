using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;

namespace WpfApp2.ViewModel
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        private string? _oldPassword;
        private string? _newPassword;
        private string? _confirmPassword;

        public ICommand BackCommand { get; }
        public string? OldPassword
        {
            get => _oldPassword;
            set => SetProperty(ref _oldPassword, value);
        }

        public string? NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public string? ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public ICommand ChangePasswordCommand { get; }

        private void ValidatePasswordChange()
        {
            if (OldPassword == null || NewPassword == null || ConfirmPassword == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (NewPassword != ConfirmPassword)
            {
                MessageBox.Show("Mật khẩu mới và xác nhận mật khẩu không khớp!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (NewPassword.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (OldPassword == NewPassword)
            {
                MessageBox.Show("Mật khẩu mới không thể giống mật khẩu cũ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (OldPassword != UserSession.Instance.Account.Password)
            {
                MessageBox.Show("Mật khẩu cũ không đúng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }
        private void ChangePasswordAsync()
        {
            ValidatePasswordChange();
            var accountService = new Service.AccountService();
            var result = accountService.UpdatePasswordAsync(UserSession.Instance.Account.LoginId, NewPassword);
            MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            Back();
        }

        private void Back()
        {
            App.ViewModel.CurrentView = new Account.AccountInformationViewModel();
        }
        public ChangePasswordViewModel()
        {
            ICommand backCommand = new RelayCommand(_ => Back());
            ChangePasswordCommand = new RelayCommand(_ => ChangePasswordAsync());
        }
    }
}
