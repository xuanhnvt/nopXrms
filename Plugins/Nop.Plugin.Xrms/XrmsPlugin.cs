using System;
using System.Linq;

using Microsoft.AspNetCore.Routing;
using Nop.Core.Plugins;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Customers;
using Nop.Core.Domain.Customers;
using Nop.Web.Framework;
using Nop.Web.Framework.Menu;

using Nop.Plugin.Xrms.Data;
using Nop.Plugin.Xrms.Domain;
using Nop.Plugin.Xrms.Services;
using Nop.Core.Domain.Security;

namespace Nop.Plugin.Xrms
{
    public class XrmsPlugin : BasePlugin, IAdminMenuPlugin
    {
        private readonly XrmsObjectContext _objectContext;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerService _customerService;
        private readonly IPermissionService _permissionService;
        private readonly IMaterialGroupService _materialGroupService;

        public XrmsPlugin(XrmsObjectContext objectContext,
            ILocalizationService localizationService,
            ICustomerService customerService,
            IPermissionService permissionService,
            IMaterialGroupService materialGroupService)
        {
            _objectContext = objectContext;
            _localizationService = localizationService;
            _customerService = customerService;
            _permissionService = permissionService;
            _materialGroupService = materialGroupService;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Install()
        {
            _objectContext.Install();

            var accessAdminPanelPermission = _permissionService.GetPermissionRecordBySystemName(StandardPermissionProvider.AccessAdminPanel.SystemName);
            // install system customer role first, and AccessAdminPanel permission
            var customerRole = new CustomerRole
            {
                Name = "Cashiers",
                Active = true,
                IsSystemRole = false,
                SystemName = XrmsCustomerDefaults.CashiersRoleName
            };
            customerRole.PermissionRecordCustomerRoleMappings
                .Add(new PermissionRecordCustomerRoleMapping
                {
                    PermissionRecord = accessAdminPanelPermission
                });
            _customerService.InsertCustomerRole(customerRole);

            customerRole = new CustomerRole
            {
                Name = "Waiters",
                Active = true,
                IsSystemRole = false,
                SystemName = XrmsCustomerDefaults.WaitersRoleName
            };
            customerRole.PermissionRecordCustomerRoleMappings
                .Add(new PermissionRecordCustomerRoleMapping
                {
                    PermissionRecord = accessAdminPanelPermission
                });
            _customerService.InsertCustomerRole(customerRole);

            customerRole = new CustomerRole
            {
                Name = "Kitchen",
                Active = true,
                IsSystemRole = false,
                SystemName = XrmsCustomerDefaults.KitchenRoleName
            };
            customerRole.PermissionRecordCustomerRoleMappings
                .Add(new PermissionRecordCustomerRoleMapping
                {
                    PermissionRecord = accessAdminPanelPermission
                });
            _customerService.InsertCustomerRole(customerRole);
            
            // then add xrms default permission
            _permissionService.InstallPermissions(new XrmsPermissionProvider());


            // create locales
            #region Admin Menu

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin", "Xrms");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Materials", "Materials");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.MaterialGroups", "Material Groups");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Suppliers", "Suppliers");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Tables", "Tables");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Cashier", "Cashier");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Waiter", "Waiter");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Kitchen", "Kitchen");

            #endregion // Admin Menu

            #region Suppliers

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Notifications.Created", "The new supplier has been created successfully.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Notifications.Updated", "The supplier has been updated successfully.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Notifications.Deleted", "The supplier has been deleted successfully.");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.ActivityLog.AddNewSupplier", "Added a new supplier ('{0}')");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.ActivityLog.EditSupplier", "Edited a supplier ('{0}')");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.ActivityLog.DeleteSupplier", "Deleted a supplier ('{0}')");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.List.Title", "Suppliers");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.List.Search.SupplierName", "Supplier name");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Create.Title", "Create a new supplier");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Edit.Title", "Edit supplier details");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Buttons.BackToList", "back to supplier list");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Tabs.Info", "Supplier info");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Tabs.Materials", "Materials");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Tabs.Materials.Columns.Material", "Material");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Tabs.Materials.Buttons.RemoveMaterial", "Remove");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Tabs.Materials.Buttons.AddMaterials", "Add Materials");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Tabs.Materials.Hints.SaveBeforeEdit", "You need to save the supplier before you can add materials for this supplier page.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.AddMaterialsPopup.Title", "Add materials into supplier");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Fields.Name", "Name");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Fields.Name.Required", "Please provide a name.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Fields.Description", "Description");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Fields.Picture", "Picture");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Fields.DisplayOrder", "Display Order");

            #endregion Suppliers

            #region Material Groups

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Notifications.Created", "The new material group has been created successfully.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Notifications.Updated", "The material group has been updated successfully.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Notifications.Deleted", "The material group has been deleted successfully.");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.ActivityLog.AddNewMaterialGroup", "Added a new material group ('{0}')");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.ActivityLog.EditMaterialGroup", "Edited a material group ('{0}')");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.ActivityLog.DeleteMaterialGroup", "Deleted a material group ('{0}')");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.List.Title", "Material Groups");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.List.Hints.ImportFromExcelTip", "Imported groups are distinguished by ID. If the ID already exists, then its corresponding group will be updated. You should not specify ID (leave 0) for new groups.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.List.Search.MaterialGroupName", "Group name");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Create.Title", "Create a new material group");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Edit.Title", "Edit material group details");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Buttons.BackToList", "back to material group list");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Tabs.Info", "Material group info");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Tabs.Materials", "Materials");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Tabs.Materials.Columns.Material", "Material");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Tabs.Materials.Buttons.RemoveMaterial", "Remove");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Tabs.Materials.Buttons.AddMaterials", "Add Materials");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Tabs.Materials.Hints.SaveBeforeEdit", "You need to save the group before you can add materials for this group page.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.AddMaterialsPopup.Title", "Add materials into group");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.Name", "Name");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.Name.Required", "Please provide a name.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.Description", "Description");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.Parent", "Parent group");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.Parent.None", "None");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.Picture", "Picture");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.DisplayOrder", "Display Order");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.AclCustomerRoles", "Limited to customer roles");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.LimitedToStores", "Limited to stores");

            #endregion Material Groups

            #region Materials

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Messages.Edit", "The stock quantity has been edited.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Messages.EditWarehouse", "Materials have been moved {0} {1}.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Messages.EditWarehouse.Old", "from the {0}");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Messages.EditWarehouse.New", "to the {0}");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Notifications.Created", "The new material has been created successfully.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Notifications.Updated", "The material has been updated successfully.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Notifications.Deleted", "The material has been deleted successfully.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Notifications.Copied", "The material has been copied successfully.");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.ActivityLog.AddNewMaterial", "Added a new material ('{0}')");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.ActivityLog.EditMaterial", "Edited a material ('{0}')");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.ActivityLog.DeleteMaterial", "Deleted a material ('{0}')");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.List.Search.MaterialName", "Material name");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.List.Search.MaterialGroup", "Material group");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.List.Search.IncludeSubGroup", "Search sub groups");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.List.Search.Supplier", "Supplier");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.List.Search.Warehouse", "Warehouse");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.List.Title", "Materials");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.List.Hints.ImportFromExcelTip", "Imported materials are distinguished by ID. If the ID already exists, then its corresponding material will be updated. You should not specify ID (leave 0) for new materials.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.List.Buttons.DownloadPDF", "Download list as PDF");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Details.Create.Title", "Create a new material");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Details.Edit.Title", "Edit material details");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Details.Buttons.BackToList", "back to material list");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Details.Tabs.Info", "Material info");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Details.Tabs.Info.Sections.CommonInfo", "General information");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Details.Tabs.Info.Sections.Inventory", "Inventory");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Details.Tabs.StockQuantityHistory", "Stock quantity history");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Details.Tabs.StockQuantityHistory.Hint", "Here you can see a history of the material stock quantity changes.");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Name", "Name");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Name.Required", "Please provide a name.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Description", "Description");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.MaterialGroup", "Material Group");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Picture", "Picture");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.DisplayOrder", "Display Order");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Code", "Code");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Supplier", "Manufacturer");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.ManageInventoryMethod", "Inventory method");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Warehouse", "Warehouse");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Warehouse.None", "None");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.StockQuantity", "Stock Quantity");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.UsedQuantity", "Used Quantity");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.MinStockQuantity", "Minimum Quantity");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Unit", "Unit");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Cost", "Cost");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.AdminComment", "Admin comment");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.Warehouse", "Warehouse");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.Combination", "Combination");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.QuantityAdjustment", "Quantity Adjustment");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.StockQuantity", "Stock Quantity");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.Message", "Message");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.CreatedOn", "Datetime");

            #endregion Materials

            #region Product Extensions

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Products.Details.Tabs.ProductRecipes", "Product Recipes");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Products.Details.Tabs.ProductRecipes.Buttons.AddRecipes", "Add Recipes");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Products.Details.Tabs.ProductRecipes.Hints.SaveBeforeEdit", "You need to save the product before you can add recipes.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Products.Details.AddProductRecipesPopup.Title", "Add materials into Product Recipe");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.ProductRecipes.Fields.Name", "Material");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.ProductRecipes.Fields.Unit", "Unit");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.ProductRecipes.Fields.DisplayOrder", "Display Order");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.ProductRecipes.Fields.Quantity", "Quantity");

            #endregion Product Extensions

            #region Tables

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Tables.Notifications.Created", "The new table has been created successfully.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Tables.Notifications.Updated", "The table has been updated successfully.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Tables.Notifications.Deleted", "The table has been deleted successfully.");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.ActivityLog.AddNewTable", "Added a new table ('{0}')");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.ActivityLog.EditTable", "Edited a table ('{0}')");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.ActivityLog.DeleteTable", "Deleted a table ('{0}')");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Tables.List.Title", "Tables");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Tables.List.Hints.ImportFromExcelTip", "Imported tables are distinguished by ID. If the ID already exists, then its corresponding table will be updated. You should not specify ID (leave 0) for new table.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Tables.List.Search.TableName", "Table name");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Tables.Details.Create.Title", "Create a new table");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Tables.Details.Edit.Title", "Edit table details");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Tables.Details.Buttons.BackToList", "back to table list");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Tables.Details.Tabs.Info", "Table info");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Tables.Fields.Name", "Name");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Tables.Fields.Name.Required", "Please provide a name.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Tables.Fields.Description", "Description");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Tables.Fields.State", "State");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Tables.Fields.Picture", "Picture");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Catalog.Tables.Fields.DisplayOrder", "Display Order");

            #endregion Tables

            #region In-Store Orders

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.Notifications.Created", "The new order has been created successfully.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.Notifications.Updated", "The order has been updated successfully.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.Notifications.Cancelled", "The order has been cancelled successfully.");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.ActivityLog.CreateNewInStoreOrder", "Created a new order ('{0}')");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.ActivityLog.EditInStoreOrder", "Editted an order ('{0}')");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.ActivityLog.CancelInStoreOrder", "Cancelled an order ('{0}')");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.Table", "Table");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.Validations.Table.Required", "Please provide a '{PropertyName}'.");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.Waiter", "Waiter");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.Code", "Order No");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.State", "State");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.DisplayOrder", "Display Order");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.PrintCount", "Print Count");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.CreatedTime", "Created Time");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.UpdatedTime", "Updated Time");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.BilledTime", "Billed Time");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.CheckedOutTime", "Checkout Time");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.OrderItems.Fields.Modifying", "Modifying");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.OrderItems.Fields.ProductName", "Product");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.OrderItems.Fields.ProductPrice", "Unit price");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.OrderItems.Fields.Quantity", "Quantity");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.OrderItems.Fields.TotalPrice", "Total price");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.Details.ProductList.Search.ProductName", "Product name");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.InStoreOrders.Details.ProductList.Search.Category", "Category");

            #endregion In-Store Orders

            #region Cashier Orders

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Cashier.Orders.List.Title", "Cashier Orders");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Cashier.Orders.Details.Create.Title", "Create New Order");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Cashier.Orders.Details.Edit.Title", "Edit Order Details");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Cashier.Orders.Details.Buttons.BackToList", "back to order list");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Cashier.Orders.Details.Tabs.OrderDetails", "Order details");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Cashier.Orders.Details.Tabs.OrderDetails.General", "General information");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Cashier.Orders.Details.Tabs.OrderDetails.OrderItems", "Order items");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Cashier.Orders.Details.Tabs.ProductList", "Product list");

            #endregion Cashier Orders

            #region Waiter Orders

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Waiter.Orders.List.Title", "Waiter Orders");

            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Waiter.Orders.Details.Create.Title", "Create New Order");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Waiter.Orders.Details.Edit.Title", "Edit Order Details");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Waiter.Orders.Details.Buttons.BackToList", "back to order list");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Waiter.Orders.Details.Tabs.OrderDetails", "Order details");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Waiter.Orders.Details.Tabs.OrderDetails.General", "General information");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Waiter.Orders.Details.Tabs.OrderDetails.OrderItems", "Order items");
            _localizationService.AddOrUpdatePluginLocaleResource("Xrms.Admin.Waiter.Orders.Details.Tabs.ProductList", "Product list");

            #endregion Waiter Orders

            // create default data
            this._materialGroupService.InsertMaterialGroup(new MaterialGroup()
            {
                Name = "Undefined",
                Description = "Use as defaut group. User could not delete or update info.",
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            });

            base.Install();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Uninstall()
        {
            _objectContext.Uninstall();

            // remove permissions
            _permissionService.UninstallPermissions(new XrmsPermissionProvider());

            // then remove system customer roles
            var customerRole = _customerService.GetCustomerRoleBySystemName(XrmsCustomerDefaults.CashiersRoleName);
            if (customerRole != null)
                _customerService.DeleteCustomerRole(customerRole);
            customerRole = _customerService.GetCustomerRoleBySystemName(XrmsCustomerDefaults.WaitersRoleName);
            if (customerRole != null)
                _customerService.DeleteCustomerRole(customerRole);
            customerRole = _customerService.GetCustomerRoleBySystemName(XrmsCustomerDefaults.KitchenRoleName);
            if (customerRole != null)
                _customerService.DeleteCustomerRole(customerRole);

            // delete all locale resources

            #region Admin Menu

            _localizationService.DeletePluginLocaleResource("Xrms.Admin");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Materials");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.MaterialGroups");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Suppliers");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Tables");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Cashier");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Waiter");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Kitchen");

            #endregion // Admin Menu

            #region Suppliers

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Notifications.Created");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Notifications.Updated");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Notifications.Deleted");

            _localizationService.DeletePluginLocaleResource("Xrms.ActivityLog.AddNewSupplier");
            _localizationService.DeletePluginLocaleResource("Xrms.ActivityLog.EditSupplier");
            _localizationService.DeletePluginLocaleResource("Xrms.ActivityLog.DeleteSupplier");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.List.Title");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.List.Search.SupplierName");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Create.Title");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Edit.Title");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Buttons.BackToList");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Tabs.Info");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Tabs.Materials");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Tabs.Materials.Columns.Material");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Tabs.Materials.Buttons.RemoveMaterial");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Tabs.Materials.Buttons.AddMaterials");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.Tabs.Materials.Hints.SaveBeforeEdit");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Details.AddMaterialsPopup.Title");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Fields.Name");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Fields.Name.Required");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Fields.Description");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Fields.Picture");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Suppliers.Fields.DisplayOrder");

            #endregion Suppliers

            #region Material Groups

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Notifications.Created");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Notifications.Updated");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Notifications.Deleted");

            _localizationService.DeletePluginLocaleResource("Xrms.ActivityLog.AddNewMaterialGroup");
            _localizationService.DeletePluginLocaleResource("Xrms.ActivityLog.EditMaterialGroup");
            _localizationService.DeletePluginLocaleResource("Xrms.ActivityLog.DeleteMaterialGroup");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.List.Title");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.List.Hints.ImportFromExcelTip");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.List.Search.MaterialGroupName");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Create.Title");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Buttons.BackToList");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Edit.Title");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Tabs.Info");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Tabs.Materials");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Tabs.Materials.Columns.Material");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Tabs.Materials.Buttons.RemoveMaterial");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Tabs.Materials.Hints.SaveBeforeEdit");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.Tabs.Materials.Buttons.AddMaterials");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Details.AddMaterialsPopup.Title");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.Name");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.Name.Required");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.Description");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.Parent");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.Parent.None");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.Picture");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.DisplayOrder");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.AclCustomerRoles");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.MaterialGroups.Fields.LimitedToStores");

            #endregion Material Groups

            #region Materials

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Messages.Edit");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Messages.EditWarehouse");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Messages.EditWarehouse.Old");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Messages.EditWarehouse.New");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Notifications.Created");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Notifications.Updated");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Notifications.Deleted");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Notifications.Copied");

            _localizationService.DeletePluginLocaleResource("Xrms.ActivityLog.AddNewMaterial");
            _localizationService.DeletePluginLocaleResource("Xrms.ActivityLog.EditMaterial");
            _localizationService.DeletePluginLocaleResource("Xrms.ActivityLog.DeleteMaterial");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.List.Search.MaterialName");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.List.Search.MaterialGroup");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.List.Search.IncludeSubGroup");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.List.Search.Supplier");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.List.Search.Warehouse");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.List.Title");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.List.Hints.ImportFromExcelTip");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Details.Create.Title");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.List.Buttons.DownloadPDF");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Details.Buttons.BackToList");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Details.Edit.Title");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Details.Tabs.Info");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Details.Tabs.Info.Sections.CommonInfo");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Details.Tabs.Info.Sections.Inventory");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Details.Tabs.StockQuantityHistory");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Details.Tabs.StockQuantityHistory.Hint");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Name");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Name.Required");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Description");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.MaterialGroup");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Picture");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.DisplayOrder");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Code");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Supplier");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.ManageInventoryMethod");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Warehouse");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Warehouse.None");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.StockQuantity");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.UsedQuantity");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.MinStockQuantity");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Unit");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.Cost");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.Fields.AdminComment");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.Warehouse");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.Combination");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.QuantityAdjustment");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.StockQuantity");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.Message");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Materials.StockQuantityHistory.Fields.CreatedOn");

            #endregion Materials

            #region Product Extensions

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Products.Details.Tabs.ProductRecipes");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Products.Details.Tabs.ProductRecipes.Buttons.AddRecipes");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Products.Details.Tabs.ProductRecipes.Hints.SaveBeforeEdit");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Products.Details.AddProductRecipesPopup.Title");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.ProductRecipes.Fields.Name");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.ProductRecipes.Fields.Unit");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.ProductRecipes.Fields.DisplayOrder");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.ProductRecipes.Fields.Quantity");

            #endregion Product Extensions

            #region Tables

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Tables.Notifications.Created");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Tables.Notifications.Updated");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Tables.Notifications.Deleted");

            _localizationService.DeletePluginLocaleResource("Xrms.ActivityLog.AddNewTable");
            _localizationService.DeletePluginLocaleResource("Xrms.ActivityLog.EditTable");
            _localizationService.DeletePluginLocaleResource("Xrms.ActivityLog.DeleteTable");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Tables.List.Title");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Tables.List.Hints.ImportFromExcelTip");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Tables.List.Search.TableName");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Tables.Details.Create.Title");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Tables.Details.Edit.Title");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Tables.Details.Buttons.BackToList");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Tables.Details.Tabs.Info");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Tables.Fields.Name");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Tables.Fields.Name.Required");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Tables.Fields.Description");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Tables.Fields.State");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Tables.Fields.Picture");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Catalog.Tables.Fields.DisplayOrder");

            #endregion Tables

            #region In-Store Orders

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.Notifications.Created");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.Notifications.Updated");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.Notifications.Cancelled");

            _localizationService.DeletePluginLocaleResource("Xrms.ActivityLog.CreateNewInStoreOrder");
            _localizationService.DeletePluginLocaleResource("Xrms.ActivityLog.EditInStoreOrder");
            _localizationService.DeletePluginLocaleResource("Xrms.ActivityLog.CancelInStoreOrder");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.Table");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.Validations.Table.Required");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.Waiter");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.Code");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.State");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.DisplayOrder");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.PrintCount");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.CreatedTime");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.UpdatedTime");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.BilledTime");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.Fields.CheckedOutTime");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.OrderItems.Fields.Modifying");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.OrderItems.Fields.ProductName");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.OrderItems.Fields.ProductPrice");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.OrderItems.Fields.Quantity");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.OrderItems.Fields.TotalPrice");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.Details.ProductList.Search.ProductName");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.InStoreOrders.Details.ProductList.Search.Category");

            #endregion In-Store Orders

            #region Cashier Orders

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Cashier.Orders.List.Title");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Cashier.Orders.Details.Create.Title");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Cashier.Orders.Details.Edit.Title");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Cashier.Orders.Details.Buttons.BackToList");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Cashier.Orders.Details.Tabs.OrderDetails");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Cashier.Orders.Details.Tabs.OrderDetails.General");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Cashier.Orders.Details.Tabs.OrderDetails.OrderItems");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Cashier.Orders.Details.Tabs.ProductList");

            #endregion Cashier Orders

            #region Waiter Orders

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Waiter.Orders.List.Title");

            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Waiter.Orders.Details.Create.Title");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Waiter.Orders.Details.Edit.Title");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Waiter.Orders.Details.Buttons.BackToList");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Waiter.Orders.Details.Tabs.OrderDetails");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Waiter.Orders.Details.Tabs.OrderDetails.General");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Waiter.Orders.Details.Tabs.OrderDetails.OrderItems");
            _localizationService.DeletePluginLocaleResource("Xrms.Admin.Waiter.Orders.Details.Tabs.ProductList");

            #endregion Waiter Orders

            base.Uninstall();
        }

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var xrmsSitemap = new XmlSiteMap();
            xrmsSitemap.LoadFrom("~/Plugins/Xrms/Areas/Admin/xrmsSitemap.config");
            var pluginNode = xrmsSitemap.RootNode;
            rootNode.ChildNodes.Add(pluginNode);
            /*var pluginNode = new SiteMapNode()
            {
                SystemName = "Cashier",
                Title = "Cashier",
                Visible = true
            };

            pluginNode.ChildNodes.Add(new SiteMapNode()
            {
                SystemName = "CashierOrders",
                Title = "Orders",
                ControllerName = "CashierOrder",
                ActionName = "List",
                IconClass = "fa-dot-circle-o",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.Admin } },
            });

            rootNode.ChildNodes.Add(pluginNode);

            pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Catalog");
            if (pluginNode != null)
            {
                //pluginNode.ChildNodes.Add(menuItem);
                // do nothing
            }
            else
            {
                pluginNode = rootNode;
                //rootNode.ChildNodes.Add(menuItem);
            }

            pluginNode.ChildNodes.Add(new SiteMapNode()
            {
                SystemName = "Materials",
                Title = "Materials",
                ControllerName = "Material",
                ActionName = "List",
                IconClass = "fa-dot-circle-o",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.Admin } },
            });

            pluginNode.ChildNodes.Add(new SiteMapNode()
            {
                SystemName = "MaterialGroups",
                Title = "Material Groups",
                ControllerName = "MaterialGroup",
                ActionName = "List",
                IconClass = "fa-dot-circle-o",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.Admin } },
            });

            pluginNode.ChildNodes.Add(new SiteMapNode()
            {
                SystemName = "Suppliers",
                Title = "Suppliers",
                ControllerName = "Supplier",
                ActionName = "List",
                IconClass = "fa-dot-circle-o",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.Admin } },
            });

            pluginNode.ChildNodes.Add(new SiteMapNode()
            {
                SystemName = "Tables",
                Title = "Tables",
                ControllerName = "Table",
                ActionName = "List",
                IconClass = "fa-dot-circle-o",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.Admin } },
            });
            */
        }
    }
}