using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Model
{
    public class Violation
    {
        public int Id { get; set; }
        public string? Description { get; set; }//Mô tả
        public int FineAmount { get; set; }//Mức phạt

    }

}
