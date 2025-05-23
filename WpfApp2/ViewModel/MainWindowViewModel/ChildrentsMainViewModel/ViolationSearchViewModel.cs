using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp2.Model;
using WpfApp2.Service;
using WpfApp2.View;

namespace WpfApp2.ViewModel.MainWindowViewModel.ChildrentsMainViewModel
{
    public class ViolationSearchViewModel : BaseViewModel
    {
        public ObservableCollection<Report> ReportList { get; set; }
        public ICommand ViolationRecordAddCommand { get; set; }
        public ICommand XemChiTietCommand { get; set; }

        // Hàm load danh sách biên bản trong datagrid
        private async void LoadReportList()
        {
            // Lấy danh sách biên bản từ database
            ReportService reportService = new ReportService();
            var reports = await reportService.GetAllReportsAsync() ;
            foreach (var report in reports)
            {
                ReportList.Add(report);
            }
        }
        public ViolationSearchViewModel()
        {
            
            ReportList = new ObservableCollection<Report>();
            ViolationRecordAddCommand = new RelayCommand( _ => AddViolationRecord());
            XemChiTietCommand = new RelayCommand<Report>(report => ViewDetails(report));
            LoadReportList();
        }

        private void AddViolationRecord()
        {
            App.ViewModel!.CurrentView = new ViolationRecord();
        }

        private void ViewDetails(Report report)
        {
            // TODO: Logic hiển thị chi tiết biên bản
        }
    }
    public class DatagridItem
    {
        public int ReportId { get; set; }
        public string? ViolationDetails { get; set; }
        public DateTime ViolationDate { get; set; }
        public string? VehicleId { get; set; }
        public string? OfficerId { get; set; }
    }
}