class Furniture : Product
{

    public string Brand { get; set; }

    public string Material { get; set; }

    public decimal WeightKg { get; set; }

    public Furniture(string name, decimal price, string brand, string material, decimal weightKg) : base(name, price)
    {
        this.Brand = brand;
        this.Material = material;
        this.WeightKg = weightKg;
    }

    public override string GetDescription()
    {
        return $"Name: {Name} from {Brand}, Price: {Price:C}, Material: {Material}, Weight: {WeightKg}kg";
    }

    public override decimal CalculateDiscount()
    {
        return 0.1m;
    }
}
