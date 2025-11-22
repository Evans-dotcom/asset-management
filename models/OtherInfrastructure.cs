using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
   

    public class OtherInfrastructure
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset AcquisitionDate { get; set; } = DateTimeOffset.UtcNow;

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
        public class OtherInfrastructureCreateDto
        {
            public string AssetName { get; set; }
            public string Location { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public DateTimeOffset AcquisitionDate { get; set; }
            public string Remarks { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
        }

        public class OtherInfrastructureApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
