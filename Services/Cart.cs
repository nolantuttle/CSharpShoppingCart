using Microsoft.EntityFrameworkCore;
class Cart
{

    /// <summary>
    /// Primary key for Cart
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Foreign key to link each Cart to a User
    /// </summary>
    public int UserId { get; set; }

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
        var trackedProduct = context.Products.Find(inventoryItem.Product.Id);   // Pull from latest db context, which is tracking this inventoryItem.
        if (existing != null)    // Product is already in cart! We must add more quantity.
        {
            existing.Quantity += quantity;
        }
        else if (trackedProduct != null)   // Product is not in cart, but is valid. Make new CartItem and add quantity
        {
            context.CartItems.Add(new CartItem(trackedProduct, quantity));
        }
        context.SaveChanges();
        return true;
    }

    /// <summary>
    /// Abstraction, calls restore stock method for Inventory class so Cart class does not know about Inventory logic. Removes item from Cart.
    /// </summary>
    /// <param name="cartItem">Item to be removed from cart</param>
    /// <param name="quantity">Quantity of cart item to remove</param>
    /// <param name="inventory">Up-to-date instance of the inventory being checked</param>
    /// <returns>True on successful cart item removal, false for exception</returns>
    public bool RemoveItem(int cartItemPk, int quantity, Inventory inventory)
    {
        using var context = new AppDbContext();
        var cartItemToRemove = context.CartItems
        .Include(c => c.Product)
        .FirstOrDefault(c => c.Id == cartItemPk); // Find cartItem by its Pk, include Product row
        if (cartItemToRemove is null)
        {
            return false;
        }
        if (quantity <= 0 || cartItemToRemove.Quantity < quantity)
        {
            return false;
        }

        if (!inventory.RestoreStock(cartItemToRemove.Product, quantity))
        {
            return false;
        }

        if (cartItemToRemove.Quantity == quantity)  // Removing all quantity of cartItem
        {
            context.CartItems.Remove(cartItemToRemove);
        }
        else    // Removing only some cartItem quantity
        {
            cartItemToRemove.Quantity -= quantity;
        }
        context.SaveChanges();
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
            Console.WriteLine($"{c.Id}" + c.Product.GetDescription() + $"\nQuantity: {c.Quantity}");
        }
    }

    /// <summary>
    /// Removes each CartItem and its full quantity from cart
    /// </summary>
    /// <param name="inventory">We must pass in an Inventory instance here to return all cart items to stock</param>
    /// <returns>True on success, false on failure</returns>
    public bool Clear(Inventory inventory)
    {
        using var context = new AppDbContext();
        var items = context.CartItems
            .Include(c => c.Product)
            .ToList();
        foreach (CartItem c in items)
        {
            if (!RemoveItem(c.Id, c.Quantity, inventory))
            {
                return false;
            }
        }
        return true;
    }
}