using Library.API;
using Library.API.Controllers;

namespace Library.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            string[] Summaries = new[]
            {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            var controller = new WeatherForecastController(null);

            //Act
            var weatherForNextWeek = controller.Get();

            //Assert

            Assert.NotNull(weatherForNextWeek);
            Assert.True(weatherForNextWeek.Count() == 5);


            for(int i = 0; i<5; i++)
            {
                WeatherForecast? weatherDay = weatherForNextWeek.ElementAt(i);
                Assert.NotNull(weatherDay);
                Assert.True(weatherDay.Date.Day == DateTime.Now.AddDays(i+1).Day);
                Assert.True(weatherDay.TemperatureC >= -22);
                Assert.True(weatherDay.TemperatureC < 55);
                Assert.Contains(weatherDay.Summary, Summaries);
            }

        }

        
    }
}