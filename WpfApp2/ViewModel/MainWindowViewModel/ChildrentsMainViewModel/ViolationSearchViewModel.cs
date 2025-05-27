using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WpfApp2.Model;
using WpfApp2.Service;
using WpfApp2.View;

namespace WpfApp2.ViewModel.MainWindowViewModel.ChildrentsMainViewModel
{
    public class ViolationSearchViewModel : BaseViewModel
    {
        #region Fields & Properties

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
            }
        }

        private int _pageNumber = 0;
        private const int _pageSize = 20;
        private bool _isLoading;
        private bool _isDataLoaded;

        public ObservableCollection<Report> ReportList { get; } = new ObservableCollection<Report>();
        public ICollectionView FilteredData { get; }

        public ICommand SearchCommand { get; }
        public ICommand ViolationRecordAddCommand { get; }
        public ICommand XemChiTietCommand { get; }
        public ICommand LoadMoreCommand { get; }

        #endregion

        public ViolationSearchViewModel()
        {
            // Khởi tạo CollectionView cho filter
            FilteredData = CollectionViewSource.GetDefaultView(ReportList);
            FilteredData.Filter = FilterReports;

            ViolationRecordAddCommand = new RelayCommand(_ => AddViolationRecord());
            XemChiTietCommand = new RelayCommand<int>(id => ViewDetails(id));
            SearchCommand = new RelayCommand(async _ => await ExecuteSearch());
            LoadMoreCommand = new RelayCommand(async _ => await LoadNextPageAsync());

            // Lazy load trang đầu tiên
            _ = LoadNextPageAsync();
        }

        #region Lazy Load (Phân trang)
        public async Task LoadNextPageAsync()
        {
            if (_isLoading) return;
            _isLoading = true;
            try
            {
                var reportService = new ReportService();
                var newItems = await reportService.GetPageAsync(_pageNumber, _pageSize);
                if (newItems.Count == 0)
                {
                    _isDataLoaded = true;
                    return;
                }
                foreach (var item in newItems)
                {
                    ReportList.Add(item);
                }
                _pageNumber++;
                FilteredData.Refresh();
            }
            finally
            {
                _isLoading = false;
            }
        }
        #endregion

        #region Tìm kiếm
        private bool FilterReports(object obj)
        {
            if (obj is Report report)
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                    return true;

                return (report.Person?.Name?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    || report.Id.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                    || (report.Police?.Person?.Name?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    || (report.VehicleId?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return false;
        }

        private async Task ExecuteSearch()
        {
            if (!_isDataLoaded)
            {
                await LoadAllReportsDataAsync();
            }
            FilteredData.Refresh();
        }

        private async Task LoadAllReportsDataAsync()
        {
            var reportService = new ReportService();
            var allItems = await reportService.GetAllReportsAsync();

            Application.Current.Dispatcher.Invoke(() =>
            {
                ReportList.Clear();
                foreach (var report in allItems)
                    ReportList.Add(report);
                _isDataLoaded = true;
            });
        }
        #endregion

        #region Command Actions
        private void AddViolationRecord()
        {
            App.ViewModel!.CurrentView = new ViolationRecord();
        }

        private void ViewDetails(int id)
        {
            App.ViewModel!.CurrentView = new ViolationDetail(id);
        }
        #endregion
    }
}
