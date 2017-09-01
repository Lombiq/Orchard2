using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Orchard.Environment.Extensions;

namespace Orchard.Mvc.LocationExpander
{
    public class ModularViewLocationExpanderProvider : IViewLocationExpanderProvider
    {
        private readonly IExtensionManager _extensionManager;

        public ModularViewLocationExpanderProvider(IExtensionManager extensionManager)
        {
            _extensionManager = extensionManager;
        }

        public int Priority => 5;

        /// <inheritdoc />
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }

        /// <inheritdoc />
        public virtual IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
                                                               IEnumerable<string> viewLocations)
        {
            if (context.ActionContext.ActionDescriptor is PageActionDescriptor)
            {
                var pageViewLocations = PageViewLocations().ToList();
                pageViewLocations.AddRange(viewLocations);
                return pageViewLocations;

                IEnumerable<string> PageViewLocations()
                {
                    foreach (var location in viewLocations)
                    {
                        if (location.EndsWith("/{1}/{0}" + RazorViewEngine.ViewExtension))
                        {
                            yield return location.Replace("/{1}/{0}", "/{1}/Views/PageViews/{0}");
                        }
                    }
                }
            }

            // Get Extension, and then add in the relevant views.
            var extension = _extensionManager.GetExtension(context.AreaName);

            if (!extension.Exists)
            {
                return viewLocations;
            }

            var result = new List<string>();

            var extensionViewsPath = '/' + extension.SubPath.Replace('\\', '/').Trim('/') + "/Views";
            result.Add(extensionViewsPath + "/{1}/{0}" + RazorViewEngine.ViewExtension);
            result.Add(extensionViewsPath + "/Shared/{0}" + RazorViewEngine.ViewExtension);

            result.AddRange(viewLocations);

            return result;
        }
    }
}