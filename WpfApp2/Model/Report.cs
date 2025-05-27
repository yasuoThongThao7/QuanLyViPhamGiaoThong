using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    public class Report
    {
        [Key]public int Id { get; set; }
        public string? PersonId { get; set; }   // CCCD người vi phạm
        public string? PoliceId { get; set; }   // Mã công an
        public string? VehicleId { get; set; }      // Xe bị phạt
        public DateTime Date { get; set; }      // Ngày lập biên bản
        public string? Location { get; set; }   // Địa điểm vi phạm
        public int TotalFine { get; set; }      // Tổng số tiền phạt
        public bool IsPaid { get; set; }          // Đã nộp phạt hay chưa (true/false)
        public DateTime? PaidDate { get; set; } // Ngày nộp phạt

        // Liên kết
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; }
        [ForeignKey("PoliceId")]
        public virtual Police Police { get; set; }
        public virtual ICollection<ReportInfo>? Violations { get; set; }  // Danh sách lỗi vi phạm
    }

}
