using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
   
    public class StocksRegister
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalValue { get; set; }
        public string Location { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset LastRestocked { get; set; } = DateTimeOffset.UtcNow;
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

        public class StocksRegisterCreateDto
        {
            public string ItemName { get; set; }
            public string Unit { get; set; }
            public int Quantity { get; set; }
            public decimal UnitCost { get; set; }
            public decimal TotalValue { get; set; }
            public string Location { get; set; }
            public DateTimeOffset LastRestocked { get; set; }
            public string Remarks { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
        }

        public class StocksRegisterApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
