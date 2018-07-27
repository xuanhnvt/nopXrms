using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Nop.Plugin.Xrms.Areas.Admin.Validators;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.MaterialGroups
{
    [Validator(typeof(UpdateMaterialGroupValidator))]
    public partial class UpdateMaterialGroupModel : BaseNopModel
    {
        public UpdateMaterialGroupModel()
        {

        }

        [NopResourceDisplayName("Xrms.Admin.Catalog.MaterialGroups.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Catalog.MaterialGroups.Fields.Description")]
        public string Description { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Catalog.MaterialGroups.Fields.Parent")]
        public int ParentGroupId { get; set; }

        [UIHint("Picture")]
        [NopResourceDisplayName("Xrms.Admin.Catalog.MaterialGroups.Fields.Picture")]
        public int PictureId { get; set; }

        [NopResourceDisplayName("Xrms.Admin.Catalog.MaterialGroups.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

    }
}
