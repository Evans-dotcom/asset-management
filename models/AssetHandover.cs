using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class AssetHandover
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string FromEmployee { get; set; }
        public string ToEmployee { get; set; }
        public string Department { get; set; } 
        public string DepartmentUnit { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset ContractDate { get; set; } = DateTimeOffset.UtcNow;
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateHandedOver { get; set; } = DateTimeOffset.UtcNow;
        public string Condition { get; set; }
        public string Remarks { get; set; }
    }
}
