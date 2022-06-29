using Library.API;
using Library.API.Controllers;
using Library.API.Data;
using Library.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Library.UnitTest
{
    public class BooksControllerTests : TestBase
    {
        [Fact]
        public void GetShouldReturnAllBooks()
        {
           
            var controller = new BooksController(_context);

            var result = controller.Get();

            Assert.Equal(6, result.Count()); 

        }

        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        public void GetShouldReturnBook(int ID)
        {
            var controller = new BooksController(_context);

            var result = controller.Get(ID);

            Assert.NotNull(result);
            Assert.Equal(result.Id, ID);
            Assert.NotNull(result.Title);
            Assert.NotNull(result.ISBN);
            Assert.True(result.Year > 0);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2345)]
        [InlineData(-1263)]
        [InlineData(-1)]
        public void GetShouldReturnNull(int ID)
        {
            var controller = new BooksController(_context);

            var result = controller.Get(ID);

            Assert.Null(result);
        }

        [Theory]
        [InlineData("Przedwiośnie", "Stefan Żeromski", "Nowa Era", 1924, "9781429498876")]
        [InlineData("Dziady", "Adam Mickiewicz", null, 1820, "9781911414001")]
        [InlineData("Dziady", "Adam Mickiewicz", "Wydawca", 1820, "9781911414001")]
        [InlineData("Dziady", "Adam Mickiewicz", "Wydawca", null, "9781911414001")]
        public void PostShouldAddBook(string title, string author, string publisher, int year, string isbn)
        {
            var newBook = new Book{
                Title = title,
                Author = author,
                Publisher = publisher,
                Year = year,
                ISBN = isbn
            };

            var controller = new BooksController(_context);


            var result = controller.Post(newBook);


            Assert.True(result > 0);
        }

        [Theory]
        [InlineData(null, null, null, null, null)]
        [InlineData("", "", "", null, "")]
        [InlineData("", "", "", 0, "")]
        [InlineData(null, "Stefan Żeromski", "Nowa Era", 1924, "9781429498876")]
        [InlineData("Dziady", "Adam Mickiewicz", "Wydawca", 1820, null)]
        [InlineData("Dziady", "Adam Mickiewicz", "Wydawca", 1820, "97819")]
        [InlineData("Dziady", "Adam Mickiewicz", "Wydawca", 0, "9781911414001")]
        [InlineData("Dziady", "Adam Mickiewicz", "Wydawca", null, "9781911414001")]
        [InlineData("", "Adam Mickiewicz", "Wydawca", 0, "9781911414001")]
        public void PostShouldNotAddBook(string title, string author, string publisher, int? year, string isbn)
        {
            var newBook = new Book
            {
                Title = title,
                Author = author,
                Publisher = publisher,
                Year = year,
                ISBN = isbn
            };

            var controller = new BooksController(_context);


            var result = controller.Post(newBook);


            Assert.True(result == -1);
        }


    }
}
