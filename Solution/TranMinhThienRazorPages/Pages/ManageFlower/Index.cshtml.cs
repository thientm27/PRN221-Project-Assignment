﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Models;
using Repositories;
using Repositories.Implementation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RazorPage.Pages.ManageFlower
{
    public class IndexModel : PageModel
    {
        private readonly IFlowerBouquetRepository flowerBouquetRepository = new FlowerBouquetRepository();
        private readonly ICategoryRepository categoryRepository = new CategoryRepository();
        private readonly ISupplierRepository supplierRepository = new SupplierRepository();
        public IList<FlowerBouquet> FlowerBouquet { get;set; }

        public void OnGet()
        {
            FlowerBouquet = flowerBouquetRepository.GetAllFlower();
            foreach (var item in FlowerBouquet)
            {
                var tmp1 = supplierRepository.GetAllSupplier().Where(o => o.SupplierId == item.SupplierId).ToList();
                var tmp2 = categoryRepository.GetAllCategory().Where(o => o.CategoryId == item.CategoryId).ToList();

                if (tmp1.Count > 0)
                {
                    item.Supplier = supplierRepository.GetAllSupplier().Where(o => o.SupplierId == item.SupplierId).ToList()[0];
                }
                if (tmp2.Count > 0)
                {
                    item.Category = categoryRepository.GetAllCategory().Where(o => o.CategoryId == item.CategoryId).ToList()[0];
                }

               
            }

        }
    }
}