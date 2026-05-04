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
User.SeedAdmin();

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
        string adminLabel = currentUser.IsAdmin ? "Admin " : "";
        Console.WriteLine($"\n=== Welcome {adminLabel}{currentUser.Username} ===");
        Console.WriteLine("1 = View Products | 2 = View/Edit Cart | 3 = Checkout | 4 = Quit" + (currentUser.IsAdmin ? " | 5 = Admin Panel" : ""));
        input = Console.ReadLine();
        Console.Clear();
        switch (input)
        {
            case "1": // store menu
                Console.WriteLine("\n=== Store Inventory ===");
                inventory.DisplayInventory();
                Console.WriteLine($"\nYou have {currentUser.Money:C}!");
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
                Console.WriteLine($"\nYou have {currentUser.Money:C}!");
                Console.WriteLine("1 = Return Products | 2 = Clear Cart | 4 = Quit");
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

                    case "2":
                        if (currentUser.cart.ReturnAll(inventory))
                            Console.WriteLine("Cart cleared, all items returned to inventory.");
                        else
                            Console.WriteLine("Something went wrong clearing the cart. Please try again.");
                        break;

                    case "4":
                        break;
                }
                break;

            case "3": // checkout
                Console.WriteLine("\n=== Checkout ===");
                currentUser.cart.DisplayCart();
                decimal subtotal = currentUser.cart.GetSubtotal();
                Console.WriteLine($"\nSubtotal: {subtotal:C}");
                Console.WriteLine($"Your balance: {currentUser.Money:C}");
                if (currentUser.Money < subtotal)
                {
                    Console.WriteLine("Insufficient funds.");
                    break;
                }
                Console.WriteLine("Confirm purchase? (y/n)");
                string? confirmInput = Console.ReadLine();
                if (confirmInput?.ToLower() != "y") break;
                if (currentUser.Checkout())
                    Console.WriteLine("Purchase successful!");
                else
                    Console.WriteLine("Checkout failed. Please try again.");
                break;

            case "4":   // logout
                currentUser = null;
                break;

            case "5" when currentUser.IsAdmin: // admin panel
                Console.WriteLine("\n=== Admin Panel ===");
                Console.WriteLine("1 = Add Product | 2 = Remove Product | 3 = Edit User | 4 = Back");
                string? adminInput = Console.ReadLine();
                Console.Clear();
                switch (adminInput)
                {
                    case "1":
                        Console.WriteLine("Product type (Book/Electronics/DryGood/Furniture):");
                        string? productType = Console.ReadLine();
                        Console.WriteLine("Name:");
                        string? newProductName = Console.ReadLine();
                        Console.WriteLine("Price:");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal newPrice)) { Console.WriteLine("Invalid price."); break; }
                        Console.WriteLine("Quantity:");
                        if (!int.TryParse(Console.ReadLine(), out int newQuantity)) { Console.WriteLine("Invalid quantity."); break; }
                        Product? newProduct = null;
                        switch (productType?.ToLower())
                        {
                            case "book":
                                Console.WriteLine("Author:");
                                string? author = Console.ReadLine();
                                Console.WriteLine("Genre:");
                                string? genre = Console.ReadLine();
                                Console.WriteLine("Rating (1-5):");
                                if (!int.TryParse(Console.ReadLine(), out int rating)) { Console.WriteLine("Invalid rating."); break; }
                                newProduct = new Book(newProductName, newPrice, rating, author, genre);
                                break;
                            case "electronics":
                                Console.WriteLine("Brand:");
                                string? brand = Console.ReadLine();
                                Console.WriteLine("Voltage (V):");
                                if (!decimal.TryParse(Console.ReadLine(), out decimal voltage)) { Console.WriteLine("Invalid voltage."); break; }
                                Console.WriteLine("Voltage type (AC/DC):");
                                string? voltageType = Console.ReadLine();
                                Console.WriteLine("Model number:");
                                string? modelNumber = Console.ReadLine();
                                newProduct = new Electronics(newProductName, newPrice, brand, voltage, voltageType, modelNumber);
                                break;
                            case "drygood":
                                Console.WriteLine("Brand:");
                                brand = Console.ReadLine();
                                Console.WriteLine("Weight (kg):");
                                if (!decimal.TryParse(Console.ReadLine(), out decimal weight)) { Console.WriteLine("Invalid weight."); break; }
                                Console.WriteLine("Expiry date (yyyy-mm-dd):");
                                if (!DateTime.TryParse(Console.ReadLine(), out DateTime expiry)) { Console.WriteLine("Invalid date."); break; }
                                newProduct = new DryGood(newProductName, newPrice, brand, weight, expiry);
                                break;
                            case "furniture":
                                Console.WriteLine("Brand:");
                                brand = Console.ReadLine();
                                Console.WriteLine("Material:");
                                string? material = Console.ReadLine();
                                Console.WriteLine("Weight (kg):");
                                if (!decimal.TryParse(Console.ReadLine(), out decimal furnitureWeight)) { Console.WriteLine("Invalid weight."); break; }
                                newProduct = new Furniture(newProductName, newPrice, brand, material, furnitureWeight);
                                break;
                            default:
                                Console.WriteLine("Invalid product type.");
                                break;
                        }
                        if (newProduct != null)
                        {
                            if (inventory.AddProduct(newProduct, newQuantity))
                                Console.WriteLine("Product added.");
                            else
                                Console.WriteLine("Failed to add product. Name may already exist.");
                        }
                        break;

                    case "2":
                        Console.WriteLine("Enter product name to remove:");
                        string? removeProductName = Console.ReadLine();
                        InventoryItem? itemToRemove = inventory.GetProduct(removeProductName);
                        if (itemToRemove is null) { Console.WriteLine("Product not found."); break; }
                        inventory.RemoveProduct(itemToRemove);
                        Console.WriteLine("Product removed.");
                        break;

                    case "3":
                        Console.WriteLine("Enter username:");
                        string? targetUsername = Console.ReadLine();
                        Console.WriteLine("1 = Edit Balance | 2 = Edit Admin Status | 3 = Delete User | 4 = Back");
                        string? editUserInput = Console.ReadLine();
                        switch (editUserInput)
                        {
                            case "1":
                                Console.WriteLine("Enter new balance:");
                                if (!decimal.TryParse(Console.ReadLine(), out decimal newBalance)) { Console.WriteLine("Invalid amount."); break; }
                                if (User.SetBalance(currentUser, targetUsername, newBalance))
                                    Console.WriteLine("Balance updated.");
                                else
                                    Console.WriteLine("Failed. User not found or invalid amount.");
                                break;

                            case "2":
                                Console.WriteLine("Grant or revoke? (grant/revoke):");
                                string? adminAction = Console.ReadLine();
                                bool grantValue = adminAction?.ToLower() == "grant";
                                if (User.SetAdminStatus(currentUser, targetUsername, grantValue))
                                    Console.WriteLine("Admin status updated.");
                                else
                                    Console.WriteLine("Failed. User not found.");
                                break;

                            case "3":
                                if (User.DeleteUser(currentUser, targetUsername))
                                    Console.WriteLine("User deleted.");
                                else
                                    Console.WriteLine("Failed. User not found.");
                                break;

                            case "4":
                                break;
                        }
                        break;

                    case "4":
                        break;
                }
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