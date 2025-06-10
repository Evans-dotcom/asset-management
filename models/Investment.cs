namespace Asset_management.models
{
    public class Investment
    {
        public int Id { get; set; }
        public string InvestmentType { get; set; }
        public string Institution { get; set; }
        public DateTime DateInvested { get; set; }
        public decimal Amount { get; set; }
        public decimal ExpectedReturn { get; set; }
        public string Remarks { get; set; }
    }
}
