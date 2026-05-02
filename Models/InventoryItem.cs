using Microsoft.EntityFrameworkCore;

class InventoryItem
{
    public int Id { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }

    /// <summary>
    /// Primary key for InventoryItem (does not match Product Pk!!)
    /// </summary>
    private InventoryItem() { }

    public InventoryItem(Product product, int quantity)
    {
        this.Product = product;
        this.Quantity = quantity;
    }
}