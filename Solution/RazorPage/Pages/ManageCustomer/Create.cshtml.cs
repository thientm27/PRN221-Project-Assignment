using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject.Models;
using Repositories.Implementation;
using Repositories;

namespace RazorPage.Pages.ManageCustomer
{
    public class CreateModel : PageModel
    {
        private readonly ICustomerRepository customerRepository = new CustomerRepository();

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            customerRepository.AddNewCustomer(Customer);

            return RedirectToPage("./Index");
        }
    }
}
