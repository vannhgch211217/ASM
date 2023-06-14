namespace ASM.Models
{
    public class CartItem
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public float Total { get { return Quantity * Price; } }
        public string Image { get; set; }
        public CartItem(Product product) 
        {
            ProductId = product.ProductId;
            ProductName = product.ProductName;
            Price = product.Price;
            Quantity = 1;
            Image = product.Image;

        }
    }
}
