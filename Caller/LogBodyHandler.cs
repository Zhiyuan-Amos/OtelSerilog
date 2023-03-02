using System.Text.Json;

namespace Caller;

public class LogBodyHandler : DelegatingHandler
{
    private readonly ILogger<LogBodyHandler> _logger;

    public LogBodyHandler(ILogger<LogBodyHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        if (request.Content is JsonContent requestContent)
        {
            var requestBody = JsonSerializer.Serialize(requestContent.Value);
            _logger.LogInformation("Sending HTTP request {Method} {AbsoluteUri} {RequestBody}", 
                request.Method, request.RequestUri!.AbsoluteUri, requestBody);
        }
        else
        {
            _logger.LogInformation("Sending HTTP request {Method} {AbsoluteUri}", 
                request.Method, request.RequestUri!.AbsoluteUri);
        }

        var response = await base.SendAsync(request, cancellationToken);

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogInformation("Response from HTTP request {Method} {AbsoluteUri} {ResponseBody}", 
            request.Method, request.RequestUri!.AbsoluteUri, responseBody);

        return response;
    }
}
