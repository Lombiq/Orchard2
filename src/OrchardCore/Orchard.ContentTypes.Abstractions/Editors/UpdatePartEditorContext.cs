﻿using OrchardCore.ContentManagement.Metadata.Builders;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;

namespace OrchardCore.ContentTypes.Editors
{
    public class UpdatePartEditorContext : UpdateContentDefinitionEditorContext<ContentPartDefinitionBuilder>
    {
        public UpdatePartEditorContext(
                ContentPartDefinitionBuilder builder,
                IShape model,
                string groupId,
                IShapeFactory shapeFactory,
                IShape layout,
                IUpdateModel updater)
            : base(builder, model, groupId, shapeFactory, layout, updater)
        {
        }
    }
}
