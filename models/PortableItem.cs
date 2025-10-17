using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class PortableItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string TagNumber { get; set; }
        public string AssignedTo { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
        public string Location { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateIssued { get; set; } = DateTimeOffset.UtcNow;
        public string Condition { get; set; }
        public string Remarks { get; set; }
    }
}
