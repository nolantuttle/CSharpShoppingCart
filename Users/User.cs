class User
{
    private string Username { get; set; }
    private string Password { get; set; }
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

        return true;
    }
    public bool Register(string username, string password)
    {
        return true;
    }
    public void Deduct(decimal amount)
    {

    }
    public bool Refund(decimal amount)
    {
        return true;
    }

}