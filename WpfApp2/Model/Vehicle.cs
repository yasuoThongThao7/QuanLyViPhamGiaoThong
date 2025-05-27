using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    //Loại xe
    public class Type
    {
        [Key]public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class Brand
    {
        [Key] public int Id { get; set; }
        public string? Name { get; set; }
    }
    //Phương tiện
    public class Vehicle
    {
        [Key] public string Id { get; set; }
        public int BrandId { get; set; }
        public int TypeId { get; set; }
        public string OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual Person Person { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand? Brand { get; set; }
        [ForeignKey("TypeId")]
        public virtual Type? Type { get; set; }
    }

}
