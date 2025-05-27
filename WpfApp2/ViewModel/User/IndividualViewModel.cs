using System;
using System.ComponentModel;
using WpfApp2.Model;
using WpfApp2.Service;

namespace WpfApp2.ViewModel.User
{
    public class IndividualViewModel : BaseViewModel
    {
        private string? _name;
        public string? Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string? _cccd;
        public string? CCCD
        {
            get => _cccd;
            set { _cccd = value; OnPropertyChanged(nameof(CCCD)); }
        }

        private DateTime _birth;
        public DateTime Birth
        {
            get => _birth;
            set { _birth = value; OnPropertyChanged(nameof(Birth)); }
        }

        private string? _address;
        public string? Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(nameof(Address)); }
        }

        private string? _phoneNumber;
        public string? PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(nameof(PhoneNumber)); }
        }

        private int _numberOfViolations;
        public int NumberOfViolations
        {
            get => _numberOfViolations;
            set { _numberOfViolations = value; OnPropertyChanged(nameof(NumberOfViolations)); }
        }

        private decimal _fine;
        public decimal Fine
        {
            get => _fine;
            set { _fine = value; OnPropertyChanged(nameof(Fine)); }
        }

        private string? _gender;
        public string? Gender
        {
            get => _gender;
            set { _gender = value; OnPropertyChanged(nameof(Gender)); }
        }

        public IndividualViewModel()
        {
            setProperty();
        }

        public void setProperty()
        {
            Person person = UserSession.Instance.Person;
            if (person == null)
            {
                return;
            }
            Name = person.Name;
            CCCD = person.CCCD;
            Birth = person.birth ?? DateTime.MinValue;
            Address = person.Address;
            PhoneNumber = person.PhoneNumber;
            NumberOfViolations = UserSession.Instance.Reports.Count;
            Fine = UserSession.Instance.Fine;
        }

    }
}
