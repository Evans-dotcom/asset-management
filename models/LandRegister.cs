namespace Asset_management.models
{
    public class LandRegister
    {
        public int Id { get; set; }
        public string ParcelNumber { get; set; }
        public string Location { get; set; }
        public decimal Acreage { get; set; }
        public string TitleDeedNumber { get; set; }
        public DateTime DateAcquired { get; set; }
        public string OwnershipStatus { get; set; }
        public decimal LandValue { get; set; }
    }

}
