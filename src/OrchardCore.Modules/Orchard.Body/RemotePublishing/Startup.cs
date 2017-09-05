using OrchardCore.Modules;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.MetaWeblog;

namespace OrchardCore.Body.RemotePublishing
{
    [RequireFeatures("Orchard.RemotePublishing")]
    public class RemotePublishingStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMetaWeblogDriver, BodyMetaWeblogDriver>();
        }
    }
}
