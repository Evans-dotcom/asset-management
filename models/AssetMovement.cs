using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class AssetMovement
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateMoved { get; set; } = DateTimeOffset.UtcNow;
        public string MovedBy { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset ContractDate { get; set; } = DateTimeOffset.UtcNow;
    }
}
