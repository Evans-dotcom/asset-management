using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class MajorMaintenance
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string MaintenanceType { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateStarted { get; set; } = DateTimeOffset.UtcNow;
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateCompleted { get; set; } = DateTimeOffset.UtcNow;
        public decimal Cost { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
    }
}
