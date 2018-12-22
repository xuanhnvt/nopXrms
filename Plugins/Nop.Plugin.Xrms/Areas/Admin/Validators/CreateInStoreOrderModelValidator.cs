using System;
using FluentValidation;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Plugin.Xrms.Areas.Admin.Models.InStoreOrders;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Areas.Admin.Validators
{
    public partial class CreateInStoreOrderModelValidator : BaseNopValidator<CreateInStoreOrderModel>
    {
        public CreateInStoreOrderModelValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.TableId).NotEqual(0).WithMessage(localizationService.GetResource("Xrms.Admin.InStoreOrders.Validations.Table.Required"));
            //RuleFor(x => x.OrderCode).NotEqual(0).WithMessage("Please select '{PropertyName}'");
            //.WithMessage(localizationService.GetResource("Xrms.Admin.Cashier.Orders.Fields.Table.Required"));
            //RuleFor(x => x.AddedOrderItems).NotNull().WithMessage("Must have at least one order item.");
            //RuleForEach(x => x.AddedOrderItems).NotNull().WithMessage("Must have at least one order item.");
        }
    }
}
