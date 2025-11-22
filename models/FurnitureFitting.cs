using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
   

    public class FurnitureFitting
    {
        public int Id { get; set; }

        [Required]
        public string ItemDescription { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string DepartmentUnit { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset PurchaseDate { get; set; } = DateTimeOffset.UtcNow;

        [Required]
        public decimal PurchaseCost { get; set; }

        [Required]
        public string ResponsibleOfficer { get; set; }

        [Required]
        public string Condition { get; set; }

        [Required]
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
        // DTOs
        public class FurnitureFittingCreateDto
        {
            [Required] public string ItemDescription { get; set; }
            [Required] public string SerialNumber { get; set; }
            [Required] public int Quantity { get; set; }
            [Required] public string Location { get; set; }
            [Required] public string Department { get; set; }
            [Required] public string DepartmentUnit { get; set; }
            [Required] public DateTimeOffset PurchaseDate { get; set; }
            [Required] public decimal PurchaseCost { get; set; }
            [Required] public string ResponsibleOfficer { get; set; }
            [Required] public string Condition { get; set; }
        }

        public class FurnitureFittingApproveDto
        {
            public bool Approve { get; set; }
            [Required] public string Remarks { get; set; }
        }
    }
}
