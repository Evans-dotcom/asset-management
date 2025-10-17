using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class RoadsInfrastructure
    {
        public int Id { get; set; }
        public string RoadName { get; set; }
        public string Location { get; set; }
        public decimal LengthKm { get; set; }
        public decimal ConstructionCost { get; set; }
        public int YearConstructed { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset ContractDate { get; set; } = DateTimeOffset.UtcNow;
    }
}
