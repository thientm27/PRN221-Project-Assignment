using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObject.Models;
using RazorPage.ViewModels;
using Repositories;
using Repositories.Implementation;

namespace RazorPage.Pages.ManageOrder
{
    public class EditModel : PageModel
    {
        private readonly IOrderRepository _orderRepository = new OrderRepository();
        private readonly IOrderDetailRepository _orderDetailRepository = new OrderDetailRepository();
        private readonly ICustomerRepository _customerRepository = new CustomerRepository();

        [BindProperty] public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var loginUser = HttpContext.Session.GetObjectFromJson<Customer>("user");

            if (loginUser == null || loginUser.CustomerId != -1) // not login or not admin
            {
                return RedirectToPage("../Login/Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            Order = _orderRepository.GetOrderById((int)id);

            if (Order == null)
            {
                return NotFound();
            }

            ViewData["CustomerId"] = new SelectList(_customerRepository.GetAllCustomer(), "CustomerId", "CustomerName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                _orderRepository.UpdateOrder(Order);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Order.OrderId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OrderExists(int id)
        {
            return _orderRepository.GetAllOrders().Any(e => e.OrderId == id);
        }
    }
}