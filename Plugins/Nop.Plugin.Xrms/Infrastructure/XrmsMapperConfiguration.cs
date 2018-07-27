﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Xrms.Domain;
using Nop.Plugin.Xrms.Areas.Admin.Models.MaterialGroups;
using Nop.Plugin.Xrms.Areas.Admin.Models.Materials;
using Nop.Plugin.Xrms.Areas.Admin.Models.Suppliers;

namespace Nop.Plugin.Xrms.Infrastructure
{
    /// <summary>
    /// AutoMapper configuration for models
    /// </summary>
    public class XrmsMapperConfiguration : Profile, IOrderedMapperProfile
    {
        public XrmsMapperConfiguration()
        {
            #region MaterialGroup
            // from entity to view model
            CreateMap<MaterialGroup, MaterialGroupListItemViewModel>();
            CreateMap<MaterialGroup, MaterialGroupDetailsPageViewModel>();

            // from action model to entity
            CreateMap<CreateMaterialGroupModel, MaterialGroup>();
            CreateMap<UpdateMaterialGroupModel, MaterialGroup>();

            // from action model to view model
            CreateMap<CreateMaterialGroupModel, MaterialGroupDetailsPageViewModel>();
            CreateMap<UpdateMaterialGroupModel, MaterialGroupDetailsPageViewModel>();

            #endregion // MaterialGroup

            #region Material

            // from entity to view model
            CreateMap<Material, MaterialListItemViewModel>();
            CreateMap<Material, MaterialDetailsPageViewModel>();

            // from action model to entity
            CreateMap<CreateMaterialModel, Material>();
            CreateMap<UpdateMaterialModel, Material>();

            // from action model to view model
            CreateMap<CreateMaterialModel, MaterialDetailsPageViewModel>();
            CreateMap<UpdateMaterialModel, MaterialDetailsPageViewModel>();

            #endregion // Material

            #region Supplier

            // from entity to view model
            CreateMap<Supplier, SupplierListItemViewModel>();
            //CreateMap<Supplier, SupplierDetailsPageViewModel>();

            // from action model to entity
            CreateMap<SupplierModel, Supplier>();

            // from action model to view model
            //CreateMap<SupplierModel, SupplierDetailsPageViewModel>();

            #endregion // Supplier

        }

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 1;
    }
}