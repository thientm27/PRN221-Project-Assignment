using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Models;
using RazorPage.ViewModels;
using Repositories.Implementation;
using Repositories;

namespace RazorPage.Pages.ManageCustomer
{
    public class EditModel : PageModel
    {
        private readonly ICustomerRepository customerRepository = new CustomerRepository();
        [BindProperty] public Customer Customer { get; set; }

        public IActionResult OnGet(int? id)
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
            else
            {
                Customer = customerRepository.GetCustomerById((int)id);
            }


            if (Customer == null)
            {
                return NotFound();
            }

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

            if (!ValidateInputs())
            {
                return Page();
            }

            try
            {
                customerRepository.UpdateCustomer(Customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(Customer.CustomerId))
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

        private bool CustomerExists(int id)
        {
            return customerRepository.GetCustomerById(id) != null;
        }

        private bool ValidateInputs()
        {
            var isValid = true;

            if (string.IsNullOrEmpty(Customer.Email))
            {
                ModelState.AddModelError("Customer.Email", "Email cannot null");
                isValid = false;
            }
            else
            {
                // Regular expression pattern for email validation
                string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

                if (!Regex.IsMatch(Customer.Email, emailPattern))
                {
                    ModelState.AddModelError("Customer.Email", "Invalid email address");
                    return false;
                }
            }

            if (string.IsNullOrEmpty(Customer.CustomerName))
            {
                ModelState.AddModelError("Customer.CustomerName", "CustomerName cannot null");
                isValid = false;
            }

            if (string.IsNullOrEmpty(Customer.City))
            {
                ModelState.AddModelError("Customer.City", "City cannot null");
                isValid = false;
            }

            if (string.IsNullOrEmpty(Customer.Country))
            {
                ModelState.AddModelError("Customer.Country", "Country cannot null");
                isValid = false;
            }

            if (string.IsNullOrEmpty(Customer.Password))
            {
                ModelState.AddModelError("Customer.Password", "Password cannot null");
                isValid = false;
            }

            return isValid;
        }
    }
}