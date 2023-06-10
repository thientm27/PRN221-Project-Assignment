using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Models;
using Repositories.Implementation;
using Repositories;

namespace RazorPage.Pages.ManageFlower
{
    public class DeleteModel : PageModel
    {
        private readonly IFlowerBouquetRepository flowerBouquetRepository = new FlowerBouquetRepository();

        [BindProperty]
        public FlowerBouquet FlowerBouquet { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FlowerBouquet = flowerBouquetRepository.GetFlowerById(id);

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

            FlowerBouquet = flowerBouquetRepository.GetFlowerById(id);

            if (FlowerBouquet != null)
            {
               flowerBouquetRepository.DeleteFlower(FlowerBouquet.FlowerBouquetId);
            }

            return RedirectToPage("./Index");
        }
    }
}
