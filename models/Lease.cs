using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{


    public class Lease
    {
        public int Id { get; set; }
        public string LeaseItem { get; set; }
        public string Lessor { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset LeaseStart { get; set; } = DateTimeOffset.UtcNow;

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset LeaseEnd { get; set; } = DateTimeOffset.UtcNow;

        public decimal LeaseCost { get; set; }
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

        public LeaseAssetStatus Status { get; set; } = LeaseAssetStatus.Pending;

        public enum LeaseAssetStatus
        {
            Pending,
            Approved,
            Rejected
        }
        public class LeaseCreateDto
        {
            public string LeaseItem { get; set; }
            public string Lessor { get; set; }
            public DateTimeOffset LeaseStart { get; set; }
            public DateTimeOffset LeaseEnd { get; set; }
            public decimal LeaseCost { get; set; }
            public string Remarks { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
            public string RequestedBy { get; set; }
        }

        public class LeaseApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
