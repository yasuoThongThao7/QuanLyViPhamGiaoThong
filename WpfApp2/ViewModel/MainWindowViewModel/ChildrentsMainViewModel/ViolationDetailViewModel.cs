using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp2.Model;
using WpfApp2.Service;
using WpfApp2.View;
using WpfApp2.View.Citizen;

namespace WpfApp2.ViewModel.MainWindowViewModel.ChildrentsMainViewModel
{

    public class ViolationDetailViewModel : BaseViewModel
    {
        private Person _person = new Person();
        public Person Person
        {
            get => _person;
            set => SetProperty(ref _person, value);
        }

        private string _vehicleId = string.Empty;
        public string VehicleId
        {
            get => _vehicleId;
            set => SetProperty(ref _vehicleId, value);
        }

        private Vehicle _vehicle = new Vehicle();
        public Vehicle Vehicle
        {
            get => _vehicle;
            set => SetProperty(ref _vehicle, value);
        }

        private Police _police = new Police();
        public Police Police
        {
            get => _police;
            set => SetProperty(ref _police, value);
        }

        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private string _location = string.Empty;
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        private decimal _totalFine;
        public decimal TotalFine
        {
            get => _totalFine;
            set => SetProperty(ref _totalFine, value);
        }

        private bool _isPaid;
        public bool IsPaid
        {
            get => _isPaid;
            set => SetProperty(ref _isPaid, value);
        }

        private ObservableCollection<Violation> _violations = new ObservableCollection<Violation>();
        public ObservableCollection<Violation> Violations
        {
            get => _violations;
            set => SetProperty(ref _violations, value);
        }

        private void LoadReportDetails(Report report)
        {
            Person = report.Person ?? new Person();
            VehicleId = report.VehicleId ?? string.Empty;
            Vehicle = report.Vehicle ?? new Vehicle();
            Police = report.Police ?? new Police();
            Date = report.Date;
            Location = report.Location ?? string.Empty;
            TotalFine = report.TotalFine;
            IsPaid = report.IsPaid;

            if (report.Violations != null)
            {
                Violations = new ObservableCollection<Violation>(
                    report.Violations
                        .Where(v => v.Violation != null)
                        .Select(v => v.Violation!)
                );
            }
            else
            {
                Violations = new ObservableCollection<Violation>();
            }
        }
        public ICommand BackCommand { get; }
 


        private async Task LoadViolationDetails(int reportId)
        {
            var reportService = new ReportService();
            var report = await reportService.GetReportByIdAsync(reportId);
            if (report != null)
            {
                LoadReportDetails(report);
            }
        }
        public void Back()
        {
            if (UserSession.Instance.Account.Role.Equals("Police"))
                App.ViewModel!.CurrentView = new ViolationSearch();
            else
                App.ViewModel!.CurrentView = new RecordLookup();
        }
        public ViolationDetailViewModel(int reportId)
        {
            _ = LoadViolationDetails(reportId);
            BackCommand = new RelayCommand(_ => Back());
        }

    }
}
