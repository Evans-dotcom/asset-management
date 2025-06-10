namespace Asset_management.models
{
    public class BiologicalAsset
    {
        public int Id { get; set; }
        public string AssetType { get; set; }
        public int Quantity { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public string Location { get; set; }
        public decimal Value { get; set; }
        public string Notes { get; set; }
    }
}
