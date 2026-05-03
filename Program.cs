using Microsoft.EntityFrameworkCore;

// Auto-migrate on startup
using var dbContext = new AppDbContext();
dbContext.Database.Migrate();

// Shared references
Inventory inventory = new Inventory();
User? currentUser = null;
bool isRunning = true;

// Seed test inventory
SeedData(inventory);
inventory.DisplayInventory();

// Main loop
while (isRunning)
{
    Console.WriteLine("\n=== Shopping Cart ===");
    Console.WriteLine("1 = Register | 2 = Login | 3 = Quit");
    string? input = Console.ReadLine();
    switch (input)
    {
        case "1":
            // Register flow
            Console.WriteLine("\n=== Registration ===");
            Console.WriteLine("Enter your username:");
            string? username = Console.ReadLine();
            Console.WriteLine("\nEnter your password:");
            string? password = Console.ReadLine();
            currentUser = new User(username, password);
            if (currentUser.Register(username, password))
            {
                Console.WriteLine($"\nGood job {currentUser.Username}!");
            }

            break;
        case "2":
            // Login flow
            Console.WriteLine("\n=== Login ===");
            Console.WriteLine("Enter your username:");
            username = Console.ReadLine();
            Console.WriteLine("\nEnter your password:");
            password = Console.ReadLine();
            if (currentUser is null)
            {
                break;
            }
            if (currentUser.Login(username, password))
            {
                Console.WriteLine($"\nGood job {currentUser.Username}!");
            }
            break;
        case "3":
            isRunning = false;
            break;
        default:
            Console.WriteLine("Invalid option.");
            break;
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