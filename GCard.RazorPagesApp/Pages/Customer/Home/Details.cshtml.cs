using GCard.DataAccess.Repository;
using GCard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GCard.RazorPagesApp.Pages.Customer.Home
{
    public class DetailsModel : PageModel
    {
        private readonly IRepositoryService _repoService;
        public ShoppingCart ShoppingCart { get; set; }
        public DetailsModel(IRepositoryService repoService)
        {
            _repoService = repoService;
        }

        public void OnGet(int id)
        {
            ShoppingCart = new()
            {
                Count = 1,
                ProductItem = _repoService.ProductItemRepository.GetWithCondition(i => i.Id == id, includeProp: "ItemType,Occasion")
            };
        }
    }
}
