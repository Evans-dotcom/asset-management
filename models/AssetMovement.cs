namespace Asset_management.models
{
    public class AssetMovement
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public DateTime DateMoved { get; set; }
        public string MovedBy { get; set; }
        public string Remarks { get; set; }
    }
}
