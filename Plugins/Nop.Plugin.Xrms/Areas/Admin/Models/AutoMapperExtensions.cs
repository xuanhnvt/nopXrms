using System;
using Nop.Plugin.Xrms.Domain;
using Nop.Plugin.Xrms.Areas.Admin.Models.MaterialGroups;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Xrms.Areas.Admin.Models.Materials;
using Nop.Plugin.Xrms.Areas.Admin.Models.Suppliers;

namespace Nop.Plugin.Xrms.Areas.Admin.Models
{
    public static class AutoMapperExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        #region MaterialGroup

        // from entity to view model
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static MaterialGroupListItemViewModel ToListItemViewModel(this MaterialGroup entity)
        {
            return entity.MapTo<MaterialGroup, MaterialGroupListItemViewModel>();
        }

        public static MaterialGroupDetailsPageViewModel ToDetailsViewModel(this MaterialGroup entity)
        {
            return entity.MapTo<MaterialGroup, MaterialGroupDetailsPageViewModel>();
        }

        public static MaterialGroupDetailsPageViewModel ToDetailsViewModel(this MaterialGroup entity, MaterialGroupDetailsPageViewModel viewModel)
        {
            return entity.MapTo(viewModel);
        }

        // from action model to entity
        public static MaterialGroup ToEntity(this CreateMaterialGroupModel model)
        {
            return model.MapTo<CreateMaterialGroupModel, MaterialGroup>();
        }

        public static MaterialGroup ToEntity(this CreateMaterialGroupModel model, MaterialGroup entity)
        {
            return model.MapTo(entity);
        }

        public static MaterialGroup ToEntity(this UpdateMaterialGroupModel model)
        {
            return model.MapTo<UpdateMaterialGroupModel, MaterialGroup>();
        }

        public static MaterialGroup ToEntity(this UpdateMaterialGroupModel model, MaterialGroup entity)
        {
            return model.MapTo(entity);
        }

        // from action model to view model
        public static MaterialGroupDetailsPageViewModel ToDetailsViewModel(this CreateMaterialGroupModel model)
        {
            return model.MapTo<CreateMaterialGroupModel, MaterialGroupDetailsPageViewModel>();
        }

        public static MaterialGroupDetailsPageViewModel ToDetailsViewModel(this CreateMaterialGroupModel model, MaterialGroupDetailsPageViewModel viewModel)
        {
            return model.MapTo(viewModel);
        }

        public static MaterialGroupDetailsPageViewModel ToDetailsViewModel(this UpdateMaterialGroupModel model)
        {
            return model.MapTo<UpdateMaterialGroupModel, MaterialGroupDetailsPageViewModel>();
        }

        public static MaterialGroupDetailsPageViewModel ToDetailsViewModel(this UpdateMaterialGroupModel model, MaterialGroupDetailsPageViewModel viewModel)
        {
            return model.MapTo(viewModel);
        }

        #endregion // MaterialGroup

        #region Material

        // from entity to view model
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static MaterialListItemViewModel ToListItemViewModel(this Material entity)
        {
            return entity.MapTo<Material, MaterialListItemViewModel>();
        }

        public static MaterialDetailsPageViewModel ToDetailsViewModel(this Material entity)
        {
            return entity.MapTo<Material, MaterialDetailsPageViewModel>();
        }

        public static MaterialDetailsPageViewModel ToDetailsViewModel(this Material entity, MaterialDetailsPageViewModel viewModel)
        {
            return entity.MapTo(viewModel);
        }

        // from action model to entity
        public static Material ToEntity(this CreateMaterialModel model)
        {
            return model.MapTo<CreateMaterialModel, Material>();
        }

        public static Material ToEntity(this CreateMaterialModel model, Material entity)
        {
            return model.MapTo(entity);
        }

        public static Material ToEntity(this UpdateMaterialModel model)
        {
            return model.MapTo<UpdateMaterialModel, Material>();
        }

        public static Material ToEntity(this UpdateMaterialModel model, Material entity)
        {
            return model.MapTo(entity);
        }

        // from action model to view model
        public static MaterialDetailsPageViewModel ToDetailsViewModel(this CreateMaterialModel model)
        {
            return model.MapTo<CreateMaterialModel, MaterialDetailsPageViewModel>();
        }

        public static MaterialDetailsPageViewModel ToDetailsViewModel(this CreateMaterialModel model, MaterialDetailsPageViewModel viewModel)
        {
            return model.MapTo(viewModel);
        }

        public static MaterialDetailsPageViewModel ToDetailsViewModel(this UpdateMaterialModel model)
        {
            return model.MapTo<UpdateMaterialModel, MaterialDetailsPageViewModel>();
        }

        public static MaterialDetailsPageViewModel ToDetailsViewModel(this UpdateMaterialModel model, MaterialDetailsPageViewModel viewModel)
        {
            return model.MapTo(viewModel);
        }

        #endregion // Material

        #region Supplier

        // from entity to view model
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static SupplierListItemViewModel ToListItemViewModel(this Supplier entity)
        {
            return entity.MapTo<Supplier, SupplierListItemViewModel>();
        }

        /*public static SupplierDetailsPageViewModel ToDetailsViewModel(this Supplier entity)
        {
            return entity.MapTo<Supplier, SupplierDetailsPageViewModel>();
        }

        public static SupplierDetailsPageViewModel ToDetailsViewModel(this Supplier entity, SupplierDetailsPageViewModel viewModel)
        {
            return entity.MapTo(viewModel);
        }*/

        // from action model to entity
        public static Supplier ToEntity(this SupplierModel model)
        {
            return model.MapTo<SupplierModel, Supplier>();
        }

        public static Supplier ToEntity(this SupplierModel model, Supplier entity)
        {
            return model.MapTo(entity);
        }

        // from action model to view model
        /*public static SupplierDetailsPageViewModel ToDetailsViewModel(this SupplierModel model)
        {
            return model.MapTo<SupplierModel, SupplierDetailsPageViewModel>();
        }

        public static SupplierDetailsPageViewModel ToDetailsViewModel(this SupplierModel model, SupplierDetailsPageViewModel viewModel)
        {
            return model.MapTo(viewModel);
        }*/

        #endregion // Supplier
    }
}
