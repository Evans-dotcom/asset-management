using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class AccountsReceivable
    {
        public int Id { get; set; }
        public required string DebtorName { get; set; }
        public decimal AmountDue { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DueDate { get; set; } = DateTimeOffset.UtcNow;
        public string Reason { get; set; }
        public string Remarks { get; set; }
        public string RequestedBy { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset RequestedAt { get; set; } = DateTimeOffset.UtcNow;

        public string? ApprovedBy { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset? ApprovalDate { get; set; }
        public string? ApprovalRemarks { get; set; }

        public class AccountsReceivableCreateDto
        {
            public string DebtorName { get; set; }
            public decimal AmountDue { get; set; }
            public DateTimeOffset DueDate { get; set; }
            public string Reason { get; set; }
            public string Remarks { get; set; }
            public string RequestedBy { get; set; }
        }

        public class AccountsReceivableApproveDto
        {
            public bool Approve { get; set; }
            public string Remarks { get; set; }
        }
    }
}
