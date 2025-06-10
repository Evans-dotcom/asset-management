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
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public string Location { get; set; }
        public string ResponsibleOfficer { get; set; }
    }
}
