using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.Tables
{
    public partial class TableDetailsPageViewModel : CreateTableModel
    {
        public TableDetailsPageViewModel()
        {

        }

        public int Id { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Catalog.Tables.Fields.State")]
        public int State { get; set; }

        #region Nested classes

        #endregion
    }
}
