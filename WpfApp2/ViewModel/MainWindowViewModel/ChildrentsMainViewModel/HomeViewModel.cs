using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WpfApp2.Model;
using WpfApp2.Service;

namespace WpfApp2.ViewModel.MainWindowViewModel.ChildrentsMainViewModel
{
    /// <summary>
    /// ViewModel cho màn hình Home, hiển thị thống kê và danh sách báo cáo với chức năng phân trang và tìm kiếm.
    /// </summary>
    public class HomeViewModel : BaseViewModel
    {
        #region Thống kê phía trên
        private int _reportNumber;
        private int _carNumber;
        private int _humanNumber;

        public int ReportNumber
        {
            get => _reportNumber;
            set => SetProperty(ref _reportNumber, value);
        }

        public int CarNumber
        {
            get => _carNumber;
            set => SetProperty(ref _carNumber, value);
        }

        public int HumanNumber
        {
            get => _humanNumber;
            set => SetProperty(ref _humanNumber, value);
        }

        /// <summary>
        /// Load số liệu thống kê: tổng biên bản, xe, người.
        /// </summary>
        private void LoadMenu()
        {
            var reportService = new ReportService();
            var vehicleService = new VehicleService();
            var personService = new PersonService();

            Task.Run(async () =>
            {
                var reportCount = await reportService.GetReportCountAsync();
                var carCount = await vehicleService.GetVehicleCountAsync();
                var humanCount = await personService.GetPersonCountAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    ReportNumber = reportCount;
                    CarNumber = carCount;
                    HumanNumber = humanCount;
                });
            });
        }
        #endregion

        #region Danh sách báo cáo & phân trang
        private int _pageNumber;
        private const int _pageSize = 10;
        private bool _isLoading;

        public ObservableCollection<ReportViewModel> Reports { get; } = new ObservableCollection<ReportViewModel>();

        public ICollectionView FilteredData { get; private set; }

        public async void LoadNextPageAsync()
        {
            if (_isDataLoaded) return; // Bỏ qua nếu toàn bộ dữ liệu đã được tải
            if (_isLoading) return;
            _isLoading = true;
            try
            {
                var reportService = new ReportService();
                var newItems = await reportService.GetPageAsync(_pageNumber, _pageSize);

                // Nếu không còn dữ liệu mới, đánh dấu đã load hết
                if (newItems == null || newItems.Count == 0)
                {
                    _isDataLoaded = true;
                    return;
                }

                foreach (var item in newItems)
                {
                    Reports.Add(new ReportViewModel
                    {
                        Id = item.Id,
                        Violator = item.Person?.Name ?? string.Empty,
                        Day = item.Date.ToString("dd-MM-yyyy"),
                        TotalFine = item.TotalFine,
                        Status = item.IsPaid ? "Đã nộp" : "Chưa nộp"
                    });
                }
                _pageNumber++;
                FilteredData.Refresh();

                // Nếu số lượng bản ghi trả về nhỏ hơn page size, đã hết dữ liệu
                if (newItems.Count < _pageSize)
                {
                    _isDataLoaded = true;
                }
            }
            finally
            {
                _isLoading = false;
            }
        }

        #endregion

        #region Tìm kiếm
        private bool _isDataLoaded;
        private string _searchText;

        /// <summary>
        /// Text ô tìm kiếm, chỉ cập nhật khi nhấn nút tìm kiếm
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        /// <summary>
        /// Command gắn với nút Tìm kiếm
        /// </summary>
        public ICommand SearchCommand { get; }

        /// <summary>
        /// Điều kiện lọc báo cáo theo Violator hoặc Id
        /// </summary>
        private bool FilterReports(object obj)
        {
            if (obj is ReportViewModel report)
            {
                if (string.IsNullOrEmpty(SearchText))
                    return true;

                return report.Violator?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true
                    || report.Id.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true;
            }
            return false;
        }

        /// <summary>
        /// Load toàn bộ dữ liệu khi lần đầu tìm kiếm
        /// </summary>
        private async Task LoadAllReportsDataAsync()
        {
            var reportService = new ReportService();
            var allItems = await reportService.GetAllReportsAsync();

            var tempList = new List<ReportViewModel>();
            foreach (var item in allItems)
            {
                tempList.Add(new ReportViewModel
                {
                    Id = item.Id,
                    Violator = item.Person?.Name ?? string.Empty,
                    Day = item.Date.ToString("dd-MM-yyyy"),
                    TotalFine = item.TotalFine,
                    Status = item.IsPaid ? "Đã nộp" : "Chưa nộp"
                });
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                Reports.Clear();
                foreach (var report in tempList)
                    Reports.Add(report);
                _isDataLoaded = true;
            });
        }

        /// <summary>
        /// Hàm gọi khi nhấn nút Tìm kiếm: tải lần đầu nếu cần, sau đó refresh filter
        /// </summary>
        private async Task ExecuteSearch()
        {
            if (!_isDataLoaded)
                await LoadAllReportsDataAsync();
            else
                FilteredData.Refresh();
        }
        #endregion

        /// <summary>
        /// Constructor: khởi tạo dữ liệu và commands
        /// </summary>
        public HomeViewModel()
        {
            LoadMenu();
            FilteredData = CollectionViewSource.GetDefaultView(Reports);
            FilteredData.Filter = FilterReports;
            SearchCommand = new RelayCommand(async _ => ExecuteSearch());
            LoadNextPageAsync();
        }
    }

    /// <summary>
    /// ViewModel con đại diện cho từng báo cáo trong danh sách
    /// </summary>
    public class ReportViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Violator { get; set; } = string.Empty;
        public string Day { get; set; } = string.Empty;
        public int TotalFine { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
