using Microsoft.EntityFrameworkCore;

class InventoryItem
{
    public int Id { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }

    private InventoryItem() { }

    public InventoryItem(Product product, int quantity)
    {
        this.Product = product;
        this.Quantity = quantity;
    }
}