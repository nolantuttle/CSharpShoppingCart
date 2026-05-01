class Inventory
{
    private List<Product> Stock { get; set; }

    public Inventory()
    {
        this.Stock = new List<Product>();
    }

    public void AddProduct(Product product)
    {
        Stock.Append(product);
    }

    public void RemoveProduct(Product product)
    {
        Stock.Remove(product);
    }

    public Product GetProduct(string name)
    {
        return null;
    }

    public void DisplayInventory()
    {
        for (int i = 0; i < 1; i++)
        {
            Console.WriteLine(Stock.ElementAt(i));
        }

    }

}