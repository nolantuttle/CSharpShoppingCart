// Seed some test data
Inventory inventory = new Inventory();
Book book = new Book("The Pragmatic Programmer", 29.99m, 4, "Hunt & Thomas", "Technology");
inventory.AddProduct(book, 10);

// Verify it works
inventory.DisplayInventory();