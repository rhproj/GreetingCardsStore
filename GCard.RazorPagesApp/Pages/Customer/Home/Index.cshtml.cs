using GCard.DataAccess.Repository;
using GCard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GCard.RazorPagesApp.Pages.Customer.Home
{
    public class IndexModel : PageModel
    {
        private readonly IRepositoryService _repoService;
        public IEnumerable<ProductItem> ProductItems { get; set; }
        public IEnumerable<ItemType> ItemTypes { get; set; }
        public IEnumerable<Occasion> Occasions { get; set; }
        public IndexModel(IRepositoryService repoService)
        {
            _repoService = repoService;
        }

        public void OnGet()
        {
            ProductItems = _repoService.ProductItemRepository.GetAll(includeProp: "ItemType,Occasion");
            ItemTypes = _repoService.ItemTypeRepository.GetAll();
            Occasions = _repoService.OccasionRepository.GetAll();
        }
    }
}
