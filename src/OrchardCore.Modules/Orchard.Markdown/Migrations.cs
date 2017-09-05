﻿using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.MetaData;
using OrchardCore.Data.Migration;

namespace OrchardCore.Markdown
{
    public class Migrations : DataMigration
    {
        IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition("MarkdownPart", builder => builder
                .Attachable()
                .WithDescription("Provides a Markdown formatted body for your content item."));

            return 1;
        }
    }
}