using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
   

    public class IntangibleAsset
    {
        public int Id { get; set; }
        public string AssetType { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateAcquired { get; set; } = DateTimeOffset.UtcNow;

        public decimal Value { get; set; }
        public int UsefulLifeYears { get; set; }

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
        public class IntangibleAssetCreateDto
        {
            public string AssetType { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public int UsefulLifeYears { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
        }

        public class IntangibleAssetApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
