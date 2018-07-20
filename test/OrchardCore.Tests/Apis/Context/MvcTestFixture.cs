using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace OrchardCore.Tests.Apis.Context
{
    public class OrchardTestFixture<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var shellsApplicationDataPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");

            if (Directory.Exists(shellsApplicationDataPath))
            {
                Directory.Delete(shellsApplicationDataPath, true);
            }

            builder.UseContentRoot(Directory.GetCurrentDirectory());
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHostBuilderFactory.CreateFromAssemblyEntryPoint(
                typeof(Cms.Web.Startup).Assembly, Array.Empty<string>())
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<TStartup>();
        }
    }
}