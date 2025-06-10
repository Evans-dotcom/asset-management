namespace Asset_management.models
{
    public class AssetReconciliation
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public DateTime DateReconciled { get; set; }
        public int PhysicalCount { get; set; }
        public int SystemCount { get; set; }
        public string ReconciledBy { get; set; }
        public string Discrepancy { get; set; }
        public string Remarks { get; set; }
    }
}
