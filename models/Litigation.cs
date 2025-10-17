using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class Litigation
    {
        public int Id { get; set; }
        public string CaseNumber { get; set; }
        public string PartiesInvolved { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateFiled { get; set; } = DateTimeOffset.UtcNow;
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
    }
}
