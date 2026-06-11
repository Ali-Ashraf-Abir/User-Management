namespace task4.Queue.Interfaces;

using System.Threading.Channels;
using task4.Job;

public interface IEmailQueue
{
    ValueTask QueueEmailAsync(EmailMessage email);

    ValueTask<EmailMessage> DequeueEmailAsync(
        CancellationToken cancellationToken);
}