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

namespace RazorPage.Pages.ManageOrder
{
    public class DeleteModel : PageModel
    {
        private readonly IOrderRepository _orderRepository = new OrderRepository();

        [BindProperty]
        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
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

        public async Task<IActionResult> OnPostAsync(int? id)
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
