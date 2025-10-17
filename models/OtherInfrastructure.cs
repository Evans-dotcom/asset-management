using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class OtherInfrastructure
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset AcquisitionDate { get; set; } = DateTimeOffset.UtcNow;
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
    }
}
