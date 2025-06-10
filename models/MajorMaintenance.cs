namespace Asset_management.models
{
    public class MajorMaintenance
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string MaintenanceType { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateCompleted { get; set; }
        public decimal Cost { get; set; }
        public string Remarks { get; set; }
    }
}
