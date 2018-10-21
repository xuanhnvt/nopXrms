﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Nop.Services.Helpers;

using CQRSlite.Events;
using Nop.Plugin.Xrms.Cqrs.ReadModel.Events.CurrentOrder;
using Nop.Plugin.Xrms.Services;

using CurrentOrderEntity = Nop.Plugin.Xrms.Domain.CurrentOrder;
using CurrentOrderItemEntity =  Nop.Plugin.Xrms.Domain.CurrentOrderItem;
using Nop.Services.Orders;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Services.Logging;
using Nop.Core.Data;
using Nop.Plugin.Xrms.Domain;
using Nop.Plugin.Xrms.Areas.Admin.Models.CurrentOrders;
using Nop.Plugin.Xrms.Areas.Admin.Models;
using Nop.Services.Catalog;

namespace Nop.Plugin.Xrms.Cqrs.ReadModel.Events.Handlers
{
    public class CurrentOrderEventHandlers : ICancellableEventHandler<CreatedEvent>,
        ICancellableEventHandler<AddedOrderItemEvent>,
        ICancellableEventHandler<ChangedOrderItemQuantityEvent>,
        ICancellableEventHandler<ChangedOrderItemStateEvent>
    {
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly IWebHelper _webHelper;
        private readonly ICustomNumberFormatter _customNumberFormatter;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        private readonly ICurrentOrderService _currentOrderService;
        private readonly ILogger _logger;
        private readonly IRepository<CqrsEvent> _cqrsEventReposiory;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IClientNotificationService _clientNotificationService;

        public CurrentOrderEventHandlers(IStoreContext storeContext,
            IWorkContext workContext,
            IWebHelper webHelper,
            ICustomNumberFormatter customNumberFormatter,
            IOrderService orderService,
            IProductService productService,
            ICurrentOrderService currentOrderService,
            IRepository<CqrsEvent> cqrsEventReposiory,
            IClientNotificationService clientNotificationService,
            IDateTimeHelper dateTimeHelper,
            ILogger logger)
        {
            _customNumberFormatter = customNumberFormatter;
            _storeContext = storeContext;
            _workContext = workContext;
            _webHelper = webHelper;
            _orderService = orderService;
            _productService = productService;
            _currentOrderService = currentOrderService;
            _cqrsEventReposiory = cqrsEventReposiory;
            _clientNotificationService = clientNotificationService;
            _dateTimeHelper = dateTimeHelper;
            _logger = logger;
        }

        /// <summary>
        /// Initialize an order, save and add order notes
        /// </summary>
        /// <returns></returns>
        protected virtual Order InitializeOrder(Guid orderGuid)
        {
            var currentCustomer = _workContext.CurrentCustomer;
            var currentStore = _storeContext.CurrentStore;
            _logger.InsertLog(Core.Domain.Logging.LogLevel.Information, "Current Context", String.Format("Store = {0}, Customer = {1}", currentStore.Id, currentCustomer.Username));
            var order = new Order
            {
                StoreId = currentStore.Id,
                OrderGuid = orderGuid,
                CustomerId = currentCustomer.Id,
                CustomerLanguageId = _workContext.WorkingLanguage.Id,
                CustomerTaxDisplayType = Core.Domain.Tax.TaxDisplayType.ExcludingTax,
                //CustomerTaxDisplayType = details.CustomerTaxDisplayType,
                CustomerIp = _webHelper.GetCurrentIpAddress(),
                //OrderSubtotalInclTax = details.OrderSubTotalInclTax,
                //OrderSubtotalExclTax = details.OrderSubTotalExclTax,
                //OrderSubTotalDiscountInclTax = details.OrderSubTotalDiscountInclTax,
                //OrderSubTotalDiscountExclTax = details.OrderSubTotalDiscountExclTax,
                //OrderShippingInclTax = details.OrderShippingTotalInclTax,
                //OrderShippingExclTax = details.OrderShippingTotalExclTax,
                //PaymentMethodAdditionalFeeInclTax = details.PaymentAdditionalFeeInclTax,
                //PaymentMethodAdditionalFeeExclTax = details.PaymentAdditionalFeeExclTax,
                //TaxRates = details.TaxRates,
                //OrderTax = details.OrderTaxTotal,
                //OrderTotal = details.OrderTotal,
                RefundedAmount = decimal.Zero,
                //OrderDiscount = details.OrderDiscountAmount,
                //CheckoutAttributeDescription = details.CheckoutAttributeDescription,
                //CheckoutAttributesXml = details.CheckoutAttributesXml,
                //CustomerCurrencyCode = details.CustomerCurrencyCode,
                //CurrencyRate = details.CustomerCurrencyRate,
                //AffiliateId = details.AffiliateId,
                OrderStatus = OrderStatus.Pending,
                //AllowStoringCreditCardNumber = processPaymentResult.AllowStoringCreditCardNumber,
                //CardType = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processPaymentRequest.CreditCardType) : string.Empty,
                //CardName = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processPaymentRequest.CreditCardName) : string.Empty,
                //CardNumber = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processPaymentRequest.CreditCardNumber) : string.Empty,
                //MaskedCreditCardNumber = _encryptionService.EncryptText(_paymentService.GetMaskedCreditCardNumber(processPaymentRequest.CreditCardNumber)),
                //CardCvv2 = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processPaymentRequest.CreditCardCvv2) : string.Empty,
                //CardExpirationMonth = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processPaymentRequest.CreditCardExpireMonth.ToString()) : string.Empty,
                //CardExpirationYear = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processPaymentRequest.CreditCardExpireYear.ToString()) : string.Empty,
                //PaymentMethodSystemName = processPaymentRequest.PaymentMethodSystemName,
                //AuthorizationTransactionId = processPaymentResult.AuthorizationTransactionId,
                //AuthorizationTransactionCode = processPaymentResult.AuthorizationTransactionCode,
                //AuthorizationTransactionResult = processPaymentResult.AuthorizationTransactionResult,
                //CaptureTransactionId = processPaymentResult.CaptureTransactionId,
                //CaptureTransactionResult = processPaymentResult.CaptureTransactionResult,
                //SubscriptionTransactionId = processPaymentResult.SubscriptionTransactionId,
                //PaymentStatus = processPaymentResult.NewPaymentStatus,
                PaidDateUtc = null,
                BillingAddressId = (int) _workContext.CurrentCustomer.BillingAddressId,
                //BillingAddress = details.BillingAddress,
                ShippingAddressId = (int)_workContext.CurrentCustomer.ShippingAddressId,
                //ShippingAddress = details.ShippingAddress,
                ShippingStatus = Core.Domain.Shipping.ShippingStatus.ShippingNotRequired,
                //ShippingStatus = details.ShippingStatus,
                //ShippingMethod = details.ShippingMethodName,
                //PickUpInStore = details.PickUpInStore,
                PickUpInStore = false,
                //PickupAddress = details.PickupAddress,
                //ShippingRateComputationMethodSystemName = details.ShippingRateComputationMethodSystemName,
                //CustomValuesXml = _paymentService.SerializeCustomValues(processPaymentRequest),
                //VatNumber = details.VatNumber,
                CreatedOnUtc = DateTime.UtcNow,
                CustomOrderNumber = string.Empty
            };

            try
            {

                _orderService.InsertOrder(order);

                // generate and set custom order number
                order.CustomOrderNumber = _customNumberFormatter.GenerateOrderCustomNumber(order);
                _orderService.UpdateOrder(order);
            }
            catch (Exception ex)
            {
                _logger.InsertLog(Core.Domain.Logging.LogLevel.Error, "Insert order error", ex.Message, _workContext.CurrentCustomer);
                throw ex;
            }

            //reward points history
            /*if (details.RedeemedRewardPointsAmount <= decimal.Zero)
                return order;

            _rewardPointService.AddRewardPointsHistoryEntry(details.Customer, -details.RedeemedRewardPoints, order.StoreId,
                string.Format(_localizationService.GetResource("RewardPoints.Message.RedeemedForOrder", order.CustomerLanguageId), order.CustomOrderNumber),
                order, details.RedeemedRewardPointsAmount);
            _customerService.UpdateCustomer(details.Customer);*/

            return order;
        }
        
        public async Task Handle(CreatedEvent message, CancellationToken token)
        {
            try
            {
                var order = InitializeOrder(message.Id);
                var currentOrder = new CurrentOrderEntity()
                {
                    OrderId = order.Id,
                    OrderCode = order.CustomOrderNumber,
                    AggregateId = message.Id,
                    Version = message.Version,
                    TableId = message.TableId,
                    CreatedOnUtc = message.TimeStamp.UtcDateTime,
                    UpdatedOnUtc = message.TimeStamp.UtcDateTime
                };
                _currentOrderService.InsertOrder(currentOrder);

                var notifyModel = currentOrder.ToNotifyCreatedOrderModel();
                notifyModel.TableName = currentOrder.Table.Name;
                notifyModel.CreatedOnUtc = _dateTimeHelper.ConvertToUserTime(currentOrder.CreatedOnUtc, DateTimeKind.Utc).ToString("dd/MM HH:mm:ss");
                notifyModel.UpdatedOnUtc = _dateTimeHelper.ConvertToUserTime(currentOrder.UpdatedOnUtc, DateTimeKind.Utc).ToString("dd/MM HH:mm:ss");


                await this._clientNotificationService.NotifyCreatedOrderEvent(notifyModel);
            }
            catch (Exception ex)
            {

                var @event = _cqrsEventReposiory.Table.Where(e => e.AggregateId == message.Id && e.Version == message.Version).FirstOrDefault();
                if (@event != null)
                {
                    _cqrsEventReposiory.Delete(@event);
                }

                _logger.InsertLog(Core.Domain.Logging.LogLevel.Error, "Insert cashier order error", ex.Message, _workContext.CurrentCustomer);
                throw ex;
            }
        }


        public async Task Handle(AddedOrderItemEvent message, CancellationToken token)
        {
            var order = _currentOrderService.GetOrderByGuid(message.Id);
            order.Version = message.Version;
            order.UpdatedOnUtc = message.TimeStamp.UtcDateTime;
            _currentOrderService.UpdateOrder(order);
            var item = new CurrentOrderItemEntity()
            {
                AggregateId = message.OrderItemGuid,
                CurrentOrderId = order.Id,
                ProductId = message.ProductId,
                Quantity = message.Quantity,
                //AreaId = message.LocationId,
                CreatedOnUtc = message.TimeStamp.UtcDateTime,
                UpdatedOnUtc = message.TimeStamp.UtcDateTime
            };
            _currentOrderService.InsertOrderItem(item);

            // fill in model values from the entity
            var notifyModel = order.ToNotifyChangedOrderItemModel(); var product = _productService.GetProductById(item.ProductId);
            var orderItemModel = new CurrentOrderDetailsPageViewModel.OrderItemViewModel()
            {
                Id = item.Id,
                AggregateId = item.AggregateId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                OldQuantity = item.Quantity,
                Version = item.Version,

                ProductName = product.Name,
                ProductPrice = product.Price
            };
            notifyModel.UpdatedOnUtc = _dateTimeHelper.ConvertToUserTime(order.UpdatedOnUtc, DateTimeKind.Utc).ToString("dd/MM HH:mm:ss");
            notifyModel.ChangedOrderItem = orderItemModel;

            await this._clientNotificationService.NotifyAddedOrderItemEvent(notifyModel);
        }


        public async Task Handle(ChangedOrderItemQuantityEvent message, CancellationToken token)
        {
            var order = _currentOrderService.GetOrderByGuid(message.Id);
            order.Version = message.Version;
            order.UpdatedOnUtc = message.TimeStamp.UtcDateTime;
            _currentOrderService.UpdateOrder(order);
            var item = _currentOrderService.GetOrderItemByGuid(message.OrderItemGuid);
            item.Quantity = message.Quantity;
            item.UpdatedOnUtc = message.TimeStamp.UtcDateTime;
            _currentOrderService.UpdateOrderItem(item);

            // fill in model values from the entity
            var notifyModel = order.ToNotifyChangedOrderItemModel(); var product = _productService.GetProductById(item.ProductId);
            var orderItemModel = new CurrentOrderDetailsPageViewModel.OrderItemViewModel()
            {
                Id = item.Id,
                AggregateId = item.AggregateId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                OldQuantity = item.Quantity,
                Version = item.Version,

                ProductName = product.Name,
                ProductPrice = product.Price
            };
            notifyModel.UpdatedOnUtc = _dateTimeHelper.ConvertToUserTime(order.UpdatedOnUtc, DateTimeKind.Utc).ToString("dd/MM HH:mm:ss");
            notifyModel.ChangedOrderItem = orderItemModel;

            await this._clientNotificationService.NotifyChangedOrderItemQuantityEvent(notifyModel);
        }


        public async Task Handle(ChangedOrderItemStateEvent message, CancellationToken token)
        {
            var order = _currentOrderService.GetOrderByGuid(message.Id);
            order.Version = message.Version;
            order.UpdatedOnUtc = message.TimeStamp.UtcDateTime;
            _currentOrderService.UpdateOrder(order);

            var item = _currentOrderService.GetOrderItemByGuid(message.OrderItemGuid);
            item.StateId = message.State;
            item.UpdatedOnUtc = message.TimeStamp.UtcDateTime;
            _currentOrderService.UpdateOrderItem(item);

            // fill in model values from the entity
            var notifyModel = order.ToNotifyChangedOrderItemModel(); var product = _productService.GetProductById(item.ProductId);
            var orderItemModel = new CurrentOrderDetailsPageViewModel.OrderItemViewModel()
            {
                Id = item.Id,
                AggregateId = item.AggregateId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                OldQuantity = item.Quantity,
                Version = item.Version,

                ProductName = product.Name,
                ProductPrice = product.Price
            };
            notifyModel.UpdatedOnUtc = _dateTimeHelper.ConvertToUserTime(order.UpdatedOnUtc, DateTimeKind.Utc).ToString("dd/MM HH:mm:ss");
            notifyModel.ChangedOrderItem = orderItemModel;

            await this._clientNotificationService.NotifyChangedOrderItemQuantityEvent(notifyModel);

            //return Task.CompletedTask;
        }
    }
}