using GCard.DataAccess.Repository;
using GCard.Model;
using GCard.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GCard.MVCApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepositoryService _repoService;

        public HomeController(ILogger<HomeController> logger, IRepositoryService repoService)
        {
            _logger = logger;
            _repoService = repoService;
        }

        public IActionResult Index()
        {
            IEnumerable<ProductItemVM> productItems = _repoService.ProductItemVMRepository.GetAll(includeProp: "ItemType,Occasion");
            //IEnumerable<ItemType> itemTypes = _repoService.ItemTypeRepository.GetAll();
            //IEnumerable<Occasion> occasions = _repoService.OccasionRepository.GetAll();

            return View(productItems);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}