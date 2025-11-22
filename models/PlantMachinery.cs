using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
   

    public class PlantMachinery
    {
        public int Id { get; set; }
        public string EquipmentName { get; set; }
        public string SerialNumber { get; set; }
        public string MakeModel { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset PurchaseDate { get; set; } = DateTimeOffset.UtcNow;
        public decimal Value { get; set; }
        public string Location { get; set; }
        public string OperationalStatus { get; set; }
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
        public class PlantMachineryCreateDto
        {
            public string EquipmentName { get; set; }
            public string SerialNumber { get; set; }
            public string MakeModel { get; set; }
            public DateTimeOffset PurchaseDate { get; set; }
            public decimal Value { get; set; }
            public string Location { get; set; }
            public string OperationalStatus { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
        }

        public class PlantMachineryApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
