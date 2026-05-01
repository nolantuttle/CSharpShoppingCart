class Furniture : Product
{

    private string Brand { get; set; }

    private string Material { get; set; }

    private decimal WeightKg { get; set; }

    public Furniture(string name, decimal price, string brand, string material, decimal weightKg) : base(name, price)
    {
        this.Brand = brand;
        this.Material = material;
        this.WeightKg = weightKg;
    }

    override public string GetDescription()
    {
        return this.Brand;
    }

    public override decimal CalculateDiscount()
    {
        return 10.0m;
    }
}
