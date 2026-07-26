using System;
using NUnit.Framework;
using Moq;
using CustomerCommLib;

namespace CustomerCommTests
{
    [TestFixture]
    public class MailSenderTests
    {
        [Test]
        public void SendMailToCustomer_WhenMocked_ReturnsTrueAndInvokesCorrectly()
        {
            // Arrange
            var mockMailSender = new Mock<IMailSender>();
            
            mockMailSender.Setup(m => m.SendMail(It.IsAny<string>(), It.IsAny<string>()))
                          .Returns(true);

            var customerComm = new CustomerComm(mockMailSender.Object);

            // Act
            bool testResult = customerComm.SendMailToCustomer();

            // Assert
            Assert.That(testResult, Is.True);
            mockMailSender.Verify(m => m.SendMail("cust123@abc.com", "Some Message"), Times.Once);
        }
    }
}