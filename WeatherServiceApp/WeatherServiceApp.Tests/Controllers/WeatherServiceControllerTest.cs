using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WeatherServiceApp.Controllers;

namespace WeatherServiceApp.Tests.Controllers
{
    [TestClass]
    public class WeatherServiceControllerTest
    {
        [TestMethod]
        public void WeatherAppIndex()
        {
            // Arrange
            WeatherServiceController controller = new WeatherServiceController();
            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
