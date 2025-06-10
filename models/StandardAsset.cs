namespace Asset_management.models
{
    public class StandardAsset
    {
        public int Id { get; set; }
        public string AssetDescription { get; set; }
        public string SerialNumber { get; set; }
        public string MakeModel { get; set; }
        public string TagNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string PvNumber { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal DepreciationRate { get; set; }
        public decimal AnnualDepreciation { get; set; }
        public decimal AccumulatedDepreciation { get; set; }
        public decimal NetBookValue { get; set; }
        public string ResponsibleOfficer { get; set; }
        public string Location { get; set; }
        public string AssetCondition { get; set; }
        public string Notes { get; set; }
    }
}
