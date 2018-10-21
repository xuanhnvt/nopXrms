using System;
using FluentValidation;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Plugin.Xrms.Areas.Admin.Models.CurrentOrders;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Areas.Admin.Validators
{
    public partial class CreateCurrentOrderValidator : BaseNopValidator<CreateCurrentOrderModel>
    {
        public CreateCurrentOrderValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.TableId).NotEqual(0).WithMessage(localizationService.GetResource("Xrms.Admin.Catalog.CurrentOrders.Fields.Table.Required"));
        }
    }
}
