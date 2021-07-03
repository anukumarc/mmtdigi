using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Order.API.Interfaces;
using Order.API.Models;

namespace Order.API.Controllers.v1
{
    [ApiController]
    [Route("api/{version:apiVersion}/[Controller]")]
    [ApiVersion("1.0")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        
        // Intialzes the class and gets the Order Repository via dependency injection 
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // This POST method accepts customer email and customer number to check any recent orders.
        // It returns the customer details along with recent order details, if exists.
        [HttpPost]
        public async Task<ActionResult<OrderStatusDto>>  GetOrderStatus([FromBody] UserDto userDto)
        {
            // Mandatory validation for customer id and email address.
            if(userDto == null ||
                string.IsNullOrWhiteSpace(userDto.CustomerId) ||
                string.IsNullOrWhiteSpace(userDto.User))
            {
                return BadRequest("Please supply the customer number and email address");
            }

            // Get the customer details.
            var customer = await _orderRepository.GetCustomerDetailsAsync(userDto.User);
            if(customer == null)
            {
                return NotFound("No matching customer record found with the supplied values");
            }
            else
            {
                // Check the customer email is matching with customer id.
                if(customer.Email != userDto.User || customer.CustomerId != userDto.CustomerId)
                {
                    // Since this entity cannot be processed, shall return either 400 Bad Request or 422 Unprocessable Entity response.
                    return BadRequest("The supplied customer number do match with the customer email");
                }
            }

            // Get the recent order, if any.
            var recentOrder = await _orderRepository.GetRecentOrderAsync(userDto.CustomerId);

            // Update the delivery address of the customer.
            if (recentOrder != null)
            {
                recentOrder.DliveryAddress = string.Format("{0} {1} {2} {3}",
                                                            customer.HouseNumber,
                                                            customer.Street,
                                                            customer.Town,
                                                            customer.Postcode);
            }

            // Prepare the order status to return.
            var orderDetails = new OrderStatusDto()
            {
                 customer = new CustomerDto()
                 {
                     FirstName = customer.FirstName,
                     LastName = customer.LastName
                 },
                 order = recentOrder
            };

            // Return the order status.
            return orderDetails;
        }

        
    }
}
