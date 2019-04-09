using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Services.Customers;
using Nop.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nop.Plugin.Xrms.Services
{
    /// <summary>
    /// Represents vendor attribute extensions
    /// </summary>
    public static class ShoppingCartServiceExtensions
    {
        /// <summary>
        /// Add a product to shopping cart
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="product">Product</param>
        /// <param name="shoppingCartType">Shopping cart type</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="customerEnteredPrice">The price enter by a customer</param>
        /// <param name="rentalStartDate">Rental start date</param>
        /// <param name="rentalEndDate">Rental end date</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="addRequiredProducts">Whether to add required products</param>
        /// <returns>Warnings</returns>
        public static IList<string> AddToCart(this IShoppingCartService shoppingCartService,
            ICustomerService customerService,
            Customer customer, Product product,
            ShoppingCartType shoppingCartType, int storeId, out ShoppingCartItem cartItem, string attributesXml = null,
            decimal customerEnteredPrice = decimal.Zero,
            DateTime? rentalStartDate = null, DateTime? rentalEndDate = null,
            int quantity = 1, bool addRequiredProducts = true)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (product == null)
                throw new ArgumentNullException(nameof(product));

            cartItem = null;
            var warnings = new List<string>();
            /*if (shoppingCartType == ShoppingCartType.ShoppingCart && !_permissionService.Authorize(StandardPermissionProvider.EnableShoppingCart, customer))
            {
                warnings.Add("Shopping cart is disabled");
                return warnings;
            }

            if (shoppingCartType == ShoppingCartType.Wishlist && !_permissionService.Authorize(StandardPermissionProvider.EnableWishlist, customer))
            {
                warnings.Add("Wishlist is disabled");
                return warnings;
            }

            if (customer.IsSearchEngineAccount())
            {
                warnings.Add("Search engine can't add to cart");
                return warnings;
            }

            if (quantity <= 0)
            {
                warnings.Add(_localizationService.GetResource("ShoppingCart.QuantityShouldPositive"));
                return warnings;
            }*/

            //reset checkout info
            customerService.ResetCheckoutData(customer, storeId);

            var cart = customer.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == shoppingCartType)
                .LimitPerStore(storeId)
                .ToList();

            var shoppingCartItem = shoppingCartService.FindShoppingCartItemInTheCart(cart,
                shoppingCartType, product, attributesXml, customerEnteredPrice,
                rentalStartDate, rentalEndDate);

            if (shoppingCartItem != null)
            {
                //update existing shopping cart item
                var newQuantity = shoppingCartItem.Quantity + quantity;
                warnings.AddRange(shoppingCartService.GetShoppingCartItemWarnings(customer, shoppingCartType, product,
                    storeId, attributesXml,
                    customerEnteredPrice, rentalStartDate, rentalEndDate,
                    newQuantity, addRequiredProducts, shoppingCartItem.Id));

                if (warnings.Any())
                    return warnings;

                shoppingCartItem.AttributesXml = attributesXml;
                shoppingCartItem.Quantity = newQuantity;
                shoppingCartItem.UpdatedOnUtc = DateTime.UtcNow;
                customerService.UpdateCustomer(customer);

                cartItem = shoppingCartItem;
                //event notification
                //_eventPublisher.EntityUpdated(shoppingCartItem);
            }
            else
            {
                //new shopping cart item
                warnings.AddRange(shoppingCartService.GetShoppingCartItemWarnings(customer, shoppingCartType, product,
                    storeId, attributesXml, customerEnteredPrice,
                    rentalStartDate, rentalEndDate,
                    quantity, addRequiredProducts));

                if (warnings.Any())
                    return warnings;

                //maximum items validation
                /*switch (shoppingCartType)
                {
                    case ShoppingCartType.ShoppingCart:
                        if (cart.Count >= _shoppingCartSettings.MaximumShoppingCartItems)
                        {
                            warnings.Add(string.Format(_localizationService.GetResource("ShoppingCart.MaximumShoppingCartItems"), _shoppingCartSettings.MaximumShoppingCartItems));
                            return warnings;
                        }

                        break;
                    case ShoppingCartType.Wishlist:
                        if (cart.Count >= _shoppingCartSettings.MaximumWishlistItems)
                        {
                            warnings.Add(string.Format(_localizationService.GetResource("ShoppingCart.MaximumWishlistItems"), _shoppingCartSettings.MaximumWishlistItems));
                            return warnings;
                        }

                        break;
                    default:
                        break;
                }
                */

                var now = DateTime.UtcNow;
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartType = shoppingCartType,
                    StoreId = storeId,
                    Product = product,
                    AttributesXml = attributesXml,
                    CustomerEnteredPrice = customerEnteredPrice,
                    Quantity = quantity,
                    RentalStartDateUtc = rentalStartDate,
                    RentalEndDateUtc = rentalEndDate,
                    CreatedOnUtc = now,
                    UpdatedOnUtc = now
                };
                customer.ShoppingCartItems.Add(shoppingCartItem);
                customerService.UpdateCustomer(customer);

                cartItem = shoppingCartItem;

                //updated "HasShoppingCartItems" property used for performance optimization
                customer.HasShoppingCartItems = customer.ShoppingCartItems.Any();
                customerService.UpdateCustomer(customer);

                //event notification
                //_eventPublisher.EntityInserted(shoppingCartItem);
            }

            return warnings;
        }

        /// <summary>
        /// Add a product to shopping cart without validation
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="product">Product</param>
        /// <param name="shoppingCartType">Shopping cart type</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="customerEnteredPrice">The price enter by a customer</param>
        /// <param name="rentalStartDate">Rental start date</param>
        /// <param name="rentalEndDate">Rental end date</param>
        /// <param name="addRequiredProducts">Whether to add required products</param>
        /// <returns>Warnings</returns>
        public static ShoppingCartItem AddShoppingCartItem(this IShoppingCartService shoppingCartService,
            ICustomerService customerService,
            Customer customer, int productId,
            ShoppingCartType shoppingCartType, int storeId,
            int quantity = 1,
            string attributesXml = null,
            decimal customerEnteredPrice = decimal.Zero,
            DateTime? rentalStartDate = null, DateTime? rentalEndDate = null,
            bool addRequiredProducts = true)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            //reset checkout info
            customerService.ResetCheckoutData(customer, storeId);

            var cart = customer.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == shoppingCartType)
                .LimitPerStore(storeId)
                .ToList();

            var now = DateTime.UtcNow;
            var shoppingCartItem = new ShoppingCartItem
            {
                ShoppingCartType = shoppingCartType,
                StoreId = storeId,
                ProductId = productId,
                AttributesXml = attributesXml,
                CustomerEnteredPrice = customerEnteredPrice,
                Quantity = quantity,
                RentalStartDateUtc = rentalStartDate,
                RentalEndDateUtc = rentalEndDate,
                CreatedOnUtc = now,
                UpdatedOnUtc = now
            };
            customer.ShoppingCartItems.Add(shoppingCartItem);
            customerService.UpdateCustomer(customer);

            //updated "HasShoppingCartItems" property used for performance optimization
            customer.HasShoppingCartItems = customer.ShoppingCartItems.Any();
            customerService.UpdateCustomer(customer);

            //event notification
            //_eventPublisher.EntityInserted(shoppingCartItem);

            return shoppingCartItem;
        }

        /// <summary>
        /// Updates the shopping cart item
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="shoppingCartItemId">Shopping cart item identifier</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="customerEnteredPrice">New customer entered price</param>
        /// <param name="rentalStartDate">Rental start date</param>
        /// <param name="rentalEndDate">Rental end date</param>
        /// <param name="quantity">New shopping cart item quantity</param>
        /// <param name="resetCheckoutData">A value indicating whether to reset checkout data</param>
        /// <returns>Warnings</returns>
        public static ShoppingCartItem UpdateShoppingCartItem(this IShoppingCartService shoppingCartService,
            ICustomerService customerService, Customer customer,
            int shoppingCartItemId,
            DateTime? rentalStartDate = null, DateTime? rentalEndDate = null,
            int quantity = 1, bool resetCheckoutData = true)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            //var warnings = new List<string>();

            var shoppingCartItem = customer.ShoppingCartItems.FirstOrDefault(sci => sci.Id == shoppingCartItemId);
            if (shoppingCartItem == null)
                return null;

            if (resetCheckoutData)
            {
                //reset checkout data
                customerService.ResetCheckoutData(customer, shoppingCartItem.StoreId);
            }

            if (quantity > 0)
            {
                //check warnings
                /*warnings.AddRange(GetShoppingCartItemWarnings(customer, shoppingCartItem.ShoppingCartType,
                    shoppingCartItem.Product, shoppingCartItem.StoreId,
                    attributesXml, customerEnteredPrice,
                    rentalStartDate, rentalEndDate, quantity, false, shoppingCartItemId));
                if (warnings.Any())
                    return warnings;*/

                //if everything is OK, then update a shopping cart item
                shoppingCartItem.Quantity = quantity;
                //shoppingCartItem.AttributesXml = attributesXml;
                //shoppingCartItem.CustomerEnteredPrice = customerEnteredPrice;
                shoppingCartItem.RentalStartDateUtc = rentalStartDate;
                shoppingCartItem.RentalEndDateUtc = rentalEndDate;
                shoppingCartItem.UpdatedOnUtc = DateTime.UtcNow;
                customerService.UpdateCustomer(customer);

                //event notification
                //_eventPublisher.EntityUpdated(shoppingCartItem);
            }
            else
            {
                //check warnings for required products
                /*warnings.AddRange(GetRequiredProductWarnings(customer, shoppingCartItem.ShoppingCartType,
                    shoppingCartItem.Product, shoppingCartItem.StoreId, quantity, false, shoppingCartItemId));
                if (warnings.Any())
                    return warnings;*/

                //delete a shopping cart item
                shoppingCartService.DeleteShoppingCartItem(shoppingCartItem, resetCheckoutData, true);
            }

            return shoppingCartItem;
        }
    }
}
