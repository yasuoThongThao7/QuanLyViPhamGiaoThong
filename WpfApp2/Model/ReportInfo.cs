using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    public class ReportInfo
    {
        [Key]public int? ReportId { get; set; }
        public int? ViolationId { get; set; }
        [ForeignKey("ReportId")]
        public virtual Report? Report { get; set; }

        [ForeignKey("ViolationId")]
        public virtual Violation? Violation { get; set; }
    }

}
