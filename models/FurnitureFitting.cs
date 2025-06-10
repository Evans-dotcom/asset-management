namespace Asset_management.models
{
    public class FurnitureFitting
    {
        public int Id { get; set; }
        public string ItemDescription { get; set; }
        public string SerialNumber { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchaseCost { get; set; }
        public string ResponsibleOfficer { get; set; }
        public string Condition { get; set; }
    }
}
