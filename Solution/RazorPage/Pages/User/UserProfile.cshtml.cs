using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repositories.Implementation;
using Repositories;
using System.Threading.Tasks;
using RazorPage.ViewModels;
using System.ComponentModel.DataAnnotations;

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
            if (userByEmail != null && userByEmail.Count > 0)
            {
                // Check if email belongs to other people
                if (userByEmail[0].CustomerId != Customer.CustomerId)
                {
                    ModelState.AddModelError("Customer.Email", "Email Already Owned");
                    return Page();
                }
            }

            try
            {
                customerRepository.UpdateCustomer(Customer);
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

        private bool CustomerExists(int id)
        {
            return customerRepository.GetCustomerById(id) != null;
        }
    }
}