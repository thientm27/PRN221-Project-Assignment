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

namespace RazorPage.Pages.ManageFlower
{
    public class IndexModel : PageModel
    {
        private readonly IFlowerBouquetRepository flowerBouquetRepository = new FlowerBouquetRepository();

        public IList<FlowerBouquet> FlowerBouquet { get;set; }

        public void OnGet()
        {
            FlowerBouquet = flowerBouquetRepository.GetAllFlower();
        }
    }
}
