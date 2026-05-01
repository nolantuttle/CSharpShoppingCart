class Electronics : Product
{

    public string Brand { get; set; }

    public decimal VoltageVolts { get; set; }

    public string VoltageType { get; set; }

    public string ModelNumber { get; set; }

    public Electronics(string name, decimal price, string brand, decimal voltageVolts, string voltageType, string modelNumber) : base(name, price)
    {
        this.Brand = brand;
        this.VoltageVolts = voltageVolts;
        this.VoltageType = voltageType;
        this.ModelNumber = modelNumber;
    }

    public override string GetDescription()
    {
        return this.Brand;
    }

    public override decimal CalculateDiscount()
    {
        return base.CalculateDiscount();
    }
}