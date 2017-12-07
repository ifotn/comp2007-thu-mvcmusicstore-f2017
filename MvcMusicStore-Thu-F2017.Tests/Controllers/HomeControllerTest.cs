using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcMusicStore_Thu_F2017;
using MvcMusicStore_Thu_F2017.Controllers;

namespace MvcMusicStore_Thu_F2017.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        HomeController controller;

        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void sumPass()
        {
            // Arrange
            HomeController controller = new HomeController();
            int x = 10;
            int y = 20;
            int expResult = 30;

            // Act
            int result = controller.sum(x, y);

            // Assert
            Assert.AreEqual(expResult, result);
        }

        [TestMethod]
        public void sumFail()
        {
            // Arrange
            HomeController controller = new HomeController();
            int x = 10;
            int y = 20;
            int expResult = 40;

            // Act
            int result = controller.sum(x, y);

            // Assert
            Assert.AreNotEqual(expResult, result);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            // instantiate the HomeController declared at the class level
            controller = new HomeController();
        }

        [TestMethod]
        public void getWeatherBelowZeroValid()
        {
            // Arrange
            int Temp = -11;

            // Act
            string result = controller.getWeather(Temp);

            // Assert
            Assert.AreEqual("I need a coffee", result);
        }

        [TestMethod]
        public void getWeatherBelowZeroInvalid()
        {
            // Arrange
            int Temp = -11;

            // Act
            string result = controller.getWeather(Temp);

            // Assert
            Assert.AreNotEqual("I need a hot chocolate", result);
        }

        [TestMethod]
        public void getWeatherAboveZeroValid()
        {
            // Arrange
            int Temp = 11;

            // Act
            string result = controller.getWeather(Temp);

            // Assert
            Assert.AreEqual("I need a tea", result);
        }

        [TestMethod]
        public void getWeatherAbove24()
        {
            // Arrange
            int Temp = 28;

            // Act
            string result = controller.getWeather(Temp);

            // Assert
            Assert.AreEqual("I need a beer", result);
        }

    }
}
