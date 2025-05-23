using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Model;
using WpfApp2.Service;
using System.Linq;
using WpfApp2.View;

namespace WpfApp2.ViewModel.MainWindowViewModel.ChildrentsMainViewModel
{
    public class ViolationRecordViewModel : BaseViewModel
    {
       

        #region Thuộc tính người vi phạm 
        public string CCCD
        {
            get => _cccd;
            set
            {
                _cccd = value;
                OnPropertyChanged();
                if (_cccd.Length >=10 && _cccd.Length <= 12)
                {
                    DisplayPersonInfo();
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
            // Gọi dịch vụ để lấy thông tin người vi phạm từ cơ sở dữ liệu
            var personService = new PersonService();
            var person = await personService.GetPersonByIdAsync(CCCD);
            if (person != null)
            {
                Name = person.Name!;
                Address = person.Address!;
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin người vi phạm.");
            }
        }

        #endregion

        #region Thông tin phương tiện
        // Biển số xe
        public string NumberPlate 
        { 
            get => _numberPlate;
            set
            {
                _numberPlate = value;
                OnPropertyChanged();
                if (_numberPlate.Length == 9)
                {
                    DisplayVehicleInfo();
                }
            }
        }
        private string _numberPlate;

        // Hãng xe
        private int _carCompany;
        public int CarCompany
        {
            get => _carCompany;
            set { _carCompany = value; OnPropertyChanged(nameof(CarCompany)); }
        }

        // Kiểu xe
        private int _vehicleType;
        public int VehicleType
        {
            get => _vehicleType;
            set { _vehicleType = value; OnPropertyChanged(nameof(VehicleType)); }
        }
        // Hàm show thông tin phương tiện sau khi nhập biển số
        private async void DisplayVehicleInfo()
        {
            // Gọi dịch vụ để lấy thông tin phương tiện từ cơ sở dữ liệu
            var vehicleService = new VehicleService();
            var vehicle = await vehicleService.GetVehicleByIdAsync(NumberPlate);
            if (vehicle != null)
            {
                CarCompany = vehicle.BrandId!;
                VehicleType = vehicle.TypeId!;
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin phương tiện.");
            }
        }


        #endregion

        #region Thông tin vi phạm
        public DateTime Day { get => _day; set { _day = value; OnPropertyChanged(); } }
        private DateTime _day = DateTime.Now;

        public string Location { get => _location; set { _location = value; OnPropertyChanged(); } }
        private string _location;

        //Danh sách lỗi trong combobox
        public ObservableCollection<Violation> ViolationTypes { get; set; }

        // Hàm khởi tạo dữ liệu trong danh sách lỗi vi phạm
        private void LoadViolationTypes()
        {
            Task.Run(async () =>
            {
                var service = new ViolationService();
                var violationTypes = await service.GetAllViolationsAsync();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var violation in violationTypes)
                    {
                        ViolationTypes.Add(violation);
                    }
                });
            });
        }


        // Kiểu SelectedType là lỗi vi phạm
        public Violation? SelectedViolationType
        {
            get => _selectedViolationType;
            set { _selectedViolationType = value; OnPropertyChanged(); }
        }
        private Violation? _selectedViolationType;
        // Danh sách lỗi vi phạm trong datagrid
        public ObservableCollection<Violation> ViolationList { get; set; }
        
        //Tiền phạt
        public int Fine
        {
            get => _fine;
            set { _fine = value; OnPropertyChanged(nameof(Fine)); }
        }
        private int _fine;
        // Đã nộp tiền phạt hay chưa
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
        // Hàm khi nhấn nút Add vi phạm
        private void AddViolation()
        {
            if(SelectedViolationType is Violation violation)
            {
                ViolationList.Add(violation);
                CalculateTotalFine();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn lỗi vi phạm.");
            }
            SelectedViolationType = null; // Reset lại lựa chọn
        }
        // Hàm khi ko thể thêm vi phạm
        private bool CanAddViolation()
        {
            return SelectedViolationType != null;
        }
        // Hàm xóa vi phạm trong datagrid
        private void RemoveViolation(Violation violation)
        {
            ViolationList.Remove(violation);
            CalculateTotalFine();
        }
        // Hàm tính tổng tiêng phạt 
        private void CalculateTotalFine()
        {
            Fine = ViolationList.Sum(v => v.FineAmount);
        }
        // Hàm tạo mới biên bản
        private Report CreateNewReport()
        {
            // Tạo một đối tượng mới cho biên bản
            var reportService = new ReportService();
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
                Violations = ViolationList.Select(ViolationList => new ReportInfo
                {
                    ViolationId = ViolationList.Id,
                    ReportId = ViolationList.FineAmount
                }).ToList()
            };
            return report;
        }
        // Hàm lưu biên bản vào cơ sở dữ liệu
        private async Task SaveReport(Report report)
        {
            var reportService = new ReportService();
            await reportService.AddReportAsync(report);
        }

        // Hàm của nút lưu biên bản
        private async Task Save()
        {

            var report = CreateNewReport(); // Tạo mới biên bản'
            if (report != null)
            {
                // Lưu biên bản vào cơ sở dữ liệu
                await SaveReport(report);
                MessageBox.Show("Biên bản đã được lưu thành công!");
                App.ViewModel!.CurrentView = new ViolationSearch();
            }
            else
            {
                MessageBox.Show("Có lỗi khi tạo biên bản");
            }
        }
        // Hàm hủy thao tác
        private void Cancel()
        {
           var result =  MessageBox.Show(
               "Bạn có chắc chắn muốn hủy ko ?", 
               "Hủy tạo biên bản",
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
            // Khởi tạo danh sách 
            ViolationTypes = new ObservableCollection<Violation>();
            ViolationList = new ObservableCollection<Violation>();
            LoadViolationTypes();
            // Khởi tạo các lệnh
            DeleteCommand = new RelayCommand<Violation>(violation => RemoveViolation(violation));
            AddCommand = new RelayCommand(_ => AddViolation(), _ => CanAddViolation());
            SaveCommand = new RelayCommand( async _ => await Save());
            CancelCommand = new RelayCommand(_ => Cancel());
        }
        #endregion
    }
    // Class cho danh sách lỗi vi phạm hiển thị trên datagrid
 
}
