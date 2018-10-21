using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Nop.Plugin.Xrms.Areas.Admin.Validators;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.CurrentOrders
{
    public partial class UpdateCurrentOrderItemsModel
    {
        private ICollection<OrderItemModel> _updatedOrderItems;

        public UpdateCurrentOrderItemsModel()
        {

        }

        public Guid AggregateId { get; set; }

        public int Version { get; set; }

        public ICollection<OrderItemModel> UpdatedOrderItems
        {
            get { return _updatedOrderItems ?? (_updatedOrderItems = new List<OrderItemModel>()); }
            set { _updatedOrderItems = value; }
        }

        #region Nested classes

        #endregion
    }
}
