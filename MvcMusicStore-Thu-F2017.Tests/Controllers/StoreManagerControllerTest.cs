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

        [TestMethod]
        public void DetailsInvalidNoId()
        {
            // Arrange
            int? id = null;

            // Act
            var actual = controller.Details(id);

            // Assert
            Assert.AreEqual("Error", actual.ViewName);
        }

        // GET: Edit
        [TestMethod]
        public void EditAlbumValid()
        {
            // ACT
            var actual = (Album)controller.Edit(1).Model;

            // ASSERT
            Assert.AreEqual(albums.ToList()[0], actual);
        }

        [TestMethod]
        public void EditInvalidNoId()
        {
            //arrange
            int? id = null;

            //act
            var actual = (ViewResult)controller.Edit(id);

            //assert
            Assert.AreEqual("Error", actual.ViewName);
        }

        [TestMethod]
        public void EditInvalidAlbumId()
        {
            // Act
            ViewResult result = controller.Edit(-314) as ViewResult;

            // Assert
            Assert.AreEqual("Error", result.ViewName);
        }

        // GET: Delete
        [TestMethod]
        public void DeleteValidAlbum()
        {
            // Act            
            var actual = (Album)controller.Delete(1).Model;

            // Assert            
            Assert.AreEqual(albums.ToList()[0], actual);
        }

        // Delete invalid ID test
        [TestMethod]
        public void DeleteInvalidAlbumId()
        {
            // Arrange
            int id = 87656765;

            // Act
            ViewResult actual = (ViewResult)controller.Delete(id);

            // Assert
            Assert.AreEqual("Error", actual.ViewName);
        }

        [TestMethod]
        public void DeleteInvalidNoId()
        {
            // arrange           
            int? id = null;

            // act           
            ViewResult actual = controller.Delete(id);

            // assert           
            Assert.AreEqual("Error", actual.ViewName);
        }

        // GET: Create
        [TestMethod]
        public void CreateViewLoads()
        {
            // act - cast the return type as ViewResult
            ViewResult actual = (ViewResult)controller.Create();

            // assert
            Assert.AreEqual("Create", actual.ViewName);
            Assert.IsNotNull(actual.ViewBag.ArtistId);
            Assert.IsNotNull(actual.ViewBag.GenreId);
        }

        // POST: Create
        [TestMethod]
        public void CreateValidAlbum()
        {
            // arrange
            Album album = new Album
            {
                AlbumId = 4,
                Title = "Album 4",
                Price = 4
            };

            // act
            RedirectToRouteResult actual = (RedirectToRouteResult)controller.Create(album);

            // assert
            Assert.AreEqual("Index", actual.RouteValues["action"]);
        }
       
        [TestMethod]
        public void CreateInvalidAlbum()
        {
            // arrange
            controller.ModelState.AddModelError("key", "error message");

            Album album = new Album
            {
                AlbumId = 4,
                Title = "Album 4",
                Price = 4
            };

            // act - cast the return type as ViewResult
            ViewResult actual = (ViewResult)controller.Create(album);

            // assert
            Assert.AreEqual("Create", actual.ViewName);
            Assert.IsNotNull(actual.ViewBag.ArtistId);
            Assert.IsNotNull(actual.ViewBag.GenreId);
        }

        // POST: Edit
        [TestMethod]
        public void EditValidAlbum()
        {
            // arrange
            Album album = albums.ToList()[0];

            // act
            RedirectToRouteResult actual = (RedirectToRouteResult)controller.Edit(album);

            // assert
            Assert.AreEqual("Index", actual.RouteValues["action"]);
        }

        [TestMethod]
        public void EditInvalidAlbum()
        {
            // arrange
            controller.ModelState.AddModelError("key", "error message");

            Album album = new Album
            {
                AlbumId = 4,
                Title = "Album 4",
                Price = 4
            };

            // act - cast the return type as ViewResult
            ViewResult actual = (ViewResult)controller.Edit(album);

            // assert
            Assert.AreEqual("Edit", actual.ViewName);
            Assert.IsNotNull(actual.ViewBag.ArtistId);
            Assert.IsNotNull(actual.ViewBag.GenreId);
        }

        // POST: DeleteConfirmed
        [TestMethod]
        public void DeleteConfirmedValidAlbum()
        {
            // Act            
            RedirectToRouteResult actual = (RedirectToRouteResult)controller.DeleteConfirmed(1);

            // Assert            
            Assert.AreEqual("Index", actual.RouteValues["action"]);
        }

        // Delete invalid ID test
        [TestMethod]
        public void DeleteConfirmedInvalidAlbumId()
        {
            // Arrange
            int id = 87656765;

            // Act
            ViewResult actual = (ViewResult)controller.DeleteConfirmed(id);

            // Assert
            Assert.AreEqual("Error", actual.ViewName);
        }

        [TestMethod]
        public void DeleteConfirmedInvalidNoId()
        {
            // arrange           
            int? id = null;

            // act           
            ViewResult actual = (ViewResult)controller.DeleteConfirmed(id);

            // assert           
            Assert.AreEqual("Error", actual.ViewName);
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
