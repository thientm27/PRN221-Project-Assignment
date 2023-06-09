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
    public class DetailsModel : PageModel
    {
        private readonly BussinessObject.Models.FUFlowerBouquetManagementContext _context;

        public DetailsModel(BussinessObject.Models.FUFlowerBouquetManagementContext context)
        {
            _context = context;
        }

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
    }
}
