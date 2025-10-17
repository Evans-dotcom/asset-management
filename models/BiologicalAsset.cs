using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class BiologicalAsset
    {
        public int Id { get; set; }
        public string AssetType { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset AcquisitionDate { get; set; } = DateTimeOffset.UtcNow;
        public string Location { get; set; }
        public decimal Value { get; set; }
        public string Notes { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset ContractDate { get; set; } = DateTimeOffset.UtcNow;
    }
}
