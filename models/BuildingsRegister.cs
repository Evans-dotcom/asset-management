namespace Asset_management.models
{
    public class BuildingsRegister
    {
        public int Id { get; set; }
        public string BuildingName { get; set; }
        public string Location { get; set; }
        public string UsePurpose { get; set; }
        public DateTime DateConstructed { get; set; }
        public decimal ConstructionCost { get; set; }
        public decimal Depreciation { get; set; }
        public decimal NetBookValue { get; set; }
    }
}
