using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    public class Account
    {
        [Key] public string? LoginId { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
