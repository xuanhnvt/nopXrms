using System;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.CurrentOrders
{
    public partial class SearchCurrentOrdersModel : BaseNopModel
    {
        [NopResourceDisplayName("Xrms.Admin.Cashier.Orders.List.Search.TableName")]
        public string SearchTableName { get; set; }
    }
}
