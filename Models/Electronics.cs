class Electronics : Product
{

    private string Brand { get; set; }

    private decimal VoltageVolts { get; set; }

    private string VoltageType { get; set; }

    private string ModelNumber { get; set; }

    public Electronics(string name, decimal price, string brand, decimal voltageVolts, string voltageType, string modelNumber) : base(name, price)
    {
        this.Brand = brand;
        this.VoltageVolts = voltageVolts;
        this.VoltageType = voltageType;
        this.ModelNumber = modelNumber;
    }

    override public string GetDescription()
    {
        return this.Brand;
    }

    public override decimal CalculateDiscount()
    {
        return base.CalculateDiscount();
    }
}