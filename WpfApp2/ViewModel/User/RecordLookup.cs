using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfApp2.Model;
using WpfApp2.View;
using WpfApp2.View.Citizen;

namespace WpfApp2.ViewModel.User
{
    public class RecordLookup : BaseViewModel
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
        public ObservableCollection<ViolationRecordVmd> RecordList
        {
            get => _recordList;
            set => SetProperty(ref _recordList, value);
        }

        // Command "Tìm kiếm"
        public ICommand SearchCommand { get; }

        // Command "Back"
        public ICommand BackCommand { get; }

        public RecordLookup()
        {
            BackCommand = new RelayCommand(_ => Back());
            SearchCommand = new RelayCommand(_ => Search());

            // Dữ liệu mẫu ban đầu
            RecordList = new ObservableCollection<ViolationRecordVmd>(
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

        private void Back()
        {
            App.ViewModel.CurrentView = new UserHome();
        }

        private void Search()
        {
            // Tìm kiếm giả lập (có thể thay bằng gọi DB hoặc API thực tế)
            RecordList.Clear();

        }
        //Xem chi tiết biên bản
        private void ViewDetail(string maBienBan)
        {

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
