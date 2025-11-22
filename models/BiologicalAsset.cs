using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class BiologicalAsset
    {
        public int Id { get; set; }
        public string AssetType { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset? AcquisitionDate { get; set; } = DateTimeOffset.UtcNow;
        public string Location { get; set; }
        public decimal Value { get; set; }
        public string Notes { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset? ContractDate { get; set; } = DateTimeOffset.UtcNow;
        public string RequestedBy { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset? RequestedAt { get; set; } = DateTimeOffset.UtcNow;
        public string? ApprovedBy { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset? ApprovalDate { get; set; }
        public string? ApprovalRemarks { get; set; }

        public class BiologicalAssetCreateDto
        {
            public string AssetType { get; set; }
            public int Quantity { get; set; }
            public DateTimeOffset? AcquisitionDate { get; set; }
            public string Location { get; set; }
            public decimal Value { get; set; }
            public string Notes { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
            public DateTimeOffset? ContractDate { get; set; }
            public string RequestedBy { get; set; }
        }

        public class BiologicalAssetApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
