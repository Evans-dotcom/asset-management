namespace Asset_management.models
{
    public class IntangibleAsset
    {
        public int Id { get; set; }
        public string AssetType { get; set; }
        public string Description { get; set; }
        public DateTime DateAcquired { get; set; }
        public decimal Value { get; set; }
        public int UsefulLifeYears { get; set; }
    }
}
