using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class AssetMovement
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateMoved { get; set; } = DateTimeOffset.UtcNow;
        public string MovedBy { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset ContractDate { get; set; } = DateTimeOffset.UtcNow;
        public string RequestedBy { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset RequestedAt { get; set; } = DateTimeOffset.UtcNow;

        public string? ApprovedBy { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset? ApprovalDate { get; set; }
        public string? ApprovalRemarks { get; set; }

        public class AssetMovementCreateDto
        {
            public int AssetId { get; set; }
            public string FromLocation { get; set; }
            public string ToLocation { get; set; }
            public DateTimeOffset? DateMoved { get; set; }
            public string MovedBy { get; set; }
            public string Remarks { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
            public DateTimeOffset? ContractDate { get; set; }
            public string RequestedBy { get; set; }
        }

        public class AssetMovementApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
