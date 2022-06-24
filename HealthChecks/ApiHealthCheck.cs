using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public class ApiHealthCheck : IHealthCheck
{
  public ApiHealthCheck() { }

  public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
      CancellationToken cancellationToken = default)
  {
    if (DateTime.Now.Ticks % 2 == 0)
    {
      return HealthCheckResult.Healthy("Api is Healthy");
    }
    else
    {
      if (DateTime.Now.Ticks % 2 == 0)
      {
        return HealthCheckResult.Degraded("Api is Degraded");
      }
      else
      {
        return HealthCheckResult.Unhealthy("Api is Unhealthy");
      }
    }
  }
}
