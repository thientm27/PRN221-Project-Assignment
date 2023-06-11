using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Models;
using RazorPage.ViewModels;
using Repositories;
using Repositories.Implementation;

namespace RazorPage.Pages.ManageCustomer
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerRepository customerRepository = new CustomerRepository();


        public IList<Customer> Customer { get; set; }
        [BindProperty] public string SearchValue { get; set; }

        public IActionResult OnGet()
        {
            SearchValue = "";
            var loginUser = HttpContext.Session.GetObjectFromJson<Customer>("user");

            if (loginUser == null || loginUser.CustomerId != -1) // not login or not admin
            {
                return RedirectToPage("../Login/Login");
            }

            Customer = customerRepository.GetAllCustomer();
            return Page();
        }

        public IActionResult OnPostSearch()
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                Customer = customerRepository.GetAllCustomer();
                return Page();
            }

            var search = SearchValue.ToUpper().Trim();
            Customer = customerRepository.GetAllCustomer()
                .Where(O => O.CustomerName.ToUpper().Trim().Contains(search)
                            || O.CustomerName.ToUpper().Trim().Contains(search)
                            || O.City.ToUpper().Trim().Contains(search)
                            || O.Country.ToUpper().Trim().Contains(search)
                            || O.Email.ToUpper().Trim().Contains(search)
                ).ToList();
            return Page();
        }
    }
}