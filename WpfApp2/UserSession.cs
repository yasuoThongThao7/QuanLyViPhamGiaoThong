using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp2.Model;
using WpfApp2.Service;

namespace WpfApp2
{
    public class UserSession
    {
        // Singleton instance
        private static UserSession? _instance;

        // Properties 
        public Account Account { get; private set; } // Thuộc tính tài khoản
        public Police Police { get; private set; }// Thuộc tính khi là công an 
        public Person Person { get; private set; }// Thuộc tính khi là công dân

        public int Fine { get; set; } = 0; // Tiền phạt khi là coong dân 
        public List<Report> Reports { get; set; } // Danh sách biên bản khi là công dân
        public string Role => Account?.Role!; // Thuộc tính trạng thái đăng nhập

        // Private constructor
        private UserSession() { }

        // Phương thức truy cập duy nhất
        public static UserSession Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UserSession();
                return _instance;
            }
        }

        // Gán dữ liệu sau khi đăng nhập
        public async Task SetUserData(Account? account)
        {
            if (account == null)
                return;

            Account = account;

            if (account.Role == "Police")
            {
                var service = new PoliceService();
                Police = await service.GetPoliceAsync(account.LoginId);

                if (Police != null)
                    Person = Police.Person!;
            }
            else if (account.Role == "Citizen")
            {
                Reports = new List<Report>();
                var service = new PersonService();
                Person = await service.GetPersonByIdAsync(account.LoginId);
                if (Person != null)
                {
                    var reportService = new ReportService();
                    Reports = await reportService.GetReportsByPersonIdAsync(Person.CCCD!);
                    Fine = Reports.Sum(p => p.TotalFine);
                    App.IndividualVmd?.setProperty();
                }
            }
        }

        // Xóa thông tin khi đăng xuất
        public void Clear()
            {
                Account = null!;
                Police = null!;
                Person = null!;
            }
        
    }
}
