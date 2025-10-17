using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class AccountsPayable
    {
        public int Id { get; set; }
        public required string CreditorName { get; set; }
        public decimal AmountDue { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DueDate { get; set; } = DateTimeOffset.UtcNow;
        public required string Reason { get; set; }
        public required string Remarks { get; set; }
    }
}
