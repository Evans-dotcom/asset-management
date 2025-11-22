using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class AssetHandover
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string FromEmployee { get; set; }
        public string ToEmployee { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset ContractDate { get; set; } = DateTimeOffset.UtcNow;
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateHandedOver { get; set; } = DateTimeOffset.UtcNow;
        public string Condition { get; set; }
        public string Remarks { get; set; }
        public string RequestedBy { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset RequestedAt { get; set; } = DateTimeOffset.UtcNow;

        public string? ApprovedBy { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset? ApprovalDate { get; set; }
        public string? ApprovalRemarks { get; set; }

        public class AssetHandoverCreateDto
        {
            public int AssetId { get; set; }
            public string FromEmployee { get; set; }
            public string ToEmployee { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
            public DateTimeOffset? ContractDate { get; set; }
            public DateTimeOffset? DateHandedOver { get; set; }
            public string Condition { get; set; }
            public string Remarks { get; set; }
            public string RequestedBy { get; set; }
        }

        public class AssetHandoverApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
