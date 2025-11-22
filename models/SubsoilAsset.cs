using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
   

    public class SubsoilAsset
    {
        public int Id { get; set; }
        public string ResourceType { get; set; }
        public string Location { get; set; }
        public string EstimatedVolume { get; set; }
        public string OwnershipStatus { get; set; }
        public decimal ValueEstimate { get; set; }
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
        public class SubsoilAssetCreateDto
        {
            public string ResourceType { get; set; }
            public string Location { get; set; }
            public string EstimatedVolume { get; set; }
            public string OwnershipStatus { get; set; }
            public decimal ValueEstimate { get; set; }
            public string Remarks { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
        }

        public class SubsoilAssetApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
