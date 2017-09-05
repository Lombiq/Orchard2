using System.Threading.Tasks;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.MetaData;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Liquid.Model;
using OrchardCore.Liquid.ViewModels;

namespace OrchardCore.Liquid.Drivers
{
    public class LiquidPartDisplay : ContentPartDisplayDriver<LiquidPart>
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ILiquidTemplateManager _liquidTemplatemanager;

        public LiquidPartDisplay(
            IContentDefinitionManager contentDefinitionManager,
            ILiquidTemplateManager liquidTemplatemanager)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _liquidTemplatemanager = liquidTemplatemanager;
        }

        public override IDisplayResult Display(LiquidPart liquidPart)
        {
            return Combine(
                Shape<LiquidPartViewModel>("LiquidPart", m => BuildViewModel(m, liquidPart))
                    .Location("Detail", "Content:10"),
                Shape<LiquidPartViewModel>("LiquidPart_Summary", m => BuildViewModel(m, liquidPart))
                    .Location("Summary", "Content:10")
            );
        }

        public override IDisplayResult Edit(LiquidPart liquidPart)
        {
            return Shape<LiquidPartViewModel>("LiquidPart_Edit", m => BuildViewModel(m, liquidPart));
        }

        public override async Task<IDisplayResult> UpdateAsync(LiquidPart model, IUpdateModel updater)
        {
            await updater.TryUpdateModelAsync(model, Prefix, t => t.Liquid);

            return Edit(model);
        }

        private void BuildViewModel(LiquidPartViewModel model, LiquidPart liquidPart)
        {
            model.Liquid = liquidPart.Liquid;
            model.LiquidPart = liquidPart;
            model.ContentItem = liquidPart.ContentItem;
        }
    }
}
