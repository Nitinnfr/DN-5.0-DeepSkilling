using System;
using NUnit.Framework;
using Moq;
using CustomerCommLib;

namespace CustomerCommTests
{
    // Requirement: Use TestFixture attribute on top of the test class
    [TestFixture]
    public class MailSenderTests
    {
        private Mock<IMailSender> _mockMailSender;
        private CustomerComm _customerComm;

        // Requirement: Use OneTimeSetUp attribute on top of the init method
        [OneTimeSetUp]
        public void Init()
        {
            // Configure the mock object layout here once before test execution
            _mockMailSender = new Mock<IMailSender>();
            
            // Requirement: Configure SendMail() to accept any two string arguments and always return true
            _mockMailSender.Setup(m => m.SendMail(It.IsAny<string>(), It.IsAny<string>()))
                           .Returns(true);

            // Inject the mock object into our class under test
            _customerComm = new CustomerComm(_mockMailSender.Object);
        }

        // Requirement: Use TestCase attribute on top of the test method
        // We can pass different email and message parameters to show the test works dynamically
        [TestCase("cust123@abc.com", "Some Message")]
        [TestCase("testuser@domain.com", "Another Message")]
        public void SendMailToCustomer_WhenInvoked_AlwaysReturnsTrue(string toAddress, string message)
        {
            // Act: Invoke the method under test
            bool testResult = _customerComm.SendMailToCustomer();

            // Requirement: Finally assert the return value to "true"
            Assert.That(testResult, Is.True);
        }
    }
}