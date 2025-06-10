namespace Asset_management.models
{
    public class Imprest
    {
        public int Id { get; set; }
        public string Officer { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateIssued { get; set; }
        public string Purpose { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}
