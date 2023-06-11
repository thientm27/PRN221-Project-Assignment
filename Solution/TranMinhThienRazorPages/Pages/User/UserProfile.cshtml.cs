using System.Collections.Generic;
using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories.Implementation;
using Repositories;
using System.Threading.Tasks;
using RazorPage.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RazorPage.Pages.User
{
    public class UserProfileModel : PageModel
    {
        private readonly ICustomerRepository customerRepository = new CustomerRepository();
        
        [BindProperty] public Customer Customer { get; set; }

        [BindProperty] public bool ChangePassword { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        public IActionResult OnGet()
        {
            var loginUser = HttpContext.Session.GetObjectFromJson<Customer>("user");

            if (loginUser == null || loginUser.CustomerId == -1) // not login or admin
            {
                return RedirectToPage("../Login/Login");
            }

            Customer = customerRepository.GetCustomerById(loginUser.CustomerId);
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

            if (ChangePassword)
            {
                var oldPass = customerRepository.GetCustomerById(Customer.CustomerId).Password;
                if (oldPass != OldPassword)
                {
                    ModelState.AddModelError("OldPassword", "Incorrect old password.");
                    return Page();
                }

                if (ConfirmPassword != Password)
                {
                    ModelState.AddModelError("Password", "The password and confirmation password do not match.");
                    return Page();
                }

                Customer.Password = Password;
            }
            else
            {
                Customer.Password = customerRepository.GetCustomerById(Customer.CustomerId).Password;
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

            try
            {
                if (!customerRepository.AddNewCustomer2(Customer))
                {
                    ModelState.AddModelError("Customer.Email", "EMAIL NO VALID");
                    return Page();
                }
                HttpContext.Session.SetObjectAsJson("user", Customer);
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

            return RedirectToPage("./ShopView");
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

            if (ChangePassword)
            {
                if (string.IsNullOrEmpty(Password))
                {
                    ModelState.AddModelError("Password", "Password cannot null");
                    isValid = false;
                }
                if (string.IsNullOrEmpty(OldPassword))
                {
                    ModelState.AddModelError("OldPassword", "OldPassword cannot null");
                    isValid = false;
                }
                if (string.IsNullOrEmpty(ConfirmPassword))
                {
                    ModelState.AddModelError("ConfirmPassword", "ConfirmPassword cannot null");
                    isValid = false;
                }
                
            }
            
            return isValid;
        }
        private bool CustomerExists(int id)
        {
            return customerRepository.GetCustomerById(id) != null;
        }
    }
}