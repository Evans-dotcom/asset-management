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
       // public DateTime DueDate { get; set; }
        public string Reason { get; set; }
        public string Remarks { get; set; }
    }
}
