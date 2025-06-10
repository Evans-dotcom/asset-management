namespace Asset_management.models
{
    public class LossesRegister
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public DateTime LossDate { get; set; }
        public string Cause { get; set; }
        public string ReportedBy { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}
