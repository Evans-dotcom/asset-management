using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class Imprest
    {
        public int Id { get; set; }
        public string Officer { get; set; }
        public decimal Amount { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateIssued { get; set; } = DateTimeOffset.UtcNow;
        public string Purpose { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
        //public DateTime ContractDate { get; set; }
    }
}
