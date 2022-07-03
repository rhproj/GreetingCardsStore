using GCard.DataAccess.Repository;
using GCard.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace GCard.RazorPagesApp.Pages.Customer.Cart
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IRepositoryService _repoService;
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public decimal? CartTotal { get; set; }
        public IndexModel(IRepositoryService repoService)
        {
            _repoService = repoService;
            CartTotal = 0;
        }

        public void OnGet()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                ShoppingCartList = _repoService.ShoppingCartRepository.GetAll(filter: u => u.ApplicationUserId == claim.Value,
                    includeProp:"ProductItem"); //,ProductItem.ItemType,ProductItem.Occasion

                //CartTotal = ShoppingCartList.Sum(m => m.ProductItem.Price * m.Count);
            }
        }

        public IActionResult OnPostPlus(int cartId)
        {
            var cart = _repoService.ShoppingCartRepository.GetWithCondition(c => c.Id == cartId);
            _repoService.ShoppingCartRepository.IncrementCount(cart, 1);

            return RedirectToPage("/Customer/Cart/Index");
        }

        public IActionResult OnPostMinus(int cartId)
        {
            var cart = _repoService.ShoppingCartRepository.GetWithCondition(c => c.Id == cartId);
            if (cart.Count == 1)
            {
                _repoService.ShoppingCartRepository.Delete(cart);
            }
            else
            {
                _repoService.ShoppingCartRepository.DecrementCount(cart, 1);
            }

            return RedirectToPage("/Customer/Cart/Index");
        }

        public IActionResult OnPostRemove(int cartId)
        {
            var cart = _repoService.ShoppingCartRepository.GetWithCondition(c => c.Id == cartId);
            _repoService.ShoppingCartRepository.Delete(cart);
            _repoService.Save();
            return RedirectToPage("/Customer/Cart/Index");
        }
    }
}
