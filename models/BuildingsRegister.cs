using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class BuildingsRegister
    {
        public int Id { get; set; }
        public string BuildingName { get; set; }
        public string Location { get; set; }
        public string UsePurpose { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateConstructed { get; set; } = DateTimeOffset.UtcNow;
        public decimal ConstructionCost { get; set; }
        public decimal Depreciation { get; set; }
        public decimal NetBookValue { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
        public string RequestedBy { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset RequestedAt { get; set; } = DateTimeOffset.UtcNow;
        public string? ApprovedBy { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset? ApprovalDate { get; set; }
        public string? ApprovalRemarks { get; set; }

        public class BuildingsRegisterCreateDto
        {
            public string BuildingName { get; set; }
            public string Location { get; set; }
            public string UsePurpose { get; set; }
            public DateTimeOffset? DateConstructed { get; set; }
            public decimal ConstructionCost { get; set; }
            public decimal Depreciation { get; set; }
            public decimal NetBookValue { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
            public string RequestedBy { get; set; }
        }

        public class BuildingsRegisterApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
