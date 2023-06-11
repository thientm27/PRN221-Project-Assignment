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
        [BindProperty] public Order Order { get; set; }
        [BindProperty] public decimal Total { get; set; }
        private List<CartItem> Cart { get; set; }
        public IActionResult OnGet()
        {
            var loginUser = HttpContext.Session.GetObjectFromJson<Customer>("user");

            if (loginUser == null || loginUser.CustomerId != -1) // not login or not admin
            {
                return RedirectToPage("../Login/Login");
            }
            
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart");
            if (Cart == null)
            {
                return RedirectToPage("./Index");
            }

            Order = new Order();
            Total = Cart.Sum(i => i.Item.UnitPrice * i.Quantity);
            Order.Total = Total;
            ViewData["CustomerId"] = new SelectList(_customerRepository.GetAllCustomer(), "CustomerId", "CustomerName");
            return Page();
        }



        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart");
            Total = Cart.Sum(i => i.Item.UnitPrice * i.Quantity);
            
            // CREATE ORDER
            var orderId = _orderRepository.AddOrder(Order.CustomerId, Order.ShippedDate, Total.ToString(),
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
            foreach (var orderDetail in Cart)
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
            Cart.Clear();
            HttpContext.Session.SetObjectAsJson("cart", Cart);
            return RedirectToPage("./Index");
        }
    }
}