using System.Collections.Generic;
using Backend.Controllers;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using StoreService.Persistence;
using System.Linq;
using Backend.Core.Contracts;
using Moq;
using Xunit;
using System;

namespace Backend.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var mockRepo = new Mock<IUnitOfWork>();
            mockRepo.Setup(repo => repo.BookingRepository.Get(null, null, "Event,Branches,Company,Package,Location,Presentation")).Returns(GetTestSessions());
            var controller = new BookingController(mockRepo.Object);

            // Act
            var result = controller.GetAll();

            // Assert
            var viewResult = Xunit.Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Xunit.Assert.IsAssignableFrom<ICollection<Booking>>(viewResult.Value);
            System.Console.WriteLine(model.Count());
            Assert.Equal(1, model.Count());
        }

        private Booking[] GetTestSessions()
        {
            Booking[] bookings = new Booking[1];
            bookings[0] = new Booking();
            Console.WriteLine(bookings.Count());
            return bookings;
        }
    }
}
