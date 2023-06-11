using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Implementation;
using Repositories;
using System.Threading.Tasks;

namespace RazorPage.Pages.Login
{
    public class RegisterModel : PageModel
    {
        private readonly ICustomerRepository customerRepository = new CustomerRepository();

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty] public Customer Customer { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
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

            if (!customerRepository.AddNewCustomer2(Customer))
            {
                ModelState.AddModelError("Customer.Email", "EMAIL NO VALID");
                return Page();
            }

            return RedirectToPage("./Login");
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
                ModelState.AddModelError("Password", "Password cannot null");
                isValid = false;
            }
            
            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                ModelState.AddModelError("ConfirmPassword", "ConfirmPassword cannot null");
                isValid = false;
            }
            else
            {
                if (Customer.Password.CompareTo(ConfirmPassword) != 0)
                {
                    ModelState.AddModelError("Password", "The password and confirmation password do not match.");
                    isValid = false;
                }
            }
            
            return isValid;
        }
    }
}