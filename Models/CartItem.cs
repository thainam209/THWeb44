namespace ECommerceCart.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; } = null!; // Đảm bảo không bị lỗi null

        public CartItem() { } // Cần có constructor mặc định nếu sử dụng serialization

        public CartItem(Product product, int quantity)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));
            ProductId = product.Id;
            Quantity = quantity;
        }
    }
}
