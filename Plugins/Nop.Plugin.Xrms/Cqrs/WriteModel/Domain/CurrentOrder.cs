using CQRSlite.Domain;
using Nop.Core.Domain.Logging;
using Nop.Core.Infrastructure;
using Nop.Plugin.Xrms.Cqrs.ReadModel.Events.CurrentOrder;
using Nop.Plugin.Xrms.Cqrs.WriteModel.Commands.CurrentOrder;
using Nop.Services.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Xrms.Cqrs.WriteModel.Domain
{
    public class CurrentOrder : AggregateRoot
    {
        private bool _activated;
        private List<OrderItem> _orderItems = new List<OrderItem>();

        #region Public Properties

        public string Code { get; private set; }
        public int TableId { get; private set; }
        public List<OrderItem> OrderItems
        {
            get { return _orderItems; }
        }

        #endregion

        public CurrentOrder() { }

        public CurrentOrder(CreateCmd message)
        {
            List<CreatedEvent.OrderItem> orderItems = new List<CreatedEvent.OrderItem>();
            foreach(var item in message.CommandModel.AddedOrderItems)
            {
                orderItems.Add(new CreatedEvent.OrderItem(CompGuid.NewGuid(), item.ProductId, item.Quantity));
            }
            ApplyChange(new CreatedEvent(message.Id, message.CommandModel.TableId, orderItems));
        }

        private void Apply(CreatedEvent e)
        {
            _activated = true;
            Id = e.Id;
            TableId = e.TableId;
            foreach (var item in e.OrderItems)
            {
                _orderItems.Add(new OrderItem()
                {
                    OrderItemGuid = item.Guid,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    State = 0
                });
            }
        }

        private void Apply(ChangedTableEvent e)
        {
            TableId = e.TableId;
        }

        private void Apply(AddedOrderItemEvent e)
        {
            var logger = EngineContext.Current.Resolve<ILogger>();
            logger.InsertLog(LogLevel.Information, "Apply AddedOrderItemEvent",
                String.Format("OrderItemGuid = {0}, ProductId = {1}, Quantity = {2}", e.OrderItemGuid, e.ProductId, e.Quantity));
            _orderItems.Add(new OrderItem()
            {
                OrderItemGuid = e.OrderItemGuid,
                ProductId = e.ProductId,
                Quantity = e.Quantity,
                State = 0
            });
        }

        private void Apply(ChangedOrderItemQuantityEvent e)
        {
            var logger = EngineContext.Current.Resolve<ILogger>();
            logger.InsertLog(LogLevel.Information, "Apply ChangedOrderItemQuantity",
                String.Format("OrderItemGuid = {0}, ProductId = {1}, Quantity = {2}", e.OrderItemGuid, e.ProductId, e.Quantity));
            var item = _orderItems.Find(i => i.OrderItemGuid == e.OrderItemGuid);
            if (item != null)
            {
                item.Quantity = e.Quantity;
            }
            else
            {
                throw new Exception(String.Format("No have order item '{0}' in order.", e.OrderItemGuid));
            }
        }

        private void Apply(ChangedOrderItemStateEvent e)
        {
            var logger = EngineContext.Current.Resolve<ILogger>();
            logger.InsertLog(LogLevel.Information, "Apply ChangedOrderItemQuantity",
                String.Format("OrderItemGuid = {0}, ProductId = {1}, State = {2}", e.OrderItemGuid, e.ProductId, e.State));
            var item = _orderItems.Find(i => i.OrderItemGuid == e.OrderItemGuid);
            if (item != null)
            {
                item.State = e.State;
            }
            else
            {
                throw new Exception(String.Format("No have order item '{0}' in order.", e.OrderItemGuid));
            }
        }

        private void Apply(DeletedEvent e)
        {
            _activated = false;
        }

        public void Delete()
        {
            if (!_activated) throw new InvalidOperationException("already deactivated");
            ApplyChange(new DeletedEvent(Id));
        }

        public void AddOrderItem(Guid orderItemGuid, int productId, int quantity)
        {
            ApplyChange(new AddedOrderItemEvent(Id, orderItemGuid, productId, quantity));
        }

        public void ChangeOrderItemQuantity(Guid orderItemGuid, int productId, int quantity)
        {
            // validate before do command
            //var item = _orderItems.Find(i => i.OrderItemGuid == orderItemGuid);
            //if (item != null)
            //{
            //    if (item.State != OrderItemState.Added)
            //    {
            //        // do command
            //        ApplyChange(new ChangedOrderItemQuantity(Id, orderItemGuid, productId, quantity));
            //    }
            //    else
            //    {
            //        // throw exception
            //    }
            //}
            //else
            //{
            //    throw new Exception(String.Format("No have order item '{0}' in order.", e.OrderItemGuid));
            //}
            ApplyChange(new ChangedOrderItemQuantityEvent(Id, orderItemGuid, productId, quantity));
        }

        public void ChangeOrderItemState(Guid orderItemGuid, int productId, int state)
        {
            ApplyChange(new ChangedOrderItemStateEvent(Id, orderItemGuid, productId, state));
        }

        public void ChangeTable(Guid orderItemGuid, int tableId)
        {
            ApplyChange(new ChangedTableEvent(Id, tableId));
        }
    }
}
