using System;
using System.Collections.ObjectModel;
using WpfApp2.Model;
using WpfApp2.Service;

namespace WpfApp2.ViewModel.MainWindowViewModel.ChildrentsMainViewModel
{
    public class ViolationDetailViewModel : BaseViewModel
    {
        // CCCD của người vi phạm
        private string _personId;
        public string PersonId
        {
            get => _personId;
            set => SetProperty(ref _personId, value);
        }

        // Thông tin chi tiết của người vi phạm (họ tên, địa chỉ...)
        private Person _person;
        public Person Person
        {
            get => _person;
            set => SetProperty(ref _person, value);
        }

        // Biển số xe của phương tiện vi phạm
        private string _vehicleId;
        public string VehicleId
        {
            get => _vehicleId;
            set => SetProperty(ref _vehicleId, value);
        }

        // Thông tin phương tiện (loại xe, hãng xe...)
        private Vehicle _vehicle;
        public Vehicle Vehicle
        {
            get => _vehicle;
            set => SetProperty(ref _vehicle, value);
        }

        // Thông tin cán bộ công an lập biên bản
        private Police _police;
        public Police Police
        {
            get => _police;
            set => SetProperty(ref _police, value);
        }

        // Ngày lập biên bản
        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        // Địa điểm vi phạm
        private string _location;
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        // Tổng số tiền phạt cho biên bản này
        private decimal _totalFine;
        public decimal TotalFine
        {
            get => _totalFine;
            set => SetProperty(ref _totalFine, value);
        }

        // Trạng thái thanh toán (đã nộp phạt hay chưa)
        private bool _isPaid;
        public bool IsPaid
        {
            get => _isPaid;
            set => SetProperty(ref _isPaid, value);
        }

        // Danh sách các lỗi vi phạm trong biên bản
        private ObservableCollection<Violation> _violations;
        public ObservableCollection<Violation> Violations
        {
            get => _violations;
            set => SetProperty(ref _violations, value);
        }

        // Fix for CS0029 and CS8604
        private void LoadReportDetails(Report report)
        {
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

            PersonId = report.PersonId!;
            VehicleId = report.VehicleId!;
            Police = report.Police!;
            Date = report.Date;
            Location = report.Location!;
            TotalFine = report.TotalFine;
            IsPaid = report.IsPaid;
        }

        // Constructor 
        public ViolationDetailViewModel(string report)
        {
            
        }
        public ViolationDetailViewModel()
        {

        }
    }
}
