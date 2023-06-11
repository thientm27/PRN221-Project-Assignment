using System;
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
    public class CartModel : PageModel
    {
        public List<CartItem> Cart { get; set; }
        public decimal Total { get; set; }
        [BindProperty]
        public DateTime ShipDate { get; set; }

        private readonly IFlowerBouquetRepository _flowerBouquetRepository = new FlowerBouquetRepository();
        private readonly IOrderRepository _orderRepository = new OrderRepository();
        private readonly IOrderDetailRepository _orderDetailRepository = new OrderDetailRepository();
        public IActionResult OnGet()
        {
            
            var loginUser = HttpContext.Session.GetObjectFromJson<Customer>("user");

            if (loginUser == null || loginUser.CustomerId != -1) // not login or not admin
            {
                return RedirectToPage("../Login/Login");
            }
            
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart");
            if (Cart == null) // Not found old cart
            {
                Cart = new List<CartItem>(); // create new
               
            }
            Total = Cart.Sum(i => i.Item.UnitPrice * i.Quantity);
            return Page();
        }

        public IActionResult OnGetBuyNow(string id)
        {
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart"); // try to get old cart from session
            if (Cart == null) // Not found old cart
            {
                Cart = new List<CartItem>(); // create new
                var flowerItem = _flowerBouquetRepository.GetFlowerById(int.Parse(id));
                Cart.Add(new CartItem
                {
                    Item = flowerItem,
                    Quantity = 1
                });
                HttpContext.Session.SetObjectAsJson("cart", Cart); // set to session
            }
            else // Already have cart
            {
                int index = CheckExist(Cart, int.Parse(id)); // Check if already have item in cart
                if (index == -1) // Not see? add new with quantity 1
                {
                    var flowerItem = _flowerBouquetRepository.GetFlowerById(int.Parse(id));
                    Cart.Add(new CartItem
                    {
                        Item = flowerItem,
                        Quantity = 1
                    });
                }
                else
                {
                    Cart[index].Quantity++; // Already have -> add 1 to cart
                }

                HttpContext.Session.SetObjectAsJson("cart", Cart); // set to session
            }

            return RedirectToPage("Cart"); // Load Page
        }
        public IActionResult OnPostUpdate(int[] quantities)
        {
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart");
            for (var i = 0; i < Cart.Count; i++)
            {
                Cart[i].Quantity = quantities[i];
            }

            HttpContext.Session.SetObjectAsJson("cart", Cart);
            return RedirectToPage("Cart"); // Reload
        }

        public IActionResult OnGetDelete(string id)
        {
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart");
            int index = CheckExist(Cart, int.Parse(id));
            Cart.RemoveAt(index);
            HttpContext.Session.SetObjectAsJson("cart", Cart);
            return RedirectToPage("Cart");
        }
        
        public IActionResult OnPostCheckout()
        {
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart");
            if (Cart == null || Cart.Count == 0)
            {
                ModelState.AddModelError(String.Empty, "You do not have any item");
                return Page();
            }
            // Validation quantiy
            foreach (var orderDetail in Cart)
            {
                // Check Quantity
                var currentFlower = _flowerBouquetRepository.GetFlowerById(orderDetail.Item.FlowerBouquetId);
                if (currentFlower.UnitsInStock < orderDetail.Quantity)
                {
                    ModelState.AddModelError(String.Empty, currentFlower.FlowerBouquetName + " Only have "+ currentFlower.UnitsInStock+ " left!");
                    return Page();
                }
            }
            
            return RedirectToPage("./Create"); // done validation
            
           
        }
        
        private int CheckExist(List<CartItem> cart, int id)
        {
            for (var i = 0; i < cart.Count; i++)
            {
                if (cart[i].Item.FlowerBouquetId == id)
                {
                    return i;
                }
            }

            return -1;
        }

    }
}
