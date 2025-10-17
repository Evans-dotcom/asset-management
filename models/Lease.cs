using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class Lease
    {
        public int Id { get; set; }
        public string LeaseItem { get; set; }
        public string Lessor { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset LeaseStart { get; set; } = DateTimeOffset.UtcNow;
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset LeaseEnd { get; set; } = DateTimeOffset.UtcNow;
        public decimal LeaseCost { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
    }
}
