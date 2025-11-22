using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class EquipmentSignout
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public string IssuedTo { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateIssued { get; set; } = DateTimeOffset.UtcNow;
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset ExpectedReturnDate { get; set; } = DateTimeOffset.UtcNow;
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset ActualReturnDate { get; set; } = DateTimeOffset.UtcNow;
        public string ConditionOnReturn { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
        public string RequestedBy { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset? RequestedAt { get; set; } = DateTimeOffset.UtcNow;
        public string? ApprovedBy { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset? ApprovalDate { get; set; }
        public string? ApprovalRemarks { get; set; }

        public class EquipmentSignoutCreateDto
        {
            public int EquipmentId { get; set; }
            public string IssuedTo { get; set; }
            public DateTimeOffset? DateIssued { get; set; }
            public DateTimeOffset? ExpectedReturnDate { get; set; }
            public DateTimeOffset? ActualReturnDate { get; set; }
            public string ConditionOnReturn { get; set; }
            public string Remarks { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
            public string RequestedBy { get; set; }
        }

        public class EquipmentSignoutApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
