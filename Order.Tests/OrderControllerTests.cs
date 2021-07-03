using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Moq;
using Order.API.Controllers.v1;
using Order.API.Interfaces;
using Order.API.Models;
using Xunit;

namespace Order.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public async Task GetOrderStatus_ReturnsBadRequest_WithNoInputs()
        {
            // Arrange
            var repoStub = new Mock<IOrderRepository>();
            repoStub.Setup(repo => repo.GetRecentOrderAsync(It.IsAny<string>()))
                .ReturnsAsync((CustomerOrder)null);

            var controller = new OrderController(repoStub.Object);

            // Act
            var actionResult = await controller.GetOrderStatus(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetOrderStatus_ReturnsBadRequest_WithNoEmailOrCustomerNo()
        {
            // Arrange
            var repoStub = new Mock<IOrderRepository>();
            repoStub.Setup(repo => repo.GetRecentOrderAsync(It.IsAny<string>()))
                .ReturnsAsync((CustomerOrder)null);

            var controller = new OrderController(repoStub.Object);

            // Act
            var actionResult = await controller.GetOrderStatus(new UserDto { CustomerId = "", User = "xyz@gmail.com" });

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetOrderStatus_ReturnsNotFound_WithNonExistingCustomerDetail()
        {
            // Arrange
            var repoStub = new Mock<IOrderRepository>();
            repoStub.Setup(repo => repo.GetRecentOrderAsync(It.IsAny<string>()))
                .ReturnsAsync((CustomerOrder)null);

            repoStub.Setup(repo => repo.GetCustomerDetailsAsync(It.IsAny<string>()))
                .ReturnsAsync((Customer)null);

            var controller = new OrderController(repoStub.Object);

            // Act
            var actionResult = await controller.GetOrderStatus(new UserDto { CustomerId = "X4567", User = "abc@gmail.com" });

            // Assert
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetOrderStatus_ReturnsNotFound_UnmatchedCustomerNoAndEmail()
        {
            // Arrange
            var repoStub = new Mock<IOrderRepository>();
            repoStub.Setup(repo => repo.GetRecentOrderAsync(It.IsAny<string>()))
                .ReturnsAsync((CustomerOrder)null);

            var mockCustomer = GetMockCustomer();
            repoStub.Setup(repo => repo.GetCustomerDetailsAsync(It.IsAny<string>()))
                .ReturnsAsync(mockCustomer);

            var controller = new OrderController(repoStub.Object);

            // Act
            var actionResult = await controller.GetOrderStatus(new UserDto { CustomerId = "X4567", User = "jack@gmail.com" });

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task  GetOrderStatus_ReturnsExpectedItem_WithExistingCustomerDetail()
        {
            // Arrange
            var repoStub = new Mock<IOrderRepository>();
            var mockOrder = GetMockOrder(); 
            repoStub.Setup(repo => repo.GetRecentOrderAsync(It.IsAny<string>()))
                .ReturnsAsync(mockOrder);

            var mockCustomer = GetMockCustomer();
            repoStub.Setup(repo => repo.GetCustomerDetailsAsync(It.IsAny<string>()))
                .ReturnsAsync(mockCustomer); 

            var controller = new OrderController(repoStub.Object);

            // Act
            var result = await controller.GetOrderStatus(new UserDto { CustomerId = "A1234", User = "jack@gmail.com" });

            // Assert
            Assert.IsType<OrderStatusDto>(result.Value);
            var resultDto = result.Value;

            resultDto.customer.FirstName = "anu"; 

            // Match customer details
            resultDto.customer.Should().BeEquivalentTo(
                mockCustomer,
                options => options.ComparingByMembers<Customer>().ExcludingMissingMembers()
            );

            // Match the order details
            resultDto.order.Should().BeEquivalentTo(
                mockOrder,
                options => options.ComparingByMembers<CustomerOrder>().ExcludingMissingMembers()
            );
            // Match the products
            resultDto.order.OrderItems.Should().BeEquivalentTo(
                mockOrder.OrderItems,
                options => options.ComparingByMembers<OrderItem>().ExcludingMissingMembers()
            );
        }

        private CustomerOrder  GetMockOrder()
        {
            // Returns a mock Order Status object with two products
            return new CustomerOrder()
            {
                OrderNumber = "PO123",
                OrderDate = DateTime.Today,
                DeliveryExpected = DateTime.Today.AddDays(5),
                DliveryAddress = "",
                OrderItems = new List<OrderItem>()
                {
                    new OrderItem(){ Product = "Item 1", Quantity = 5, PriceEach = 5 },
                    new OrderItem(){ Product = "Item 2", Quantity = 10, PriceEach = 25 }
                } 
            };
        }

        private Customer GetMockCustomer()
        {
            // Returns a mock customer details object with minimum details
            return new Customer()
            {
                CustomerId  = "A1234",
                FirstName = "Jack",
                LastName = "Daniel",
                Email =  "jack@gmail.com",
                HouseNumber = "2B",
                Street = "South Gate",
                Town =   "Uppingham",
                Postcode = "LE10 6BY",
            };
        }
    }
}
