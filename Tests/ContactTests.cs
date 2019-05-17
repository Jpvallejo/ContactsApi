using NUnit.Framework;
using System;
using System.Net;
using WebApi.Controllers;
using WebApi.Models;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestGetContactWithoutGuidReturnsIdIsIncorrect()
        {
            var controller = new ContactsController();
            var id = "jpwodksapod";
            var result = controller.Get(id);
            Assert.That(result.Value == "Id is Incorrect");
        }

        [Test]
        public void TestGetContactWithNewGuidReturnsNotFound()
        {
            var controller = new ContactsController();
            var id = Guid.NewGuid().ToString();
            var result = controller.Get(id);
            Assert.That(result.Value == "Not Found");
        }

        [Test]
        public void TestCreateContactWithCorrectValuesCreates()
        {
            var controller = new ContactsController();
            var model = new ContactViewModel()
            {
                Name = "John Peres",
                Birthday = "28/02/1998",
                Email = "johnp@gmail.com",
                Company = "Fake Company",
                PersonalPhone = "1122009988",
                WorkPhone = "1122119988",
                Address = "Fake St. 123",
                City = "Buenos Aires",
                State = "Buenos Aires"
            };
            Guid guid;
            var result = controller.Create(model);
            Assert.That(result.StatusCode == (int)HttpStatusCode.Created && Guid.TryParse(result.Value.ToString(),out guid) );
        }
    }
}