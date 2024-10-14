namespace Fanex.Data.SampleData.Models;

public class Product : Entity
{
    public int Id { get; set; }

    public string ProductName { get; set; }

    public string QuantityPerUnit { get; set; }

    public decimal UnitPrice { get; set; }

    public byte UnitsInStock { get; set; }

    public byte UnitsOnOrder { get; set; }

    public byte ReorderLevel { get; set; }

    public bool Discontinued { get; set; }

    public Supplier Supplier { get; set; }

    public Category Category { get; set; }
}