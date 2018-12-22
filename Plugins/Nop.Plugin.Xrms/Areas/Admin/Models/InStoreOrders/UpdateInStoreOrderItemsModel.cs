using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Nop.Plugin.Xrms.Areas.Admin.Validators;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;
using Nop.Plugin.Xrms.Areas.Admin.Models.InStoreOrders;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.InStoreOrders
{
    public partial class UpdateInStoreOrderItemsModel
    {
        private ICollection<InStoreOrderItemModel> _updatedOrderItems;

        public UpdateInStoreOrderItemsModel()
        {

        }

        public Guid AggregateId { get; set; }

        public int Version { get; set; }

        public ICollection<InStoreOrderItemModel> UpdatedOrderItems
        {
            get { return _updatedOrderItems ?? (_updatedOrderItems = new List<InStoreOrderItemModel>()); }
            set { _updatedOrderItems = value; }
        }

        #region Nested classes

        #endregion
    }
}
