using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Models;
using Repositories;
using Repositories.Implementation;

namespace RazorPage.Pages.ManageCustomer
{
    public class IndexModel : PageModel
    {

        private readonly ICustomerRepository customerRepository = new CustomerRepository();


        public IList<Customer> Customer { get;set; }

        public async Task OnGetAsync()
        {
            Customer = customerRepository.GetAllCustomer();
        }
    }
}
