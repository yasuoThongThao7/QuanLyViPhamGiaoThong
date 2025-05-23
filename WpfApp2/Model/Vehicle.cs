using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    //Loại xe
    public class VehicleType
    {
        [Key]public string? Id { get; set; }
        public string? Name { get; set; }
    }

    public class Brand
    {
        [Key] public string? Id { get; set; }
        public string? Name { get; set; }
    }
    //Phương tiện
    public class Vehicle
    {
        [Key] public string? Id { get; set; }
        public int BrandId { get; set; }
        public int TypeId { get; set; }
        public string? OwnerId { get; set; }
    }

}
