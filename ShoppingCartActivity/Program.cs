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
            Console.WriteLine($"{Id,-3} {Name,-10} {Price,8:F2} {RemainingStock,8}");
        }
        public bool HasEnoughStock(int quantity)
        {
            return RemainingStock >= quantity;
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
                Console.WriteLine("=========== FOOD STORE ==========");
                Console.WriteLine($"{"ID",-3} {"PRODUCT",-13} {"PRICE",-10} {"STOCK",-8}");
                Console.WriteLine("---------------------------------");
                DisplayMenu(products);
                Console.WriteLine("=================================");
                Console.WriteLine("\n1. Buy Products\n2. View Cart\n3. Exit\n");
                Console.Write("Input your Choice: ");
                string choice = Console.ReadLine();
                Console.Clear();
                if (choice == "1")
                {
                    if (cartCount >= cart.Length)
                    {
                        Console.WriteLine("Cart limit exceeded (max 5 products total)\n");
                        continue;
                    }
                    else if (cartQuantity >= 20)
                    {
                        Console.WriteLine("Cart already full (max 20 items).\n");
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("=========== FOOD STORE ==========");
                        Console.WriteLine($"{"ID",-3} {"PRODUCT",-13} {"PRICE",-10} {"STOCK",-8}");
                        Console.WriteLine("---------------------------------");
                        DisplayMenu(products);
                        Console.WriteLine("=================================");
                        Console.Write("\nInput product ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int productId))
                        {
                            Console.WriteLine("Invalid product ID.\n");
                            continue;
                        }

                        if (productId < 1 || productId > products.Length)
                        {
                            Console.WriteLine("Invalid product ID.\n");
                            continue;
                        }

                        Product selectedProd = products[productId - 1];

                        if (selectedProd.RemainingStock == 0)
                        {
                            Console.WriteLine("This product is out of stock.\n");
                            continue;
                        }

                        Console.Write("Input quantity: ");
                        if (!int.TryParse(Console.ReadLine(), out int quantity))
                        {
                            Console.WriteLine("Invalid quantity.\n");
                            continue;
                        }

                        if (quantity <= 0)
                        {
                            Console.WriteLine("Quantity must be greater than 0.\n");
                            continue;
                        }
                        Console.Clear();



                        bool found = false;

                        if (cartQuantity + quantity > 20)
                        {
                            Console.WriteLine("Cart limit exceeded (max 20 items total).\n");
                            continue;
                        }
                        if (!selectedProd.HasEnoughStock(quantity))
                        {
                            Console.WriteLine($"The stock of {selectedProd.Name} is only {selectedProd.RemainingStock}\n");
                            continue;
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
                    Console.WriteLine("=========== CART ==========");
                    Console.WriteLine($"{"ITEM",-10} {"QTY",-5} {"SUBTOTAL",-10}");
                    Console.WriteLine("--------------------------");
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

                    Console.WriteLine($"\nGrand Total: ${grandTotal}");

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
                        Console.WriteLine("\n=========== RECEIPT ===========");
                        Console.WriteLine($"{"ITEM",-10} {"QTY",-5} {"SUBTOTAL",-10}");

                        for (int i = 0; i < cartCount; i++)
                        {
                            cart[i].DisplayCartItem();
                        }

                        Console.WriteLine("------------------------------");
                        Console.WriteLine($"Grand Total:     ${grandTotal:F2}");
                        Console.WriteLine($"Discount:        ${discount:F2}");
                        Console.WriteLine($"Final Total:     ${finalTotal:F2}");
                        Console.WriteLine("===============================");
                        cart = new CartItem[5];
                        cartCount = 0;
                        cartQuantity = 0;

                        Console.WriteLine("\n\n======= UPDATED FOOD MENU =======");
                        Console.WriteLine($"{"ID",-3} {"Product",-13} {"Price",-10} {"Stock"}");
                        Console.WriteLine("---------------------------------");
                        DisplayMenu(products);
                        Console.WriteLine("=================================");
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