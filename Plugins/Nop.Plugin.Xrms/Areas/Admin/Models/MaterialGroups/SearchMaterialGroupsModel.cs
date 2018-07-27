using System;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.MaterialGroups
{
    public partial class SearchMaterialGroupsModel : BaseNopModel
    {
        [NopResourceDisplayName("Xrms.Admin.Catalog.MaterialGroups.List.Search.MaterialGroupName")]
        public string SearchMaterialGroupName { get; set; }
    }
}
