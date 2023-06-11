using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Models;
using RazorPage.ViewModels;
using Repositories.Implementation;
using Repositories;

namespace RazorPage.Pages.ManageCustomer
{
    public class DetailsModel : PageModel
    {
        private readonly ICustomerRepository customerRepository = new CustomerRepository();



        public Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
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

            Customer = customerRepository.GetCustomerById((int)id);

            if (Customer == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
