using Nop.Core;
using Nop.Plugin.Xrms.Domain;
using System.Collections.Generic;

namespace Nop.Plugin.Xrms.Services
{
    /// <summary>
    /// Material service
    /// </summary>
    public partial interface IMaterialService
    {
        #region Materials

        /// <summary>
        /// Delete a material
        /// </summary>
        /// <param name="material">Material</param>
        void DeleteMaterial(Material material);

        /// <summary>
        /// Delete materials
        /// </summary>
        /// <param name="materials">Materials</param>
        void DeleteMaterials(IList<Material> materials);

        /// <summary>
        /// Gets material
        /// </summary>
        /// <param name="materialId">Material identifier</param>
        /// <returns>Material</returns>
        Material GetMaterialById(int materialId);

        /// <summary>
        /// Gets materials by identifier
        /// </summary>
        /// <param name="materialIds">Material identifiers</param>
        /// <returns>Materials</returns>
        IList<Material> GetMaterialsByIds(int[] materialIds);

        /// <summary>
        /// Inserts a material
        /// </summary>
        /// <param name="material">Material</param>
        void InsertMaterial(Material material);

        /// <summary>
        /// Updates the material
        /// </summary>
        /// <param name="material">Material</param>
        void UpdateMaterial(Material material);

        /// <summary>
        /// Updates the materials
        /// </summary>
        /// <param name="materials">Material</param>
        void UpdateMaterials(IList<Material> materials);

        /// <summary>
        /// Get number of material in certain material group
        /// </summary>
        /// <param name="groupIds">Group identifiers</param>
        /// <param name="warehouseId">Warehouse identifier; 0 to load all records</param>
        /// <returns>Number of materials</returns>
        int GetNumberOfMaterialsInGroup(IList<int> groupIds = null, int warehouseId = 0);

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
        IPagedList<Material> SearchMaterials(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            IList<int> groupIds = null,
            int warehouseId = 0,
            string keywords = null,
            bool searchDescriptions = false,
            bool searchSku = true,
            int languageId = 0,
            bool showHidden = false);

        /// <summary>
        /// Get low stock materials
        /// </summary>
        /// <param name="vendorId">Vendor identifier; 0 to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Materials</returns>
        //IPagedList<Material> GetLowStockMaterials(int vendorId = 0, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets a material by code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Material</returns>
        Material GetMaterialByCode(string code);

        /// <summary>
        /// Gets a materials by code array
        /// </summary>
        /// <param name="codeArray">Code array</param>
        /// <returns>Materials</returns>
        IList<Material> GetMaterialsByCode(string[] codeArray);

        #endregion Materials

        #region Inventory management methods

        /// <summary>
        /// Adjust inventory
        /// </summary>
        /// <param name="material">Material</param>
        /// <param name="quantityToChange">Quantity to increase or decrease</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="message">Message for the stock quantity history</param>
        //void AdjustInventory(Material material, int quantityToChange, string attributesXml = "", string message = "");

        /// <summary>
        /// Reserve the given quantity in the warehouses.
        /// </summary>
        /// <param name="material">Material</param>
        /// <param name="quantity">Quantity, must be negative</param>
        //void ReserveInventory(Material material, int quantity);

        /// <summary>
        /// Unblocks the given quantity reserved items in the warehouses
        /// </summary>
        /// <param name="material">Material</param>
        /// <param name="quantity">Quantity, must be positive</param>
        //void UnblockReservedInventory(Material material, int quantity);

        /// <summary>
        /// Book the reserved quantity
        /// </summary>
        /// <param name="material">Material</param>
        /// <param name="warehouseId">Warehouse identifier</param>
        /// <param name="quantity">Quantity, must be negative</param>
        /// <param name="message">Message for the stock quantity history</param>
        //void BookReservedInventory(Material material, int warehouseId, int quantity, string message = "");

        /// <summary>
        /// Reverse booked inventory (if acceptable)
        /// </summary>
        /// <param name="material">material</param>
        /// <param name="shipmentItem">Shipment item</param>
        /// <returns>Quantity reversed</returns>
        /// <param name="message">Message for the stock quantity history</param>
        //int ReverseBookedInventory(Material material, ShipmentItem shipmentItem, string message = "");

        #endregion Inventory management methods

        #region Material warehouse inventory

        /// <summary>
        /// Deletes a MaterialWarehouseInventory
        /// </summary>
        /// <param name="pwi">MaterialWarehouseInventory</param>
        //void DeleteMaterialWarehouseInventory(MaterialWarehouseInventory pwi);

        #endregion Material warehouse inventory

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
        void AddStockQuantityHistoryEntry(Material material, int quantityAdjustment, int stockQuantity,
            int warehouseId = 0, string message = "", int? combinationId = null);

        /// <summary>
        /// Get the history of the material stock quantity changes
        /// </summary>
        /// <param name="material">Material</param>
        /// <param name="warehouseId">Warehouse identifier; pass 0 to load all entries</param>
        /// <param name="combinationId">Material attribute combination identifier; pass 0 to load all entries</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of stock quantity change entries</returns>
        IPagedList<MaterialQuantityHistory> GetStockQuantityHistory(Material material, int warehouseId = 0, int combinationId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue);

        #endregion Stock quantity history

        #region Product Recipe

        /// <summary>
        /// Deletes a product recipe item
        /// </summary>
        /// <param name="productRecipe">Product recipe</param>
        void DeleteProductRecipeItem(ProductRecipe productRecipe);

        /// <summary>
        /// Gets a product recipe collection
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product recipe collection</returns>
        IList<ProductRecipe> GetProductRecipesByProductId(int productId, bool showHidden = false);

        /// <summary>
        /// Gets a product recipe item
        /// </summary>
        /// <param name="productRecipeId">Product recipe identifier</param>
        /// <returns>Product recipe</returns>
        ProductRecipe GetProductRecipeById(int productRecipeId);

        /// <summary>
        /// Inserts a product recipe item
        /// </summary>
        /// <param name="productRecipe">>Product recipe</param>
        void InsertProductRecipe(ProductRecipe productRecipe);

        /// <summary>
        /// Updates the product recipe
        /// </summary>
        /// <param name="productRecipe">>Product recipe</param>
        void UpdateProductRecipe(ProductRecipe productRecipe);

        #endregion Product Recipe
    }
}