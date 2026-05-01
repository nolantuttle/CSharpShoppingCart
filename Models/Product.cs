abstract class Product
{
    protected string Name { get; set; }
    protected decimal Price { get; set; }


    public Product(string name, decimal price)
    {
        this.Name = name;
        this.Price = price;
    }

    public abstract string GetDescription();

    public virtual decimal CalculateDiscount()
    {
        return 0m;
    }

}