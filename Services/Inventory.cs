class Inventory
{

    public Inventory()
    {

    }

    public void AddProduct(Product product)
    {
        using var context = new AppDbContext();
        context.Products.Add(product);
        context.SaveChanges();
    }

    public void RemoveProduct(Product product)
    {
        using var context = new AppDbContext();
        var remove = context.Products.Find(product.Id);
        if (remove != null)
        {
            context.Products.Remove(remove);
        }
        context.SaveChanges();
    }

    public Product GetProduct(string name)
    {
        using var context = new AppDbContext();
        var product = context.Products.FirstOrDefault(p => p.Name == name);
        return product;
    }

    public void DisplayInventory()
    {
        using var context = new AppDbContext();
        List<Product> products = context.Products.ToList();
        foreach (Product product in products)
        {
            Console.WriteLine(product.GetDescription());
        }

    }

}