namespace Asset_management.models
{
    public class AccountsPayable
    {
        public int Id { get; set; }
        public required string CreditorName { get; set; }
        public decimal AmountDue { get; set; }
        public DateTime DueDate { get; set; }
        public required string Reason { get; set; }
        public required string Remarks { get; set; }
    }
}
