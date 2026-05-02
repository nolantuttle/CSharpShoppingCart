class CartItem
{
    /// <summary>
    /// Primary key for CartItem (does not match Product Pk!!)
    /// </summary>  
    public int Id { get; set; }

    public Product Product { get; set; }

    public int Quantity { get; set; }

    private CartItem() { }

    public CartItem(Product product, int quantity)
    {
        this.Product = product;
        this.Quantity = quantity;
    }

}