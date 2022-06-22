using GCard.DataAccess.Repository;
using GCard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GCard.RazorPagesApp.Pages.Admin.ProductItemPage
{
    public class IndexModel : PageModel
    {
        private readonly IRepositoryService _repoService;
        public IEnumerable<ProductItem> ProductItems { get; set; }
        public IndexModel(IRepositoryService repoService)
        {
            _repoService = repoService;
        }

        public void OnGet()
        {
            ProductItems = _repoService.ProductItemRepository.GetAll();
        }
    }
}
