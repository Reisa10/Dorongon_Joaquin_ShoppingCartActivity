using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartActivity
{
    class Product
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }

        public int RemainingStock { get; set; }

        public Product(int id, string name, double price, int remainingStock, string category)
        {
            Id = id;
            Name = name;
            Price = price;
            RemainingStock = remainingStock;
            Category = category;
        }
        public void DisplayProduct()
        {
            Console.WriteLine($"{Id,-3} {Name,-10} {Price,8:F2} {RemainingStock,8}");
        }
        public bool HasEnoughStock(int quantity)
        {
            return RemainingStock >= quantity;
        }
        public void DeductStock(int quantity)
        {
            RemainingStock -= quantity;
        }
        public void ReorderAlert() 
        {
            Console.WriteLine("LOW STOCK ALERT");

            RemainingStock -= 5;
            
        }
    }



    class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double Subtotal { get; set; }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
            Subtotal = product.Price * quantity;
        }

        public void Update(int quantity)
        {
            Quantity += quantity;
            Subtotal = Product.Price * Quantity;
        }
        public void DisplayCartItem()
        {
            Console.WriteLine($"{Product.Name,-10} {Quantity,-5} ${Subtotal:F2}");
        }        
    }
}
