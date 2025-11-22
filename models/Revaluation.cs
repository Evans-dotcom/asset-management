using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    
    public class LossesRegister
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset LossDate { get; set; } = DateTimeOffset.UtcNow;
        public string Cause { get; set; }
        public string ReportedBy { get; set; }
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

        public class LossesRegisterCreateDto
        {
            public int AssetId { get; set; }
            public DateTimeOffset LossDate { get; set; }
            public string Cause { get; set; }
            public string ReportedBy { get; set; }
            public string Remarks { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
        }

        public class LossesRegisterApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
