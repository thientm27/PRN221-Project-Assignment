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

        [BindProperty] public FlowerBouquet FlowerBouquet { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!CheckValidation(FlowerBouquet))
            {
                ViewData["CategoryId"] =
                    new SelectList(categoryRepository.GetAllCategory(), "CategoryId", "CategoryName");

                ViewData["SupplierId"] =
                    new SelectList(supplierRepository.GetAllSupplier(), "SupplierId", "SupplierName");

                return Page();
            }

            flowerBouquetRepository.CreateFlower(FlowerBouquet);
            return RedirectToPage("./Index");
        }

        private bool CheckValidation(FlowerBouquet flowerBouquet)
        {
            if (flowerBouquet.UnitsInStock < 0)
            {
                ModelState.AddModelError(string.Empty, "UnitsInStock must be >= 0");
                return false;
            }

            if (flowerBouquet.UnitPrice <= 0)
            {
                ModelState.AddModelError(string.Empty, "UnitPrice must be >= 0");
                return false;
            }

            if (string.IsNullOrEmpty(flowerBouquet.FlowerBouquetName))
            {
                ModelState.AddModelError(nameof(flowerBouquet.FlowerBouquetName), "FlowerBouquetName is required");
                return false;
            }
            else if (flowerBouquet.FlowerBouquetName.Length > 100)
            {
                ModelState.AddModelError(nameof(flowerBouquet.FlowerBouquetName),
                    "FlowerBouquetName cannot exceed 100 characters");
                return false;
            }

            if (string.IsNullOrEmpty(flowerBouquet.Description))
            {
                flowerBouquet.Description = "";
            }
            if (!string.IsNullOrEmpty(flowerBouquet.Description) && flowerBouquet.Description.Length > 500)
            {
                ModelState.AddModelError(nameof(flowerBouquet.Description), "Description cannot exceed 500 characters");
                return false;
            }
            
            // Add more validation rules as needed

            return true;
        }
    }
}