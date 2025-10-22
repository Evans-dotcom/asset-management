using System;
using System.Linq;
using System.Threading.Tasks;
using Asset_management.models;
using Asset_management.services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Asset_management.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string[] to, string subject, string htmlBody, string plainBody = null)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            foreach (var recipient in to.Distinct())
            {
                message.To.Add(MailboxAddress.Parse(recipient));
            }
            message.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = htmlBody,
                TextBody = plainBody ?? StripHtml(htmlBody)
            };
            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            var secureSocket = _settings.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
            await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, secureSocket);
            if (!string.IsNullOrEmpty(_settings.SmtpUser))
            {
                await client.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPass);
            }
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public Task NotifyAssetCreatedAsync(string assetType, int assetId, string assetSummary, string requesterEmail)
        {
            var subject = $"🔔 New {assetType} Awaiting Approval";
            var html = $@"
        <div style='font-family:Segoe UI,Arial,sans-serif;max-width:600px;margin:auto;border:1px solid #ddd;border-radius:10px;padding:20px;background:#f9fafb;'>
            <h2 style='color:#0056b3;text-align:center;'>New {assetType} Registered</h2>
            <p style='font-size:15px;color:#333;'>A new <strong>{assetType}</strong> has been registered and awaits your approval.</p>

            <table style='width:100%;border-collapse:collapse;margin:15px 0;'>
                <tr><td style='padding:8px;border-bottom:1px solid #ddd;'><strong>Asset ID:</strong></td><td style='padding:8px;border-bottom:1px solid #ddd;'>{assetId}</td></tr>
                <tr><td style='padding:8px;border-bottom:1px solid #ddd;'><strong>Summary:</strong></td><td style='padding:8px;border-bottom:1px solid #ddd;'>{assetSummary}</td></tr>
                <tr><td style='padding:8px;border-bottom:1px solid #ddd;'><strong>Requested by:</strong></td><td style='padding:8px;border-bottom:1px solid #ddd;'>{requesterEmail}</td></tr>
            </table>

            <p style='font-size:15px;color:#333;'>Please <a href='http://localhost:59801/api/Users/login' style='color:#007bff;text-decoration:none;font-weight:bold;'>log in to the admin portal</a> to review and approve or reject this request.</p>

            <p style='text-align:center;color:#888;font-size:13px;margin-top:25px;'>Asset Management System © {DateTime.Now.Year}</p>
        </div>";
            var to = _settings.AdminEmails;
            return SendEmailAsync(to, subject, html);
        }

        public Task NotifyAssetApprovalAsync(string assetType, int assetId, string assetSummary, string requesterEmail, bool approved, string remarks, string actedBy)
        {
            var statusColor = approved ? "#28a745" : "#dc3545";
            var statusText = approved ? "Approved ✅" : "Rejected ❌";

            var subject = $"Your {assetType} Registration has been {statusText}";
            var html = $@"
        <div style='font-family:Segoe UI,Arial,sans-serif;max-width:600px;margin:auto;border:1px solid #ddd;border-radius:10px;padding:20px;background:#f9fafb;'>
            <h2 style='color:{statusColor};text-align:center;'>{statusText}</h2>
            <p style='font-size:15px;color:#333;'>Your <strong>{assetType}</strong> registration (<strong>ID #{assetId}</strong>) has been {statusText.ToLower()}.</p>

            <table style='width:100%;border-collapse:collapse;margin:15px 0;'>
                <tr><td style='padding:8px;border-bottom:1px solid #ddd;'><strong>Summary:</strong></td><td style='padding:8px;border-bottom:1px solid #ddd;'>{assetSummary}</td></tr>
                <tr><td style='padding:8px;border-bottom:1px solid #ddd;'><strong>Actioned by:</strong></td><td style='padding:8px;border-bottom:1px solid #ddd;'>{actedBy}</td></tr>
                <tr><td style='padding:8px;border-bottom:1px solid #ddd;'><strong>Remarks:</strong></td><td style='padding:8px;border-bottom:1px solid #ddd;'>{remarks}</td></tr>
            </table>

            <p style='text-align:center;color:#888;font-size:13px;margin-top:25px;'>Asset Management System © {DateTime.Now.Year}</p>
        </div>";
            var to = new[] { requesterEmail };
            return SendEmailAsync(to, subject, html);
        }

        private static string StripHtml(string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;
            var array = new char[html.Length];
            var arrayIndex = 0;
            var inside = false;
            for (var i = 0; i < html.Length; i++)
            {
                var let = html[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
