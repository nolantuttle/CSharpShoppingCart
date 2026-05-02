using Microsoft.EntityFrameworkCore;
class Cart
{

    /// <summary>
    /// Each CartItem has a List of Products and a quantity int. 
    /// </summary>
    private List<CartItem> Items { get; } = new List<CartItem>();

    public int Id { get; set; }

    public Cart()
    {
        this.Items = new List<CartItem>();
    }

    /// <summary>
    /// This method pulls a product from the store's inventory, linked with CartItem via Product Pk in db, adds to cart.
    /// </summary>
    /// <param name="product">We must pass in InventoryItem here and not CartItem to pull from store's actual inventory.</param>
    /// <param name="quantity">Amount of InventoryItem to add to cart</param>
    /// <returns></returns>
    public bool AddItem(InventoryItem inventoryItem, int quantity)
    {
        if (quantity <= 0)
        {
            return false;
        }
        if (inventoryItem.Quantity < quantity)
        {
            return false;
        }
        using var context = new AppDbContext();
        var existing = context.CartItems
        .Include(p => p.Product)
        .FirstOrDefault(p => p.Product.Id == inventoryItem.Product.Id); // IMPORTANT: We must compare using Product's Pk, not inventoryItem Pk! Product Pk is the source of truth.
        if (existing != null)    // Product is already in cart! We must add more quantity.
        {
            existing.Quantity += quantity;
            context.SaveChanges();
            return true;
        }
        else    // Product is not in cart, make new CartItem and add that quantity!
        {
            context.CartItems.Add(new CartItem(inventoryItem.Product, quantity));
            context.SaveChanges();
            return true;
        }
    }

    public bool RemoveItem(CartItem cartItem, int quantity)
    {
        if (quantity <= 0)
        {
            return false;
        }
        if (cartItem.Quantity < quantity)
        {
            return false;
        }
        using var context = new AppDbContext();
        var existing = context.InventoryItems
        .Include(p => p.Product)
        .FirstOrDefault(p => p.Product.Id == cartItem.Product.Id); // IMPORTANT: We must compare using Product's Pk, not inventoryItem Pk! Product Pk is the source of truth.
        if (existing != null)    // Product found in store's inventory! Add quantity back and remove from cart.
        {
            existing.Quantity += quantity;
            context.CartItems.Remove(context.CartItems.Find(cartItem.Product.Id));
            context.SaveChanges();
            return true;
        }
        else    // Product not found in store inventory, need new object.
        {
            context.InventoryItems.Add(new InventoryItem(cartItem.Product, quantity));
            context.SaveChanges();
            return true;
        }
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