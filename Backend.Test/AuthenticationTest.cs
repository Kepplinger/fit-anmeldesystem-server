using System.Collections.Generic;
using Backend.Controllers;
using Backend.Core.Entities;
using System.Linq;
using Backend.Core.Contracts;
using Xunit;
using System;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Test
{
    public class AuthenticationTest
    {
        [Fact]
        public void SampleTest()
        {
            // Arrange
            mockRepo.Setup(repo => repo.BookingRepository.Get(null, null, "Event,Branches,Company,Package,Location,Presentation")).Returns(GetAllBookings());
            var controller = new BookingController(mockRepo.Object);

            // Act
            var result = controller.GetAll();

            // Assert
            var viewResult = Xunit.Assert.IsAssignableFrom<OkObjectResult>(result);
            var model = Xunit.Assert.IsAssignableFrom<ICollection<Booking>>(viewResult.Value);
            System.Console.WriteLine(model.Count());
            Assert.Equal(1, model.Count());
        }

        private Booking[] GetAllBookings()
        {
            Booking[] bookings = new Booking[1];
            bookings[0] = new Booking();
            Console.WriteLine(bookings.Count());
            return bookings;
        }

        /*[Fact]
        public void TestCodeForgotten()
        {
            // Arrange
            var mockRepo = new Mock<IUnitOfWork>();
            mockRepo.Setup(repo => repo.CompanyRepository.Get(p => p.Contact.Email.Equals(""), null, "")).Returns(GetNoCompanies());
            var auth = new AuthenticationController(mockRepo.Object);

            // Act
            var result = auth.SendCompanyCodeForgotten(null);

            // Assert
            var viewResult = Xunit.Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            //var model = Xunit.Assert.IsAssignableFrom<AnonymousObject>(viewResult.Value);
            Assert.Equal("Es wurde keine E-Mail übermittelt!", result.ToJsonToString());
        }

        private Company[] GetNoCompanies()
        {
            Company[] companies = new Company[1];
            companies[0] = new Company();
            Console.WriteLine(companies.Count());
            return companies;
        }*/
    }
}
