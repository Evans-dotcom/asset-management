using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class LossesRegister
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset LossDate { get; set; } = DateTimeOffset.UtcNow;
        public string Cause { get; set; }
        public string ReportedBy { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
    }
}
