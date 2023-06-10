using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Models;
using Repositories.Implementation;
using Repositories;
using RazorPage.ViewModels;

namespace RazorPage.Pages.User
{
    public class OrderHistoryModel : PageModel
    {
        private readonly ICustomerRepository customerRepository = new CustomerRepository();

        private readonly IOrderRepository orderRepository = new OrderRepository();

        public List<Order> Order { get;set; }

        public IActionResult OnGet()
        {
            var loginUser = HttpContext.Session.GetObjectFromJson<Customer>("user");

            if (loginUser == null)
            {
                return RedirectToPage("../Login/Login");
            }
            var Customer = customerRepository.GetCustomerById(loginUser.CustomerId);
            if (Customer == null)
            {
                return NotFound();
            }
            Order = orderRepository.GetOrdersByCustomer(Customer.CustomerId);

            return Page();

        }
   
    }
}
