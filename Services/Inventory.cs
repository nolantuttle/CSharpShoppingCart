using Microsoft.EntityFrameworkCore;
class Inventory
{

    public Inventory()
    {

    }

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
        if (context.Products.Find(product.Id) == null)
        {
            context.Products.Add(product);  // Adds to primary product table
            context.SaveChanges();  // IMPORTANT: save changes before modifying new db context in RestoreStock()
            RestoreStock(product, quantity);    // Adds product to InventoryItems
            return true;
        }
        return false;
    }

    public void RemoveProduct(InventoryItem inventoryItem)
    {
        using var context = new AppDbContext();
        var remove = context.InventoryItems.Find(inventoryItem.Product.Id);
        if (remove != null)
        {
            context.InventoryItems.Remove(remove);
        }
        context.SaveChanges();
    }

    /// <summary>
    /// Adds passed in Product and its quantity back if item is found in store inventory, if not found, creates a new inventory item object. We create a new object because store item behavior is remove item if quantity == 0.
    /// </summary>
    /// <param name="product">Product to restore stock for</param>
    /// <param name="quantity">Amount of stock to restore</param>
    /// <returns>True on successful stock restore, false upon failure</returns>
    public bool RestoreStock(Product product, int quantity)
    {
        using var context = new AppDbContext();
        var existing = context.InventoryItems
            .Include(i => i.Product)
            .FirstOrDefault(i => i.Product.Id == product.Id);
        if (existing is not null)
        {
            existing.Quantity += quantity;
            context.SaveChanges();
            return true;
        }
        else
        {
            context.InventoryItems.Add(new InventoryItem(product, quantity));
            context.SaveChanges();
            return true;
        }
    }

    public InventoryItem GetProduct(string name)
    {
        using var context = new AppDbContext();
        InventoryItem product = context.InventoryItems
        .Include(p => p.Product)
        .FirstOrDefault(p => p.Product.Name == name);
        return product;
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