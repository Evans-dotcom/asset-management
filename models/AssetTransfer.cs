using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class AssetTransfer
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string FromDepartment { get; set; }
        public string ToDepartment { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateTransferred { get; set; } = DateTimeOffset.UtcNow;
        public string ApprovedBy { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
        public string RequestedBy { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset RequestedAt { get; set; } = DateTimeOffset.UtcNow;
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset? ApprovalDate { get; set; }
        public string? ApprovalRemarks { get; set; }

        public class AssetTransferCreateDto
        {
            public int AssetId { get; set; }
            public string FromDepartment { get; set; }
            public string ToDepartment { get; set; }
            public DateTimeOffset? DateTransferred { get; set; }
            public string Remarks { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
            public string RequestedBy { get; set; }
        }

        public class AssetTransferApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
