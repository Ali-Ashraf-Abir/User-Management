namespace task4.Services.Interfaces;
public interface IEmailService
{
    Task<EmailResult> SendAsync(string to, string subject, string body, CancellationToken ct = default);
}
 
public record EmailResult(bool Success,Guid? Id = null, string? Error = null);