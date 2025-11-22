using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
   

    public class RoadsInfrastructure
    {
        public int Id { get; set; }
        public string RoadName { get; set; }
        public string Location { get; set; }
        public decimal LengthKm { get; set; }
        public decimal ConstructionCost { get; set; }
        public int YearConstructed { get; set; }
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
        public RDAssetStatus Status { get; set; } = RDAssetStatus.Pending;

        public enum RDAssetStatus
        {
            Pending,
            Approved,
            Rejected
        }
        public class RoadsInfrastructureCreateDto
        {
            public string RoadName { get; set; }
            public string Location { get; set; }
            public decimal LengthKm { get; set; }
            public decimal ConstructionCost { get; set; }
            public int YearConstructed { get; set; }
            public string Remarks { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
        }

        public class RoadsInfrastructureApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
