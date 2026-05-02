/// <summary>
/// Abstract base class for all products in the store
/// </summary>
abstract class Product
{
    /// <summary>
    /// Get/set name of the product
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Get/set price of the product
    /// </summary>
    protected decimal Price { get; set; }

    /// <summary>
    /// Primary key for each product
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Paramaterized constructor for Product
    /// </summary>
    /// <param name="name">Name of product</param>
    /// <param name="price">Price of product</param>
    public Product(string name, decimal price)
    {
        this.Name = name;
        this.Price = price;
    }

    /// <summary>
    /// Concatenates member variables according to subclass type to return a custom description for each Product
    /// </summary>
    /// <returns>Description as string value</returns>
    public abstract string GetDescription();


    /// <summary>
    /// Calculates the discount for this product type
    /// </summary>
    /// <returns>Discount as a decimal value</returns>
    public virtual decimal CalculateDiscount()
    {
        return 0m;
    }

}