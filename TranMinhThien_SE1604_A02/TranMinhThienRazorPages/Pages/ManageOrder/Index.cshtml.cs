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

namespace RazorPage.Pages.ManageOrder
{
    public class IndexModel : PageModel
    {
        private readonly IOrderRepository _orderRepository = new OrderRepository();
        private readonly ICustomerRepository _customerRepository = new CustomerRepository();
        public IList<Order> Order { get;set; }
        [BindProperty] public string SearchValue { get; set; }
        public IActionResult  OnGet()
        {
            var loginUser = HttpContext.Session.GetObjectFromJson<Customer>("user");

            if (loginUser == null || loginUser.CustomerId != -1) // not login or not admin
            {
                return RedirectToPage("../Login/Login");
            }
            
            Order = _orderRepository.GetAllOrders();
            HttpContext.Session.SetObjectAsJson("cart", null);
      

            return Page();
        }
        
        public IActionResult OnPostSearch()
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                 Order = _orderRepository.GetAllOrders();
                 return Page();
            }

            var search = SearchValue.ToUpper().Trim();
            Order = _orderRepository.GetAllOrders()
                .Where(O => O.Customer.CustomerName.ToUpper().Trim().Contains(search)
                            || O.OrderStatus.ToUpper().Trim().Contains(search)

                ).ToList();
            return Page();
        }
    }
}
