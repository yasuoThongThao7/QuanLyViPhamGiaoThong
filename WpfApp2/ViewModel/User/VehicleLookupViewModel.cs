using System.Windows;
using System.Windows.Input;
using WpfApp2.Model;
using WpfApp2.Service;
using WpfApp2.View.Citizen;

namespace WpfApp2.ViewModel.User
{
    public class VehicleLookupViewModel : BaseViewModel
    {
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        private Vehicle _vehicleInfo;
        public Vehicle VehicleInfo
        {
            get => _vehicleInfo;
            set => SetProperty(ref _vehicleInfo, value);
        }

        public ICommand SearchCommand { get; }
        public ICommand BackCommand { get; }

        public VehicleLookupViewModel()
        {
            SearchCommand = new RelayCommand(async _ => await ExecuteSearch(_));
            BackCommand = new RelayCommand(Back);
        }

        private void Back(object parameter)
        {
            App.ViewModel!.CurrentView = new UserHome();
        }

        private async Task ExecuteSearch(object parameter)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                VehicleInfo = null;
                MessageBox.Show("Vui lòng nhập biển số xe để tra cứu.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var vehicleService = new VehicleService();
            var vehicle = await vehicleService.GetVehicleByIdAsync(SearchText);
            if (vehicle != null)
            {
                VehicleInfo = vehicle;
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin xe với biển số này.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
