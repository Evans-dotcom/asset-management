namespace Asset_management.models
{
    public class Litigation
    {
        public int Id { get; set; }
        public string CaseNumber { get; set; }
        public string PartiesInvolved { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public DateTime DateFiled { get; set; }
        public string Remarks { get; set; }
    }
}
