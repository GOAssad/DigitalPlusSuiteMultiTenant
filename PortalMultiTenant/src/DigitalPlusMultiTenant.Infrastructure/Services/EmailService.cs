using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace DigitalPlusMultiTenant.Infrastructure.Services;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string htmlBody);
}

public class EmailService : IEmailService
{
    private readonly string _host;
    private readonly int _port;
    private readonly string _from;
    private readonly string _password;
    private readonly string _displayName;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration config, ILogger<EmailService> logger)
    {
        _host = config["Smtp:Host"] ?? "smtp.hostinger.com";
        _port = int.TryParse(config["Smtp:Port"], out var p) ? p : 465;
        _from = config["Smtp:From"] ?? "notify@integraia.tech";
        _password = config["Smtp:Password"] ?? "";
        _displayName = config["Smtp:DisplayName"] ?? "Digital One";
        _logger = logger;
    }

    public async Task SendAsync(string to, string subject, string htmlBody)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_displayName, _from));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlBody };

            using var client = new SmtpClient();
            await client.ConnectAsync(_host, _port, SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(_from, _password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Email enviado a {To}: {Subject}", to, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enviando email a {To}: {Subject}", to, subject);
            throw;
        }
    }
}
