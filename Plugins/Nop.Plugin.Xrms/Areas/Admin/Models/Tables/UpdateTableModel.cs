using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Nop.Plugin.Xrms.Areas.Admin.Validators;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.Tables
{
    [Validator(typeof(UpdateTableValidator))]
    public partial class UpdateTableModel : BaseNopModel
    {
        public UpdateTableModel()
        {

        }

        [NopResourceDisplayName("Xrms.Admin.Catalog.Tables.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Catalog.Tables.Fields.Description")]
        public string Description { get; set; }

        [UIHint("Picture")]
        [NopResourceDisplayName("Xrms.Admin.Catalog.Tables.Fields.Picture")]
        public int PictureId { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Catalog.Tables.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

    }
}
