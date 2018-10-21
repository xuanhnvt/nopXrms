using System;
using System.Collections.Generic;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Security;
using Nop.Services.Security;

namespace Nop.Plugin.Xrms.Services
{
    /// <summary>
    /// XRMS permission provider
    /// </summary>
    public partial class XrmsPermissionProvider : IPermissionProvider
    {
        //admin area permissions
        public static readonly PermissionRecord ManageSuppliers = new PermissionRecord { Name = "Admin area. XRMS Manage Suppliers", SystemName = "XrmsManageSuppliers", Category = "Catalog" };
        public static readonly PermissionRecord ManageMaterials = new PermissionRecord { Name = "Admin area. XRMS Manage Materials", SystemName = "XrmsManageMaterials", Category = "Catalog" };
        public static readonly PermissionRecord ManageMaterialGroups = new PermissionRecord { Name = "Admin area. XRMS Manage Material Groups", SystemName = "XrmsManageMaterialGroups", Category = "Catalog" };
        public static readonly PermissionRecord ManageTables = new PermissionRecord { Name = "Admin area. XRMS Manage Tables", SystemName = "XrmsManageTables", Category = "Catalog" };
        public static readonly PermissionRecord ManageCashierOrders = new PermissionRecord { Name = "Admin area. XRMS Manage CashierOrders", SystemName = "XrmsManageCashierOrder", Category = "Standard" };

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
                ManageCashierOrders
            };
        }

        /// <summary>
        /// Get default permissions
        /// </summary>
        /// <returns>Default Permissions</returns>
        public virtual IEnumerable<DefaultPermissionRecord> GetDefaultPermissions()
        {
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
                        ManageCashierOrders
                    }
                }
            };
        }
    }
}
