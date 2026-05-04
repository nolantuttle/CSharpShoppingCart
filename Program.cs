using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

// Auto-migrate on startup
using var dbContext = new AppDbContext();
dbContext.Database.Migrate();

// Shared references
Inventory inventory = new Inventory();
User? currentUser = null;
bool isRunning = true;
string? input;

// Seed test inventory
SeedData(inventory);

Console.Clear();

// Main loop
while (isRunning)
{
    if (currentUser is null)
    {
        Console.WriteLine("\n=== Shopping Cart ===");
        Console.WriteLine("1 = Register | 2 = Login | 3 = Credits | 4 = Quit");
        input = Console.ReadLine();
        Console.Clear();
        switch (input)
        {
            case "1":
                // Register flow
                Console.WriteLine("\n=== Registration ===");
                Console.WriteLine("Enter your username:");
                string? username = Console.ReadLine();
                Console.WriteLine("\nEnter your password:");
                string? password = Console.ReadLine();
                bool registered = User.Register(username, password);
                if (registered)
                    Console.WriteLine($"Successfully registered! Please login.");
                else
                    Console.WriteLine("Registration failed.");

                break;
            case "2":
                // Login flow
                Console.WriteLine("\n=== Login ===");
                Console.WriteLine("Enter your username:");
                username = Console.ReadLine();
                Console.WriteLine("\nEnter your password:");
                password = Console.ReadLine();
                currentUser = User.Login(username, password);
                if (currentUser is not null)
                {
                    Console.WriteLine($"\nGood job {currentUser.Username}! You are now logged in!");
                }
                break;
            case "3":
                Console.WriteLine("Written by Nolan Tuttle. MIT License.");
                break;
            case "4":
                isRunning = false;
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
    else
    {
        Console.WriteLine($"\n=== Welcome {currentUser.Username} ===");
        Console.WriteLine("1 = View Products | 2 = View/Edit Cart | 3 = Checkout | 4 = Quit");
        input = Console.ReadLine();
        Console.Clear();
        switch (input)
        {
            case "1": // store menu
                Console.WriteLine("\n=== Store Inventory ===");
                inventory.DisplayInventory();
                Console.WriteLine($"You have {currentUser.Money}!");
                Console.WriteLine("1 = Buy Products | 4 = Quit");
                string? storeInput = Console.ReadLine();
                switch (storeInput)
                {
                    case "1":
                        Console.WriteLine("Enter name of product to purchase:");
                        string? productName = Console.ReadLine();
                        Console.WriteLine("How many would you like to purchase?");
                        if (!int.TryParse(Console.ReadLine(), out int quantity))
                        {
                            Console.WriteLine("Invalid number.");
                            break;
                        }
                        InventoryItem inventoryItem = inventory.GetProduct(productName);
                        currentUser.AddToCart(inventoryItem, quantity);
                        break;
                    case "4":
                        break;
                }
                break;

            case "2": // cart menu
                Console.WriteLine("\n=== Your Cart ===");
                currentUser.cart.DisplayCart();
                Console.WriteLine($"You have {currentUser.Money}!");
                Console.WriteLine("1 = Return Products | 4 = Quit");
                string? cartInput = Console.ReadLine();
                Console.Clear();
                switch (cartInput)
                {
                    case "1":
                        Console.WriteLine("Enter ID of product to return:");
                        if (!int.TryParse(Console.ReadLine(), out int cartItemPk))
                        {
                            Console.WriteLine("Invalid number.");
                            break;
                        }
                        Console.WriteLine("Enter quantity to return:");
                        if (!int.TryParse(Console.ReadLine(), out int returnQuantity))
                        {
                            Console.WriteLine("Invalid number.");
                            break;
                        }
                        if (currentUser.cart.RemoveItem(cartItemPk, returnQuantity, inventory))
                        {
                            Console.WriteLine("Item has been returned and refunded.");
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong refunding that item. Please try again.");
                        }
                        break;

                    case "4":
                        break;
                }
                break;

            case "3": // checkout
                break;

            case "4":   // logout
                currentUser = null;
                break;
        }
    }
}

void SeedData(Inventory inventory)
{
    using var context = new AppDbContext();
    if (context.Products.Any()) return;

    inventory.AddProduct(new Book("The Pragmatic Programmer", 29.99m, 5, "Hunt & Thomas", "Technology"), 10);
    inventory.AddProduct(new Electronics("RTX 4090", 1599.99m, "Nvidia", 110m, "AC", "4090-XT"), 5);
    inventory.AddProduct(new DryGood("Bush's Beans", 3.99m, "Bush's", 0.5m, new DateTime(2030, 1, 1)), 50);
    inventory.AddProduct(new Furniture("Standing Desk", 299.99m, "IKEA", "Wood", 25.0m), 8);

}