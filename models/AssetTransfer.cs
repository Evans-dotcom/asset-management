using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class AssetTransfer
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string FromDepartment { get; set; }
        public string ToDepartment { get; set; }
        [Column(TypeName = "timestamptz")] 
        public DateTimeOffset DateTransferred { get; set; } = DateTimeOffset.UtcNow;
        public string ApprovedBy { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
    }
}
