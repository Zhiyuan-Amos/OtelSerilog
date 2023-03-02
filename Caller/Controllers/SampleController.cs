using Microsoft.AspNetCore.Mvc;

namespace Caller.Controllers;

[ApiController]
[Route("/")]
public class SampleController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SampleController> _logger;
    private static int _count;

    public SampleController(IHttpClientFactory httpClientFactory,
        ILogger<SampleController> logger)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        _count++;
        _logger.LogInformation("Count = {Count}", _count);
        var response = await _httpClient.GetAsync("http://localhost:5145");
        var toReturn = await response.Content.ReadAsStringAsync();
        return Ok(toReturn);
    }
    
    [HttpGet("/error")]
    public async Task<ActionResult> Error()
    {
        _count++;
        _logger.LogInformation("Count = {Count}", _count);
        var response = await _httpClient.GetAsync("http://localhost:5145/error");
        response.EnsureSuccessStatusCode();

        return Ok();
    }
    
    [HttpGet("/post")]
    public async Task<ActionResult> Post()
    {
        _count++;
        _logger.LogInformation("Count = {Count}", _count);
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5145", new Foo { One = "one", Two = true });
        response.EnsureSuccessStatusCode();
        return Ok();
    }
    
    [HttpGet("/post/error")]
    public async Task<ActionResult> PostError()
    {
        _count++;
        _logger.LogInformation("Count = {Count}", _count);
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5145", new Foo());
        response.EnsureSuccessStatusCode();
        return Ok();
    }

    private class Foo
    {
        public string One { get; set; }
        public bool Two { get; set; }
    }
}

