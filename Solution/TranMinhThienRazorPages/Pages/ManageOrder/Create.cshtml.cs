using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject.Models;
using RazorPage.ViewModels;
using Repositories;
using Repositories.Implementation;

namespace RazorPage.Pages.ManageOrder
{
    public class CreateModel : PageModel
    {
        private readonly IOrderRepository _orderRepository = new OrderRepository();
        private readonly IOrderDetailRepository _orderDetailRepository = new OrderDetailRepository();
        private readonly ICustomerRepository _customerRepository = new CustomerRepository();

        public IActionResult OnGet()
        {
            ViewData["CustomerId"] = new SelectList(_customerRepository.GetAllCustomer(), "CustomerId", "CustomerName");
            return Page();
        }

        [BindProperty] public Order Order { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart");
            // CREATE ORDER
            Order.Total = cart.Sum(i => i.Item.UnitPrice * i.Quantity);
            var orderId = _orderRepository.AddOrder(Order.CustomerId, Order.ShippedDate, Order.Total.ToString(),
                Order.OrderStatus,
                out var message);
            
            // Check Error
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(String.Empty, message);
                return Page();
            }

            // Create detail
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var orderDetail in cart)
            {
                orderDetails.Add(new OrderDetail()
                {
                    OrderId = orderId,
                    FlowerBouquetId = orderDetail.Item.FlowerBouquetId,
                    Discount = 0,
                    Quantity = orderDetail.Quantity,
                    UnitPrice = orderDetail.Item.UnitPrice,
                });
            }

            try
            {
                _orderDetailRepository.AddOrderDetails(orderDetails);
            }
            catch (Exception exception)
            {
                _orderRepository.DeleteOrder(orderId);
                ModelState.AddModelError(String.Empty, "Error when create details");
                return Page();
            }
            cart.Clear();
            HttpContext.Session.SetObjectAsJson("cart", cart);
            return RedirectToPage("./Index");
        }
    }
}