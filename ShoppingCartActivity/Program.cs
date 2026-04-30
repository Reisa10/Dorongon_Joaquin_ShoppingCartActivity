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

        static string YesNo(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string ans = Console.ReadLine().ToUpper().Trim();
                if (ans == "Y" || ans == "N") return ans;
                Console.WriteLine("Invalid input. Please enter Y or N only.\n");
            }
        }

        static void Main()
        {
            int receiptNo = 0;
            Order[] orderHistory = new Order[10];
            int orderCount = 0;

            Product[] products = new Product[10];
            products[0] = new Product(1, "Apple", 20, 10, "Fruit");
            products[1] = new Product(2, "Banana", 10, 15, "Fruit");
            products[2] = new Product(3, "Milk", 50, 3, "Beverage");
            products[3] = new Product(4, "Bread", 40, 5, "Food");
            products[4] = new Product(5, "Eggs", 7, 30, "Food");
            products[5] = new Product(6, "Asparagus", 20, 35, "Vegetable");
            products[6] = new Product(7, "Malunggay", 5, 12, "Vegetable");
            products[7] = new Product(8, "Takoyaki", 10, 12, "Food");
            products[8] = new Product(9, "Water", 2, 500, "Beverage");
            products[9] = new Product(10, "CHB", 250, 100, "Brick");



            CartItem[] cart = new CartItem[5];
            int cartCount = 0;
            int cartQuantity = 0;

            while (true)
            {
                Console.WriteLine("======== DORONGON'S SARI-SARI STORE ============");
                Console.WriteLine($"{"ID",-3} {"PRODUCT",-13} {"PRICE",-10} {"STOCK",-8} {"CATEGORY",-5}");
                Console.WriteLine("------------------------------------------------");
                DisplayMenu(products);
                Console.WriteLine("================================================");
                Console.WriteLine("\n1. Buy Products\n2. Cart Management\n3. Search\n4. Order History\n5. Exit\n");
                Console.Write("Input your Choice: ");
                string choice = Console.ReadLine();
                Console.Clear();

                // ══════════════════════════════════════════════════════════════
                // 1. BUY PRODUCTS
                // ══════════════════════════════════════════════════════════════
                if (choice == "1")
                {
                    if (cartCount >= cart.Length)
                    {
                        Console.WriteLine("Cart limit exceeded (max 5 products total)\n");
                        Pause();
                        continue;
                    }
                    else if (cartQuantity >= 30)
                    {
                        Console.WriteLine("Cart already full (max 30 items).\n");
                        Pause();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("======== DORONGON'S SARI-SARI STORE ============");
                        Console.WriteLine($"{"ID",-3} {"PRODUCT",-13} {"PRICE",-10} {"STOCK",-8} {"CATEGORY",-5}");
                        Console.WriteLine("------------------------------------------------");
                        DisplayMenu(products);
                        Console.WriteLine("================================================");
                        Console.Write("\nInput product ID: ");

                        if (!int.TryParse(Console.ReadLine(), out int productId))
                        {
                            Console.WriteLine("Invalid product ID.\n");
                            Pause();
                            continue;
                        }

                        if (productId < 1 || productId > products.Length)
                        {
                            Console.WriteLine("Invalid product ID.\n");
                            Pause();
                            continue;
                        }

                        Product selectedProd = products[productId - 1];

                        if (selectedProd.RemainingStock == 0)
                        {
                            Console.WriteLine($"{selectedProd.Name} is out of stock.\n");
                            Pause();
                            continue;
                        }

                        Console.Write("Input quantity: ");
                        if (!int.TryParse(Console.ReadLine(), out int quantity))
                        {
                            Console.WriteLine("Invalid quantity.\n");
                            Pause();
                            continue;
                        }

                        if (quantity <= 0)
                        {
                            Console.WriteLine("Quantity must be greater than 0.\n");
                            Console.Clear();
                            continue;
                        }

                        if (cartQuantity + quantity > 30)
                        {
                            Console.WriteLine("Cart limit exceeded (max 30 items total).\n");
                            Pause();
                            continue;
                        }

                        if (!selectedProd.HasEnoughStock(quantity))
                        {
                            Console.WriteLine($"The stock of {selectedProd.Name} is only {selectedProd.RemainingStock}\n");
                            Pause();
                            continue;
                        }

                        bool found = false;
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
                        Console.WriteLine("\nItem added to cart.\n\n");
                        Pause();
                    }
                }

                // ══════════════════════════════════════════════════════════════
                // 2. CART MANAGEMENT
                // ══════════════════════════════════════════════════════════════
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
                        Console.WriteLine($"{"ID",-3}{"ITEM",-10} {"QTY",-5} {"SUBTOTAL",-10}");
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

                        // ── 1. VIEW CART ──────────────────────────────────────
                        if (cartChoice == "1")
                        {
                            Console.WriteLine("=========== CART ==========");
                            Console.WriteLine($"{"ID",-3}{"ITEM",-10} {"QTY",-5} {"SUBTOTAL",-10}");
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
                                Console.WriteLine($"Discount (10%): ${discount:F2}");
                            }

                            double finalTotal = grandTotal - discount;
                            Console.WriteLine("--------------------------");
                            Console.WriteLine($"\nGrand Total: ${grandTotal:F2}");
                            Console.WriteLine($"Final Total: ${finalTotal:F2}\n");
                            Pause();
                        }

                        // ── 2. UPDATE ITEM QUANTITY ───────────────────────────
                        else if (cartChoice == "2")
                        {
                            Console.WriteLine("=========== UPDATE ITEM ==========");
                            Console.WriteLine($"{"ID",-3}{"ITEM",-10} {"QTY",-5} {"SUBTOTAL",-10}");
                            Console.WriteLine("----------------------------------");
                            for (int i = 0; i < cartCount; i++)
                            {
                                cart[i].DisplayCartItem();
                            }

                            Console.Write("\nEnter Product ID to update: ");
                            if (!int.TryParse(Console.ReadLine(), out int updateId))
                            {
                                Console.WriteLine("Invalid ID.\n"); 
                                Pause(); 
                                continue;
                            }

                            bool foundItem = false;
                            for (int i = 0; i < cartCount; i++)
                            {
                                if (cart[i].Product.Id == updateId)
                                {
                                    foundItem = true;
                                    Console.WriteLine($"\nCurrent quantity of {cart[i].Product.Name}: {cart[i].Quantity}");
                                    Console.Write("\nEnter new quantity: ");
                                    if (!int.TryParse(Console.ReadLine(), out int newQty) || newQty <= 0)
                                    {
                                        Console.WriteLine("Invalid quantity.\n"); 
                                        Pause(); 
                                        break;
                                    }

                                    int diff = newQty - cart[i].Quantity;

                                    if (diff > 0 && !cart[i].Product.HasEnoughStock(diff))
                                    {
                                        Console.WriteLine($"Only {cart[i].Product.RemainingStock} additional unit(s) available.\n");
                                        Pause(); 
                                        break;
                                    }

                                    if (cartQuantity - cart[i].Quantity + newQty > 30)
                                    {
                                        Console.WriteLine("Cannot update - would exceed 30-item cart limit.\n");
                                        Pause(); 
                                        break;
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
                                Console.WriteLine("Product not found in cart.\n"); 
                                Pause();
                            }
                        }

                        // ── 3. REMOVE AN ITEM ─────────────────────────────────
                        else if (cartChoice == "3")
                        {
                            Console.WriteLine("=========== REMOVE ITEM ==========");
                            Console.WriteLine($"{"ID",-3}{"ITEM",-10} {"QTY",-5} {"SUBTOTAL",-10}");
                            Console.WriteLine("----------------------------------");
                            for (int i = 0; i < cartCount; i++)
                            {
                                cart[i].DisplayCartItem();
                            }

                            Console.Write("\nEnter Product ID to remove: ");
                            if (!int.TryParse(Console.ReadLine(), out int removeId))
                            {
                                Console.WriteLine("Invalid ID.\n"); 
                                Pause(); 
                                continue;
                            }

                            bool foundRemove = false;
                            for (int i = 0; i < cartCount; i++)
                            {
                                if (cart[i].Product.Id == removeId)
                                {
                                    foundRemove = true;
                                    cart[i].Product.DeductStock(-cart[i].Quantity); 
                                    cartQuantity -= cart[i].Quantity;

                                    // Shift array left
                                    for (int j = i; j < cartCount - 1; j++)
                                        cart[j] = cart[j + 1];
                                    cart[--cartCount] = null;

                                    Console.WriteLine("Item removed from cart.\n");
                                    Pause(); 
                                    break;
                                }
                            }

                            if (!foundRemove)
                            {
                                Console.WriteLine("Product not found in cart.\n"); 
                                Pause();
                            }
                        }

                        // ── 4. CLEAR CART ─────────────────────────────────────
                        else if (cartChoice == "4")
                        {
                            string confirm = YesNo("Are you sure you want to clear the cart? (Y/N): ");
                            if (confirm == "Y")
                            {
                                for (int i = 0; i < cartCount; i++)
                                {
                                    cart[i].Product.DeductStock(-cart[i].Quantity); // restore stock
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
                                Console.WriteLine("Cancelled.\n"); 
                                Pause();
                            }
                        }

                        // ── 5. CHECKOUT ───────────────────────────────────────
                        else if (cartChoice == "5")
                        {
                            double grandTotal = 0;
                            for (int i = 0; i < cartCount; i++) grandTotal += cart[i].Subtotal;

                            double discount = grandTotal >= 5000 ? grandTotal * 0.10 : 0;
                            double finalTotal = grandTotal - discount;

                            Console.WriteLine("=========== CHECKOUT ==========");
                            Console.WriteLine($"Grand Total:  ${grandTotal:F2}");
                            if (discount > 0)
                            Console.WriteLine($"Discount (10%): ${discount:F2}");
                            Console.WriteLine($"Final Total:  ${finalTotal:F2}\n");

                            string confirm = YesNo("Proceed to checkout? (Y/N): ");
                            Console.Clear();

                            if (confirm == "Y")
                            {
                                // Payment validation loop
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
                                Console.Clear();
                                double change = payment - finalTotal;

                                receiptNo++;
                                DateTime now = DateTime.Now;

                                // Print
                                Console.WriteLine("\n=========== RECEIPT ===========");
                                Console.WriteLine($"Receipt No : {receiptNo:D4}");
                                Console.WriteLine($"Date       : {now:MMMM dd, yyyy h:mm tt}");
                                Console.WriteLine("-------------------------------");
                                Console.WriteLine($"{"ITEM",-10} {"QTY",-5} {"SUBTOTAL",-10}");
                                Console.WriteLine("-------------------------------");
                                for (int i = 0; i < cartCount; i++) cart[i].DisplayCartItem();
                                Console.WriteLine("-------------------------------");
                                Console.WriteLine($"Grand Total:  ${grandTotal:F2}");
                                Console.WriteLine($"Discount:     ${discount:F2}");
                                Console.WriteLine($"Final Total:  ${finalTotal:F2}");
                                Console.WriteLine($"Payment:      ${payment:F2}");
                                Console.WriteLine($"Change:       ${change:F2}");
                                Console.WriteLine("===============================");

                                // Save order to history
                                CartItem[] snapshot = new CartItem[cartCount];
                                for (int i = 0; i < cartCount; i++) snapshot[i] = cart[i];

                                orderHistory[orderCount] = new Order(receiptNo, now, snapshot, cartCount,
                                                                     grandTotal, discount, finalTotal,
                                                                     payment, change);
                                orderCount++;

                                // Low stock alert
                                Console.WriteLine("\nLOW STOCK ALERT:");
                                foreach (Product p in products)
                                {
                                    p.ReorderAlert();
                                }

                                // Reset cart
                                cart = new CartItem[5];
                                cartCount = 0;
                                cartQuantity = 0;
                                Pause();
                                inCart = false;
                            }
                            else
                            {
                                Console.WriteLine("Returning to cart menu.\n");
                            }
                        }

                        // ── 6. BACK TO MAIN MENU ──────────────────────────────
                        else if (cartChoice == "6")
                        {
                            inCart = false;
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice. Please enter 1-6.\n"); Pause();
                        }
                    }
                }

                // ══════════════════════════════════════════════════════════════
                // 3. SEARCH
                // ══════════════════════════════════════════════════════════════
                else if (choice == "3")
                {
                    Console.Clear();
                    Console.Write("Input the Product or Category you want to search: ");
                    string search = Console.ReadLine().ToUpper();
                    Console.WriteLine($"\n{"ID",-3} {"PRODUCT",-13} {"PRICE",-10} {"STOCK",-8} {"CATEGORY",-5}");
                    Console.WriteLine("------------------------------------------------");

                    bool found = false;
                    foreach (Product i in products)
                    {
                        if (i.Name.ToUpper().Contains(search) || i.Category.ToUpper().Contains(search))
                        {
                            i.DisplayProduct();
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        Console.WriteLine($"{search} was not found!");
                    }

                    Pause();
                }

                // ══════════════════════════════════════════════════════════════
                // 4. ORDER HISTORY
                // ══════════════════════════════════════════════════════════════
                else if (choice == "4")
                {
                    Console.Clear();
                    Console.WriteLine("=========== ORDER HISTORY ===========");

                    if (orderCount == 0)
                    {
                        Console.WriteLine("No orders yet.\n");
                        Pause();
                        continue;
                    }

                    for (int i = 0; i < orderCount; i++)
                    {
                        Console.WriteLine($"Receipt #{orderHistory[i].ReceiptNo:D4} - Final Total: ${orderHistory[i].FinalTotal:F2}");
                    }

                    Console.WriteLine("=====================================");
                    string viewDetail = YesNo("View a detailed receipt? (Y/N): ");

                    if (viewDetail == "Y")
                    {
                        Console.Write("Enter receipt number (e.g. 1): ");
                        if (!int.TryParse(Console.ReadLine(), out int viewNo) || viewNo < 1 || viewNo > orderCount)
                        {
                            Console.WriteLine("Invalid receipt number.\n");
                            Pause();
                            continue;
                        }

                        Order o = orderHistory[viewNo - 1];
                        Console.Clear();
                        Console.WriteLine("\n=========== RECEIPT ===========");
                        Console.WriteLine($"Receipt No : {o.ReceiptNo:D4}");
                        Console.WriteLine($"Date       : {o.Date:MMMM dd, yyyy h:mm tt}");
                        Console.WriteLine("-------------------------------");
                        Console.WriteLine($"{"ITEM",-10} {"QTY",-5} {"SUBTOTAL",-10}");
                        Console.WriteLine("-------------------------------");
                        for (int i = 0; i < o.ItemCount; i++) o.Items[i].DisplayCartItem();
                        Console.WriteLine("-------------------------------");
                        Console.WriteLine($"Grand Total:  ${o.GrandTotal:F2}");
                        Console.WriteLine($"Discount:     ${o.Discount:F2}");
                        Console.WriteLine($"Final Total:  ${o.FinalTotal:F2}");
                        Console.WriteLine($"Payment:      ${o.Payment:F2}");
                        Console.WriteLine($"Change:       ${o.Change:F2}");
                        Console.WriteLine("===============================");
                    }

                    Pause();
                }

                // ══════════════════════════════════════════════════════════════
                // 5. EXIT
                // ══════════════════════════════════════════════════════════════
                else if (choice == "5")
                {
                    Console.WriteLine("Goodbye!");
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter 1-5.\n");
                    Pause() ;
                }
            }
        }
    }
}