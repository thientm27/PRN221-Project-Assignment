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
    public class DeleteModel : PageModel
    {
        private readonly IOrderRepository _orderRepository = new OrderRepository();

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

            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            Order = _orderRepository.GetOrderById((int)id);

            if (Order != null)
            {
                _orderRepository.DeleteOrder((int)id);
            }


            return RedirectToPage("./Index");
        }
    }
}