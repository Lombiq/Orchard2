﻿using Microsoft.Extensions.Configuration;
using OrchardCore.DisplayManagement;
using OrchardCore.Environment.Commands;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Extensions.Manifests;
using OrchardCore.Environment.Shell.Data;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddOrchardCms(this IServiceCollection services)
        {
            return AddOrchardCms(services, null);
        }

        public static IServiceCollection AddOrchardCms(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddThemingHost();
            services.AddManifestDefinition("Theme.txt", "theme");
            services.AddExtensionLocation("Themes");
            services.AddSitesFolder("App_Data", "Sites");
            services.AddCommands();
            services.AddAuthentication();
            services.AddModules(modules => 
            {
                if (configuration != null)
                {
                    modules.WithConfiguration(configuration);
                }

                modules.WithDefaultFeatures("Orchard.Mvc", "Orchard.Settings", "Orchard.Setup", "Orchard.Recipes", "Orchard.Commons");
            });

            return services;
        }
    }
}
