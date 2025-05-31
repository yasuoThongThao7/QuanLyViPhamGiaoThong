using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp2.ViewModel.Account
{
    class AccountInformationViewModel:BaseViewModel
    {
        public ICommand ChangePasswordCommand { get; set; } 
        private string? _name;
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public void ChangePassword()
        {
            App.ViewModel.CurrentView = new View.ChangePassword();
        }
        public AccountInformationViewModel() 
        {
            Name = UserSession.Instance.Person.Name;
            ChangePasswordCommand = new RelayCommand(_ => ChangePassword());
        }
    }
}
