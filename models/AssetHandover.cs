namespace Asset_management.models
{
    public class AssetHandover
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string FromEmployee { get; set; }
        public string ToEmployee { get; set; }
        public string Department { get; set; }
        public DateTime DateHandedOver { get; set; }
        public string Condition { get; set; }
        public string Remarks { get; set; }
    }
}
