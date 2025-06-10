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
    }
}
