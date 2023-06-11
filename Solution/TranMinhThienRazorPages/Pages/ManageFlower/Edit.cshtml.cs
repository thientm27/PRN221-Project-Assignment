using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Models;
using RazorPage.ViewModels;
using Repositories.Implementation;
using Repositories;

namespace RazorPage.Pages.ManageFlower
{
    public class EditModel : PageModel
    {
        private readonly IFlowerBouquetRepository flowerBouquetRepository = new FlowerBouquetRepository();
        private readonly ICategoryRepository categoryRepository = new CategoryRepository();
        private readonly ISupplierRepository supplierRepository = new SupplierRepository();


        [BindProperty] public FlowerBouquet FlowerBouquet { get; set; }

        public IActionResult OnGet(int? id)
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

            ViewData["CategoryId"] = new SelectList(categoryRepository.GetAllCategory(), "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(supplierRepository.GetAllSupplier(), "SupplierId", "SupplierName");
            return Page();
        }

        // To protect from overposting attacks, enable the spescific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public  IActionResult OnPost()
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
            
            try
            {
                FlowerBouquet.FlowerBouquetStatus = 1;
                flowerBouquetRepository.UpdateFlower(FlowerBouquet);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlowerBouquetExists(FlowerBouquet.FlowerBouquetId))
                {
                    return NotFound();
                }
                
            }

            return RedirectToPage("./Index");
        }

        private bool FlowerBouquetExists(int id)
        {
            return flowerBouquetRepository.GetFlowerById(id) != null;
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