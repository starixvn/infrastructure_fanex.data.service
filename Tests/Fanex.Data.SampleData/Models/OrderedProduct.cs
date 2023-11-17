namespace Fanex.Data.SampleData.Models
{
    public class OrderedProduct : Entity
    {
        public decimal UnitPrice { get; set; }

        public byte Quantity { get; set; }

        public decimal Discount { get; set; }

        public Product Product { get; set; }
    }
}