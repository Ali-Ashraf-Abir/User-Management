using System.Threading.Channels;
using task4.Job;
using task4.Queue.Interfaces;

public class EmailQueue : IEmailQueue
{
    private readonly Channel<EmailMessage> _queue;

    public EmailQueue()
    {
        _queue = Channel.CreateUnbounded<EmailMessage>();
    }

    public async ValueTask QueueEmailAsync(
        EmailMessage email)
    {
        await _queue.Writer.WriteAsync(email);
    }

    public async ValueTask<EmailMessage> DequeueEmailAsync(
        CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(
            cancellationToken);
    }
}