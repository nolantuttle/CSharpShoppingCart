# CSharpShoppingCart

A console-based shopping cart simulator built in C# as a revamp of an earlier Java project. Designed to demonstrate all four pillars of object-oriented programming through a realistic retail management system with user authentication, role-based admin access, inventory management, and a full purchase/return flow.

<img width="1031" height="554" alt="UML" src="https://github.com/user-attachments/assets/1d7b163e-461d-482f-88ce-83ca529acda4" />

---

## Tech Stack

| Technology | Purpose |
|---|---|
| C# / .NET 8 | Primary language and runtime |
| SQLite | Persistent local database |
| Entity Framework Core | ORM — code-first schema, LINQ queries |
| Visual Studio Code | IDE |
| draw.io | UML and flowchart design |

---

## OOP Pillars

### Encapsulation
Fields are  exposed through C# auto-properties with controlled access modifiers appropriate to each class. For example, `Price` on `Product` uses `protected decimal Price { get; set; }` — readable by subclasses, not writable by external code. Mutation happens only through well-defined public methods like `Deduct()` and `Refund()` on `User`.

### Abstraction
`Product` is declared `abstract` — it cannot be instantiated directly and defines the contract that all product types must fulfill. `GetDescription()` is abstract, meaning every subclass is required by the compiler to provide its own implementation. `CalculateDiscount()` is `virtual`, providing a sensible default that subclasses may override.

```csharp
public abstract string GetDescription();
public virtual decimal CalculateDiscount() => 0m;
```

### Inheritance
Four concrete product types inherit from `Product`: `Book`, `Electronics`, `DryGood`, and `Furniture`. Each subclass calls `base(name, price)` to delegate common field initialization to the parent constructor, and extends the base with its own domain-specific fields.

```csharp
class Book : Product
{
    public int Rating { get; private set; }
    public string Author { get; private set; }
    public string Genre { get; private set; }

    public Book(string name, decimal price, int rating, string author, string genre)
        : base(name, price)
    {
        Rating = rating;
        Author = author;
        Genre = genre;
    }
}
```

### Polymorphism
`ShoppingCart` holds a `List<Product>` — a heterogeneous collection of different product types behind a common interface. When iterating to calculate totals or display items, each product's overridden `GetDescription()` and `CalculateDiscount()` are dispatched at runtime based on the actual type, not the declared type.

```csharp
foreach (Product product in Cart)
{
    Console.WriteLine(product.GetDescription());
    decimal discount = product.CalculateDiscount();
}
```

A `Book`, `Electronics`, and `Furniture` in the same cart each describe themselves and calculate their own discounts differently — one interface, many behaviors.

---

## Project Structure

```
CSharpShoppingCart/
├── Models/
│   ├── Product.cs          # Abstract base class
│   ├── Book.cs             # Subclass — Rating, Author, Genre
│   ├── Electronics.cs      # Subclass — Brand, VoltageRating, ModelNumber
│   ├── DryGood.cs          # Subclass — Brand, WeightKg, ExpiryDate
│   └── Furniture.cs        # Subclass — Brand, Material, WeightKg
├── Services/
│   ├── Inventory.cs        # Store stock management
│   └── ShoppingCart.cs     # Per-user cart logic
├── User.cs                 # Auth, role flag, money balance
├── Program.cs              # Entry point, menu loop
├── AppDbContext.cs          # EF Core DbContext
└── CSharpShoppingCart.csproj
```

---

## Flowchart

<img width="1577" height="1060" alt="Flowchart" src="https://github.com/user-attachments/assets/edf544db-fe2c-44e7-b4bf-9e1f98acf4fe" />
