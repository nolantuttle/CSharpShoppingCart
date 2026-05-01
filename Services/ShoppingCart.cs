class ShoppingCart
{

    private List<Product> Cart { get; set; }

    public ShoppingCart()
    {
        this.Cart = new List<Product>();
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
        Cart.Clear();
    }
}