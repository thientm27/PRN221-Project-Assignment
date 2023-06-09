using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject.Models;

namespace RazorPage.Pages.ManageFlower
{
    public class CreateModel : PageModel
    {
        private readonly BussinessObject.Models.FUFlowerBouquetManagementContext _context;

        public CreateModel(BussinessObject.Models.FUFlowerBouquetManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
        ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierId");
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

            _context.FlowerBouquets.Add(FlowerBouquet);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
