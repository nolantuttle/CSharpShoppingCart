class Electronics : Product
{

    public string Brand { get; set; }

    public decimal VoltageVolts { get; set; }

    public string VoltageType { get; set; }

    public string ModelNumber { get; set; }

    public Electronics() : base("", 0m) { }

    public Electronics(string name, decimal price, string brand, decimal voltageVolts, string voltageType, string modelNumber) : base(name, price)
    {
        this.Brand = brand;
        this.VoltageVolts = voltageVolts;
        this.VoltageType = voltageType;
        this.ModelNumber = modelNumber;
    }

    public override string GetDescription()
    {
        return $"Name: {Name} from {Brand}, Price: {Price:C}, Max Input Voltage: {VoltageVolts:F2}V {VoltageType}, Model Number: {ModelNumber}";
    }

    public override decimal CalculateDiscount()
    {
        return base.CalculateDiscount();    // No discount
    }
}