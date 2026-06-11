using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using task4.Models;
using task4.Services.Interfaces;

namespace task4.Services;

public class BrevoEmailService : IEmailService
{
    private readonly BrevoOptions _options;
    private readonly HttpClient _http;

    public BrevoEmailService(IOptions<BrevoOptions> options, HttpClient http)
    {
        _options = options.Value;
        _http = http;
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
                htmlContent = body
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email");
            request.Headers.Add("api-key", _options.ApiKey);
            request.Content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var response = await _http.SendAsync(request, ct);
            var responseBody = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Brevo error: {responseBody}");
                return new EmailResult(false, Error: responseBody);
            }

            return new EmailResult(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new EmailResult(false, Error: ex.ToString());
        }
    }
}