using Polly;
using Polly.Wrap;
using Serilog;

namespace Common.Resilience;
public static class PollyGenericHandler<T> where T: new()
{
    public static AsyncPolicyWrap<T> GenericPolicyWrapAsync()
    {
        var maxRetryCount = 2;
        var waitBetweenRetriesInMilliseconds = 2000;

        IAsyncPolicy<T> retryPolicy = Policy<T>
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: maxRetryCount,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(waitBetweenRetriesInMilliseconds),
                onRetry: (result, retryTimespan, retryAttempts, context) => 
                    Log.Warning($"Polly Retry: context {context.PolicyKey} " +
                            $"- Retry {retryAttempts}/{maxRetryCount}, {retryTimespan.TotalMilliseconds}ms to next Retry - Exception: " +
                            $"{result.Exception.Message}")
            );

        IAsyncPolicy<T> fallbackPolicy = Policy<T>
            .Handle<Exception>()
            .FallbackAsync(async (cancelationToken) =>
            {
                return new T();
            },
            async (result) =>
            {
                Log.Error($"Polly Fallback - Error {result.Exception.Source} - {result.Exception.Message}");
                await Task.FromResult(true);
            });

        var wrapRetryPolicy = Policy.WrapAsync<T>(fallbackPolicy, retryPolicy);
        return wrapRetryPolicy;

    }
}
