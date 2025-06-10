namespace Asset_management.models
{
    public class AccountsReceivable
    {
        public int Id { get; set; }
        public required string DebtorName { get; set; }
        public decimal AmountDue { get; set; }
        public DateTime DueDate { get; set; }
        public string Reason { get; set; }
        public string Remarks { get; set; }
    }
}
