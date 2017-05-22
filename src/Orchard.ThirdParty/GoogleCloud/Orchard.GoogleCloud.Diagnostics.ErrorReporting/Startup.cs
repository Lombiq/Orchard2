using System;
using System.Reflection;
using Google.Cloud.Diagnostics.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Modules;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Orchard.Environment.Shell;

namespace Orchard.GoogleCloud.Diagnostics.ErrorReporting
{
    public class Startup : StartupBase
    {
        private readonly ShellSettings _shellSettings;
        private readonly IHostingEnvironment _env;

        public Startup(
            ShellSettings shellSettings,
            IHostingEnvironment env)
        {
            _shellSettings = shellSettings;
            _env = env;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsProduction())
            {
                var rawCredentials = _shellSettings["GoogleCredentials"];
                var projectId = JObject.Parse(rawCredentials).Value<string>("project_id");

                var version = Assembly
                    .GetEntryAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion;

                services.AddGoogleExceptionLogging(projectId, _shellSettings.Name, version);
            }
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            if (_env.IsProduction())
            {
                app.UseGoogleExceptionLogging();
            }
        }
    }
}
