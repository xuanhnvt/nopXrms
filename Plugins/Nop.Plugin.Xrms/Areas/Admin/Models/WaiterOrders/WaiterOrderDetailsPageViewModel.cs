using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

using Nop.Plugin.Xrms.Areas.Admin.Models.InStoreOrders;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.WaiterOrders
{
    public partial class WaiterOrderDetailsPageViewModel
    {
        public WaiterOrderDetailsPageViewModel()
        {
            OrderView = new InStoreOrderViewModel();
            ProductListView = new ProductListViewModel();
        }
        public InStoreOrderViewModel OrderView { get; set; }
        public ProductListViewModel ProductListView { get; set; }

        #region Nested classes

        #endregion
    }
}
