namespace Asset_management.models
{
    public class AssetTransfer
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string FromDepartment { get; set; }
        public string ToDepartment { get; set; }
        public DateTime DateTransferred { get; set; }
        public string ApprovedBy { get; set; }
        public string Remarks { get; set; }
    }
}
