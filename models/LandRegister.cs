using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class LandRegister
    {
        public int Id { get; set; }
        public string ParcelNumber { get; set; }
        public string Location { get; set; }
        public decimal Acreage { get; set; }
        public string TitleDeedNumber { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateAcquired { get; set; } = DateTimeOffset.UtcNow;

        public string OwnershipStatus { get; set; }
        public decimal LandValue { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }

        public string RequestedBy { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset RequestedAt { get; set; } = DateTimeOffset.UtcNow;

        public string? ApprovedBy { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset? ApprovalDate { get; set; }

        public string? ApprovalRemarks { get; set; }

        public LandRegisterStatus Status { get; set; } = LandRegisterStatus.Pending;

        public enum LandRegisterStatus
        {
            Pending,
            Approved,
            Rejected
        }

        public class LandRegisterCreateDto
        {
            public string ParcelNumber { get; set; }
            public string Location { get; set; }
            public decimal Acreage { get; set; }
            public string TitleDeedNumber { get; set; }
            public DateTimeOffset DateAcquired { get; set; }
            public string OwnershipStatus { get; set; }
            public decimal LandValue { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
        }

        public class LandRegisterApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
