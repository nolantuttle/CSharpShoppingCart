class DryGood : Product
{

    private string Brand { get; set; }

    private decimal WeightKg { get; set; }

    private DateTime ExpiryDate { get; set; }

    public DryGood(string name, decimal price, string brand, decimal weightKg, DateTime expiryDate) : base(name, price)
    {
        this.Brand = brand;
        this.WeightKg = weightKg;
        this.ExpiryDate = expiryDate;
    }

    override public string GetDescription()
    {
        return this.Brand;
    }

    public override decimal CalculateDiscount()
    {
        return 0.075m;
    }
}