using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class WorkInProgress
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset StartDate { get; set; } = DateTimeOffset.UtcNow;
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset ExpectedCompletion { get; set; } = DateTimeOffset.UtcNow;
        public decimal CurrentValue { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
    }
}
