using GCard.DataAccess.Repository;
using GCard.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public int OrderTotal { get; set; }
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
                //OrderHeader = new()
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
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //ShoppingCartVM = new ShoppingCartVM()
            //{
            //    ShoppingCartList = _repoService.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == claim.Value, includeProp: "Product") //,
            //    //OrderHeader = new()
            //};

            ///to pop-te Name,Adress, Phone lets extract:
            //ShoppingCartVM.OrderHeader.ApplicationUser = _repoService.ApplicationUserRepo.GetWithCondition(u => u.Id == claim.Value);

            //ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            //ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            //ShoppingCartVM.OrderHeader.Address = ShoppingCartVM.OrderHeader.ApplicationUser.Address;


            //foreach (var cart in ShoppingCartVM.ListCart)
            //{
            //    ShoppingCartVM.OrderHeader.OrderTotal += cart.Product.Price * cart.Count;
            //}

            return View(ShoppingCartVM);
        }

        //[HttpPost]
        //[ActionName("Summary")] //?
        //[ValidateAntiForgeryToken]
        //public IActionResult SummaryPOST()  //SummaryPOST(ShoppingCartVM ShoppingCartVM), better add [BindProperty] above
        //{
        //    var claimsIdentity = (ClaimsIdentity)User.Identity;
        //    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        //    //we coud use our collection, but better get from DB(shopcart table?):
        //    ShoppingCartVM.ListCart = _repoService.ShoppingCartRepo.GetAll(u => u.ApplicationUserId == claim.Value, includeProps: "Product");

        //    ShoppingCartVM.OrderHeader.PaymentStatus = StaticDitails.PaymentStatusPending;
        //    ShoppingCartVM.OrderHeader.OrderStatus = StaticDitails.StatusPending;
        //    ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
        //    ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

        //    foreach (var cart in ShoppingCartVM.ListCart)
        //    {
        //        ShoppingCartVM.OrderHeader.OrderTotal += cart.Product.Price * cart.Count;
        //    }

        //    _repoService.OrderHeaderRepo.Add(ShoppingCartVM.OrderHeader);
        //    _repoService.Save();

        //    foreach (var cart in ShoppingCartVM.ListCart)
        //    {
        //        OrderDetail orderDetail = new()
        //        {
        //            ProductId = cart.ProductId,
        //            OrderId = ShoppingCartVM.OrderHeader.Id,
        //            Price = cart.Product.Price,
        //            Count = cart.Count
        //        };
        //        _repoService.OrderDetailRepo.Add(orderDetail);
        //        _repoService.Save();
        //    }

        //    ///STRIPE
        //    var domain = "https://localhost:7028/";
        //    var options = new SessionCreateOptions
        //    {
        //        LineItems = new List<SessionLineItemOptions>(),
        //        Mode = "payment",
        //        SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
        //        CancelUrl = domain + $"customer/cart/index",
        //    };

        //    foreach (var item in ShoppingCartVM.ListCart)
        //    {
        //        var sessionLineItem = new SessionLineItemOptions
        //        {
        //            PriceData = new SessionLineItemPriceDataOptions
        //            {
        //                UnitAmount = (long)(item.Product.Price * 100),
        //                Currency = "usd",
        //                ProductData = new SessionLineItemPriceDataProductDataOptions
        //                {
        //                    Name = item.Product.Title
        //                },
        //            },
        //            Quantity = item.Count
        //        };
        //        options.LineItems.Add(sessionLineItem);
        //    }

        //    var service = new SessionService();
        //    Session session = service.Create(options);

        //    //ShoppingCartVM.OrderHeader.SessionId = session.Id;
        //    //ShoppingCartVM.OrderHeader.PaymentIntentId = session.PaymentIntentId;
        //    //_repoService.Save(); //Instead we moved it to OHRepo:

        //    _repoService.OrderHeaderRepo.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
        //    _repoService.Save();
        //    Response.Headers.Add("Location", session.Url);
        //    return new StatusCodeResult(303);

        //    //_repoService.ShoppingCartRepo.DeleteRange(ShoppingCartVM.ListCart);
        //    //_repoService.Save();
        //    //return RedirectToAction("Index","Home");
        //}


        //public IActionResult OrderConfirmation(int id)
        //{
        //    OrderHeader orderHeader = _repoService.OrderHeaderRepo.GetWithCondition(o => o.Id == id);
        //    var service = new SessionService();
        //    Session session = service.Get(orderHeader.SessionId);

        //    if (session.PaymentStatus.ToLower() == "paid")
        //    {
        //        _repoService.OrderHeaderRepo.UpdateStatus(id, StaticDitails.StatusApproved, StaticDitails.PaymentStatusApproved);
        //        _repoService.Save();
        //    }
        //    List<ShoppingCart> shoppingCarts = _repoService.ShoppingCartRepo.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

        //    _repoService.ShoppingCartRepo.DeleteRange(shoppingCarts);
        //    _repoService.Save();

        //    return View(id);
        //}
    }
}
