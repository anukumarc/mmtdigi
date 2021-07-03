using System;
using System.Threading.Tasks;
using Order.API.Models;

namespace Order.API.Interfaces
{
    public interface  IOrderRepository
    {
        // Interface to get the recent order details
        Task<CustomerOrder> GetRecentOrderAsync(string CustomerId);
        // Interface to get the customer details
        Task<Customer> GetCustomerDetailsAsync(string customerEmail);
    }
}
