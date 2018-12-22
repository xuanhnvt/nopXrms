
namespace Nop.Plugin.Xrms.Domain
{
    /// <summary>
    /// Represents default values related to customers data
    /// </summary>
    public static partial class XrmsCustomerDefaults
    {
        // extend system customer roles
        #region System customer roles

        /// <summary>
        /// Gets a system name of 'cashiers' customer role
        /// </summary>
        public static string CashiersRoleName => "Cashiers";

        /// <summary>
        /// Gets a system name of 'waiters' customer role
        /// </summary>
        public static string WaitersRoleName => "Waiters";

        /// <summary>
        /// Gets a system name of 'kitchen' customer role
        /// </summary>
        public static string KitchenRoleName => "Kitchen";

        #endregion
    }
}