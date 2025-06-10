using System.Windows;
using System.Windows.Input;
using WpfApp2.Service;

namespace WpfApp2.ViewModel.AccountAccess
{
    class SignUpViewModel : BaseViewModel
    {
        #region Properties Login
        private string _userName;
        private string _password;

        private bool _isLoading;
        #endregion

        #region Commands
        public ICommand LoginCommand { get; }
        #endregion

        #region Getters and Setters

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        #endregion


        //Hàm Login
        public async Task ExecuteLogin()
        {
            try
            {
                IsLoading = true;
                AccountService accountService = new AccountService();
                var account = await accountService.CheckPasswordAsync(_userName, _password);
                IsLoading = false;

                if (account != null)
                {
                    await UserSession.Instance.SetUserData(account);
                    CloseWindow(true);
                }
                else
                {
                    MessageBox.Show("Đăng nhập thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                IsLoading = false;
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //Hàm close window 
        private void CloseWindow(bool dialogResult)
        {
            // Tìm SignUp và đóng nó
            Window window = Application.Current.Windows
                                     .OfType<Window>()
                                     .FirstOrDefault(w => w.DataContext == this)!;

            if (window != null)
            {
                window.DialogResult = true;
                window.Close();
            }
            else
            {
                MessageBox.Show("Không tìm thấy cửa sổ đăng nhập để đóng!");
            }
        }

        #region Properties SignUp
        //Các thuộc tính cho SignUp
        private string _CCCD;
        private string _name;
        private string _phone;
        private string _birth;
        private string _newPassword;
        private string _confirmPassword;
        #endregion

        public ICommand CreateAccountCommand { get; }
        #region Getters and Setters SignUp
        public string CCCD
        {
            get => _CCCD;
            set
            {
                if (_CCCD == value) return;
                _CCCD = value;
                OnPropertyChanged(nameof(CCCD));


                if (value != null && (value.Length == 11 || value.Length == 12))
                {
                    // Khởi chạy phương thức 
                    _ = LoadPersonInfoAsync();
                }
                else if (value != null && value.Length > 0 && value.Length < 11)
                {
                    // Reset các trường thông tin
                    ClearPersonInfo();
                }
            }
        }
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }
        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(nameof(Phone)); }
        }
        public string Birth
        {
            get => _birth;
            set { _birth = value; OnPropertyChanged(nameof(Birth)); }
        }
        public string NewPassword
        {
            get => _newPassword;
            set { _newPassword = value; OnPropertyChanged(nameof(NewPassword)); }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(nameof(ConfirmPassword)); }
        }
        #endregion
        #region function 
        //Hàm tạo tài khoản
        private async Task CreateAccount()
        {
            // Tạo tài khoản mới
            IsLoading = true;
            AccountService accountService = new AccountService();
            if (string.IsNullOrEmpty(_CCCD) || string.IsNullOrEmpty(NewPassword))
            {
                MessageBox.Show("Vui lòng nhập CCCD và mật khẩu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsLoading = false;
                return;
            }


            if (NewPassword != ConfirmPassword)
            {
                MessageBox.Show("Mật khẩu không khớp.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsLoading = false;
                return;
            }
            if (NewPassword.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsLoading = false;
                return;
            }

            if (await accountService.SearchAsync(CCCD))
            {
                MessageBox.Show("CCCD này đã có tài khoản.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                IsLoading = false;
                return;
            }

            // Tạo tài khoản mới
            Model.Account newAccount = new Model.Account
            {
                LoginId = CCCD,
                Password = NewPassword,
                Role = "Citizen"
            };
            await accountService.Add(newAccount);
            // Đăng nhập tự động sau khi tạo tài khoản
            await UserSession.Instance.SetUserData(newAccount);
            MessageBox.Show("Tạo tài khoản thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            IsLoading = false;
            CloseWindow(true);
        }


        // Phương thức riêng để xử lý bất đồng bộ
        private async Task LoadPersonInfoAsync()
        {
            try
            {
                // Hiển thị loading nếu cần
                IsLoading = true;

                PersonService personService = new PersonService();
                var person = await personService.GetPersonByIdAsync(CCCD);

                if (person != null)
                {
                    Name = person.Name ?? string.Empty;
                    Phone = person.PhoneNumber ?? string.Empty;
                    // With this corrected line:  
                    Birth = person.birth?.ToString("dd-MM-yyyy") ?? string.Empty; // Lấy ngày sinh từ đối tượng person
                }
                else
                {
                    // Hiển thị thông báo
                    MessageBox.Show("Không tìm thấy thông tin cá nhân.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    // Reset các trường thông tin
                    ClearPersonInfo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearPersonInfo();
            }
            finally
            {
                IsLoading = false;
            }
        }

        // Phương thức để xóa thông tin khi cần
        private void ClearPersonInfo()
        {
            Name = string.Empty;
            Phone = string.Empty;
            Birth = string.Empty;
        }

        #endregion

        #region Constructor
        public SignUpViewModel()
        {
            LoginCommand = new RelayCommand(async _ => await ExecuteLogin());
            CreateAccountCommand = new RelayCommand(async _ => await CreateAccount());
        }
        #endregion

    }
}