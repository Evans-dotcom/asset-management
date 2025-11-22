using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    

    public class MotorVehicle
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int YearOfManufacture { get; set; }
        public string EngineNumber { get; set; }
        public string ChassisNumber { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset PurchaseDate { get; set; } = DateTimeOffset.UtcNow;

        public decimal PurchasePrice { get; set; }
        public string Location { get; set; }
        public string ResponsibleOfficer { get; set; }
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
        public class MotorVehicleCreateDto
        {
            public string RegistrationNumber { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public int YearOfManufacture { get; set; }
            public string EngineNumber { get; set; }
            public string ChassisNumber { get; set; }
            public DateTimeOffset PurchaseDate { get; set; }
            public decimal PurchasePrice { get; set; }
            public string Location { get; set; }
            public string ResponsibleOfficer { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
        }

        public class MotorVehicleApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
