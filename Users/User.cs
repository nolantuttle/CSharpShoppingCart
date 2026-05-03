using Microsoft.EntityFrameworkCore;

class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    private Cart cart { get; set; }
    private bool IsAdmin { get; set; }
    private decimal Money { get; set; }

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
        this.Money = 0m;
    }

    public User(string username, string password)
    {
        this.Username = username;
        this.Password = password;
        this.cart = new Cart();
        this.IsAdmin = false;
        this.Money = 0m;
    }

    public bool Login(string username, string password)
    {
        using var context = new AppDbContext();
        var user = context.Users
        .FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user is not null)
        {
            Console.WriteLine($"User {username} Successfully Logged In!");
            return true;
        }
        return false;
    }
    public bool Register(string username, string password)
    {
        if (username.Length <= 5)   // Username length
        {
            return false;
        }
        if (password.Length <= 8)   // Password length
        {
            return false;
        }

        using var context = new AppDbContext();
        if (context.Users.Any(u => u.Username == username)) // Duplicate username
        {
            return false;
        }

        User user = new User(username, password);
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
            context.SaveChanges();
            return true;
        }
        return false;
    }
}