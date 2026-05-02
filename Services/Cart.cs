class Cart
{

    private List<Product> Items { get; set; }

    public Cart()
    {
        this.Items = new List<Product>();
    }

    public void AddItem(Product product, int quantity)
    {

    }
    public void RemoveItem(Product product, int quantity)
    {

    }
    public decimal GetSubtotal()
    {
        return 0m;
    }
    public void DisplayCart()
    {

    }

    public void Clear()
    {
        Items.Clear();
    }
}