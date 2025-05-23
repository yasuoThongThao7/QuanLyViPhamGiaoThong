using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.ViewModel.Account
{
    class AccountInformationViewModel:BaseViewModel
    {
        private string? _name;
        private string? _rank;
        private string? _unit;
        private string? _militaryId;
        private string? _address;
        private string? _phoneNumber;

        public string Name
        {
            get => _name!;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Rank
        {
            get => _rank!;
            set { _rank = value; OnPropertyChanged(); }
        }

        public string Unit
        {
            get => _unit!;
            set { _unit = value; OnPropertyChanged(); }
        }

        public string MilitaryId
        {
            get => _militaryId!;
            set { _militaryId = value; OnPropertyChanged(); }
        }

        public string Address
        {
            get => _address!;
            set { _address = value; OnPropertyChanged(); }
        }

        public string PhoneNumber
        {
            get => _phoneNumber!;
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        public AccountInformationViewModel() 
        {
                        
        }
    }
}
