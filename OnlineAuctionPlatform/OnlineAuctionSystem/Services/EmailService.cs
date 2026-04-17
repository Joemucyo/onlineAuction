using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace OnlineAuctionSystem.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var section = _configuration.GetSection("EmailSettings");
        var smtpHost = section["SmtpHost"] ?? throw new InvalidOperationException("EmailSettings:SmtpHost is missing.");
        var smtpPortRaw = section["SmtpPort"] ?? throw new InvalidOperationException("EmailSettings:SmtpPort is missing.");
        var senderEmail = section["SenderEmail"] ?? throw new InvalidOperationException("EmailSettings:SenderEmail is missing.");
        var senderName = section["SenderName"] ?? "GavelPro Auctions";
        var username = section["Username"] ?? throw new InvalidOperationException("EmailSettings:Username is missing.");
        var password = section["Password"] ?? throw new InvalidOperationException("EmailSettings:Password is missing.");
        username = username.Replace(" ", string.Empty).Trim();
        password = password.Replace(" ", string.Empty).Trim();

        if (!int.TryParse(smtpPortRaw, out var smtpPort))
        {
            throw new InvalidOperationException("EmailSettings:SmtpPort must be a valid integer.");
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderName, senderEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(username, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex) when (ex is AuthenticationException or SmtpCommandException or SmtpProtocolException)
        {
            throw new InvalidOperationException(
                $"SMTP authentication failed for '{smtpHost}' using configured credentials. Verify EmailSettings:Username and EmailSettings:Password.",
                ex
            );
        }
    }
}
