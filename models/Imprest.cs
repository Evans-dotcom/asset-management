using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
   

    public class Imprest
    {
        public int Id { get; set; }
        public string Officer { get; set; }
        public decimal Amount { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateIssued { get; set; } = DateTimeOffset.UtcNow;

        public string Purpose { get; set; }
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
        public class ImprestCreateDto
        {
            public string Officer { get; set; }
            public decimal Amount { get; set; }
            public DateTimeOffset DateIssued { get; set; }
            public string Purpose { get; set; }
            public string Remarks { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
            public string RequestedBy { get; set; }
        }

        public class ImprestApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
