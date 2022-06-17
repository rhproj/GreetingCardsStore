using GCard.DataAccess.Repository;
using GCard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GCard.RazorPagesApp.Pages.Admin.ItemTypePage
{
    public class IndexModel : PageModel
    {
        private readonly IRepositoryService _repoService; 
        public IEnumerable<ItemType> ItemTypes { get; set; }
        public IndexModel(IRepositoryService repoService)  
        {
            _repoService = repoService;  
        }

        public void OnGet()
        {
            ItemTypes = _repoService.ItemTypeRepository.GetAll();
        }
    }
}
