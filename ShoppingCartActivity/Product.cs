using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartActivity
{
    class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Category { get; private set; }
        public double Price { get; private set; }
        public int RemainingStock { get; private set; }

        public Product(int id, string name, double price, int remainingStock, string category)
        {
            if (price < 0) throw new ArgumentException("Price cannot be negative.");
            if (remainingStock < 0) throw new ArgumentException("Stock cannot be negative.");

            Id = id;
            Name = name;
            Price = price;
            RemainingStock = remainingStock;
            Category = category;
        }

        public void DisplayProduct()
        {
            Console.WriteLine($"{Id,-3} {Name,-10} {Price,8:F2} {RemainingStock,8} {Category,10}");
        }

        public bool HasEnoughStock(int quantity)
        {
            return RemainingStock >= quantity;
        }

        public void DeductStock(int quantity)
        {
            if (RemainingStock - quantity < 0)
                throw new InvalidOperationException($"Not enough stock for {Name}.");
            RemainingStock -= quantity;
        }

        public void ReorderAlert()
        {
            if (RemainingStock <= 5)
                Console.WriteLine($"{Name} has only {RemainingStock} stock(s) left.");
        }
        public void RestoreStock(int quantity)
        {
            if (quantity < 0) throw new ArgumentException("Quantity must be positive.");
            RemainingStock += quantity;
        }
    }



    class CartItem
    {
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public double Subtotal { get; private set; }

        public CartItem(Product product, int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than 0.");
            Product = product;
            Quantity = quantity;
            Subtotal = product.Price * quantity;
        }

        public void Update(int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity to add must be greater than 0.");
            Quantity += quantity;
            Subtotal = Product.Price * Quantity;
        }

        public void SetQuantity(int newQty)
        {
            if (newQty <= 0) throw new ArgumentException("Quantity must be greater than 0.");
            Quantity = newQty;
            Subtotal = Product.Price * Quantity;
        }

        public void DisplayCartItem()
        {
            Console.WriteLine($"{Product.Id,-3}{Product.Name,-10} {Quantity,-5} ${Subtotal:F2}");
        }
    }
    class Order
    {
        public int ReceiptNo { get; private set; }
        public DateTime Date { get; private set; }
        public CartItem[] Items { get; private set; }
        public int ItemCount { get; private set; }
        public double GrandTotal { get; private set; }
        public double Discount { get; private set; }
        public double FinalTotal { get; private set; }
        public double Payment { get; private set; }
        public double Change { get; private set; }

        public Order(int receiptNo, DateTime date, CartItem[] items, int itemCount,
                     double grandTotal, double discount, double finalTotal,
                     double payment, double change)
        {
            if (grandTotal < 0) throw new ArgumentException("Grand total cannot be negative.");
            if (discount < 0) throw new ArgumentException("Discount cannot be negative.");
            if (payment < finalTotal) throw new ArgumentException("Payment cannot be less than final total.");

            ReceiptNo = receiptNo;
            Date = date;
            Items = items;
            ItemCount = itemCount;
            GrandTotal = grandTotal;
            Discount = discount;
            FinalTotal = finalTotal;
            Payment = payment;
            Change = change;
        }
    }
}