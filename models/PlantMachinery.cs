using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class PlantMachinery
    {
        public int Id { get; set; }
        public string EquipmentName { get; set; }
        public string SerialNumber { get; set; }
        public string MakeModel { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset PurchaseDate { get; set; } = DateTimeOffset.UtcNow;
        public decimal Value { get; set; }
        public string Location { get; set; }
        public string OperationalStatus { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
    }
}
