using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.CurrentOrders
{
    public partial class CurrentOrderListItemViewModel
    {
        public CurrentOrderListItemViewModel()
        {

        }

        public int Id { get; set; }

        public Guid AggregateId { get; set; }

        public int Version { get; set; }

        public int OrderId { get; set; }

        public string OrderCode { get; set; }

        public string TableName { get; set; }

        public int State { get; set; }

        public int PrintCount { get; set; }

        public string BilledOnUtc { get; set; }

        public string CreatedOnUtc { get; set; }

        public string UpdatedOnUtc { get; set; }

        public decimal SubTotalPrice { get; set; }
    }
}
