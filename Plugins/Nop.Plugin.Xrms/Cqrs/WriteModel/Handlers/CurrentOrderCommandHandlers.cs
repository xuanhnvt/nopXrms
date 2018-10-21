using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CQRSlite.Domain;
using CQRSlite.Commands;
using Nop.Plugin.Xrms.Cqrs.WriteModel.Commands.CurrentOrder;
using Nop.Plugin.Xrms.Cqrs.WriteModel.Domain;
using Nop.Services.Logging;
using Nop.Core.Domain.Logging;

namespace Nop.Plugin.Xrms.Cqrs.WriteModel.Handlers
{
    public class CurrentOrderCommandHandlers: ICommandHandler<CreateCmd>,
        ICommandHandler<UpdateOrderItemsCmd>
    {
        private readonly ISession _session;
        private readonly ILogger _logger;

        public CurrentOrderCommandHandlers(ISession session, ILogger logger)
        {
            _session = session;
            _logger = logger;
        }

        public async Task Handle(CreateCmd message)
        {
            var order = new CurrentOrder(message.Id, message.CommandModel.TableId);
            foreach (var item in message.CommandModel.AddedOrderItems)
            {
                order.AddOrderItem(CompGuid.NewGuid(), item.ProductId, item.Quantity);
            }
            await _session.Add(order);
            await _session.Commit();
        }

        public async Task Handle(UpdateOrderItemsCmd message)
        {
            try
            {
                _logger.InsertLog(LogLevel.Information, "Get order", String.Format("AggregateId = {0}, Version = {1}, Number items = {2}", message.Id, message.ExpectedVersion, message.CommandModel.UpdatedOrderItems.Count));
                var order = await _session.Get<CurrentOrder>(message.Id, message.ExpectedVersion);
                _logger.InsertLog(LogLevel.Information, "Order Information", String.Format("AggregateId = {0}, Version = {1}, Number items = {2}", order.Id, order.Version, order.OrderItems.Count));
                foreach (var item in order.OrderItems)
                {
                    _logger.InsertLog(LogLevel.Information, "Existing Order Item", String.Format("AggregateId = {0}, Product Id = {1}, Quantity = {2}", item.OrderItemGuid, item.ProductId, item.Quantity));
                }

                foreach (var item in message.CommandModel.UpdatedOrderItems)
                {
                    if (item.Id != 0)
                    {
                        _logger.InsertLog(LogLevel.Information, "Change order item quantity", String.Format("AggregateId = {0}, Product Id = {1}, Quantity = {2}", item.AggregateId, item.ProductId, item.Quantity));
                        order.ChangeOrderItemQuantity(item.AggregateId, item.ProductId, item.Quantity);
                    }
                    else
                    {
                        _logger.InsertLog(LogLevel.Information, "Add order item", String.Format("AggregateId = {0}, Product Id = {1}, Quantity = {2}", item.AggregateId, item.ProductId, item.Quantity));
                        order.AddOrderItem(CompGuid.NewGuid(), item.ProductId, item.Quantity);
                    }
                }
                await _session.Commit();
            }
            catch (Exception ex)
            {
                _logger.InsertLog(LogLevel.Error, "Handle command UpdateOrderItemsCmd error", ex.Message);
                throw ex;
            }
        }
    }
}
