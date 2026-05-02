using Microsoft.EntityFrameworkCore;
class Cart
{

    /// <summary>
    /// Primary key for Cart
    /// </summary>
    public int Id { get; set; }

    public Cart() { }

    /// <summary>
    /// This method pulls a product from the store's inventory, linked with CartItem via Product Pk in db, adds to cart.
    /// </summary>
    /// <param name="product">We must pass in InventoryItem here and not CartItem to pull from store's actual inventory.</param>
    /// <param name="quantity">Amount of InventoryItem to add to cart</param>
    /// <returns>True for successful inventory item added to cart, false on failure</returns>
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

    /// <summary>
    /// Abstraction, calls restore stock method for Inventory class so Cart class does not know about Inventory logic. Removes item from Cart.
    /// </summary>
    /// <param name="cartItem">Item to be removed from cart</param>
    /// <param name="quantity">Quantity of cart item to remove</param>
    /// <returns>True on successful cart item removal, false for exception</returns>
    public bool RemoveItem(CartItem cartItem, int quantity)
    {
        var inventory = new Inventory();
        if (!inventory.RestoreStock(cartItem.Product, quantity))
            return false;
        return true;
    }

    public decimal GetSubtotal()
    {
        using var context = new AppDbContext();
        var items = context.CartItems
            .Include(c => c.Product)
            .ToList();
        decimal subtotal = 0m;
        foreach (CartItem c in items)
        {
            subtotal += c.Product.CalculateDiscount() * c.Quantity * c.Product.Price;
        }
        return subtotal;
    }
    public void DisplayCart()
    {
        using var context = new AppDbContext();
        var items = context.CartItems
            .Include(c => c.Product)
            .ToList();
        foreach (CartItem c in items)
        {
            Console.WriteLine(c.Product.GetDescription() + $"\n Quantity: {c.Quantity}");
        }
    }

    /// <summary>
    /// Removes each CartItem and its full quantity from cart
    /// </summary>
    /// <returns>True on success, false on failure</returns>
    public bool Clear()
    {
        using var context = new AppDbContext();
        var items = context.CartItems
            .Include(c => c.Product)
            .ToList();
        foreach (CartItem c in items)
        {
            if (!RemoveItem(c, c.Quantity))
            {
                return false;
            }
        }
        return true;
    }
}