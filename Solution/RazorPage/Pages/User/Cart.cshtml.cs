using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.ViewModels;
using Repositories.Implementation;
using Repositories;
using System.Collections.Generic;
using System.Linq;

namespace RazorPage.Pages.User
{
    public class CartModel : PageModel
    {
        public List<CartItem> Cart { get; set; }
        public decimal Total { get; set; }
        

        private readonly IFlowerBouquetRepository flowerBouquetRepository = new FlowerBouquetRepository();
        public void OnGet()
        {
            Cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (Cart == null) // Not found old cart
            {
                Cart = new List<CartItem>(); // create new
               
            }
            Total = Cart.Sum(i => i.Item.UnitPrice * i.Quantity);
        }

        public IActionResult OnGetBuyNow(string id)
        {
            Cart = new List<CartItem>(); // set up list product
            Cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("cart"); // try to get old cart from session
            if (Cart == null) // Not found old cart
            {
                Cart = new List<CartItem>(); // create new
                var flowerItem = flowerBouquetRepository.GetFlowerById(int.Parse(id));
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
                    var flowerItem = flowerBouquetRepository.GetFlowerById(int.Parse(id));
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
            Cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int index = CheckExist(Cart, int.Parse(id));
            Cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", Cart);
            return RedirectToPage("Cart");
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
