using System;
using System.Diagnostics;
using Google.Cloud.Trace.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Modules;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DiagnosticAdapter;

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

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var diagnosticListener = serviceProvider.GetRequiredService<DiagnosticListener>();
            diagnosticListener.SubscribeWithAdapter(new GoogleCloudTraceListener());
        }
    }

    public class GoogleCloudTraceListener
    {
        [DiagnosticName("Trace.Starting")]
        public virtual void OnTraceStarting()
        {
        }

        [DiagnosticName("Trace.Finished")]
        public virtual void OnTraceFinished()
        {
            var r = new Google.Cloud.Trace.V1.Trace { };
            r.Spans.Add(new TraceSpan {  });
        }
    }
}
