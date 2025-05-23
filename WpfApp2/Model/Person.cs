using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    public class Person
    {
            [Key]public string? CCCD { get; set; }
            public string? Name { get; set; }
            public string? Address { get; set; }
            public DateTime? birth { get; set; }
            public string? PhoneNumber { get; set; }
            
            public virtual ICollection<Report>? Reports { get; set; }
 
    }
}
