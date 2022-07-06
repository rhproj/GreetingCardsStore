using GCard.DataAccess.Repository;
using GCard.Model;
using GCard.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;
using System.Security.Claims;

namespace GCard.RazorPagesApp.Pages.Customer.Cart
{
    [Authorize]
    [BindProperties]
    public class SummaryModel : PageModel
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
        private readonly IRepositoryService _repoService;
        public SummaryModel(IRepositoryService repoService)
        {
            _repoService = repoService;
            OrderHeader = new();
        }

        public void OnGet()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                ShoppingCartList = _repoService.ShoppingCartRepository.GetAll(filter: u => u.ApplicationUserId == claim.Value,
                    includeProp: "ProductItem,ProductItem.ItemType,ProductItem.Occasion");

                OrderHeader.OrderTotal = ShoppingCartList.Sum(m => m.ProductItem.Price * m.Count);

                #region ApplicationUser
                ApplicationUser applicationUser = _repoService.ApplicationUserRepository.GetWithCondition(u => u.Id == claim.Value);

                OrderHeader.Name = applicationUser.Name;
                OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
                OrderHeader.PostalCode = applicationUser.PostalCode;
                OrderHeader.Address = applicationUser.Address;
                #endregion
            }
        }

        public IActionResult OnPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(claim != null)
            {
                ShoppingCartList = _repoService.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == claim.Value, includeProp: "ProductItem");

                OrderHeader.OrderTotal = ShoppingCartList.Sum(c => c.ProductItem.Price * c.Count);

                OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                OrderHeader.OrderStatus = SD.StatusPending;
                OrderHeader.OrderDate = DateTime.Now;
                OrderHeader.ApplicationUserId = claim.Value;

                _repoService.OrderHeaderRepository.Add(OrderHeader);

                //OrderDetails for each item:
                foreach (var cart in ShoppingCartList)
                {
                    OrderDetails orderDetail = new()
                    {
                        ProductItemId = cart.ProductItemId,
                        OrderId = OrderHeader.Id,
                        Price = cart.ProductItem.Price,
                        Count = cart.Count
                    };
                    _repoService.OrderDetailRepository.Add(orderDetail);
                }
                _repoService.ShoppingCartRepository.DeleteRange(ShoppingCartList);

                #region STRIPE
                var domain = "https://localhost:7207/";
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={OrderHeader.Id}",
                    CancelUrl = domain + $"customer/cart/index",
                };

                foreach (var item in ShoppingCartList)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.ProductItem.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.ProductItem.Name
                            },
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                var service = new SessionService();
                Session session = service.Create(options);

                _repoService.OrderHeaderRepository.UpdateStripePaymentID(OrderHeader.Id, session.Id, session.PaymentIntentId);

                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303); 
                #endregion
            }
            return Page(); //not loggedin to acces the page
        }

        //public IActionResult OrderConfirmation(int id)
        //{
        //    OrderHeader orderHeader = _repoService.OrderHeaderRepository.GetWithCondition(o => o.Id == id);
        //    var service = new SessionService();
        //    Session session = service.Get(orderHeader.SessionId);

        //    if (session.PaymentStatus.ToLower() == "paid")
        //    {
        //        _repoService.OrderHeaderRepository.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
        //        _repoService.Save();
        //    }
        //    List<ShoppingCart> shoppingCarts = _repoService.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

        //    _repoService.ShoppingCartRepository.DeleteRange(shoppingCarts);

        //    return RedirectToRoute("./OrderConfirmation", new { id = OrderHeader.Id});
        //}
    }
}
