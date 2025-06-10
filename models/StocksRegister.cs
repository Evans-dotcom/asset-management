namespace Asset_management.models
{
    public class StocksRegister
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalValue { get; set; }
        public string Location { get; set; }
        public DateTime LastRestocked { get; set; }
        public string Remarks { get; set; }
    }
}
