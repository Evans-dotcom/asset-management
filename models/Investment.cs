using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class Investments
    {
        public int Id { get; set; }
        public string InvestmentType { get; set; }
        public string Institution { get; set; }
        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateInvested { get; set; } = DateTimeOffset.UtcNow;
        public decimal Amount { get; set; }
        public decimal ExpectedReturn { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
    }
}
