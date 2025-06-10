namespace Asset_management.models
{
    public class EquipmentSignout
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public string IssuedTo { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public string ConditionOnReturn { get; set; }
        public string Remarks { get; set; }
    }
}
