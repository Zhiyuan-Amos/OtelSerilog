using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Callee.Controllers;

[ApiController]
[Route("/")]
public class SampleController : ControllerBase
{
    private readonly ILogger<SampleController> _logger;
    private static int _count;

    public SampleController(ILogger<SampleController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult Get()
    {
        _count++;
        _logger.LogInformation("Count = {Count}", _count);
        return Ok("Hello World!");
    }
        
    [HttpGet("/error")]
    public ActionResult Error()
    {
        _count++;
        _logger.LogInformation("Count = {Count}", _count);
        return BadRequest();
    }
    
    [HttpPost]
    public ActionResult Post(Foo foo)
    {
        _count++;
        _logger.LogInformation("Count = {Count}", _count);
        return Ok();
    }

    public class Foo
    {
        [Required] public string One { get; set; }
        public bool Two { get; set; }
    }
}

