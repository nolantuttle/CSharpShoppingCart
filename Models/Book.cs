class Book : Product
{

    public int Rating { get; set; }

    public string Author { get; set; }

    public string Genre { get; set; }

    public Book(string name, decimal price, int rating, string author, string genre) : base(name, price)
    {
        this.Rating = rating;
        this.Author = author;
        this.Genre = genre;
    }

    public override string GetDescription()
    {
        return this.Author;
    }

    public override decimal CalculateDiscount()
    {
        return 0.15m;
    }
}