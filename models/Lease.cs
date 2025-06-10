namespace Asset_management.models
{
    public class Lease
    {
        public int Id { get; set; }
        public string LeaseItem { get; set; }
        public string Lessor { get; set; }
        public DateTime LeaseStart { get; set; }
        public DateTime LeaseEnd { get; set; }
        public decimal LeaseCost { get; set; }
        public string Remarks { get; set; }
    }
}
