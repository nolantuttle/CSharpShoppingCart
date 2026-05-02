class Book : Product
{

    public int Rating { get; set; }

    public string Author { get; set; }

    public string Genre { get; set; }

    public Book() : base("", 0m) { }

    public Book(string name, decimal price, int rating, string author, string genre) : base(name, price)
    {
        this.Rating = rating;
        this.Author = author;
        this.Genre = genre;
    }

    public override string GetDescription()
    {
        return $"Name: {Name} written by {Author}, Price: {Price:C}, Genre: {Genre} Rating: {Rating}/5";
    }

    public override decimal CalculateDiscount()
    {
        return 0.90m;    // 10% off
    }
}