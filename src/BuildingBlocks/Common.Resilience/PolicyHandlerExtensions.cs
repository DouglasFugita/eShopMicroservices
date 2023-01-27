using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Serilog;

namespace Common.Resilience;
public static class PolicyHandlerExtensions
{
    public static IHttpClientBuilder AddWaitAndRetryAsyncPolicyHandler(this IHttpClientBuilder clientBuilder, int maxAttempts, int secondsToRetry)
    {
        return clientBuilder.AddPolicyHandler(WaitAndRetryAsyncPolicy(clientBuilder, maxAttempts, secondsToRetry));
    }

    public static IHttpClientBuilder AddPolicyWrapperAsyncHandler(this IHttpClientBuilder clientBuilder)
    {
        return clientBuilder.AddPolicyHandler(
            Policy.WrapAsync<HttpResponseMessage>(WaitAndRetryAsyncPolicy(clientBuilder, 10, 5), CircuitBreakerAsyncPolicy(clientBuilder, 2, 30))
        );
    }

    public static IHttpClientBuilder AddCircuitBreakerAsyncPolicyHandler(this IHttpClientBuilder clientBuilder, int eventsBeforeBreaking, int secondsOfBreaking)
    {
        return clientBuilder.AddPolicyHandler(CircuitBreakerAsyncPolicy(clientBuilder, eventsBeforeBreaking, secondsOfBreaking));
    }

    static IAsyncPolicy<HttpResponseMessage> WaitAndRetryAsyncPolicy(IHttpClientBuilder clientBuilder, int maxAttempts, int secondsToRetry)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.BadRequest)
            .Or<BrokenCircuitException>()
            .WaitAndRetryAsync(
                retryCount: maxAttempts,
                sleepDurationProvider: _ => TimeSpan.FromSeconds(secondsToRetry),
                onRetry: (exception, nextTryInSeconds, retryCount, context) =>
                {
                    Log.Warning($"Retry: {clientBuilder.Name} - Retry count: {retryCount} - Next try in {nextTryInSeconds} seconds due to: {exception}.");
                });
    }

    static IAsyncPolicy<HttpResponseMessage> CircuitBreakerAsyncPolicy(IHttpClientBuilder clientBuilder, int eventsBeforeBreaking, int secondsOfBreaking)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: eventsBeforeBreaking,
                durationOfBreak: TimeSpan.FromSeconds(secondsOfBreaking),
                onBreak: (exception, breakDelay) =>
                {
                    Log.Error($"CircuitBreaker: {clientBuilder.Name} - Breaking the circuit for {breakDelay.TotalSeconds} seconds due to {exception}");
                },
                onReset: () =>
                {
                    Log.Information($"CircuitBreaker: {clientBuilder.Name} - Circuit Closed");
                },
                onHalfOpen: () =>
                {
                    Log.Information($"CircuitBreaker: {clientBuilder.Name} - Circuit Half-Open: Next call is a trial");
                }
            );
    }
}
