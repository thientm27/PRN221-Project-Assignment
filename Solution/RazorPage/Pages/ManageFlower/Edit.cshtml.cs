using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Models;
using Repositories.Implementation;
using Repositories;

namespace RazorPage.Pages.ManageFlower
{
    public class EditModel : PageModel
    {
        private readonly IFlowerBouquetRepository flowerBouquetRepository = new FlowerBouquetRepository();
        private readonly ICategoryRepository categoryRepository = new CategoryRepository();
        private readonly ISupplierRepository supplierRepository = new SupplierRepository();


        [BindProperty]
        public FlowerBouquet FlowerBouquet { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //FlowerBouquet = await _context.FlowerBouquets
            //    .Include(f => f.Category)
            //    .Include(f => f.Supplier).FirstOrDefaultAsync(m => m.FlowerBouquetId == id);
            FlowerBouquet = flowerBouquetRepository.GetFlowerById(id);

            if (FlowerBouquet == null)
            {
                return NotFound();
            }
              ViewData["CategoryId"] = new SelectList(categoryRepository.GetAllCategory(), "CategoryId", "CategoryName");
              ViewData["SupplierId"] = new SelectList(supplierRepository.GetAllSupplier(), "SupplierId", "SupplierId");
               return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }



            try
            {
                flowerBouquetRepository.UpdateFlower(FlowerBouquet);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlowerBouquetExists(FlowerBouquet.FlowerBouquetId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FlowerBouquetExists(int id)
        {
            return flowerBouquetRepository.GetFlowerById(id) != null;
        }
    }
}
