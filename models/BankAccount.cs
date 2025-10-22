using System;
using System.ComponentModel.DataAnnotations.Schema;
using Asset_management.NewFolder;

namespace Asset_management.models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
        public string AccountName { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset ContractDate { get; set; } = DateTimeOffset.UtcNow;
        public string OfficerInCharge { get; set; }
        public string Signatories { get; set; }
        // Approval workflow fields
        public BankAccountStatus Status { get; set; } = BankAccountStatus.Open;

        public string RequestedBy { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset RequestedAt { get; set; } = DateTimeOffset.UtcNow;

        public string? ApprovedBy { get; set; }            // admin who approved/rejected
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset? ApprovalDate { get; set; }

        public string? ApprovalRemarks { get; set; }       
    }
}
