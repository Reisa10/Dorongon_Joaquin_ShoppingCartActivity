using ShoppingCartActivity;
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
        public double Price { get; set; }

        public int RemainingStock { get; set; }

        public Product(int id, string name, double price, int remainingStock)
        {
            Id = id;
            Name = name;
            Price = price;
            RemainingStock = remainingStock;
        }
        public void DisplayProduct()
        {
            Console.WriteLine($"ID: {Id}, Name: {Name}, Price: ${Price}, Stock: {RemainingStock}");
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
            Console.WriteLine($"Product: {Product.Name}, Quantity: {Quantity}, Subtotal: ${Subtotal}\n");
        }
    }

    class Program
    {
        static void DisplayMenu(Product[] products)
        {
            foreach (Product i in products)
            {
                i.DisplayProduct();
            }
        }
        static void Main()
        {

            Product[] products = new Product[5];

            products[0] = new Product(1, "Apple", 20, 10);
            products[1] = new Product(2, "Banana", 10, 15);
            products[2] = new Product(3, "Milk", 50, 0);
            products[3] = new Product(4, "Bread", 40, 5);
            products[4] = new Product(5, "Eggs", 7, 30);

            CartItem[] cart = new CartItem[10];
            int cartCount = 0;

            while (true)
            {
                Console.WriteLine("=== FOOD STORE MENU ===\n");
                DisplayMenu(products);
                Console.WriteLine("==================");
                Console.WriteLine("\n1. Buy Products\n2. View Cart\n3. Exit\n");
                // Cart count is patrick at the moment
                Console.WriteLine($"cartcount: {cartCount}");
                Console.Write("Input your Choice: ");
                string choice = Console.ReadLine();
                Console.WriteLine();
                if (choice == "1")
                {
                    if (cartCount >= cart.Length)
                    {
                        Console.WriteLine("Cart is full.\n");

                    }
                    else
                    {
                        DisplayMenu(products);
                        Console.Write("\nInput product ID: ");
                        int productId = int.Parse(Console.ReadLine());
                        Console.Write("Input quantity: ");
                        int quantity = int.Parse(Console.ReadLine());

                        Product selectedProd = products[productId - 1];

                        bool found = false;

                        if (quantity > selectedProd.RemainingStock)
                        {
                            Console.WriteLine($"The stock of {selectedProd.Name} is only {selectedProd.RemainingStock}\n");
                        }
                        else
                        {
                            for (int i = 0; i < cartCount; i++)
                            {
                                if (cart[i].Product.Id == productId)
                                {
                                    cart[i].Update(quantity);
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                cart[cartCount] = new CartItem(selectedProd, quantity);
                                cartCount++;
                            }
                            selectedProd.RemainingStock -= quantity;
                            Console.WriteLine("Item added to cart.\n");
                        }
                    }
                }
                else if (choice == "2")
                {
                    Console.WriteLine("View cart selected\n");
                    for (int i = 0; i < cartCount; i++)
                    {
                        cart[i].DisplayCartItem();
                    }
                }
                else if (choice == "3")
                {
                    Console.WriteLine("Exit");
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid choice");
                    return;
                }
            }
        }
        public void checkOut()
        {
            Console.WriteLine("Do you want to proceed to check out? (Y/N): ");
            string choice = Console.ReadLine().ToUpper();
            if (choice == "Y")
            {

            }
            else if (choice == "N")
            {

            }
            else
            {
                Console.WriteLine("Invalid Input");
                return;
            }
        }
    }
}