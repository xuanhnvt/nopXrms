using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.Suppliers
{
    public partial class SupplierListItemViewModel
    {
        public SupplierListItemViewModel()
        {

        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int DisplayOrder { get; set; }
    }
}
