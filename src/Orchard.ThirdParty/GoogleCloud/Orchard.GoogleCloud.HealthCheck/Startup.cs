using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Modules;
using Microsoft.AspNetCore.Routing;

namespace Dtl.GoogleCloud.HealthCheck
{
    public class Startup : StartupBase
    {
        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaRoute(
                name: "Appengine Healthcheck",
                areaName: "Dtl.GoogleCloud.AppEngine.HealthCheck",
                template: "_ah/health",
                defaults: new { controller = "HealthCheck", action = "IsOk" }
            );
        }
    }
}
