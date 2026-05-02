using Microsoft.EntityFrameworkCore;
class Inventory
{

    public Inventory()
    {

    }

    public void AddProduct(InventoryItem product)
    {
        using var context = new AppDbContext();
        context.InventoryItems.Add(product);
        context.SaveChanges();
    }

    public void RemoveProduct(InventoryItem product)
    {
        using var context = new AppDbContext();
        var remove = context.InventoryItems.Find(product.Id);
        if (remove != null)
        {
            context.InventoryItems.Remove(remove);
        }
        context.SaveChanges();
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


    }

}