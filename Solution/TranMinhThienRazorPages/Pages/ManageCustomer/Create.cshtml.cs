using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject.Models;
using RazorPage.ViewModels;
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
        { var loginUser = HttpContext.Session.GetObjectFromJson<Customer>("user");

            if (loginUser == null || loginUser.CustomerId != -1) // not login or not admin
            {
                return RedirectToPage("../Login/Login");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!ValidateInputs())
            {
                return Page();
            }
            var userByEmail = customerRepository.FindCustomer(1, Customer.Email);
            bool flag = false;
            if (userByEmail != null && userByEmail.Count > 0)
            {
                foreach (var obj in userByEmail)
                {
                    if (obj.Email.CompareTo(Customer.Email) == 0)
                    {
                        if (obj.CustomerId != Customer.CustomerId)
                        {
                            flag = true; // Have save email with other user
                        }
                    }
                }
            }
            if (flag)
            {
                ModelState.AddModelError("Customer.Email", "Email Already Owned");
                return Page();
            }
            
            customerRepository.AddNewCustomer(Customer);

            return RedirectToPage("./Index");
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
