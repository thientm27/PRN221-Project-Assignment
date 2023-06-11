using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Models;
using RazorPage.ViewModels;
using Repositories;
using Repositories.Implementation;

namespace RazorPage.Pages.ManageFlower
{
    public class DetailsModel : PageModel
    {

        private readonly IFlowerBouquetRepository flowerBouquetRepository = new FlowerBouquetRepository();
        private readonly ICategoryRepository categoryRepository = new CategoryRepository();
        private readonly ISupplierRepository supplierRepository = new SupplierRepository();
        public FlowerBouquet FlowerBouquet { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var loginUser = HttpContext.Session.GetObjectFromJson<Customer>("user");

            if (loginUser == null || loginUser.CustomerId != -1) // not login or not admin
            {
                return RedirectToPage("../Login/Login");
            }
            
            if (id == null)
            {
                return NotFound();
            }

            FlowerBouquet = flowerBouquetRepository.GetFlowerById(id);
 
            
            
            if (FlowerBouquet == null)
            {
                return NotFound();
            }
            var tmp1 = supplierRepository.GetAllSupplier().Where(o => o.SupplierId == FlowerBouquet.SupplierId).ToList();
            var tmp2 = categoryRepository.GetAllCategory().Where(o => o.CategoryId == FlowerBouquet.CategoryId).ToList();


            if (tmp1.Count > 0)
            {
                FlowerBouquet.Supplier = supplierRepository.GetAllSupplier().Where(o => o.SupplierId == FlowerBouquet.SupplierId).ToList()[0];
            }
            if (tmp2.Count > 0)
            {
                FlowerBouquet.Category = categoryRepository.GetAllCategory().Where(o => o.CategoryId == FlowerBouquet.CategoryId).ToList()[0];
            }

            return Page();
        }
    }
}
