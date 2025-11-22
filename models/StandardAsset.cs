using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    

    public class StandardAsset
    {
        public int Id { get; set; }
        public string AssetDescription { get; set; }
        public string SerialNumber { get; set; }
        public string MakeModel { get; set; }
        public string TagNumber { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DeliveryDate { get; set; } = DateTimeOffset.UtcNow;
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset ContractDate { get; set; } = DateTimeOffset.UtcNow;
        public string PvNumber { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal DepreciationRate { get; set; }
        public decimal AnnualDepreciation { get; set; }
        public decimal AccumulatedDepreciation { get; set; }
        public decimal NetBookValue { get; set; }
        public string ResponsibleOfficer { get; set; }
        public string Location { get; set; }
        public string AssetCondition { get; set; }
        public string Notes { get; set; }
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
        public class StandardAssetCreateDto
        {
            public string AssetDescription { get; set; }
            public string SerialNumber { get; set; }
            public string MakeModel { get; set; }
            public string TagNumber { get; set; }
            public DateTimeOffset DeliveryDate { get; set; }
            public DateTimeOffset ContractDate { get; set; }
            public string PvNumber { get; set; }
            public decimal PurchaseAmount { get; set; }
            public decimal DepreciationRate { get; set; }
            public decimal AnnualDepreciation { get; set; }
            public decimal AccumulatedDepreciation { get; set; }
            public decimal NetBookValue { get; set; }
            public string ResponsibleOfficer { get; set; }
            public string Location { get; set; }
            public string AssetCondition { get; set; }
            public string Notes { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
        }

        public class StandardAssetApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
