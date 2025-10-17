using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class AssetReconciliation
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateReconciled { get; set; } = DateTimeOffset.UtcNow;
        public int PhysicalCount { get; set; }
        public int SystemCount { get; set; }
        public string ReconciledBy { get; set; }
        public string Discrepancy { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset ContractDate { get; set; } = DateTimeOffset.UtcNow;
    }
}
