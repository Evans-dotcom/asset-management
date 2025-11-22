using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
   

    public class Litigation
    {
        public int Id { get; set; }
        public string CaseNumber { get; set; }
        public string PartiesInvolved { get; set; }
        public string Subject { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateFiled { get; set; } = DateTimeOffset.UtcNow;

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
        public class LitigationCreateDto
        {
            public string CaseNumber { get; set; }
            public string PartiesInvolved { get; set; }
            public string Subject { get; set; }
            public DateTimeOffset DateFiled { get; set; }
            public string Remarks { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
        }

        public class LitigationApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
