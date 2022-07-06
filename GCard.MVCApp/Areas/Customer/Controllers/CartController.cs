using GCard.DataAccess.Repository;
using GCard.Model;
using GCard.Model.ViewModels;
using GCard.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace GCard.MVCApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IRepositoryService _repoService;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        //public int OrderTotal { get; set; }
        public CartController(IRepositoryService repoService)
        {
            _repoService = repoService;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = _repoService.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == claim.Value, includeProp: "ProductItem,ProductItem.ItemType,ProductItem.Occasion"), //filter only items of a particular user
                OrderHeader = new()
            };

            ShoppingCartVM.CartTotal = ShoppingCartVM.ShoppingCartList.Sum(m => m.ProductItem.Price * m.Count);

            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int cartId)
        {
            var cart = _repoService.ShoppingCartRepository.GetWithCondition(c=>c.Id == cartId);
            _repoService.ShoppingCartRepository.IncrementCount(cart, 1);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _repoService.ShoppingCartRepository.GetWithCondition(c => c.Id == cartId);
            if (cart.Count > 1)
            {
                _repoService.ShoppingCartRepository.DecrementCount(cart, 1);
            }
            else
            {
                _repoService.ShoppingCartRepository.Delete(cart);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _repoService.ShoppingCartRepository.GetWithCondition(c => c.Id == cartId);
            _repoService.ShoppingCartRepository.Delete(cart);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = _repoService.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == claim.Value, includeProp: "ProductItem"),
                OrderHeader = new()
            };

            // to pop - the Name,Adress, Phone lets extract:
            ShoppingCartVM.OrderHeader.ApplicationUser = _repoService.ApplicationUserRepository.GetWithCondition(u => u.Id == claim.Value);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            ShoppingCartVM.OrderHeader.Address = ShoppingCartVM.OrderHeader.ApplicationUser.Address;

            ShoppingCartVM.OrderHeader.OrderTotal = ShoppingCartVM.ShoppingCartList.Sum(c=>c.ProductItem.Price*c.Count);

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")] 
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST()  //SummaryPOST(ShoppingCartVM ShoppingCartVM), better add [BindProperty] above
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.ShoppingCartList = _repoService.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == claim.Value, includeProp: "ProductItem");

            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

            ShoppingCartVM.OrderHeader.OrderTotal = ShoppingCartVM.ShoppingCartList.Sum(c => c.ProductItem.Price * c.Count);
            //foreach (var cart in ShoppingCartVM.ShoppingCartList)
            //{
            //    ShoppingCartVM.OrderHeader.OrderTotal += cart.ProductItem.Price * cart.Count;
            //}

            _repoService.OrderHeaderRepository.Add(ShoppingCartVM.OrderHeader);
            //_repoService.Save();

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetails orderDetail = new()
                {
                    ProductItemId = cart.ProductItemId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.ProductItem.Price,
                    Count = cart.Count
                };
                _repoService.OrderDetailRepository.Add(orderDetail);
                //_repoService.Save();
            }

            #region STRIPE
            var domain = "https://localhost:7228/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index",
            };

            foreach (var item in ShoppingCartVM.ShoppingCartList) //LineItems:
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

            _repoService.OrderHeaderRepository.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            //_repoService.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

            #endregion
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _repoService.OrderHeaderRepository.GetWithCondition(o => o.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _repoService.OrderHeaderRepository.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
            }
            List<ShoppingCart> shoppingCarts = _repoService.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

            _repoService.ShoppingCartRepository.DeleteRange(shoppingCarts);

            return View(id);
        }
    }
}
