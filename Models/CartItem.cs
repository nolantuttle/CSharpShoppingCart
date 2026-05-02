class CartItem
{

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