using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class IntangibleAsset
    {
        public int Id { get; set; }
        public string AssetType { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateAcquired { get; set; } = DateTimeOffset.UtcNow;
        public decimal Value { get; set; }
        public int UsefulLifeYears { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
    }
}
