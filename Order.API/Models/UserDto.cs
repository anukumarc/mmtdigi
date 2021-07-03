using System;
namespace Order.API.Models
{

    // Model used to get the customer details from the user
    public class UserDto
    {
        public string User { get; set; }
        public string CustomerId { get; set; }
    }
}
