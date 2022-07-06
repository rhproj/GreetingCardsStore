using GCard.DataAccess.Repository;
using GCard.Model;
using GCard.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;

namespace GCard.RazorPagesApp.Pages.Customer.Cart
{
    public class OrderConfirmationModel : PageModel
    {
        public int OrderId { get; set; }
        private readonly IRepositoryService _repoService;
        public OrderConfirmationModel(IRepositoryService repoService)
        {
            _repoService = repoService;
        }


        public void OnGet(int id)
        {
            OrderHeader orderHeader = _repoService.OrderHeaderRepository.GetWithCondition(o => o.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid") //payment successfull
            {
                _repoService.OrderHeaderRepository.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
            }
            List<ShoppingCart> shoppingCarts = _repoService.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

            _repoService.ShoppingCartRepository.DeleteRange(shoppingCarts);
            OrderId = id;
        }
    }
}
