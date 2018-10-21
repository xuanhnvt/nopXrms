using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Nop.Plugin.Xrms.Areas.Admin.Validators;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.CurrentOrders
{
    [Validator(typeof(CreateCurrentOrderValidator))]
    public partial class CreateCurrentOrderModel
    {
        private List<OrderItemModel> _addedOrderItems;

        public CreateCurrentOrderModel()
        {

        }

        [NopResourceDisplayName("Xrms.Admin.Catalog.CurrentOrders.Fields.Table")]
        public int TableId { get; set; }

        public List<OrderItemModel> AddedOrderItems
        {
            get { return _addedOrderItems ?? (_addedOrderItems = new List<OrderItemModel>()); }
            set { _addedOrderItems = value; }
        }

        #region Nested classes

        #endregion
    }
}
