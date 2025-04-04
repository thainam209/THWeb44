using ECommerceCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceCart.Controllers
{
    public class CartController : Controller
    {
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1000, Description = "High-performance laptop" },
            new Product { Id = 2, Name = "Phone", Price = 500, Description = "Smartphone with high resolution" },
            new Product { Id = 3, Name = "Ipad", Price = 800, Description = "Portable tablet with touchscreen" },
            new Product { Id = 4, Name = "Headphones", Price = 200, Description = "Noise-cancelling headphones" },
            new Product { Id = 5, Name = "Smartwatch", Price = 300, Description = "Wearable smartwatch with fitness tracking" }
        };

        // Lấy danh sách giỏ hàng từ Session
        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return cartJson == null ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
        }

        // Lưu giỏ hàng vào Session
        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }

        // Hiển thị danh sách sản phẩm
        public IActionResult Index()
        {
            return View(_products);
        }

        // Thêm sản phẩm vào giỏ hàng
        public IActionResult AddToCart(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                var cart = GetCart();
                var cartItem = cart.FirstOrDefault(c => c.ProductId == id);
                if (cartItem != null)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { ProductId = id, Quantity = 1, Product = product });
                }
                SaveCart(cart);
            }
            return RedirectToAction("ViewCart");
        }

        // Xem giỏ hàng
        public IActionResult ViewCart()
        {
            return View(GetCart());
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        public IActionResult UpdateCart(int id, int quantity)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == id);
            if (cartItem != null)
            {
                if (quantity > 0)
                {
                    cartItem.Quantity = quantity;
                }
                else
                {
                    cart.Remove(cartItem);
                }
                SaveCart(cart);
            }
            return RedirectToAction("ViewCart");
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == id);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SaveCart(cart);
            }
            return RedirectToAction("ViewCart");
        }

        // Trả về tổng tiền của giỏ hàng
        public decimal GetTotal()
        {
            var cart = GetCart();
            return cart.Sum(c => c.Quantity * c.Product.Price);
        }
    }
}
