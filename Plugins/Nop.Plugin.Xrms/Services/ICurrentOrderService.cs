using System;
using System.Collections.Generic;
using Nop.Core;

using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Services
{
    /// <summary>
    /// Order service interface
    /// </summary>
    public partial interface ICurrentOrderService
    {
        #region Orders

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderId">The order identifier</param>
        /// <returns>Order</returns>
        CurrentOrder GetOrderById(int orderId);

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderCode">The order code</param>
        /// <returns>Order</returns>
        CurrentOrder GetOrderByCode(string orderCode);

        /// <summary>
        /// Get orders by table id
        /// </summary>
        /// <param name="tableId">Table identifier</param>
        /// <returns>Order</returns>
        CurrentOrder GetOrdersByTableId(int tableId);

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderGuid">The order identifier</param>
        /// <returns>Order</returns>
        CurrentOrder GetOrderByGuid(Guid orderGuid);

        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="order">The order</param>
        //void CancelOrder(CurrentOrder order);

        /// <summary>
        /// Deletes an order
        /// </summary>
        /// <param name="order">The order</param>
        void DeleteOrder(CurrentOrder order);

        /// <summary> 
        /// Get all orders
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Orders</returns>
        IPagedList<CurrentOrder> GetAllOrders(int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Inserts an order
        /// </summary>
        /// <param name="order">Order</param>
        void InsertOrder(CurrentOrder order);

        /// <summary>
        /// Updates the order
        /// </summary>
        /// <param name="order">The order</param>
        void UpdateOrder(CurrentOrder order);

        #endregion

        #region Orders items

        /// <summary>
        /// Get order items of order
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns>List of order item</returns>
        IList<CurrentOrderItem> GetOrderItems(int orderId);

        /// <summary>
        /// Inserts an order item
        /// </summary>
        /// <param name="item">OrderItem</param>
        void InsertOrderItem(CurrentOrderItem item);

        /// <summary>
        /// Gets an order item
        /// </summary>
        /// <param name="orderItemId">Order item identifier</param>
        /// <returns>Order item</returns>
        CurrentOrderItem GetOrderItemById(int orderItemId);

        /// <summary>
        /// Gets an order item
        /// </summary>
        /// <param name="orderItemGuid">Order item identifier</param>
        /// <returns>Order item</returns>
        CurrentOrderItem GetOrderItemByGuid(Guid orderItemGuid);

        /// <summary>
        /// Delete an order item
        /// </summary>
        /// <param name="orderItem">The order item</param>
        void DeleteOrderItem(CurrentOrderItem orderItem);

        /// <summary>
        /// Update an order item
        /// </summary>
        /// <param name="orderItem">The order item</param>
        void UpdateOrderItem(CurrentOrderItem orderItem);

        #endregion

    }
}