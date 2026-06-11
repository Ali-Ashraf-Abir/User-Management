using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using task4.Models;
using task4.Services.Interfaces;

namespace task4.Services;

public class BrevoEmailService : IEmailService
{
    private readonly BrevoOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;

    public BrevoEmailService(
        IOptions<BrevoOptions> options,
        IHttpClientFactory httpClientFactory)  // ← changed
    {
        _options = options.Value;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<EmailResult> SendAsync(
        string to,
        string subject,
        string body,
        CancellationToken ct = default)
    {
        try
        {
            var payload = new
            {
                sender = new { name = _options.FromName, email = _options.FromAddress },
                to = new[] { new { email = to } },
                subject,
                htmlContent = body,
                trackClicks = false, 
                trackOpens = false
            };

            var client = _httpClientFactory.CreateClient();  // ← changed

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.brevo.com/v3/smtp/email");

            request.Headers.Add("api-key", _options.ApiKey);
            request.Content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var response = await client.SendAsync(request, ct);
            var responseBody = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Brevo error: {responseBody}");
                return new EmailResult(false, Error: responseBody);
            }

            Console.WriteLine($"Email sent to {to} via Brevo");
            return new EmailResult(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new EmailResult(false, Error: ex.ToString());
        }
    }
}