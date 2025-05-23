using System;
using WpfApp2.Model;

namespace WpfApp2.ViewModel.MainWindowViewModel.ChildrentsMainViewModel
{
    public class OfficerProfileViewModel : BaseViewModel
    {
        private string? _officerId;
        public string? OfficerId
        {
            get => _officerId;
            set => SetProperty(ref _officerId, value);
        }

        private string? _name;
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string? _position;
        public string? Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        private string? _rank;
        public string? Rank
        {
            get => _rank;
            set => SetProperty(ref _rank, value);
        }

        private string? _department;
        public string? Department
        {
            get => _department;
            set => SetProperty(ref _department, value);
        }

        private string? _phoneNumber;
        public string? PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        private int _numberOfCases;
        public int NumberOfCases
        {
            get => _numberOfCases;
            set => SetProperty(ref _numberOfCases, value);
        }

        private int _totalFineHandled;
        public int TotalFineHandled
        {
            get => _totalFineHandled;
            set => SetProperty(ref _totalFineHandled, value);
        }
        // Hiển thị dữ liệu nên form cái nhân
        private void setProperty()
        {
            Police userSession = UserSession.Instance.Police;
            OfficerId = userSession.MilitaryID;
            Name = userSession.Person!.Name;
            Position = userSession.Ranks;
            Rank = userSession.Ranks;
            Department = userSession.Unit;
            PhoneNumber = userSession.Person.PhoneNumber;
            NumberOfCases = userSession.Reports?.Count ?? 0;
            TotalFineHandled = userSession.Reports?.Select(r => r.TotalFine).Sum() ?? 0;

        }
        public OfficerProfileViewModel()
        {
            setProperty();
        }

    }
}
