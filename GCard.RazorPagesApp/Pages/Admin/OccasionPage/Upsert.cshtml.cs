using GCard.DataAccess.Repository;
using GCard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GCard.RazorPagesApp.Pages.Admin.OccasionPage
{
    public class UpsertModel : PageModel
    {
        private readonly IRepositoryService _repoService;
        [BindProperty]
        public Occasion Occasion { get; set; }
        public UpsertModel(IRepositoryService repoService)
        {
            _repoService = repoService;
            Occasion = new();
        }

        public void OnGet(int? id)
        {
            if (id != null)
            {
                Occasion = _repoService.OccasionRepository.GetWithCondition(i => i.Id == id);
            }
        }

        public IActionResult OnPost()
        {
            if (Occasion.Id == 0)
            {
                _repoService.OccasionRepository.Add(Occasion);
                TempData["success"] = "New Occasion successfully added";
            }
            else
            {
                _repoService.OccasionRepository.Update(Occasion);
                TempData["success"] = "Occasion successfully updated";
            }

            return RedirectToPage("./Index");
        }
    }
}
