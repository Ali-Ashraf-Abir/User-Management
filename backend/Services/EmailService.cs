using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using task4.Models;
using task4.Services.Interfaces;

namespace task4.Services;

public class GmailEmailService : IEmailService
{
    private readonly EmailOptions _options;

    public GmailEmailService(IOptions<EmailOptions> options)
    {
        _options = options.Value;
    }

    public async Task<EmailResult> SendAsync(
        string to,
        string subject,
        string body,
        CancellationToken ct = default)
    {
        try
        {
            Console.WriteLine($"Host: {_options.Host}");
            Console.WriteLine($"Port: {_options.Port}");
            Console.WriteLine($"User: {_options.Username}");
            Console.WriteLine($"Password Exists: {!string.IsNullOrWhiteSpace(_options.Password)}");
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(
                    _options.FromName,
                    _options.FromAddress));

            message.To.Add(MailboxAddress.Parse(to));

            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();

            await client.ConnectAsync(
                _options.Host,
                _options.Port,
                MailKit.Security.SecureSocketOptions.SslOnConnect,
                ct);

            await client.AuthenticateAsync(
                _options.Username,
                _options.Password,
                ct);

            await client.SendAsync(message, ct);

            await client.DisconnectAsync(true, ct);

            return new EmailResult(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());

            return new EmailResult(
                false,
                Error: ex.ToString());
        }
    }
}