using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
   

    public class Investments
    {
        public int Id { get; set; }
        public string InvestmentType { get; set; }
        public string Institution { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateInvested { get; set; } = DateTimeOffset.UtcNow;

        public decimal Amount { get; set; }
        public decimal ExpectedReturn { get; set; }
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
        public class InvestmentsCreateDto
        {
            public string InvestmentType { get; set; }
            public string Institution { get; set; }
            public DateTimeOffset DateInvested { get; set; }
            public decimal Amount { get; set; }
            public decimal ExpectedReturn { get; set; }
            public string Remarks { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
        }

        public class InvestmentsApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
