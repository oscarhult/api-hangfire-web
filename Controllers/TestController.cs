using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/1/test")]
public class TestController : ControllerBase
{
  private readonly IConfiguration _configuration;
  private readonly HttpClient _httpClient;

  private readonly ILogger<TestController> _logger;
  private readonly HealthCheckService _healthCheckService;

  public TestController(
    IConfiguration configuration,
    HttpClient httpClient,
    ILogger<TestController> logger,
    HealthCheckService healthCheckService)
  {
    _configuration = configuration;
    _httpClient = httpClient;
    _logger = logger;
    _healthCheckService = healthCheckService;
  }

  [HttpGet]
  public async Task<ActionResult<TestResponse>> Test()
  {
    var report = await _healthCheckService.CheckHealthAsync();

    return Ok(new TestResponse
    {
      Success = true,
      Health = report
    });
  }

  public class TestResponse
  {
    public bool Success { get; set; }
    public HealthReport Health { get; set; }
  }
}
