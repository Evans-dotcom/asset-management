namespace Asset_management.services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string[] to, string subject, string htmlBody, string plainBody = null);
        Task NotifyAssetCreatedAsync(string assetType, int assetId, string assetSummary, string requesterEmail);
        Task NotifyAssetApprovalAsync(string assetType, int assetId, string assetSummary, string requesterEmail, bool approved, string remarks, string actedBy);
    }
}
