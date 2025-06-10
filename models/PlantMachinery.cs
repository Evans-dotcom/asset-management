namespace Asset_management.models
{
    public class PlantMachinery
    {
        public int Id { get; set; }
        public string EquipmentName { get; set; }
        public string SerialNumber { get; set; }
        public string MakeModel { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Value { get; set; }
        public string Location { get; set; }
        public string OperationalStatus { get; set; }
    }
}
