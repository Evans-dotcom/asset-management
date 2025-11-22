using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class WorkInProgress
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset StartDate { get; set; } = DateTimeOffset.UtcNow;
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset ExpectedCompletion { get; set; } = DateTimeOffset.UtcNow;
        public decimal CurrentValue { get; set; }
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

        public class WorkInProgressCreateDto
        {
            public string ProjectName { get; set; }
            public DateTimeOffset StartDate { get; set; }
            public DateTimeOffset ExpectedCompletion { get; set; }
            public decimal CurrentValue { get; set; }
            public string Remarks { get; set; }
            public string Department { get; set; }
            public string DepartmentUnit { get; set; }
            public string RequestedBy { get; set; }
        }

        public class WorkInProgressApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
