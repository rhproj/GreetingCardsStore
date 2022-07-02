using GCard.DataAccess.Repository;
using GCard.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace GCard.RazorPagesApp.Pages.Customer.Home
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IRepositoryService _repoService;
        [BindProperty]
        public ShoppingCart ShoppingCart { get; set; }
        public DetailsModel(IRepositoryService repoService)
        {
            _repoService = repoService;
        }

        public void OnGet(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCart = new()
            {
                ApplicationUserId = claim.Value, //now we have UserId populated inside shopping cart object
                ProductItemId = id,
                ProductItem = _repoService.ProductItemRepository.GetWithCondition(m => m.Id == id, includeProp: "ItemType,Occasion")
            };
            //_repoService.ShoppingCartRepository.Add(shoppingCart);
            //return RedirectToAction(nameof(Index));
        }

        public IActionResult OnPost() //������ �� ����� id ����������? - bcz we binded the Shopping Cart
        {
            if (ModelState.IsValid)
            {
                _repoService.ShoppingCartRepository.Add(ShoppingCart);


                //ShoppingCart cartFromDb = _repoService.ShoppingCartRepository.GetWithCondition(filter:
                //    u => u.ApplicationUserId == ShoppingCart.ApplicationUserId && u.ProductItemId == ShoppingCart.ProductItemId); //if + + means record exist, just increase counter

                //if (cartFromDb == null) //��� �� �������� ����� ����
                //{
                //    _repoService.ShoppingCartRepository.Add(ShoppingCart);
                //    _repoService.Save();
                //}
                //else
                //{
                //    _repoService.ShoppingCartRepository.IncrementCount(cartFromDb, ShoppingCart.Count);
                //    //������ �� ����� _repoWrapper.Save() �����? ��-���� ��� �� ������ ��� inside IncrementCount ?? 
                //}

                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
