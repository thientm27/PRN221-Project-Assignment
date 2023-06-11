using System;
using System.Collections.Generic;
using System.Linq;
using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.ViewModels;
using Repositories;
using Repositories.Implementation;

namespace RazorPage.Pages.Report
{
    public class IndexModel : PageModel
    {
        private readonly IOrderRepository _orderRepository = new OrderRepository();
        private readonly ICustomerRepository _customerRepository = new CustomerRepository();
        public IList<Order> Order { get; set; }
        [BindProperty] public string SearchValue { get; set; }

        [BindProperty] public DateTime startDay { get; set; }
        [BindProperty] public DateTime endDay { get; set; }
        [BindProperty] public decimal ?Total { get; set; }

        public IActionResult OnGet()
        {
            var loginUser = HttpContext.Session.GetObjectFromJson<Customer>("user");

            if (loginUser == null || loginUser.CustomerId != -1) // not login or not admin
            {
                return RedirectToPage("../Login/Login");
            }

            Order = _orderRepository.GetAllOrders();
            HttpContext.Session.SetObjectAsJson("cart", null);
            // foreach (var obj in Order)
            // {
            //     if (obj.CustomerId != null)
            //     {
            //         obj.Customer = _customerRepository.GetCustomerById((int)obj.CustomerId);
            //     }
            // }

            return Page();
        }

        public IActionResult OnPostSearch()
        {
            Order = _orderRepository.GetAllOrders();
            if (!ModelState.IsValid)
            {
                // Handle invalid model state
                return Page();
            }

            DateTime currentDay = DateTime.Now.Date;

            if (startDay > currentDay)
            {
                ModelState.AddModelError("startDay", "Start day must not be greater than the current day.");
                return Page();
            }

            if (endDay < startDay)
            {
                ModelState.AddModelError("endDay", "End day must be greater than start day.");
                return Page();
            }

            Order = _orderRepository.GetAllOrders()
                .Where(O => O.OrderDate >= startDay
                            && O.OrderDate <= endDay
                            && !O.OrderStatus.ToUpper().Trim().Equals("CANCEL")
                ).ToList();

            Total = Order.Sum(o => o.Total);
            var orderByDescending = Order.OrderByDescending(O => O.Total).ToList();
            Order = orderByDescending;
            return Page();
        }

    }
}