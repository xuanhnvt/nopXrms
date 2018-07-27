using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Nop.Plugin.Xrms.Areas.Admin.Validators;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Xrms.Areas.Admin.Models.Materials
{
    public partial class CreateMaterialModel : UpdateMaterialModel
    {
        public CreateMaterialModel()
        {

        }
    }
}
