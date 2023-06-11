using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Models;
using Repositories;
using Repositories.Implementation;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPage.ViewModels;

namespace RazorPage.Pages.ManageFlower
{
    public class IndexModel : PageModel
    {
        private readonly IFlowerBouquetRepository flowerBouquetRepository = new FlowerBouquetRepository();
        private readonly ICategoryRepository categoryRepository = new CategoryRepository();
        private readonly ISupplierRepository supplierRepository = new SupplierRepository();
        public IList<FlowerBouquet> FlowerBouquet { get; set; }
        [BindProperty] public string SearchValue { get; set; }

        public IActionResult OnGet()
        {
            var loginUser = HttpContext.Session.GetObjectFromJson<Customer>("user");

            if (loginUser == null || loginUser.CustomerId != -1) // not login or not admin
            {
                return RedirectToPage("../Login/Login");
            }

            FlowerBouquet = flowerBouquetRepository.GetAllFlower();
            // foreach (var item in FlowerBouquet)
            // {
            //     var tmp1 = supplierRepository.GetAllSupplier().Where(o => o.SupplierId == item.SupplierId).ToList();
            //     var tmp2 = categoryRepository.GetAllCategory().Where(o => o.CategoryId == item.CategoryId).ToList();
            //
            //     if (tmp1.Count > 0)
            //     {
            //         item.Supplier = supplierRepository.GetAllSupplier().Where(o => o.SupplierId == item.SupplierId).ToList()[0];
            //     }
            //     if (tmp2.Count > 0)
            //     {
            //         item.Category = categoryRepository.GetAllCategory().Where(o => o.CategoryId == item.CategoryId).ToList()[0];
            //     }
            //
            //    
            // }
            return Page();
        }

        public IActionResult OnPostSearch()
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                FlowerBouquet = flowerBouquetRepository.GetAllFlower();
                return Page();
            }

            var search = SearchValue.ToUpper().Trim();
            FlowerBouquet = flowerBouquetRepository.GetAllFlower()
                .Where(O => O.FlowerBouquetName.ToUpper().Trim().Contains(search)
                            || O.Supplier.SupplierName.ToUpper().Trim().Contains(search)
                            || O.Supplier.SupplierName.ToUpper().Trim().Contains(search)
                            || O.Description.ToUpper().Trim().Contains(search)
                ).ToList();
            return Page();
        }
    }
}