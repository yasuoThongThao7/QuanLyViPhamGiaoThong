﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Service;
using WpfApp2.View.Citizen;
using WpfApp2.ViewModel;

namespace WpfApp2.ViewModel.User
{
    public class OnlinePaymentViewModel : BaseViewModel
    {
        private int _reportNumber;
        public int ReportNumber
        {
            get => _reportNumber;
            set 
            { 
                SetProperty(ref _reportNumber, value);
                ImportProperty();
            }
        }

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        private string _cccd;
        public string CCCD
        {
            get => _cccd;
            set => SetProperty(ref _cccd, value);
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        private List<string> _paymentMethods;
        public List<string> PaymentMethods
        {
            get => _paymentMethods;
            set => SetProperty(ref _paymentMethods, value);
        }

        private string _selectedPaymentMethod;
        public string SelectedPaymentMethod
        {
            get => _selectedPaymentMethod;
            set => SetProperty(ref _selectedPaymentMethod, value);
        }

        private string _note;
        public string Note
        {
            get => _note;
            set => SetProperty(ref _note, value);
        }
        public ICommand ConfirmPaymentCommand { get; }
        public ICommand BackCommand { get; }
        private void Pay()
        {
            if (ReportNumber <= 0)
            {
                MessageBox.Show("Vui lòng nhập số biên bản hợp lệ.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(SelectedPaymentMethod))
            {
                MessageBox.Show("Vui lòng chọn phương thức thanh toán.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var report = UserSession.Instance.Reports.Find(r => r.Id == ReportNumber);
            if (report == null)
            {
                MessageBox.Show("Không tìm thấy biên bản phù hợp.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (report.IsPaid)
            {
                MessageBox.Show("Biên bản này đã được thanh toán trước đó.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            ReportService service = new ReportService();
            report.IsPaid = true;
            report.PaidDate = DateTime.Now;

            service.SaveChangesAsync(report).ContinueWith(task =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Thanh toán thành công");
                    App.ViewModel!.CurrentView = new UserHome();
                });
            });
        }

        
        private void ImportProperty()
        {
            var report = UserSession.Instance.Reports.Find(r => r.Id == ReportNumber);
            if (report != null)
            {
                
                FullName = report.Person!.Name!;
                CCCD = report.Person!.CCCD!;
                Amount = report.TotalFine;
            }
        }
        private void Back()
        {
            App.ViewModel!.CurrentView = new UserHome();
        }
        public OnlinePaymentViewModel()
        {
            // Initialize properties
            PaymentMethods = new List<string> { "Credit Card", "Debit Card", "PayPal" };
            // Initialize commands
            ConfirmPaymentCommand = new RelayCommand(_ => Pay());
            BackCommand = new RelayCommand(_ => Back());
        }
    }
}
