using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject.Models;
using RazorPage.ViewModels;
using Repositories.Implementation;
using Repositories;

namespace RazorPage.Pages.ManageFlower
{
    public class CreateModel : PageModel
    {
        private readonly IFlowerBouquetRepository flowerBouquetRepository = new FlowerBouquetRepository();
        private readonly ICategoryRepository categoryRepository = new CategoryRepository();
        private readonly ISupplierRepository supplierRepository = new SupplierRepository();
        public IActionResult OnGet()
        {
            var loginUser = HttpContext.Session.GetObjectFromJson<Customer>("user");

            if (loginUser == null || loginUser.CustomerId != -1) // not login or not admin
            {
                return RedirectToPage("../Login/Login");
            }
            ViewData["CategoryId"] = new SelectList(categoryRepository.GetAllCategory(), "CategoryId", "CategoryName");
      
            ViewData["SupplierId"] = new SelectList(supplierRepository.GetAllSupplier(), "SupplierId", "SupplierName");
            return Page();
        }

        [BindProperty]
        public FlowerBouquet FlowerBouquet { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            flowerBouquetRepository.CreateFlower(FlowerBouquet);
            return RedirectToPage("./Index");
        }
    }
}
