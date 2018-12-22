using System;
using System.Collections.Generic;
using System.Text;

using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.InStoreOrders
{
    public partial class CreateInStoreOrderModel
    {
        private List<InStoreOrderItemModel> _addedOrderItems;

        public CreateInStoreOrderModel()
        {

        }

        [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Fields.Table")]
        public int TableId { get; set; }

        public List<InStoreOrderItemModel> AddedOrderItems
        {
            get { return _addedOrderItems ?? (_addedOrderItems = new List<InStoreOrderItemModel>()); }
            set { _addedOrderItems = value; }
        }

        #region Nested classes

        #endregion
    }
}
