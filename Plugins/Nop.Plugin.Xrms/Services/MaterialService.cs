﻿using System;
using System.Collections.Generic;
using System.Linq;

using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Stores;
using Nop.Data;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Services
{
    /// <summary>
    /// Material service
    /// </summary>
    public partial class MaterialService : IMaterialService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : material ID
        /// </remarks>
        private const string MATERIALS_BY_ID_KEY = "Nop.material.id-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string MATERIALS_PATTERN_KEY = "Nop.material.";

        #endregion Constants

        #region Fields

        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<ProductRecipe> _productRecipeRepository;
        private readonly IRepository<LocalizedProperty> _localizedPropertyRepository;
        private readonly IRepository<AclRecord> _aclRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IRepository<MaterialQuantityHistory> _stockQuantityHistoryRepository;
        private readonly ILanguageService _languageService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly ICacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly LocalizationSettings _localizationSettings;
        private readonly CommonSettings _commonSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly IEventPublisher _eventPublisher;
        private readonly IAclService _aclService;
        private readonly IStoreMappingService _storeMappingService;

        #endregion Fields

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="materialRepository">Material repository</param>
        /// <param name="tierPriceRepository">Tier price repository</param>
        /// <param name="localizedPropertyRepository">Localized property repository</param>
        /// <param name="aclRepository">ACL record repository</param>
        /// <param name="storeMappingRepository">Store mapping repository</param>
        /// <param name="stockQuantityHistoryRepository">Stock quantity history repository</param>
        /// <param name="languageService">Language service</param>
        /// <param name="workflowMessageService">Workflow message service</param>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="dbContext">Database Context</param>
        /// <param name="workContext">Work context</param>
        /// <param name="localizationSettings">Localization settings</param>
        /// <param name="commonSettings">Common settings</param>
        /// <param name="catalogSettings">Catalog settings</param>
        /// <param name="eventPublisher">Event published</param>
        /// <param name="aclService">ACL service</param>
        /// <param name="storeMappingService">Store mapping service</param>
        public MaterialService(ICacheManager cacheManager,
            IRepository<Material> materialRepository,
            IRepository<ProductRecipe> productRecipeRepository,
            IRepository<LocalizedProperty> localizedPropertyRepository,
            IRepository<AclRecord> aclRepository,
            IRepository<StoreMapping> storeMappingRepository,
            IRepository<MaterialQuantityHistory> stockQuantityHistoryRepository,
            ILanguageService languageService,
            IWorkflowMessageService workflowMessageService,
            IDataProvider dataProvider,
            IDbContext dbContext,
            IWorkContext workContext,
            LocalizationSettings localizationSettings,
            CommonSettings commonSettings,
            CatalogSettings catalogSettings,
            IEventPublisher eventPublisher,
            IAclService aclService,
            IStoreMappingService storeMappingService)
        {
            this._cacheManager = cacheManager;
            this._materialRepository = materialRepository;
            this._productRecipeRepository = productRecipeRepository;
            this._localizedPropertyRepository = localizedPropertyRepository;
            this._aclRepository = aclRepository;
            this._storeMappingRepository = storeMappingRepository;
            this._stockQuantityHistoryRepository = stockQuantityHistoryRepository;
            this._languageService = languageService;
            this._workflowMessageService = workflowMessageService;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._workContext = workContext;
            this._localizationSettings = localizationSettings;
            this._commonSettings = commonSettings;
            this._catalogSettings = catalogSettings;
            this._eventPublisher = eventPublisher;
            this._aclService = aclService;
            this._storeMappingService = storeMappingService;
        }

        #endregion Ctor

        #region Utilities

        /// <summary>
        /// Search materials using LINQ
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="groupIds">Group identifiers</param>
        /// <param name="warehouseId">Warehouse identifier; 0 to load all records</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchDescriptions">A value indicating whether to search by a specified "keyword" in material descriptions</param>
        /// <param name="searchSku">A value indicating whether to search by a specified "keyword" in material SKU</param>
        /// <param name="searchLocalizedValue">A value indicating whether to search in localizable values</param>
        /// <param name="languageId">Language identifier (search for text searching)</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Materials</returns>
        protected virtual IPagedList<Material> SearchMaterialsUseLinq(
            int pageIndex, int pageSize, IList<int> groupIds,
            int warehouseId,
            string keywords, bool searchDescriptions, bool searchSku,
            bool searchLocalizedValue, int languageId,
            bool showHidden)
        {
            //materials
            var query = _materialRepository.Table;
            query = query.Where(p => !p.Deleted);

            //searching by keyword
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = from p in query
                            //join lp in _localizedPropertyRepository.Table on p.Id equals lp.EntityId into p_lp
                            //from lp in p_lp.DefaultIfEmpty()
                        where (p.Name.Contains(keywords)) ||
                              (searchDescriptions && p.Description.Contains(keywords)) ||
                              //SKU (exact match)
                              (searchSku && p.Code == keywords)
                        select p;
            }

            /*if (!showHidden && !_catalogSettings.IgnoreAcl)
            {
                //ACL (access control list)
                query = from p in query
                        join acl in _aclRepository.Table
                            on new { c1 = p.Id, c2 = "Material" } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into p_acl
                        from acl in p_acl.DefaultIfEmpty()
                        where !p.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
                        select p;
            }

            */
            //group filtering
            if (groupIds != null && groupIds.Any())
            {
                query = from p in query
                        where (groupIds.Contains(p.MaterialGroupId))
                        select p;
            }

            //warehouse filtering
            /*if (warehouseId > 0)
            {
                var manageStockInventoryMethodId = (int)ManageInventoryMethod.ManageStock;
                query = query.Where(p =>
                        //"Use multiple warehouses" enabled
                        //we search in each warehouse
                        (p.ManageInventoryMethodId == manageStockInventoryMethodId &&
                         p.UseMultipleWarehouses &&
                         p.MaterialWarehouseInventory.Any(pwi => pwi.WarehouseId == warehouseId))
                        ||
                        //"Use multiple warehouses" disabled
                        //we use standard "warehouse" property
                        ((p.ManageInventoryMethodId != manageStockInventoryMethodId ||
                          !p.UseMultipleWarehouses) &&
                         p.WarehouseId == warehouseId));
            }*/

            //only distinct materials (group by ID)
            //if we use standard Distinct() method, then all fields will be compared (low performance)
            //it'll not work in SQL Server Compact when searching materials by a keyword)
            query = from p in query
                    group p by p.Id into pGroup
                    orderby pGroup.Key
                    select pGroup.FirstOrDefault();

            var materials = new PagedList<Material>(query, pageIndex, pageSize);

            //return materials
            return materials;
        }

        #endregion Utilities

        #region Methods

        #region Materials

        /// <summary>
        /// Delete a material
        /// </summary>
        /// <param name="material">Material</param>
        public virtual void DeleteMaterial(Material material)
        {
            if (material == null)
                throw new ArgumentNullException(nameof(material));

            material.Deleted = true;
            //delete material
            UpdateMaterial(material);

            //event notification
            _eventPublisher.EntityDeleted(material);
        }

        /// <summary>
        /// Delete materials
        /// </summary>
        /// <param name="materials">Materials</param>
        public virtual void DeleteMaterials(IList<Material> materials)
        {
            if (materials == null)
                throw new ArgumentNullException(nameof(materials));

            foreach (var material in materials)
            {
                material.Deleted = true;
            }

            //delete material
            UpdateMaterials(materials);

            foreach (var material in materials)
            {
                //event notification
                _eventPublisher.EntityDeleted(material);
            }
        }

        /// <summary>
        /// Gets material
        /// </summary>
        /// <param name="materialId">Material identifier</param>
        /// <returns>Material</returns>
        public virtual Material GetMaterialById(int materialId)
        {
            if (materialId == 0)
                return null;

            var key = string.Format(MATERIALS_BY_ID_KEY, materialId);
            return _cacheManager.Get(key, () => _materialRepository.GetById(materialId));
        }

        /// <summary>
        /// Get materials by identifiers
        /// </summary>
        /// <param name="materialIds">Material identifiers</param>
        /// <returns>Materials</returns>
        public virtual IList<Material> GetMaterialsByIds(int[] materialIds)
        {
            if (materialIds == null || materialIds.Length == 0)
                return new List<Material>();

            var query = from p in _materialRepository.Table
                        where materialIds.Contains(p.Id) && !p.Deleted
                        select p;
            var materials = query.ToList();
            //sort by passed identifiers
            var sortedMaterials = new List<Material>();
            foreach (var id in materialIds)
            {
                var material = materials.Find(x => x.Id == id);
                if (material != null)
                    sortedMaterials.Add(material);
            }
            return sortedMaterials;
        }

        /// <summary>
        /// Inserts a material
        /// </summary>
        /// <param name="material">Material</param>
        public virtual void InsertMaterial(Material material)
        {
            if (material == null)
                throw new ArgumentNullException(nameof(material));

            //insert
            _materialRepository.Insert(material);

            //clear cache
            _cacheManager.RemoveByPattern(MATERIALS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(material);
        }

        /// <summary>
        /// Updates the material
        /// </summary>
        /// <param name="material">Material</param>
        public virtual void UpdateMaterial(Material material)
        {
            if (material == null)
                throw new ArgumentNullException(nameof(material));

            //update
            _materialRepository.Update(material);

            //cache
            _cacheManager.RemoveByPattern(MATERIALS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(material);
        }

        /// <summary>
        /// Update materials
        /// </summary>
        /// <param name="materials">Materials</param>
        public virtual void UpdateMaterials(IList<Material> materials)
        {
            if (materials == null)
                throw new ArgumentNullException(nameof(materials));

            //update
            _materialRepository.Update(materials);

            //cache
            _cacheManager.RemoveByPattern(MATERIALS_PATTERN_KEY);

            //event notification
            foreach (var material in materials)
            {
                _eventPublisher.EntityUpdated(material);
            }
        }

        /// <summary>
        /// Get number of material in certain material group
        /// </summary>
        /// <param name="groupIds">Group identifiers</param>
        /// <param name="warehouseId">Warehouse identifier; 0 to load all records</param>
        /// <returns>Number of materials</returns>
        public int GetNumberOfMaterialsInGroup(IList<int> groupIds = null, int warehouseId = 0)
        {
            //validate "groupIds" parameter
            if (groupIds != null && groupIds.Contains(0))
                groupIds.Remove(0);

            var query = _materialRepository.Table;
            query = query.Where(p => !p.Deleted);

            //group filtering
            if (groupIds != null && groupIds.Any())
            {
                query = from p in query
                        where (groupIds.Contains(p.MaterialGroupId))
                        select p;
            }

            /*if (!_catalogSettings.IgnoreAcl)
            {
                //Access control list. Allowed customer roles
                var allowedCustomerRolesIds = _workContext.CurrentCustomer.GetCustomerRoleIds();

                query = from p in query
                        join acl in _aclRepository.Table
                        on new { c1 = p.Id, c2 = "Material" } equals new { c1 = acl.EntityId, c2 = acl.EntityName } into p_acl
                        from acl in p_acl.DefaultIfEmpty()
                        where !p.SubjectToAcl || allowedCustomerRolesIds.Contains(acl.CustomerRoleId)
                        select p;
            }

            if (storeId > 0 && !_catalogSettings.IgnoreStoreLimitations)
            {
                query = from p in query
                        join sm in _storeMappingRepository.Table
                        on new { c1 = p.Id, c2 = "Material" } equals new { c1 = sm.EntityId, c2 = sm.EntityName } into p_sm
                        from sm in p_sm.DefaultIfEmpty()
                        where !p.LimitedToStores || storeId == sm.StoreId
                        select p;
            }
            */
            //only distinct materials
            var result = query.Select(p => p.Id).Distinct().Count();
            return result;
        }

        /// <summary>
        /// Search materials
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="groupIds">Group identifiers</param>
        /// <param name="warehouseId">Warehouse identifier; 0 to load all records</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchDescriptions">A value indicating whether to search by a specified "keyword" in material descriptions</param>
        /// <param name="searchSku">A value indicating whether to search by a specified "keyword" in material SKU</param>
        /// <param name="languageId">Language identifier (search for text searching)</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Materials</returns>
        public virtual IPagedList<Material> SearchMaterials(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            IList<int> groupIds = null,
            int warehouseId = 0,
            string keywords = null,
            bool searchDescriptions = false,
            bool searchSku = true,
            int languageId = 0,
            bool showHidden = false)
        {
            //search by keyword
            var searchLocalizedValue = false;
            if (languageId > 0)
            {
                if (showHidden)
                {
                    searchLocalizedValue = true;
                }
                else
                {
                    //ensure that we have at least two published languages
                    var totalPublishedLanguages = _languageService.GetAllLanguages().Count;
                    searchLocalizedValue = totalPublishedLanguages >= 2;
                }
            }

            //validate "groupIds" parameter
            if (groupIds != null && groupIds.Contains(0))
                groupIds.Remove(0);

            /*IPagedList<Material> materials;

            if (_commonSettings.UseStoredProceduresIfSupported && _dataProvider.StoredProceduredSupported)
            {
                //stored procedures are enabled and supported by the database.
                //It's much faster than the LINQ implementation below
                materials = SearchMaterialsUseStoredProcedure(pageIndex, pageSize, groupIds, manufacturerId, storeId, vendorId, warehouseId, keywords, searchDescriptions, searchManufacturerPartNumber, searchSku, allowedCustomerRolesIds, searchLocalizedValue, languageId, showHidden);
            }
            else
            {
                //stored procedures aren't supported. Use LINQ
                return SearchMaterialsUseLinq(pageIndex, pageSize, groupIds, manufacturerId, storeId, vendorId, warehouseId, keywords, searchDescriptions, searchManufacturerPartNumber, searchSku, searchLocalizedValue, allowedCustomerRolesIds, languageId, showHidden);
            }

            return materials;*/

            return SearchMaterialsUseLinq(pageIndex, pageSize, groupIds, warehouseId, keywords, searchDescriptions, searchSku, searchLocalizedValue, languageId, showHidden);
        }

        /// <summary>
        /// Get low stock materials
        /// </summary>
        /// <param name="vendorId">Vendor identifier; 0 to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Materials</returns>
        /*public virtual IPagedList<Material> GetLowStockMaterials(int vendorId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //Track inventory for material
            var query = from p in _materialRepository.Table
                        orderby p.MinStockQuantity
                        where !p.Deleted &&
                        p.ManageInventoryMethodId == (int)ManageInventoryMethod.ManageStock &&
                        p.MinStockQuantity >= (
                           p.UseMultipleWarehouses ?
                           p.MaterialWarehouseInventory.Sum(pwi => pwi.StockQuantity - pwi.ReservedQuantity) :
                           p.StockQuantity) &&
                        (vendorId == 0 || p.VendorId == vendorId)
                        select p;

            return new PagedList<Material>(query, pageIndex, pageSize);
        }*/

        /// <summary>
        /// Get low stock material combinations
        /// </summary>
        /// <param name="vendorId">Vendor identifier; 0 to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Material combinations</returns>
        /*public virtual IPagedList<MaterialAttributeCombination> GetLowStockMaterialCombinations(int vendorId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //Track inventory for material by material attributes
            var query = from p in _materialRepository.Table
                        from c in p.MaterialAttributeCombinations
                        where !p.Deleted &&
                        p.ManageInventoryMethodId == (int)ManageInventoryMethod.ManageStockByAttributes &&
                        c.StockQuantity <= 0 &&
                        (vendorId == 0 || p.VendorId == vendorId)
                        select c;
            query = query.OrderBy(c => c.MaterialId);
            return new PagedList<MaterialAttributeCombination>(query, pageIndex, pageSize);
        }
        */

        /// <summary>
        /// Gets a material by code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Material</returns>
        public virtual Material GetMaterialByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            code = code.Trim();

            var query = from p in _materialRepository.Table
                        orderby p.Id
                        where !p.Deleted &&
                        p.Code == code
                        select p;
            var material = query.FirstOrDefault();
            return material;
        }

        /// <summary>
        /// Gets a materials by code array
        /// </summary>
        /// <param name="codeArray">Code array</param>
        /// <returns>Materials</returns>
        public IList<Material> GetMaterialsByCode(string[] codeArray)
        {
            if (codeArray == null)
                throw new ArgumentNullException(nameof(codeArray));

            var query = _materialRepository.Table;
            query = query.Where(p => !p.Deleted && codeArray.Contains(p.Code));

            return query.ToList();
        }

        #endregion Materials

        #region Stock quantity history

        /// <summary>
        /// Add stock quantity change entry
        /// </summary>
        /// <param name="material">Material</param>
        /// <param name="quantityAdjustment">Quantity adjustment</param>
        /// <param name="stockQuantity">Current stock quantity</param>
        /// <param name="warehouseId">Warehouse identifier</param>
        /// <param name="message">Message</param>
        /// <param name="combinationId">Material attribute combination identifier</param>
        public virtual void AddStockQuantityHistoryEntry(Material material, int quantityAdjustment, int stockQuantity,
            int warehouseId = 0, string message = "", int? combinationId = null)
        {
            if (material == null)
                throw new ArgumentNullException(nameof(material));

            if (quantityAdjustment == 0)
                return;

            var historyEntry = new MaterialQuantityHistory
            {
                MaterialId = material.Id,
                CombinationId = combinationId,
                WarehouseId = warehouseId > 0 ? (int?)warehouseId : null,
                QuantityAdjustment = quantityAdjustment,
                StockQuantity = stockQuantity,
                Message = message,
                CreatedOnUtc = DateTime.UtcNow
            };

            _stockQuantityHistoryRepository.Insert(historyEntry);

            //event notification
            _eventPublisher.EntityInserted(historyEntry);
        }

        /// <summary>
        /// Get the history of the material stock quantity changes
        /// </summary>
        /// <param name="material">Material</param>
        /// <param name="warehouseId">Warehouse identifier; pass 0 to load all entries</param>
        /// <param name="combinationId">Material attribute combination identifier; pass 0 to load all entries</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of stock quantity change entries</returns>
        public virtual IPagedList<MaterialQuantityHistory> GetStockQuantityHistory(Material material, int warehouseId = 0, int combinationId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            if (material == null)
                throw new ArgumentNullException(nameof(material));

            var query = _stockQuantityHistoryRepository.Table.Where(historyEntry => historyEntry.MaterialId == material.Id);

            if (warehouseId > 0)
                query = query.Where(historyEntry => historyEntry.WarehouseId == warehouseId);

            if (combinationId > 0)
                query = query.Where(historyEntry => historyEntry.CombinationId == combinationId);

            query = query.OrderByDescending(historyEntry => historyEntry.CreatedOnUtc).ThenByDescending(historyEntry => historyEntry.Id);

            return new PagedList<MaterialQuantityHistory>(query, pageIndex, pageSize);
        }

        #endregion Stock quantity history

        #region Product Recipe

        /// <summary>
        /// Deletes a product recipe item
        /// </summary>
        /// <param name="productRecipe">Product recipe</param>
        public void DeleteProductRecipeItem(ProductRecipe productRecipe)
        {
            if (productRecipe == null)
                throw new ArgumentNullException(nameof(productRecipe));

            _productRecipeRepository.Delete(productRecipe);

            //event notification
            _eventPublisher.EntityDeleted(productRecipe);
        }

        /// <summary>
        /// Gets a product recipe collection
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product recipe collection</returns>
        public IList<ProductRecipe> GetProductRecipesByProductId(int productId, bool showHidden = false)
        {
            if (productId == 0)
                return new List<ProductRecipe>();

            var query = from pr in _productRecipeRepository.Table
                        join m in _materialRepository.Table on pr.MaterialId equals m.Id
                        where pr.ProductId == productId &&
                                !m.Deleted && showHidden
                        orderby pr.DisplayOrder, pr.Id
                        select pr;

            var allProductRecipes = query.ToList();
            var result = new List<ProductRecipe>();
            //no filtering
            result.AddRange(allProductRecipes);

            return result;
        }

        /// <summary>
        /// Gets a product recipe item
        /// </summary>
        /// <param name="productRecipeId">Product recipe identifier</param>
        /// <returns>Product recipe</returns>
        public ProductRecipe GetProductRecipeById(int productRecipeId)
        {
            if (productRecipeId == 0)
                return null;

            return _productRecipeRepository.GetById(productRecipeId);
        }

        /// <summary>
        /// Inserts a product recipe item
        /// </summary>
        /// <param name="productRecipe">>Product recipe</param>
        public void InsertProductRecipe(ProductRecipe productRecipe)
        {
            if (productRecipe == null)
                throw new ArgumentNullException(nameof(productRecipe));

            _productRecipeRepository.Insert(productRecipe);

            //event notification
            _eventPublisher.EntityInserted(productRecipe);
        }

        /// <summary>
        /// Updates the product recipe
        /// </summary>
        /// <param name="productRecipe">>Product recipe</param>
        public void UpdateProductRecipe(ProductRecipe productRecipe)
        {
            if (productRecipe == null)
                throw new ArgumentNullException(nameof(productRecipe));

            _productRecipeRepository.Update(productRecipe);

            //event notification
            _eventPublisher.EntityUpdated(productRecipe);
        }

        #endregion Product Recipe

        #endregion Methods
    }
}