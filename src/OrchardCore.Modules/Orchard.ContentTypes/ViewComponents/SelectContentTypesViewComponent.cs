﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.MetaData;
using OrchardCore.ContentTypes.ViewModels;

namespace OrchardCore.ContentTypes.ViewComponents
{
    public class SelectContentTypesViewComponent : ViewComponent
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public SelectContentTypesViewComponent(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public IViewComponentResult Invoke(string[] selectedContentTypes, string htmlName, string stereotype)
        {
            if (selectedContentTypes == null)
            {
                selectedContentTypes = new string[0];
            }

            var contentTypes = ContentTypeSelection.Build(_contentDefinitionManager, selectedContentTypes);

            if (!String.IsNullOrEmpty(stereotype))
            {
                contentTypes = contentTypes
                    .Where(x => x.ContentTypeDefinition.Settings.ToObject<ContentTypeSettings>().Stereotype == stereotype)
                    .ToArray();
            }

            var model = new SelectContentTypesViewModel
            {
                HtmlName = htmlName,
                ContentTypeSelections = contentTypes
            };

            return View(model);
        }
    }
}
