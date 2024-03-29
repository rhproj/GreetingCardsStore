﻿using GCard.DataAccess.Repository;
using GCard.Model;
using GCard.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Dynamic;
using System.Security.Claims;

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
            //IEnumerable<ProductItem> productList = new List<ProductItem>(); //_repoService.ProductRepo.GetAll(includeProps: "Category,CoverType");
            //return View(productList);
            //IEnumerable<ProductItem> productItems = _repoService.ProductItemRepository.GetAll(includeProp: "ItemType,Occasion");
            //IEnumerable<ItemType> itemTypes = _repoService.ItemTypeRepository.GetAll();
            //IEnumerable<Occasion> occasions = _repoService.OccasionRepository.GetAll();

            dynamic model = new ExpandoObject();
            model.Occasions = _repoService.OccasionRepository.GetAll();
            model.ProductItems = _repoService.ProductItemRepository.GetAll(includeProp: "ItemType,Occasion");

            //ProductItemVM productItemVM = new ProductItemVM();
            //productItemVM.OccasionList = occasions; //_repoService.OccasionRepository.GetAll();

            return View(model);    //productItems);
        }

        public IActionResult Details(int productId) //int id -got conflicted with 
        {
            //ProductItem productItem = _repoService.ProductItemRepository.GetWithCondition(i=>i.Id == id, includeProp: "ItemType,Occasion");
            ShoppingCart shoppingCart = new()
            {
                Count = 1,
                ProductItemId = productId,
                ProductItem = _repoService.ProductItemRepository.GetWithCondition(i => i.Id == productId, includeProp: "ItemType,Occasion")
            };

            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            //if exists
            ShoppingCart cartFromDb = _repoService.ShoppingCartRepository.GetWithCondition(c => c.ApplicationUserId == claim.Value && c.ProductItemId == shoppingCart.ProductItemId);
            if (cartFromDb == null) //no record of this item yet
            {
                _repoService.ShoppingCartRepository.Add(shoppingCart);
            }
            else
            {
                _repoService.ShoppingCartRepository.IncrementCount(cartFromDb, shoppingCart.Count);
                //_repoService.ShoppingCartRepository.Update(shoppingCart);
            }

            //_repoService.ShoppingCartRepository.Add(shoppingCart);

            return RedirectToAction(nameof(Index));
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