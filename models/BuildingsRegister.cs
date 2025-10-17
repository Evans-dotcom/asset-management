using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class BuildingsRegister
    {
        public int Id { get; set; }
        public string BuildingName { get; set; }
        public string Location { get; set; }
        public string UsePurpose { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateConstructed { get; set; } = DateTimeOffset.UtcNow;
        public decimal ConstructionCost { get; set; }
        public decimal Depreciation { get; set; }
        public decimal NetBookValue { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
    }
}
