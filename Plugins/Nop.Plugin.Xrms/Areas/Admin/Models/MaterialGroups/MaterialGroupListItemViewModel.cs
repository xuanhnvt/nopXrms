using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.MaterialGroups
{
    public partial class MaterialGroupListItemViewModel
    {
        public MaterialGroupListItemViewModel()
        {

        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public string Breadcrumb { get; set; }
    }
}
