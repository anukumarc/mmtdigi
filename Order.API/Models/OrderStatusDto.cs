using System;
using System.Collections.Generic;

namespace Order.API.Models
{
    // Model used to return the final API output with order status
    public class OrderStatusDto
    {
        public CustomerDto customer { get; set; }
        public CustomerOrder order { get; set; }
    }
}
