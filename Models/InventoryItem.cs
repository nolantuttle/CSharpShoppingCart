using Microsoft.EntityFrameworkCore;

class InventoryItem
{
    /// <summary>
    /// Primary key for InventoryItem (does not match Product Pk!!)
    /// </summary>
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