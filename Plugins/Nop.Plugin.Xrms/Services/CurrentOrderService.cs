using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Services.Events;

using Nop.Plugin.Xrms.Domain;

namespace Nop.Plugin.Xrms.Services
{
    /// <summary>
    /// Order service
    /// </summary>
    public partial class CurrentOrderService : ICurrentOrderService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CurrentOrder> _currentOrderRepository;
        private readonly IRepository<CurrentOrderItem> _currentOrderItemRepository;
        private readonly IRepository<OrderNote> _orderNoteRepository;
        private readonly IRepository<Table> _tableRepository;

        #endregion

        #region Ctor

        public CurrentOrderService(IEventPublisher eventPublisher,
            IRepository<Customer> customerRepository,
            IRepository<CurrentOrder> currentOrderRepository,
            IRepository<CurrentOrderItem> currentOrderItemRepository,
            IRepository<OrderNote> orderNoteRepository,
            IRepository<Table> tableRepository)
        {
            this._eventPublisher = eventPublisher;
            this._customerRepository = customerRepository;
            this._currentOrderRepository = currentOrderRepository;
            this._currentOrderItemRepository = currentOrderItemRepository;
            this._orderNoteRepository = orderNoteRepository;
            this._tableRepository = tableRepository;
        }

        #endregion

        #region Methods

        #region Orders

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderId">The order identifier</param>
        /// <returns>Order</returns>
        public virtual CurrentOrder GetOrderById(int orderId)
        {
            if (orderId == 0)
                return null;

            return _currentOrderRepository.GetById(orderId);
        }

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderCode">The order code</param>
        /// <returns>Order</returns>
        public virtual CurrentOrder GetOrderByCode(string orderCode)
        {
            if (string.IsNullOrEmpty(orderCode))
                return null;

            return _currentOrderRepository.Table.FirstOrDefault(o => o.OrderCode == orderCode);
        }

        /// <summary>
        /// Get orders by table id
        /// </summary>
        /// <param name="tableId">Table identifier</param>
        /// <returns>Order</returns>
        public virtual CurrentOrder GetOrdersByTableId(int tableId)
        {
            if (tableId == 0)
                return null;

            var query = from o in _currentOrderRepository.Table
                        where o.TableId == tableId
                        select o;
            var order = query.FirstOrDefault();
            return order;
        }

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderGuid">The order identifier</param>
        /// <returns>Order</returns>
        public virtual CurrentOrder GetOrderByGuid(Guid orderGuid)
        {
            if (orderGuid == Guid.Empty)
                return null;

            var query = from o in _currentOrderRepository.Table
                        where o.AggregateId == orderGuid
                        select o;
            var order = query.FirstOrDefault();
            return order;
        }

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="order">The order</param>
        /*public virtual void CancelOrder(CurrentOrder order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _currentOrderRepository.Delete(order);
            order.Cancelled = true;
            UpdateOrder(order);

            //event notification
            _eventPublisher.EntityDeleted(order);
        }*/

        /// <summary>
        /// Deletes an order
        /// </summary>
        /// <param name="order">The order</param>
        public virtual void DeleteOrder(CurrentOrder order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _currentOrderRepository.Delete(order);

            //event notification
            _eventPublisher.EntityDeleted(order);
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual IPagedList<CurrentOrder> GetAllOrders(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _currentOrderRepository.Table;
            query = query.OrderBy(o => o.CreatedOnUtc);

            //database layer paging
            return new PagedList<CurrentOrder>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Inserts an order
        /// </summary>
        /// <param name="order">Order</param>
        public virtual void InsertOrder(CurrentOrder order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _currentOrderRepository.Insert(order);

            // update table state
            var table = _tableRepository.GetById(order.TableId);
            table.State = TableState.Serving;
            _tableRepository.Update(table);

            //event notification
            _eventPublisher.EntityInserted(order);
        }

        /// <summary>
        /// Updates the order
        /// </summary>
        /// <param name="order">The order</param>
        public virtual void UpdateOrder(CurrentOrder order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _currentOrderRepository.Update(order);

            //event notification
            _eventPublisher.EntityUpdated(order);
        }

        #endregion

        #region Orders items

        /// <summary>
        /// Get order items of order
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns>List of order item</returns>
        public IList<CurrentOrderItem> GetOrderItems(int orderId)
        {
            if (orderId == 0)
                return null;

            var query = from o in _currentOrderItemRepository.Table
                        where o.CurrentOrderId == orderId
                        select o;
            var order = query.ToList();
            return order;
        }

        /// <summary>
        /// Gets an order item
        /// </summary>
        /// <param name="orderItemId">Order item identifier</param>
        /// <returns>Order item</returns>
        public virtual CurrentOrderItem GetOrderItemById(int orderItemId)
        {
            if (orderItemId == 0)
                return null;

            return _currentOrderItemRepository.GetById(orderItemId);
        }

        /// <summary>
        /// Gets an item
        /// </summary>
        /// <param name="orderItemGuid">Order identifier</param>
        /// <returns>Order item</returns>
        public virtual CurrentOrderItem GetOrderItemByGuid(Guid orderItemGuid)
        {
            if (orderItemGuid == Guid.Empty)
                return null;

            var query = from orderItem in _currentOrderItemRepository.Table
                        where orderItem.AggregateId == orderItemGuid
                        select orderItem;
            var item = query.FirstOrDefault();
            return item;
        }

        /// <summary>
        /// Inserts an order item
        /// </summary>
        /// <param name="item">OrderItem</param>
        public virtual void InsertOrderItem(CurrentOrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _currentOrderItemRepository.Insert(item);

            //event notification
            _eventPublisher.EntityInserted(item);
        }

        /// <summary>
        /// Delete an order item
        /// </summary>
        /// <param name="orderItem">The order item</param>
        public virtual void DeleteOrderItem(CurrentOrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            _currentOrderItemRepository.Delete(orderItem);

            //event notification
            _eventPublisher.EntityDeleted(orderItem);
        }

        /// <summary>
        /// Update an order item
        /// </summary>
        /// <param name="orderItem">The order item</param>
        public void UpdateOrderItem(CurrentOrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            _currentOrderItemRepository.Update(orderItem);

            //event notification
            _eventPublisher.EntityUpdated(orderItem);
        }

        #endregion

        #endregion
    }
}