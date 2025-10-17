using NuGet.Packaging.Signing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asset_management.models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public string Remarks { get; set; }

        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
        public string AccountName { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset ContractDate { get; set; } = DateTimeOffset.UtcNow;
        //public Timestamp ContractDate { get; set; }
        public string OfficerInCharge { get; set; }
        public string Signatories { get; set; }
    }
}
