using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using MagicFilesLib;

namespace DirectoryExplorer.Tests
{
    // Requirement: Use TestFixture attribute on top of the test class
    [TestFixture]
    public class DirectoryExplorerTests
    {
        private Mock<IDirectoryExplorer> _mockDirectoryExplorer;

        // Requirement: Add the following declarations in the test class
        private readonly string _file1 = "file.txt";
        private readonly string _file2 = "file2.txt";

        // Requirement: Use OneTimeSetUp attribute on top of the init method
        [OneTimeSetUp]
        public void Init()
        {
            // Instantiating the mock object wrapper
            _mockDirectoryExplorer = new Mock<IDirectoryExplorer>();

            // Hardcode our collection array of sample filenames
            var mockFilesList = new List<string> { _file1, _file2 };

            // Requirement: Configure the mock object to return our list when GetFiles is called
            _mockDirectoryExplorer
                .Setup(explorer => explorer.GetFiles(It.IsAny<string>()))
                .Returns(mockFilesList);
        }

        // Requirement: Use TestCase attribute class on top of the test method
        [TestCase(@"C:\TargetFolder")]
        [TestCase(@"/mock/path/directory")]
        public void GetFiles_WhenExecuted_AssertsFileSystemProperties(string pathInput)
        {
            // Act: Fire the method using our mock instance implementation
            ICollection<string> testResult = _mockDirectoryExplorer.Object.GetFiles(pathInput);

            // Requirements: Assert the following rules explicitly
            // 1. the collection is not null
            Assert.That(testResult, Is.Not.Null);

            // 2. the collection count is equal to 2
            Assert.That(testResult.Count, Is.EqualTo(2));

            // 3. the collection contains _file1
            Assert.That(testResult, Contains.Item(_file1));
        }
    }
}