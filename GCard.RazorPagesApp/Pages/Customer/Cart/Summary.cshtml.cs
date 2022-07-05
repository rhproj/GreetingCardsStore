using GCard.DataAccess.Repository;
using GCard.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace GCard.RazorPagesApp.Pages.Customer.Cart
{
    [Authorize]
    public class SummaryModel : PageModel
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
        private readonly IRepositoryService _repoService;
        public SummaryModel(IRepositoryService repoService)
        {
            _repoService = repoService;
            OrderHeader = new OrderHeader();
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
            }
            ApplicationUser applicationUser = _repoService.ApplicationUserRepository.GetWithCondition(u => u.Id == claim.Value);

            OrderHeader.Name = applicationUser.Name;
            OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
            OrderHeader.PostalCode = applicationUser.PostalCode;
            OrderHeader.Address = applicationUser.Address;
        }
    }
}
