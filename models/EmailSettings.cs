namespace Asset_management.models
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public bool UseSsl { get; set; }
        public string[] AdminEmails { get; set; }
    }
}
