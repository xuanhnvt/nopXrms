using System;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.Tables
{
    public partial class SearchTablesModel : BaseNopModel
    {
        [NopResourceDisplayName("Xrms.Admin.Catalog.Tables.List.Search.TableName")]
        public string SearchTableName { get; set; }
    }
}
