using System.Collections.Generic;
using System.Linq;
using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.ViewModels;
using Repositories;
using Repositories.Implementation;

namespace RazorPage.Pages.ManageOrder
{
    public class ShopViewModel : PageModel
    {
        private readonly IFlowerBouquetRepository flowerBouquetRepository = new FlowerBouquetRepository();
        private readonly ICategoryRepository categoryRepository = new CategoryRepository();
        private readonly ISupplierRepository supplierRepository = new SupplierRepository();
        public IList<FlowerBouquet> FlowerBouquet { get; set; }
        
       

        public IActionResult OnGet()
        {
            var loginUser = HttpContext.Session.GetObjectFromJson<Customer>("user");

            if (loginUser == null || loginUser.CustomerId != -1) // not login or not admin
            {
                return RedirectToPage("../Login/Login");
            }
            
            FlowerBouquet = flowerBouquetRepository.GetAllFlower();
          

            return Page();
        }

    }
}
