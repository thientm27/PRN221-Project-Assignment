using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Implementation;
using Repositories;
using System.Threading.Tasks;

namespace RazorPage.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerRepository customerRepository = new CustomerRepository();

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; }
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
                return RedirectToPage("./Login");
            }
            else if (customerLogin.CustomerId == -1)
            {
                return RedirectToPage("../ManageFlower/index");
            }
            else
            {
                return RedirectToPage("../Managecustomer/index");
            }
           
        }
    }
}
