namespace Asset_management.models
{
    public class SubsoilAsset
    {
        public int Id { get; set; }
        public string ResourceType { get; set; }
        public string Location { get; set; }
        public string EstimatedVolume { get; set; }
        public string OwnershipStatus { get; set; }
        public decimal ValueEstimate { get; set; }
        public string Remarks { get; set; }
    }
}
