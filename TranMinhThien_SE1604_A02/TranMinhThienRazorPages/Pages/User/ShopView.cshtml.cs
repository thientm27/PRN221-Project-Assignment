using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Implementation;
using Repositories;
using System.Collections.Generic;
using System.Linq;
using RazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace RazorPage.Pages.User
{
    public class ShopViewModel : PageModel
    {
        private readonly IFlowerBouquetRepository flowerBouquetRepository = new FlowerBouquetRepository();

        public IList<FlowerBouquet> FlowerBouquet { get; set; }
       

        public IActionResult OnGet()
        {
            var loginUser = HttpContext.Session.GetObjectFromJson<Customer>("user");

            if (loginUser == null || loginUser.CustomerId == -1) // not login or admin
            {
                return RedirectToPage("../Login/Login");
            }
            
            FlowerBouquet = flowerBouquetRepository.GetAllFlower();
            
            return Page();
        }

    }
}
