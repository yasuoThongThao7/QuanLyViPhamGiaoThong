using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfApp2.View;
using WpfApp2.View.Citizen;
using WpfApp2.Model;
using WpfApp2.ViewModel.MainWindowViewModel.ChildrentsMainViewModel;
using WpfApp2.Service;

namespace WpfApp2.ViewModel.User
{
    public class UserHomeViewModel : BaseViewModel
    {
        private MainViewModel mvd;

        // Command
        public ICommand LookUpCommand { get; }
        public ICommand ReportListViewCommand { get; }
        public ICommand PayCommand { get; }
        public ICommand BackCommand { get; }

        // Dữ liệu vi phạm gần đây
        private ObservableCollection<RecentViolationViewModel> _recentViolations;
        public ObservableCollection<RecentViolationViewModel> RecentViolations
        {
            get => _recentViolations;
            set => SetProperty(ref _recentViolations, value);
        }

        // Khoi tạo dữ liệu vi phạm gần đây
        public async void LoadRecentViolations()
        {
            var service = new ReportService();
            RecentViolations = new ObservableCollection<RecentViolationViewModel>();
            var reports = await service.GetReportsByPersonIdAsync(UserSession.Instance.Person.CCCD!);
            foreach (var r in reports)
            {
                if (RecentViolations.Count >= 5) break;

                RecentViolations.Add(new RecentViolationViewModel
                {
                    NumberPlate = r.Vehicle!.Id,
                    Date = r.Date,
                    Fine = r.TotalFine,
                    Status = r.IsPaid,
                    ViewDetailCommand = new RelayCommand(_ => ViewDetail(r.Id))
                });
            }
        }

        private void Back()
        {
            mvd.CurrentView = new UserHome();
        }

        private void LookUp()
        {
            mvd.CurrentView = new VehicleLookup();
        }

        private void ReportListView()
        {
            mvd.CurrentView = new RecordLookup();
        }

        private void Pay()
        {
            mvd.CurrentView = new OnlinePayment();
        }

        public UserHomeViewModel()
        {
            mvd = App.ViewModel!;
            LookUpCommand = new RelayCommand(_ => LookUp());
            ReportListViewCommand = new RelayCommand(_ => ReportListView());
            PayCommand = new RelayCommand(_ => Pay());
            BackCommand = new RelayCommand(_ => Back());
            LoadRecentViolations();


        }

        private void ViewDetail(int reportCode)
        {
            // Thay bằng logic mở chi tiết biên bản
            mvd.CurrentView = new ViolationDetailViewModel(reportCode);
        }
    }

    /// <summary>
    /// ViewModel con để hiển thị từng vi phạm gần đây trong ItemsControl.
    /// </summary>
    public class RecentViolationViewModel
    {
        public string NumberPlate { get; set; }
        public DateTime Date { get; set; }
        public decimal Fine { get; set; }
        public bool Status { get; set; }
        public ICommand ViewDetailCommand { get; set; }

        public string StatusText => Status ? "Đã đóng phạt" : "Chưa đóng phạt";
    }

}
