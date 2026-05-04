using Microsoft.EntityFrameworkCore;

class User
{
    public string Username { get; private set; }
    public string Password { get; private set; }
    public Cart cart { get; private set; }
    public bool IsAdmin { get; private set; }
    public decimal Money { get; set; }

    /// <summary>
    /// Primary key for each User
    /// </summary>
    public int Id { get; set; }

    public User()
    {
        this.Username = "";
        this.Password = "";
        this.cart = new Cart();
        this.IsAdmin = false;
        this.Money = 500.00m;
    }

    public User(string username, string password)
    {
        this.Username = username;
        this.Password = password;
        this.cart = new Cart();
        this.IsAdmin = false;
        this.Money = 0m;
    }

    public static User? Login(string username, string password)
    {
        using var context = new AppDbContext();
        var user = context.Users
        .Include(u => u.cart)
        .FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user is not null)
        {
            Console.WriteLine($"User {username} Successfully Logged In!");
            return user;
        }
        return null;
    }
    public static bool Register(string username, string password)
    {
        if (username.Length <= 5 || password.Length <= 8)   // Username/password length
        {
            return false;
        }

        using var context = new AppDbContext();
        if (context.Users.Any(u => u.Username == username)) // Duplicate username
        {
            return false;
        }

        Cart cart = new Cart();
        User user = new User(username, password);
        user.cart = cart;   // Link cart to the new user (uses fk in db)
        context.Users.Add(user);
        context.SaveChanges();
        return true;
    }
    public bool Deduct(decimal amount)
    {
        if (this.Money < amount)
        {
            return false;
        }
        using var context = new AppDbContext();
        var user = context.Users.Find(this.Id);
        if (user != null)
        {
            user.Money -= amount;
            this.Money -= amount;
            context.SaveChanges();
            return true;
        }
        return false;
    }
    public bool Refund(decimal amount)
    {
        using var context = new AppDbContext();
        var user = context.Users.Find(this.Id);
        if (user != null)
        {
            user.Money += amount;
            this.Money += amount;
            context.SaveChanges();
            return true;
        }
        return false;
    }

    public bool AddToCart(InventoryItem item, int quantity)
    {
        return this.cart.AddItem(item, quantity);
    }

    public bool Checkout()
    {
        decimal subtotal = cart.GetSubtotal();
        if (Money < subtotal) return false;
        if (!Deduct(subtotal)) return false;
        return cart.Clear();
    }

    public static bool SetAdminStatus(User requester, string targetUsername, bool value)
    {
        if (!requester.IsAdmin) return false;
        using var context = new AppDbContext();
        var target = context.Users.FirstOrDefault(u => u.Username == targetUsername);
        if (target is null) return false;
        target.IsAdmin = value;
        context.SaveChanges();
        return true;
    }

    public static bool SetBalance(User requester, string targetUsername, decimal amount)
    {
        if (!requester.IsAdmin || amount < 0) return false;
        using var context = new AppDbContext();
        var target = context.Users.FirstOrDefault(u => u.Username == targetUsername);
        if (target is null) return false;
        target.Money = amount;
        context.SaveChanges();
        return true;
    }

    public static bool DeleteUser(User requester, string targetUsername)
    {
        if (!requester.IsAdmin) return false;
        using var context = new AppDbContext();
        var target = context.Users
            .Include(u => u.cart)
            .FirstOrDefault(u => u.Username == targetUsername);
        if (target is null) return false;
        if (target.cart != null)
        {
            var cartItems = context.CartItems.ToList();
            context.CartItems.RemoveRange(cartItems);
            context.Carts.Remove(target.cart);
        }
        context.Users.Remove(target);
        context.SaveChanges();
        return true;
    }

    public static void SeedAdmin()
    {
        using var context = new AppDbContext();
        if (context.Users.Any(u => u.IsAdmin)) return;
        var admin = new User("admin", "admin");
        admin.IsAdmin = true;
        admin.Money = 0m;
        context.Users.Add(admin);
        context.SaveChanges();
    }
}