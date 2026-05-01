class Book : Product
{

    private int Rating { get; set; }

    private string Author { get; set; }

    private string Genre { get; set; }

    public Book(string name, decimal price, int rating, string author, string genre) : base(name, price)
    {
        this.Rating = rating;
        this.Author = author;
        this.Genre = genre;
    }

    override public string GetDescription()
    {
        return this.Author;
    }

    public override decimal CalculateDiscount()
    {
        return 15.0m;
    }
}