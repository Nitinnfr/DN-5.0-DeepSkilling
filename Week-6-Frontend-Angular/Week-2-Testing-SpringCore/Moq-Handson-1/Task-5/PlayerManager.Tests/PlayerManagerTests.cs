using System;
using NUnit.Framework;
using Moq;
using PlayersManagerLib;

namespace PlayerManager.Tests
{
    // Requirement: Use TestFixture attribute on top of the test class
    [TestFixture]
    public class PlayerManagerTests
    {
        private Mock<IPlayerMapper> _mockPlayerMapper;

        // Requirement: Use OneTimeSetUp attribute on top of the init method
        [OneTimeSetUp]
        public void Init()
        {
            // Initializing our virtual mock database wrapper
            _mockPlayerMapper = new Mock<IPlayerMapper>();
        }

        // Requirement: Use TestCase attribute class on top of the test method execution
        [TestCase("Sachin")]
        [TestCase("Virat")]
        public void RegisterNewPlayer_WhenNameDoesNotExist_ReturnsValidPlayerWithAttributes(string validPlayerName)
        {
            // Requirement: Configure mock object to return "false" when checking if name exists
            _mockPlayerMapper.Reset(); // Clear any previous configuration calls
            _mockPlayerMapper.Setup(mapper => mapper.IsPlayerNameExistsInDb(validPlayerName))
                             .Returns(false);

            // Act: Fire our static factory method injection pathway
            Player createdPlayer = Player.RegisterNewPlayer(validPlayerName, _mockPlayerMapper.Object);

            // Requirement: Assert various player attributes
            Assert.That(createdPlayer, Is.Not.Null);
            Assert.That(createdPlayer.Name, Is.EqualTo(validPlayerName));
            Assert.That(createdPlayer.Age, Is.EqualTo(23));
            Assert.That(createdPlayer.Country, Is.EqualTo("India"));
            Assert.That(createdPlayer.NoOfMatches, Is.EqualTo(30));

            // Verify our mock securely intercepted the application interaction pipeline
            _mockPlayerMapper.Verify(mapper => mapper.AddNewPlayerIntoDb(validPlayerName), Times.Once);
        }

        [TestCase("")]
        [TestCase("   ")]
        public void RegisterNewPlayer_WhenNameIsEmpty_ThrowsArgumentException(string invalidPlayerName)
        {
            // Replaces legacy [ExpectedException] behavior safely in modern NUnit framework
            var exception = Assert.Throws<ArgumentException>(() => 
            {
                Player.RegisterNewPlayer(invalidPlayerName, _mockPlayerMapper.Object);
            });

            Assert.That(exception.Message, Is.EqualTo("Player name can’t be empty."));
        }

        [TestCase("DuplicatePlayer")]
        public void RegisterNewPlayer_WhenNameAlreadyExists_ThrowsArgumentException(string existingName)
        {
            // Force mock configuration to return true to test the collision pipeline edge-case
            _mockPlayerMapper.Setup(mapper => mapper.IsPlayerNameExistsInDb(existingName))
                             .Returns(true);

            var exception = Assert.Throws<ArgumentException>(() => 
            {
                Player.RegisterNewPlayer(existingName, _mockPlayerMapper.Object);
            });

            Assert.That(exception.Message, Is.EqualTo("Player name already exists."));
        }
    }
}