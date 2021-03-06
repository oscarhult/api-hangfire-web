using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Globalization;

WebHost
  .CreateDefaultBuilder()
  .ConfigureServices(x =>
  {
    CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

    x.AddHealthChecks().AddCheck<ApiHealthCheck>("ApiHealth");
    x.AddHttpClient();
    x.AddControllers();
    x.AddHangfire(y =>
    {
      y.UseInMemoryStorage();
      y.UseDarkModeSupportForDashboard();
    });
    x.AddHangfireServer();
  })
  .Configure(x =>
  {
    var fileProvider = new EmbeddedFileProvider(typeof(Program).Assembly, "app.Website");
    x.UseDefaultFiles(new DefaultFilesOptions { FileProvider = fileProvider });
    x.UseStaticFiles(new StaticFileOptions { FileProvider = fileProvider });

    x.UseRouting();
    x.UseEndpoints(x =>
    {
      x.MapHealthChecks("/health");
      x.MapHangfireDashboard(new DashboardOptions
      {
        DisplayStorageConnectionString = false,
        Authorization = new[] { new AllowAllConnectionsFilter() },
        DashboardTitle = "Hangfire",
        AppPath = null,
      });
      x.MapControllers();
    });

    RecurringJob.AddOrUpdate("Heartbeat", () => Console.Write("Heartbeat"), Cron.Minutely);
  })
  .Build()
  .Run();

public class AllowAllConnectionsFilter : IDashboardAuthorizationFilter
{
  public bool Authorize(DashboardContext context) => true;
}
