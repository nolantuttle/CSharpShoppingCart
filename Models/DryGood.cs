class DryGood : Product
{

    public string Brand { get; set; }

    public decimal WeightKg { get; set; }

    public DateTime ExpiryDate { get; set; }

    public DryGood() : base("", 0m) { }

    public DryGood(string name, decimal price, string brand, decimal weightKg, DateTime expiryDate) : base(name, price)
    {
        this.Brand = brand;
        this.WeightKg = weightKg;
        this.ExpiryDate = expiryDate;
    }

    public override string GetDescription()
    {
        return $"Name: {Name} from {Brand}, Weight: {WeightKg:F2}kg, Price: {Price:C}, Expires: {ExpiryDate}";
    }

    public override decimal CalculateDiscount()
    {
        return 0.925m;  // 7.5% off
    }
}