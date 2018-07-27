using System;
using FluentValidation;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Plugin.Xrms.Areas.Admin.Models.Materials;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Areas.Admin.Validators
{
    public partial class UpdateMaterialValidator : BaseNopValidator<UpdateMaterialModel>
    {
        public UpdateMaterialValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Xrms.Admin.Catalog.Materials.Fields.Name.Required"));
        }
    }
}
