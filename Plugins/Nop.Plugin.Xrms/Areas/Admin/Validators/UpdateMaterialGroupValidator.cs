using System;
using FluentValidation;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Plugin.Xrms.Areas.Admin.Models.MaterialGroups;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Areas.Admin.Validators
{
    public partial class UpdateMaterialGroupValidator : BaseNopValidator<UpdateMaterialGroupModel>
    {
        public UpdateMaterialGroupValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Xrms.Admin.Catalog.MaterialGroups.Fields.Name.Required"));
        }
    }
}
