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
            Console.WriteLine($"{Id,2} {Name,10} {Price,10} {RemainingStock,5}");
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

            CartItem[] cart = new CartItem[5];
            int cartCount = 0;
            int cartQuantity = 0;   
            while (true)
            {
                Console.WriteLine("======= FOOD STORE MENU =======\n");
                Console.WriteLine($"{"ID",3} {"Product",10} {"Price",10} {"Stock"}");
                DisplayMenu(products);
                Console.WriteLine("===============================");
                Console.WriteLine("\n1. Buy Products\n2. View Cart\n3. Exit\n");
                Console.Write("Input your Choice: ");
                string choice = Console.ReadLine();
                Console.Clear();
                if (choice == "1")
                {
                    if (cartCount >= cart.Length)
                    {
                        Console.WriteLine("Cart is full. Can only bought 5 Products at a time\n");
                    }
                    else if (cartQuantity >= 20)
                    {
                        Console.WriteLine("Cart is full. Can only bought 20 quantity at a time\n");
                    }
                    else
                    {
                        Console.WriteLine($"{"ID",3} {"Product",10} {"Price",10} {"Stock"}");
                        DisplayMenu(products);
                        Console.Write("\nInput product ID: ");
                        int productId = int.Parse(Console.ReadLine());
                        Console.Write("Input quantity: ");
                        int quantity = int.Parse(Console.ReadLine());
                        Console.Clear();

                        Product selectedProd = products[productId - 1];

                        bool found = false;

                        if (cartQuantity + quantity > 20)
                        {
                            Console.WriteLine("Cart limit exceeded (max 20 items total).\n");
                            continue;
                        }
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
                            cartQuantity += quantity;
                            selectedProd.RemainingStock -= quantity;
                            Console.WriteLine("Item added to cart.\n");
                        }
                    }
                }
                else if (choice == "2")
                {
                    Console.WriteLine("=== CART ===\n");

                    if (cartCount == 0 || cartQuantity == 0)
                    {
                        Console.WriteLine("Cart is empty.\n");
                        continue;
                    }

                    double grandTotal = 0;

                    for (int i = 0; i < cartCount; i++)
                    {
                        cart[i].DisplayCartItem();
                        grandTotal += cart[i].Subtotal;
                    }

                    Console.WriteLine($"Grand Total: ${grandTotal}");

                    double discount = 0;

                    if (grandTotal >= 5000)
                    {
                        discount = grandTotal * 0.10;
                        Console.WriteLine($"Discount (10%): ${discount}");
                    }

                    double finalTotal = grandTotal - discount;

                    Console.WriteLine($"Final Total: ${finalTotal}\n");

                    Console.Write("Proceed to checkout? (Y/N): ");
                    string confirm = Console.ReadLine().ToUpper();
                    Console.Clear();

                    if (confirm == "Y")
                    {
                        Console.WriteLine("\n=== RECEIPT ===");

                        for (int i = 0; i < cartCount; i++)
                        {
                            cart[i].DisplayCartItem();
                        }

                        Console.WriteLine($"Grand Total: ${grandTotal:F2}");
                        Console.WriteLine($"Discount: ${discount:F2}");
                        Console.WriteLine($"Final Total: ${finalTotal:F2}");
                        cart = new CartItem[5];
                        cartCount = 0;
                        cartQuantity = 0;

                        Console.WriteLine("\n======= UPDATED FOOD MENU =======\n");
                        Console.WriteLine($"{"ID",3} {"Product",10} {"Price",10} {"Stock"}");
                        DisplayMenu(products);
                        Console.WriteLine("\nPress any key to continue\n");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else if (confirm == "N")
                    {
                        Console.WriteLine("Continue Shopping\n");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Returning to menu.\n");   
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
                    
                }
            }
        }
    }
}