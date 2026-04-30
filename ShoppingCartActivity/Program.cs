using ShoppingCartActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartActivity
{
    class Program
    {
        static void DisplayMenu(Product[] products)
        {
            foreach (Product i in products)
            {
                i.DisplayProduct();
            }
        }
        public static void Pause()
        {
            Console.WriteLine("\nPress any key to continue\n");
            Console.ReadKey();
            Console.Clear();
        }

        static void Main()
        {

            Product[] products = new Product[5];

            products[0] = new Product(1, "Apple", 20, 10, "Food");
            products[1] = new Product(2, "Banana", 10, 15, "Food");
            products[2] = new Product(3, "Milk", 50, 0, "Food");
            products[3] = new Product(4, "Bread", 40, 5, "Food");
            products[4] = new Product(5, "Eggs", 7, 30, "Food");

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
                Console.WriteLine("\n1. Buy Products\n2. View Cart\n3. Search\n4. Exit\n");
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
                            selectedProd.DeductStock(quantity);
                            Console.WriteLine("Item added to cart.\n");
                        }
                    }
                }
                else if (choice == "2")
                {
                    if (cartCount == 0)
                    {
                        Console.WriteLine("Cart is empty.\n");
                        Pause();
                        continue;
                    }

                    bool inCart = true;
                    while (inCart)
                    {
                        Console.Clear();
                        Console.WriteLine("=========== CART MANAGEMENT ==========");
                        Console.WriteLine($"{"ITEM",-10} {"QTY",-5} {"SUBTOTAL",-10}");
                        Console.WriteLine("--------------------------------------");
                        for (int i = 0; i < cartCount; i++)
                        {
                            cart[i].DisplayCartItem();
                        }
                        Console.WriteLine("======================================");
                        Console.WriteLine("\n1. View Cart");
                        Console.WriteLine("2. Update Item Quantity");
                        Console.WriteLine("3. Remove an Item");
                        Console.WriteLine("4. Clear Cart");
                        Console.WriteLine("5. Checkout");
                        Console.WriteLine("6. Back to Main Menu");
                        Console.Write("\nInput your Choice: ");
                        string cartChoice = Console.ReadLine();
                        Console.Clear();

                        if (cartChoice == "1")
                        {
                            Console.WriteLine("=========== CART ==========");
                            Console.WriteLine($"{"ITEM",-10} {"QTY",-5} {"SUBTOTAL",-10}");
                            Console.WriteLine("--------------------------");

                            double grandTotal = 0;
                            for (int i = 0; i < cartCount; i++)
                            {
                                cart[i].DisplayCartItem();
                                grandTotal += cart[i].Subtotal;
                            }

                            double discount = 0;
                            if (grandTotal >= 5000)
                            {
                                discount = grandTotal * 0.10;
                                Console.WriteLine($"Discount (10%): ${discount}");
                            }

                            double finalTotal = grandTotal - discount;
                            Console.WriteLine($"\nGrand Total: ${grandTotal}");
                            Console.WriteLine($"Final Total: ${finalTotal}\n");
                            Pause();
                        }
                        else if (cartChoice == "2")
                        {
                            Console.WriteLine("=========== UPDATE ITEM ==========");
                            Console.WriteLine($"{"ITEM",-10} {"QTY",-5} {"SUBTOTAL",-10}");
                            Console.WriteLine("----------------------------------");
                            for (int i = 0; i < cartCount; i++)
                            {
                                cart[i].DisplayCartItem();
                            }

                            Console.Write("\nEnter Product ID to update: ");
                            if (!int.TryParse(Console.ReadLine(), out int updateId))
                            {
                                Console.WriteLine("Invalid ID.\n"); Pause(); continue;
                            }

                            bool foundItem = false;
                            for (int i = 0; i < cartCount; i++)
                            {
                                if (cart[i].Product.Id == updateId)
                                {
                                    foundItem = true;
                                    Console.WriteLine($"Current quantity of {cart[i].Product.Name}: {cart[i].Quantity}");
                                    Console.Write("Enter new quantity: ");
                                    if (!int.TryParse(Console.ReadLine(), out int newQty) || newQty <= 0)
                                    {
                                        Console.WriteLine("Invalid quantity.\n"); Pause(); break;
                                    }

                                    int diff = newQty - cart[i].Quantity;

                                    if (diff > 0 && !cart[i].Product.HasEnoughStock(diff))
                                    {
                                        Console.WriteLine($"Only {cart[i].Product.RemainingStock} additional unit(s) available.\n");
                                        Pause(); break;
                                    }

                                    if (cartQuantity - cart[i].Quantity + newQty > 20)
                                    {
                                        Console.WriteLine("Cannot update – would exceed 20-item cart limit.\n");
                                        Pause(); break;
                                    }

                                    cart[i].Product.DeductStock(diff);
                                    cartQuantity += diff;
                                    cart[i].SetQuantity(newQty);
                                    Console.WriteLine($"\n{cart[i].Product.Name} quantity updated to {newQty}.\n");
                                    Pause();
                                    break;
                                }
                            }
                            if (!foundItem)
                            {
                                Console.WriteLine("Product not found in cart.\n"); Pause();
                            }
                        }
                        else if (cartChoice == "3")
                        {
                            Console.WriteLine("=========== REMOVE ITEM ==========");
                            Console.WriteLine($"{"ITEM",-10} {"QTY",-5} {"SUBTOTAL",-10}");
                            Console.WriteLine("----------------------------------");
                            for (int i = 0; i < cartCount; i++)
                            {
                                cart[i].DisplayCartItem();
                            }

                            Console.Write("\nEnter Product ID to remove: ");
                            if (!int.TryParse(Console.ReadLine(), out int removeId))
                            {
                                Console.WriteLine("Invalid ID.\n"); Pause(); continue;
                            }

                            bool foundRemove = false;
                            for (int i = 0; i < cartCount; i++)
                            {
                                if (cart[i].Product.Id == removeId)
                                {
                                    foundRemove = true;
                                    cart[i].Product.DeductStock(-cart[i].Quantity); 
                                    cartQuantity -= cart[i].Quantity;

                                    for (int j = i; j < cartCount - 1; j++)
                                        cart[j] = cart[j + 1];
                                    cart[--cartCount] = null;

                                    Console.WriteLine("Item removed from cart.\n");
                                    Pause(); break;
                                }
                            }
                            if (!foundRemove)
                            {
                                Console.WriteLine("Product not found in cart.\n"); Pause();
                            }
                        }
                        else if (cartChoice == "4")
                        {
                            Console.Write("Are you sure you want to clear the cart? (Y/N): ");
                            string confirm = Console.ReadLine().ToUpper();
                            if (confirm == "Y")
                            {
                                for (int i = 0; i < cartCount; i++)
                                {
                                    cart[i].Product.DeductStock(-cart[i].Quantity); 
                                    cart[i] = null;
                                }
                                cartCount = 0;
                                cartQuantity = 0;
                                Console.WriteLine("Cart cleared.\n");
                                Pause();
                                inCart = false;
                            }
                            else
                            {
                                Console.WriteLine("Cancelled.\n"); Pause();
                            }
                        }
                        else if (cartChoice == "5")
                        {
                            double grandTotal = 0;
                            for (int i = 0; i < cartCount; i++) grandTotal += cart[i].Subtotal;

                            double discount = grandTotal >= 5000 ? grandTotal * 0.10 : 0;
                            double finalTotal = grandTotal - discount;

                            Console.WriteLine("=========== CHECKOUT ==========");
                            Console.WriteLine($"Grand Total: ${grandTotal:F2}");
                            if (discount > 0)
                                Console.WriteLine($"Discount (10%): ${discount:F2}");
                            Console.WriteLine($"Final Total: ${finalTotal:F2}\n");

                            Console.Write("Proceed to checkout? (Y/N): ");
                            string confirm = Console.ReadLine().ToUpper();
                            Console.Clear();

                            if (confirm == "Y")
                            {
                                double payment = 0;
                                while (true)
                                {
                                    Console.Write("Enter payment: $");
                                    string rawPayment = Console.ReadLine();

                                    if (!double.TryParse(rawPayment, out payment) || payment <= 0)
                                    {
                                        Console.WriteLine("Invalid input. Please enter a valid amount.\n");
                                        continue;
                                    }

                                    if (payment < finalTotal)
                                    {
                                        Console.WriteLine($"Insufficient payment. Minimum required: ${finalTotal:F2}\n");
                                        continue;
                                    }

                                    break; 
                                }

                                double change = payment - finalTotal;

                                Console.WriteLine("\n=========== RECEIPT ===========");
                                Console.WriteLine($"{"ITEM",-10} {"QTY",-5} {"SUBTOTAL",-10}");
                                for (int i = 0; i < cartCount; i++) cart[i].DisplayCartItem();
                                Console.WriteLine("------------------------------");
                                Console.WriteLine($"Grand Total:  ${grandTotal:F2}");
                                Console.WriteLine($"Discount:     ${discount:F2}");
                                Console.WriteLine($"Final Total:  ${finalTotal:F2}");
                                Console.WriteLine($"Payment:      ${payment:F2}");
                                Console.WriteLine($"Change:       ${change:F2}");
                                Console.WriteLine("===============================");

                                Console.WriteLine("\nLOW STOCK ALERT:");
                                foreach (Product p in products)
                                {
                                    p.ReorderAlert();
                                }

                                cart = new CartItem[5];
                                cartCount = 0;
                                cartQuantity = 0;
                                Pause();
                                inCart = false;
                            }
                            else if (confirm == "N")
                            {
                                Console.WriteLine("Returning to cart menu.\n");
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Returning to cart menu.\n"); 
                                Pause();
                            }
                        }
                        else if (cartChoice == "6")
                        {
                            inCart = false;
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice.\n"); 
                            Pause();
                        }
                    }
                }
            }
                else if (choice == "3")
                {
                    Console.Clear();
                    Console.Write("Input the Product or Category you want to search: ");
                    string search = Console.ReadLine().ToUpper();
                    Console.WriteLine($"{"ID",-3} {"Product",-13} {"Price",-10} {"Stock"}");
                    bool found = false;
                    foreach (Product i in products)
                    {

                        if (i.Name.ToUpper() == search || i.Category.ToUpper() == search)
                        {
                            
                            i.DisplayProduct();                            
                        }
                        if (!found)
                        {
                            Console.WriteLine($"{search} was not found!");
                            Pause();                            
                        }
                        
                    }
                    Pause();

                }
                else if (choice == "4")
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