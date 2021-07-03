using System;
namespace Order.API.Models
{
    // Model used to store the details of products ordered by the customer
    public class OrderItem
    {
        public string Product { get; set; } //product name
        public decimal Quantity { get; set; }
        public decimal? PriceEach { get; set; }
    }
}
