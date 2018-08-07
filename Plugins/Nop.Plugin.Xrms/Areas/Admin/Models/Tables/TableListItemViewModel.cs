using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.Tables
{
    public partial class TableListItemViewModel
    {
        public TableListItemViewModel()
        {

        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public int State { get; set; }
    }
}
