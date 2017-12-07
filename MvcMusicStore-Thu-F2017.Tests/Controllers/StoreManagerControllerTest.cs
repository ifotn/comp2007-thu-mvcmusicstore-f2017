using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// additional references
using MvcMusicStore_Thu_F2017.Controllers;
using System.Web.Mvc;
using Moq;
using MvcMusicStore_Thu_F2017.Models;
using System.Linq;

namespace MvcMusicStore_Thu_F2017.Tests.Controllers
{

    [TestClass]
    public class StoreManagerControllerTest
    {
        StoreManagerController controller;
        Mock<IStoreManagerRepository> mock;
        List<Album> albums;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            mock = new Mock<IStoreManagerRepository>();

            // Mock the Album Data
            albums = new List<Album>
            {
                new Album { AlbumId = 1, Title = "Album 1", Price = 9, Artist = new Artist { ArtistId = 1, Name = "Artist 1" } },
                new Album { AlbumId = 2, Title = "Album 2", Price = 10, Artist = new Artist { ArtistId = 2, Name = "Artist 2"  } },
                new Album { AlbumId = 3, Title = "Album 3", Price = 9, Artist = new Artist { ArtistId = 3, Name = "Artist 3"  } }
            };

            // populate the mock object with our sample data
            mock.Setup(m => m.Albums).Returns(albums.AsQueryable());

            // Pass the mock to 2nd constructor
            controller = new StoreManagerController(mock.Object);
        }

        [TestMethod]
        public void IndexViewLoads()
        {
            // Arrange

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IndexReturnsAlbums()
        {
            // Act
            // call Index, set result to an Album List as specified in Index's Model
            var actual = (List<Album>)controller.Index().Model;

            // Assert
            // check if the list returned in the view matches the list we passed in to the mock
            CollectionAssert.AreEqual(albums, actual);
        }

        [TestMethod]
        public void DetailsValidAlbum()
        {
            // Act
            var actual = (Album)controller.Details(1).Model;

            // Assert
            Assert.AreEqual(albums.ToList()[0], actual);
        }

       [TestMethod]
       public void DetailsInvalidAlbum()
        {
            // Act
            var actual = (Album)controller.Details(11111).Model;

            // Assert
            Assert.IsNull(actual);
        }

        public StoreManagerControllerTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

       
    }
}
