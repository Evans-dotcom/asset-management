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
        public string RequestedBy { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset RequestedAt { get; set; } = DateTimeOffset.UtcNow;
        public string? ApprovedBy { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset? ApprovalDate { get; set; }
        public string? ApprovalRemarks { get; set; }

        public class AssetReconciliationCreateDto
        {
            public int AssetId { get; set; }
            public DateTimeOffset? DateReconciled { get; set; }
            public int PhysicalCount { get; set; }
            public int SystemCount { get; set; }
            public string ReconciledBy { get; set; }
            public string Discrepancy { get; set; }
            public string Remarks { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
            public DateTimeOffset? ContractDate { get; set; }
            public string RequestedBy { get; set; }
        }

        public class AssetReconciliationApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
