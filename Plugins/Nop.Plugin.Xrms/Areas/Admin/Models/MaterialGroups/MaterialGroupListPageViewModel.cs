using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.MaterialGroups
{
    public partial class MaterialGroupListPageViewModel
    {
        public MaterialGroupListPageViewModel()
        {

        }

        /*[NopResourceDisplayName("Xrms.Admin.Catalog.MaterialGroups.List.Search.MaterialGroupName")]
        public string SearchMaterialGroupName { get; set; }*/

        public SearchMaterialGroupsModel SearchModel { get; set; }
    }
}
