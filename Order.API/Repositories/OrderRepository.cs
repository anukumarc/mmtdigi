using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Order.API.Common;
using Order.API.Interfaces;
using Order.API.Models;


namespace Order.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public OrderRepository()
        {
            
        }

        // This method accepts the customer id and check for any recent order in the ORDERS table.
        // The it pulls the ordered items from the ORDERITEMS table with order id, if any matching order is found.
        // Finally it combines the order details with the product details and returns a CustomerOrder object.
        public async Task<CustomerOrder> GetRecentOrderAsync(string customerId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING))
                {
                    // Çhecks any recent order exists for the customer.  
                    // The query sorts the customer orders in desceidng order by ORDERDATE and ORDERID
                    // so that the latest record comes at the top even if there are mutiple order exists on the same date.
                    // Then it picks the top most record which is the latest.
                    // Since no data available to check whether the order has actually delivered,
                    // records with past 'expected delivery date' are also included in the search.
                    var query = @"SELECT TOP 1 OrderId AS OrderNumber, OrderDate, DeliveryExpected  
                                FROM ORDERS
                                WHERE CustomerId = @CustomerId
                                ORDER BY OrderDate DESC, OrderId DESC;";

                    var customerOrder = await connection.QueryFirstOrDefaultAsync<CustomerOrder>(query, new { @CustomerId = customerId });

                    // Return null if no order exists.
                    if (customerOrder == null) return null;

                    // Get the details of products ordered, if an open order exists.
                    // Replace product name with 'Gift, if the order contains gift items.
                    query = @"SELECT CASE 
                                        WHEN Ord.ContainsGift = 1 THEN 'Gift' 
                                        ELSE ProductName 
                                    END as Product, 
                                Quantity, Price as PriceEach 
                            FROM ORDERS Ord 
                            JOIN ORDERITEMS Items ON Ord.OrderId = Items.OrderId
                            JOIN PRODUCTS Prod ON Items.ProductId = Prod.ProductId
                            WHERE Ord.OrderId = @OrderId;";

                    var items = await connection.QueryAsync<OrderItem>(query, new { @OrderId = customerOrder.OrderNumber });
                    if (items != null)
                        customerOrder.OrderItems = items.ToList();

                    // Return the order details
                    return customerOrder;
                }
            }
            catch (Exception ex)
            {
                // log the exception details (not included)
                
                //throw exception 
                throw new Exception("An error has occured while getting the order details", ex);

            }
        }

        // This method is used to pull the customer details from the API provided using the customer email address.
        // If no matching record is found it will retun null.
        public async Task<Customer> GetCustomerDetailsAsync(string customerEmail)
        {
            using (var httpClient = new HttpClient())
            {
                // Build request to pull the customer details.
                httpClient.BaseAddress = new Uri(Constants.CUSTOMERAPI_BASE_URL);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                // Prepare JSON content with customer email.
                StringContent content = new StringContent(JsonConvert.SerializeObject(new { email = customerEmail }), Encoding.UTF8, "application/json");

                // Get the API response.
                using (var response = await httpClient.PostAsync(Constants.CUSTOMERAPI_REQUEST_URL, content))
                {
                    // Check the response.
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        // Deserialize the response data.
                        return JsonConvert.DeserializeObject<Customer>(apiResponse);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        // Return null if user does not exists.
                        return null;
                    }
                    else
                    {
                        // Something went wrong, throw exception
                        throw new Exception("Failed to fetch the customer details");
                    }
                }
            }
        }
    }
}
