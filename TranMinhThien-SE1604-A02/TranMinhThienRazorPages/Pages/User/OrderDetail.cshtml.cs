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
using RazorPage.ViewModels;

namespace RazorPage.Pages.User
{
    public class OrderDetailModel : PageModel
    {
        private readonly ICustomerRepository customerRepository = new CustomerRepository();

        private readonly IOrderDetailRepository orderDetailRepository = new OrderDetailRepository();
        private readonly IFlowerBouquetRepository flowerBouquetRepository = new FlowerBouquetRepository();
        public decimal Total { get; set; }
        public List<OrderDetail> OrderDetail { get;set; }

        public IActionResult OnGet(string id)
        {
            var loginUser = HttpContext.Session.GetObjectFromJson<Customer>("user");

            if (loginUser == null || loginUser.CustomerId == -1) // not login or admin
            {
                return RedirectToPage("../Login/Login");
            }
            
            
            var Customer = customerRepository.GetCustomerById(loginUser.CustomerId);
            if (Customer == null)
            {
                return NotFound();
            }

            var orderId = int.Parse(id);
            OrderDetail = orderDetailRepository.GetOrderDetailByOrderId(orderId);

            foreach (var item in OrderDetail)
            {
                item.FlowerBouquet = flowerBouquetRepository.GetFlowerById(item.FlowerBouquetId);
            }

            Total = OrderDetail.Sum(i => i.UnitPrice * i.Quantity);
            return Page();
        }
    }
}
