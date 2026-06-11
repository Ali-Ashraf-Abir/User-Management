using Microsoft.Extensions.Hosting;
using task4.Queue.Interfaces;
using task4.Services.Interfaces;

public class EmailBackgroundService : BackgroundService
{
    private readonly IEmailQueue _emailQueue;
    private readonly IEmailService _emailService;

    public EmailBackgroundService(
        IEmailQueue emailQueue,
        IEmailService emailService)
    {
        _emailQueue = emailQueue;
        _emailService = emailService;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        Console.WriteLine("EmailBackgroundService started");
        while (!stoppingToken.IsCancellationRequested)
        {
            
            var email = await _emailQueue.DequeueEmailAsync(stoppingToken);
            Console.WriteLine($"Sending email to {email.To}");
            try
            {
                await _emailService.SendAsync(
                    
                    email.To,
                    email.Subject,
                    email.Body);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}