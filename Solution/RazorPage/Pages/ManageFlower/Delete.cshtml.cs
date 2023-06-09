using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Models;

namespace RazorPage.Pages.ManageFlower
{
    public class DeleteModel : PageModel
    {
        private readonly BussinessObject.Models.FUFlowerBouquetManagementContext _context;

        public DeleteModel(BussinessObject.Models.FUFlowerBouquetManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FlowerBouquet FlowerBouquet { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FlowerBouquet = await _context.FlowerBouquets
                .Include(f => f.Category)
                .Include(f => f.Supplier).FirstOrDefaultAsync(m => m.FlowerBouquetId == id);

            if (FlowerBouquet == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FlowerBouquet = await _context.FlowerBouquets.FindAsync(id);

            if (FlowerBouquet != null)
            {
                _context.FlowerBouquets.Remove(FlowerBouquet);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
