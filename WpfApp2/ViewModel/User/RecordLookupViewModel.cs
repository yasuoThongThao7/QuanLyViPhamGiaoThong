using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WpfApp2.Model;
using WpfApp2.View;
using WpfApp2.View.Citizen;
using WpfApp2.ViewModel.MainWindowViewModel.ChildrentsMainViewModel;

namespace WpfApp2.ViewModel.User
{
    public class RecordLookupViewModel : BaseViewModel
    {

        // Thuộc tính tìm kiếm
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        // Danh sách kết quả biên bản
        private ObservableCollection<ViolationRecordVmd> _recordList;
        public ICollectionView FilteredData { get; private set; }
       
        public ICommand ViewDetailCommand { get; }
        // Command "Tìm kiếm"
        public ICommand SearchCommand { get; }

        // Command "Back"
        public ICommand BackCommand { get; }

        // Hàm khởi tạo dữ liệu trong datagrid 
        public void LoadDataGrid()
        {
            _recordList = new ObservableCollection<ViolationRecordVmd>(
                UserSession.Instance.Reports.Select(
                    r => new ViolationRecordVmd()
                    {
                        Id = r.Id.ToString(),
                        Day = r.Date.ToString("dd/MM/yyyy"),
                        NumberPlate = r.Vehicle!.Id,
                        Fine = r.TotalFine,
                        Status = r.IsPaid ? "Đã nộp" : "Chưa nộp",
                    })
            );
        }
        private bool FilterDataGrid(object obj)
        {
            if (obj is  ViolationRecordVmd item)
            {
                if (string.IsNullOrWhiteSpace(SearchText))  return true;
                return item.Id?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                       item.NumberPlate?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true;
               
            }
            return false;
        }
        public RecordLookupViewModel()
        {
            BackCommand = new RelayCommand(_ => Back());
            SearchCommand = new RelayCommand(_ => Search());
            ViewDetailCommand = new RelayCommand<string>(idStr =>
            {
                if (int.TryParse(idStr, out int id))
                {
                    ViewDetail(id);
                }
            });

            LoadDataGrid(); 
            FilteredData = CollectionViewSource.GetDefaultView(_recordList);
            FilteredData.Filter = FilterDataGrid;
        }



        private void Back()
        {
            App.ViewModel!.CurrentView = new UserHome();
        }

        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                MessageBox.Show("Vui lòng nhập thông tin tìm kiếm.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            FilteredData.Refresh();

            if (FilteredData.IsEmpty)
            {
                MessageBox.Show("Không tìm thấy kết quả phù hợp.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        //Xem chi tiết biên bản
        private void ViewDetail(int maBienBan)
        {
            App.ViewModel.CurrentView =  new ViolationDetail(maBienBan);
        }

        
    }
    //Xem chi tiết biên bản
    public class ViolationRecordVmd
    {
        public string? Id { get; set; }
        public string? Day { get; set; }
        public string? NumberPlate { get; set; }
        public int? Fine { get; set; }
        public string? Status { get; set; }
    }
}
