using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class StandardAsset
    {
        public int Id { get; set; }
        public string AssetDescription { get; set; }
        public string SerialNumber { get; set; }
        public string MakeModel { get; set; }
        public string TagNumber { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DeliveryDate { get; set; } = DateTimeOffset.UtcNow;
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset ContractDate { get; set; }= DateTimeOffset.UtcNow;
        // public DateTime DeliveryDate { get; set; }
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

        // New fields
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
       // public DateTime ContractDate { get; set; }
    }
}
