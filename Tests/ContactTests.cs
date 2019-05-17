using NUnit.Framework;
using System;
using WebApi.Controllers;

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
    }
}