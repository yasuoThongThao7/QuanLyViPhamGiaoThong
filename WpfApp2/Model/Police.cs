using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    public class Police
    {
        [Key]public string? MilitaryID { get; set; }
        public string? Ranks { get; set; }
        public string? Unit { get; set; }

        public virtual Person? Person { get; set; }
        public virtual ICollection<Report>? Reports { get; set; }
    }
}
