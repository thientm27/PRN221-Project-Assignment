using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Implementation;
using Repositories;
using System.Threading.Tasks;
using RazorPage.ViewModels;

namespace RazorPage.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerRepository customerRepository = new CustomerRepository();

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty] public Customer Customer { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var customerLogin = customerRepository.Login(Customer.Email, Customer.Password);

            // -1 admin
            // null not valid
            // user
            if (customerLogin == null)
            {
                ModelState.AddModelError(string.Empty, "User or password is incorrect");
                return Page();
            }
            HttpContext.Session.SetObjectAsJson("user", customerLogin);
            if (customerLogin.CustomerId == -1) // admin
            {
                return RedirectToPage("../ManageFlower/index");
            } // customer

            return RedirectToPage("../User/ShopView");
        }
        
        public IActionResult OnPostLogOutAsync()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("../Login/Login");
        }
    }
}