using Google.Cloud.Trace.V1;
using Microsoft.AspNetCore.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace Orchard.GoogleCloud.Diagnostics.Trace
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<TraceServiceClient>(_ => {
                return Google.Cloud.Trace.V1.TraceServiceClient.Create();
            });
        }
    }
}
