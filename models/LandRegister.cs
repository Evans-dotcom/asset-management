using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class LandRegister
    {
        public int Id { get; set; }
        public string ParcelNumber { get; set; }
        public string Location { get; set; }
        public decimal Acreage { get; set; }
        public string TitleDeedNumber { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateAcquired { get; set; } = DateTimeOffset.UtcNow;
        public string OwnershipStatus { get; set; }
        public decimal LandValue { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
    }
}
