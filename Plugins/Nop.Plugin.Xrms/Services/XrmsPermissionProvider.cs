using System;
using System.Collections.Generic;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Security;
using Nop.Plugin.Xrms.Domain;
using Nop.Services.Security;

namespace Nop.Plugin.Xrms.Services
{
    /// <summary>
    /// XRMS permission provider
    /// </summary>
    public partial class XrmsPermissionProvider : IPermissionProvider
    {
        //admin area permissions
        public static readonly PermissionRecord ManageSuppliers = new PermissionRecord { Name = "Admin area. XRMS Manage Suppliers", SystemName = "XrmsManageSuppliers", Category = "XRMS" };
        public static readonly PermissionRecord ManageMaterials = new PermissionRecord { Name = "Admin area. XRMS Manage Materials", SystemName = "XrmsManageMaterials", Category = "XRMS" };
        public static readonly PermissionRecord ManageMaterialGroups = new PermissionRecord { Name = "Admin area. XRMS Manage Material Groups", SystemName = "XrmsManageMaterialGroups", Category = "XRMS" };
        public static readonly PermissionRecord ManageTables = new PermissionRecord { Name = "Admin area. XRMS Manage Tables", SystemName = "XrmsManageTables", Category = "XRMS" };
        public static readonly PermissionRecord ManageCashierOrders = new PermissionRecord { Name = "Admin area. XRMS screen for Cashiers", SystemName = "XrmsManageCashierOrders", Category = "XRMS" };
        public static readonly PermissionRecord ManageWaiterOrders = new PermissionRecord { Name = "Admin area. XRMS screen for Waiters", SystemName = "XrmsManageWaiterOrders", Category = "XRMS" };
        public static readonly PermissionRecord ManageKitchenOrders = new PermissionRecord { Name = "Admin area. XRMS screen for Kitchen", SystemName = "XrmsManageKitchenOrders", Category = "XRMS" };

        /// <summary>
        /// Get permissions
        /// </summary>
        /// <returns>Permissions</returns>
        public virtual IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[]
            {
                ManageSuppliers,
                ManageMaterials,
                ManageMaterialGroups,
                ManageTables,
                ManageCashierOrders,
                ManageWaiterOrders,
                ManageKitchenOrders
            };
        }

        /// <summary>
        /// Get default permissions
        /// </summary>
        /// <returns>Default Permissions</returns>
        public virtual IEnumerable<DefaultPermissionRecord> GetDefaultPermissions()
        {
            //var accessAdminPanelPermission = StandardPermissionProvider.AccessAdminPanel;
            return new[]
            {
                new DefaultPermissionRecord
                {
                    CustomerRoleSystemName = NopCustomerDefaults.AdministratorsRoleName,
                    PermissionRecords = new[]
                    {
                        ManageSuppliers,
                        ManageMaterials,
                        ManageMaterialGroups,
                        ManageTables,
                        ManageCashierOrders,
                        ManageWaiterOrders,
                        ManageKitchenOrders
                    }
                },
                new DefaultPermissionRecord
                {
                    CustomerRoleSystemName = XrmsCustomerDefaults.CashiersRoleName,
                    PermissionRecords = new[]
                    {
                        //accessAdminPanelPermission,
                        ManageCashierOrders,
                        ManageWaiterOrders
                    }
                },
                new DefaultPermissionRecord
                {
                    CustomerRoleSystemName = XrmsCustomerDefaults.WaitersRoleName,
                    PermissionRecords = new[]
                    {
                        //accessAdminPanelPermission,
                        ManageWaiterOrders
                    }
                },
                new DefaultPermissionRecord
                {
                    CustomerRoleSystemName = XrmsCustomerDefaults.KitchenRoleName,
                    PermissionRecords = new[]
                    {
                        //accessAdminPanelPermission,
                        ManageKitchenOrders
                    }
                }
            };
        }
    }
}
