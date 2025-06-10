using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Model;
using WpfApp2.Service;
using System.Linq;
using WpfApp2.View;
using System.Threading.Tasks;

namespace WpfApp2.ViewModel.MainWindowViewModel.ChildrentsMainViewModel
{
    public class ViolationRecordViewModel : BaseViewModel
    {
        #region Message Properties
        private bool _isCCCDTrue = false;
        // Thông báo cho CCCD
        private string? _cccdMessage;
        public string? CccdMessage
        {
            get => _cccdMessage;
            set { _cccdMessage = value; OnPropertyChanged(); }
        }

        private string? _cccdMessageType = "Error";
        public string? CccdMessageType
        {
            get => _cccdMessageType;
            set { _cccdMessageType = value; OnPropertyChanged(); }
        }

        // Thông báo cho phương tiện
        private string? _vehicleMessage;
        public string? VehicleMessage
        {
            get => _vehicleMessage;
            set { _vehicleMessage = value; OnPropertyChanged(); }
        }

        private string? _vehicleMessageType = "Error";
        public string? VehicleMessageType
        {
            get => _vehicleMessageType;
            set { _vehicleMessageType = value; OnPropertyChanged(); }
        }

        // Thông báo cho ngày lập biên bản
        private string? _violationMessage;
        public string? ViolationMessage
        {
            get => _violationMessage;
            set { _violationMessage = value; OnPropertyChanged(); }
        }

        private string? _violationMessageType = "Error";
        public string? ViolationMessageType
        {
            get => _violationMessageType;
            set { _violationMessageType = value; OnPropertyChanged(); }
        }

        // Thông báo cho địa điểm
        private string? _locationMessage;
        public string? LocationMessage
        {
            get => _locationMessage;
            set { _locationMessage = value; OnPropertyChanged(); }
        }

        private string? _locationMessageType = "Error";
        public string? LocationMessageType
        {
            get => _locationMessageType;
            set { _locationMessageType = value; OnPropertyChanged(); }
        }
        #endregion

        #region Thuộc tính người vi phạm 
        public string CCCD
        {
            get => _cccd;
            set
            {
                _cccd = value;
                OnPropertyChanged();
                // Reset message
                CccdMessage = null;
                if (_cccd.Length >= 10 && _cccd.Length <= 12)
                {
                    DisplayPersonInfo();
                }
                else if (!string.IsNullOrEmpty(_cccd))
                {
                    CccdMessage = "CCCD phải từ 10 đến 12 ký tự.";
                    CccdMessageType = "Error";
                }
            }
        }
        private string _cccd;

        public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }
        private string _name;

        public string Address { get => _address; set { _address = value; OnPropertyChanged(nameof(Address)); } }
        private string _address;

        // Hàm hiển thị thông tin sau khi nhập cccd
        private async void DisplayPersonInfo()
        {
            var personService = new PersonService();
            var person = await personService.GetPersonByIdAsync(CCCD);
            if (person != null)
            {
                _isCCCDTrue = true;
                Name = person.Name!;
                Address = person.Address!;
                CccdMessage = null;
            }
            else
            {
                CccdMessage = "Không tìm thấy thông tin người vi phạm.";
                CccdMessageType = "Error";
                Name = string.Empty;
                Address = string.Empty;
            }
        }
        #endregion

        #region Thông tin phương tiện
        private bool _isVehicleInfoValid = false;
        private int? _vehicleTypeId; // Lưu TypeId của xe
        private ObservableCollection<Violation> _allViolations; // Lưu toàn bộ danh sách vi phạm gốc

        public string NumberPlate
        {
            get => _numberPlate;
            set
            {
                _numberPlate = value;
                OnPropertyChanged();
                VehicleMessage = null;
                if (_numberPlate.Length == 9)
                {
                    DisplayVehicleInfo();
                }
                else if (!string.IsNullOrEmpty(_numberPlate))
                {
                    VehicleMessage = "Biển số xe phải đủ 9 ký tự.";
                    VehicleMessageType = "Error";
                    _vehicleTypeId = null;
                    CarCompany = "";
                    VehicleType = "";
                    FilterViolationTypes(); // Lọc lại danh sách vi phạm khi biển số không hợp lệ
                }
            }
        }
        private string _numberPlate;

        private string _carCompany;
        public string CarCompany
        {
            get => _carCompany;
            set { _carCompany = value; OnPropertyChanged(nameof(CarCompany)); }
        }

        private string _vehicleType;
        public string VehicleType
        {
            get => _vehicleType;
            set { _vehicleType = value; OnPropertyChanged(nameof(VehicleType)); }
        }

        private async void DisplayVehicleInfo()
        {
            var vehicleService = new VehicleService();
            var vehicle = await vehicleService.GetVehicleByIdAsync(NumberPlate);
            if (vehicle != null)
            {
                _isVehicleInfoValid = true;
                _vehicleTypeId = vehicle.TypeId; // Lưu TypeId của xe
                CarCompany = vehicle.Brand?.Name ?? "";
                VehicleType = vehicle.Type?.Name ?? "";
                VehicleMessage = null;
                FilterViolationTypes(); // Lọc danh sách vi phạm theo loại xe
            }
            else
            {
                VehicleMessage = "Không tìm thấy thông tin phương tiện.";
                VehicleMessageType = "Error";
                _vehicleTypeId = null;
                CarCompany = "";
                VehicleType = "";
                FilterViolationTypes(); // Lọc lại danh sách vi phạm
            }
        }

        private void LoadViolationTypes()
        {
            Task.Run(async () =>
            {
                var service = new ViolationService();
                var violationTypes = await service.GetAllViolationsAsync();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _allViolations = new ObservableCollection<Violation>(violationTypes);
                    FilterViolationTypes();
                });
            });
        }

        private void FilterViolationTypes()
        {
            if (_allViolations == null) return; // Đảm bảo _allViolations đã được tải

            Application.Current.Dispatcher.Invoke(() =>
            {
                ViolationTypes.Clear();
                var filteredViolations = _allViolations
                    .Where(v => v.TypeId == null || (_vehicleTypeId != null && v.TypeId == _vehicleTypeId))
                    .Except(ViolationList) // Loại bỏ các vi phạm đã có trong ViolationList
                    .ToList();
                foreach (var violation in filteredViolations)
                {
                    ViolationTypes.Add(violation);
                }
            });
        }
        #endregion

        #region Thông tin vi phạm
        public DateTime Day { get => _day; set { _day = value; OnPropertyChanged(); } }
        private DateTime _day = DateTime.Now;

        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged();
                LocationMessage = string.IsNullOrWhiteSpace(_location) ? "Địa điểm không được để trống." : null;
                LocationMessageType = "Error";
            }
        }
        private string _location;

        public ObservableCollection<Violation> ViolationTypes { get; set; }

        public Violation? SelectedViolationType
        {
            get => _selectedViolationType;
            set { _selectedViolationType = value; OnPropertyChanged(); }
        }
        private Violation? _selectedViolationType;

        public ObservableCollection<Violation> ViolationList { get; set; }

        public int Fine
        {
            get => _fine;
            set { _fine = value; OnPropertyChanged(nameof(Fine)); }
        }
        private int _fine;

        public bool IsPaid
        {
            get => _isPaid;
            set { _isPaid = value; OnPropertyChanged(nameof(IsPaid)); }
        }
        private bool _isPaid;
        #endregion

        #region Command 
        public ICommand AddCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteCommand { get; }
        #endregion

        #region Hàm xử lý Command
        private void AddViolation()
        {
            if (SelectedViolationType is Violation violation)
            {
                if (ViolationList.Contains(violation))
                {
                    ViolationMessage = "Lỗi vi phạm này đã được thêm.";
                    ViolationMessageType = "Error";
                }
                else
                {
                    ViolationList.Add(violation);
                    ViolationTypes.Remove(violation);
                    CalculateTotalFine();
                    ViolationMessage = null;
                }
            }
            else
            {
                ViolationMessage = "Vui lòng chọn lỗi vi phạm.";
                ViolationMessageType = "Error";
            }
            SelectedViolationType = null;
        }


        private bool CanAddViolation()
        {
            return SelectedViolationType != null;
        }

        private void RemoveViolation(Violation violation)
        {
            ViolationList.Remove(violation);
            // Chỉ thêm lại vào ViolationTypes nếu vi phạm phù hợp với loại xe hoặc là lỗi chung
            if (violation.TypeId == null || (_vehicleTypeId != null && violation.TypeId == _vehicleTypeId))
            {
                ViolationTypes.Add(violation);
            }
            CalculateTotalFine();
        }

        private void CalculateTotalFine()
        {
            Fine = ViolationList.Sum(v => v.FineAmount);
        }

        private Report CreateNewReport()
        {
            var report = new Report
            {
                PersonId = CCCD,
                VehicleId = NumberPlate,
                TotalFine = Fine,
                Location = Location,
                PoliceId = UserSession.Instance.Police.MilitaryID,
                Date = Day,
                IsPaid = IsPaid,
                PaidDate = IsPaid ? DateTime.Now : null,
                Violations = ViolationList.Select(v => new ReportInfo
                {
                    ViolationId = v.Id
                }).ToList()
            };
            return report;
        }

        private async Task SaveReport(Report report)
        {
            var reportService = new ReportService();
            await reportService.AddReportAsync(report);
        }

        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(CCCD))
            {
                CccdMessage = "Vui lòng nhập CCCD.";
                CccdMessageType = "Error";
                return;
            }
            if (!_isCCCDTrue)
            {
                CccdMessage = "Vui lòng nhập đúng CCCD.";
                CccdMessageType = "Error";
                return;
            }

            if (string.IsNullOrWhiteSpace(NumberPlate))
            {
                VehicleMessage = "Vui lòng nhập biển số xe.";
                VehicleMessageType = "Error";
                return;
            }
            if (!_isVehicleInfoValid)
            {
                VehicleMessage = "Vui lòng nhập đúng biển số xe.";
                VehicleMessageType = "Error";
                return;
            }

            if (ViolationList == null || ViolationList.Count == 0)
            {
                ViolationMessage = "Vui lòng chọn ít nhất một lỗi vi phạm.";
                ViolationMessageType = "Error";
                return;
            }

            var report = CreateNewReport();
            if (string.IsNullOrWhiteSpace(report.Location))
            {
                LocationMessage = "Vui lòng nhập địa điểm vi phạm.";
                LocationMessageType = "Error";
                return;
            }
            if (report != null)
            {
                await SaveReport(report);
                MessageBox.Show("Biên bản đã được lưu thành công!");
                App.ViewModel!.CurrentView = new ViolationSearch();
            }
            else
            {
                MessageBox.Show("Có lỗi khi tạo biên bản");
            }
        }


        private void Cancel()
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn hủy không?",
                "Hủy tạo biên bản",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
            if (result == MessageBoxResult.Yes)
            {
                App.ViewModel!.CurrentView = new ViolationSearch();
            }
        }
        #endregion

        #region Constructor
        public ViolationRecordViewModel()
        {
            ViolationTypes = new ObservableCollection<Violation>();
            ViolationList = new ObservableCollection<Violation>();
            _allViolations = new ObservableCollection<Violation>();
            LoadViolationTypes(); 
            
            DeleteCommand = new RelayCommand<Violation>(violation => RemoveViolation(violation));
            AddCommand = new RelayCommand(_ => AddViolation(), _ => CanAddViolation());
            SaveCommand = new RelayCommand(async _ => await Save());
            CancelCommand = new RelayCommand(_ => Cancel());
        }
        #endregion
    }
}