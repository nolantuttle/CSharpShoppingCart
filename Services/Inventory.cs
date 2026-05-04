using Microsoft.EntityFrameworkCore;
class Inventory
{

    public Inventory() { }

    /// <summary>
    /// Admin-facing method for adding a new product to the store's inventory.
    /// </summary>
    /// <param name="product">Product to add to store inventory</param>
    /// <returns></returns>
    public bool AddProduct(Product product, int quantity)
    {
        using var context = new AppDbContext();
        if (quantity <= 0)
        {
            return false;
        }
        if (context.Products.Any(p => p.Name == product.Name))  // Name guard, no duplicate product names
        {
            return false;
        }
        context.Products.Add(product);  // Adds to primary product table
        context.SaveChanges();  // IMPORTANT: save changes before modifying new db context in RestoreStock()
        if (RestoreStock(context, product, quantity))    // Adds product to InventoryItems
        {
            return true;
        }
        return false;
    }

    public void RemoveProduct(InventoryItem inventoryItem)
    {
        using var context = new AppDbContext();
        var remove = context.InventoryItems.Find(inventoryItem.Id);
        if (remove != null)
        {
            var product = context.Products.Find(inventoryItem.Product.Id);
            if (product is not null)
            {
                context.InventoryItems.Remove(remove);  // Remove InventoryItem
                context.Products.Remove(product);   // Remove Product
            }
        }
        context.SaveChanges();
    }

    /// <summary>
    /// Adds passed in Product and its quantity back if item is found in store inventory, if not found, creates a new inventory item object. We create a new object because store item behavior is remove item if quantity == 0.
    /// </summary>
    /// <param name="context">Db context so only 1 context is used between Add() and RestoreStock()</param> 
    /// <param name="product">Product to restore stock for</param>
    /// <param name="quantity">Amount of stock to restore</param>
    /// <returns>True on successful stock restore, false upon failure</returns>
    public bool RestoreStock(AppDbContext context, Product product, int quantity)
    {
        var existing = context.InventoryItems
            .Include(i => i.Product)
            .FirstOrDefault(i => i.Product.Id == product.Id);

        if (existing is not null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            context.InventoryItems.Add(new InventoryItem(product, quantity));
        }
        context.SaveChanges();
        return true;
    }

    // Overloaded restore stock, called by Cart, opens its own context
    public bool RestoreStock(Product product, int quantity)
    {
        using var context = new AppDbContext();
        return RestoreStock(context, product, quantity);
    }

    public InventoryItem GetProduct(string name)
    {
        using var context = new AppDbContext();
        var inventoryItem = context.InventoryItems
        .Include(p => p.Product)
        .FirstOrDefault(p => p.Product.Name == name);
        if (inventoryItem is not null)
        {
            return inventoryItem;
        }
        return null;

    }

    public void DisplayInventory()
    {
        using var context = new AppDbContext();
        var items = context.InventoryItems
            .Include(i => i.Product)
            .ToList();
        foreach (InventoryItem i in items)
        {
            Console.WriteLine(i.Product.GetDescription() + $"\n Quantity: {i.Quantity}");
        }
    }
}