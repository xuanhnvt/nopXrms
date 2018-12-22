using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.InStoreOrders
{
    public partial class InStoreOrderListRowViewModel
    {
        public InStoreOrderListRowViewModel()
        {

        }

        public int Id { get; set; }

        public Guid AggregateId { get; set; }

        public int Version { get; set; }

        public int OrderId { get; set; }

        [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Fields.Code")]
        public string OrderCode { get; set; }

        [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Fields.Table")]
        public string TableName { get; set; }

        [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Fields.Waiter")]
        public string WaiterName { get; set; }

        [NopResourceDisplayName("Xrms.Admin.InStoreOrders.Fields.State")]
        public int StateId { get; set; }

        public int PrintCount { get; set; }

        public string BilledOnUtc { get; set; }

        public string CreatedOnUtc { get; set; }

        public string UpdatedOnUtc { get; set; }

        public decimal SubTotalPrice { get; set; }
    }
}
