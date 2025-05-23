using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WpfApp2.View;
using WpfApp2.View.Citizen;
using WpfApp2.View.Police;
namespace WpfApp2.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
            }
        }


        private object _selectedMenuItem;
        public object SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                if (_selectedMenuItem != value)
                {
                    _selectedMenuItem = value;
                    OnPropertyChanged();
                    MenuItemSelected(); 
                }
            }
        }

        private ICommand _userButtonCommand;
        public ICommand UserButtonCommand
        {
            get => _userButtonCommand;
            set
            {
                if (_userButtonCommand != value)
                {
                    _userButtonCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<MenuItemModel> _menuItems;
        public ObservableCollection<MenuItemModel> MenuItems
        {
            get => _menuItems;
            set
            {
                if (_menuItems != value)
                {
                    _menuItems = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainViewModel()
        {
            UserButtonCommand = new RelayCommand(_ => OpenUserProfile());
        }
        // Tạo giao diện cơ bản
        public void InterfaceSetup()
        {   
            // Phân quyền người dùng
            if (UserSession.Instance.Role == "Police") // Role = "Công an"
            {
                Title = "Quản lý giao thông";
                MenuItems = new ObservableCollection<MenuItemModel>
                {
                    new MenuItemModel { Icon = "HomeAnalytics", Title = "Trang chủ" },
                    new MenuItemModel { Icon = "AccountPlus", Title = "Biên bản vi phạm" },
                    new MenuItemModel { Icon = "AccountBoxEditOutline", Title = "Cán bộ" },
                };
            }
            else // Role = "Người dùng"
            {
                Title = "Tra cứu xử phạt";
                MenuItems = new ObservableCollection<MenuItemModel>
                {
                    new MenuItemModel { Icon = "HomeAnalytics", Title = "Trang chủ" },
                    new MenuItemModel { Icon = "AccountCardOutline", Title = "Cá nhân" },
                };
            }
        }

        // Xử lý khi bấm vào menu
        private void MenuItemSelected()
        {
            if (UserSession.Instance.Role == "Police")
            {
                if (SelectedMenuItem is MenuItemModel menuItem)
                {
                    // Xử lý khi bấm vào menu
                    switch (menuItem.Title)
                    {
                        case "Trang chủ":
                            // Mở giao diện Home
                            CurrentView = new Home();
                            break;
                        case "Biên bản vi phạm":
                            CurrentView = new ViolationSearch();
                            // Mở giao diện người vi phạm
                            break;
                        case "Cán bộ":
                            CurrentView = new OfficerProfile();
                            // Mở giao diện cán bộ
                            break;
                    }
                }
            }
            else
            {
                if (SelectedMenuItem is MenuItemModel menuItem)
                {
                    // Xử lý khi bấm vào menu
                    switch (menuItem.Title)
                    {
                        case "Trang chủ":
                            CurrentView = new UserHome();
                            break;
                        case "Cá nhân":
                            CurrentView = new Individual();
                            break;
                    }
                }
            }
        }

        private void OpenUserProfile()
        {
            CurrentView = new AccountInformation();
        }

    }
    public class MenuItemModel : BaseViewModel
    {
        public string? Icon { get; set; }
        public string? Title { get; set; }
        
    }
}