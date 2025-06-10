namespace Asset_management.models
{
    public class WorkInProgress
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedCompletion { get; set; }
        public decimal CurrentValue { get; set; }
        public string Remarks { get; set; }
    }
}
