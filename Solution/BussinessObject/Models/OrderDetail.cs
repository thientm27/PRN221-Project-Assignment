using System;
using System.Collections.Generic;

#nullable disable

namespace BussinessObject.Models
{
    public partial class OrderDetail
    {
        public int OrderId { get; set; }
        public int FlowerBouquetId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }

        public virtual FlowerBouquet FlowerBouquet { get; set; }
        public virtual Order Order { get; set; }
    }
}
