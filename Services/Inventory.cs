class Inventory
{
    private List<Product> Stock { get; } = new List<Product>();

    public Inventory()
    {
        this.Stock = new List<Product>();
    }

    public void AddProduct(Product product)
    {
        Stock.Add(product);
    }

    public void RemoveProduct(Product product)
    {
        Stock.Remove(product);
    }

    public Product GetProduct(string name)
    {
        return Stock.FirstOrDefault(p => p.Name == name);
    }

    public void DisplayInventory()
    {
        foreach (Product product in Stock)
        {
            Console.WriteLine(product.GetDescription());
        }

    }

}