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

        public string RequestedBy { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset RequestedAt { get; set; } = DateTimeOffset.UtcNow;

        public string? ApprovedBy { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset? ApprovalDate { get; set; }

        public string? ApprovalRemarks { get; set; }

        public AssetStatus Status { get; set; } = AssetStatus.Pending;

        public enum AssetStatus
        {
            Pending,
            Approved,
            Rejected
        }
        public class MajorMaintenanceCreateDto
        {
            public int AssetId { get; set; }
            public string MaintenanceType { get; set; }
            public DateTimeOffset DateStarted { get; set; }
            public DateTimeOffset DateCompleted { get; set; }
            public decimal Cost { get; set; }
            public string Remarks { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
        }

        public class MajorMaintenanceApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
