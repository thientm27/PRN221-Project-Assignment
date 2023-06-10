using System.Collections.Generic;
using System.Linq;
using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.ViewModels;
using Repositories;
using Repositories.Implementation;

namespace RazorPage.Pages.ManageOrder
{
    public class OrderDetailModel : PageModel
    {


        private readonly IOrderDetailRepository _orderDetailRepository = new OrderDetailRepository();
        private readonly IFlowerBouquetRepository _flowerBouquetRepository = new FlowerBouquetRepository();
        public decimal Total { get; set; }
        public List<OrderDetail> OrderDetail { get;set; }

        public IActionResult OnGet(string id)
        {
            var loginUser = HttpContext.Session.GetObjectFromJson<Customer>("user");

            if (loginUser == null || loginUser.CustomerId != -1) // not login or not admin
            {
                return RedirectToPage("../Login/Login");
            }
            
            var orderId = int.Parse(id);
            OrderDetail = _orderDetailRepository.GetOrderDetailByOrderId(orderId);

            foreach (var item in OrderDetail)
            {
                item.FlowerBouquet = _flowerBouquetRepository.GetFlowerById(item.FlowerBouquetId);
            }

            Total = OrderDetail.Sum(i => i.UnitPrice * i.Quantity);
            return Page();
        }
    }
}
