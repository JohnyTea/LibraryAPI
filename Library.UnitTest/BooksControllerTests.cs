using Library.API;
using Library.API.Controllers;
using Library.API.Data;
using Library.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.UnitTest
{
    public class BooksControllerTests : TestBase
    {
        [Fact]
        public void ShouldReturnAllBooks()
        {
           
            var controller = new BooksController(_context);

            var result = controller.Get();

            Assert.Equal(6, result.Count()); 

        }
    }
}
