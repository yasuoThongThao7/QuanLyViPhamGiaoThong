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
            if (ReportNumber > 0 && !string.IsNullOrEmpty(SelectedPaymentMethod))
            {
                ReportService service = new ReportService();
                var report = UserSession.Instance.Reports.Find(r => r.Id == ReportNumber);
                if (report != null)
                {
                    report.IsPaid = true;
                    report.PaidDate = DateTime.Now;
                }
                service.SaveChangesAsync().ContinueWith(task =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Thanh toán thành công");
                        App.ViewModel!.CurrentView = new UserHome();
                    });
                });
            }
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
