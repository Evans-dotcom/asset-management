namespace Asset_management.models
{
    public class OtherInfrastructure
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public string Remarks { get; set; }
    }
}
