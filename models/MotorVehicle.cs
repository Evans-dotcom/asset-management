using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class MotorVehicle
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int YearOfManufacture { get; set; }
        public string EngineNumber { get; set; }
        public string ChassisNumber { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset PurchaseDate { get; set; } = DateTimeOffset.UtcNow;
        public decimal PurchasePrice { get; set; }
        public string Location { get; set; }
        public string ResponsibleOfficer { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
    }
}
