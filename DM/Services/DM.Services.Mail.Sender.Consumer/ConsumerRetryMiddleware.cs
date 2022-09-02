using System;
using System.Threading;
using System.Threading.Tasks;
using Jamq.Client.Abstractions.Consuming;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace DM.Services.Mail.Sender.Consumer;

internal class ConsumerRetryMiddleware : IConsumerMiddleware
{
    private readonly AsyncRetryPolicy retryPolicy;
    
    public ConsumerRetryMiddleware(
        ILogger<ConsumerRetryMiddleware> logger)
    {
        retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(5,
            attempt => TimeSpan.FromSeconds(1 << attempt),
            (exception, _) => logger.LogWarning(exception, "Something is wrong with mail sending"));
    }
    
    public Task<ProcessResult> InvokeAsync(
        ConsumerContext context,
        ConsumerDelegate next,
        CancellationToken cancellationToken) =>
        retryPolicy.ExecuteAsync(() => next.Invoke(context, cancellationToken));
}