using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Order.API.Models
{
    // Model used to store the order details of the customer.
    // Defined only the properties that are required for the API output
    public class CustomerOrder
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string DliveryAddress { get; set; }
        public DateTime  DeliveryExpected { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
