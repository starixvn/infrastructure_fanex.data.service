namespace Fanex.Data.SampleData.Models;

using System.Collections.Generic;
using System.Linq;

public class OrderDetails : Entity
{
    public Order Order { get; set; }

    [IncomparableProperty]
    public IList<OrderedProduct> OrderedProducts { get; set; }

    public override bool Equals(Entity other)
    {
        if (other is OrderDetails otherOrderedDetails)
        {
            return base.Equals(other) && OrderedProducts.SequenceEqual(otherOrderedDetails.OrderedProducts);
        }

        return false;
    }
}