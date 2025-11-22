namespace Asset_management.DTOs
{
    public class BankAccountCreateDto
    {
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal OpeningBalance { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public string DepartmentUnit { get; set; }
        public string AccountName { get; set; }
        public DateTimeOffset? ContractDate { get; set; }
        public string OfficerInCharge { get; set; }
        public string Signatories { get; set; }
    }

    public class BankAccountApproveDto
    {
        public bool Approve { get; set; }      // true => approve, false => reject
        public string Remarks { get; set; }    // optional approval/rejection remark
    }
}
