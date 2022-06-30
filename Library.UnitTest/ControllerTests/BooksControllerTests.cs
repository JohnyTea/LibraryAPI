using Library.API;
using Library.API.Controllers;
using Library.API.Data;
using Library.API.Models;
using Library.UnitTest.Mocks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Library.UnitTest.ControllerTests
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

            //TODO Check if _context.book.count is same before and after and if "dziady" or empty is not found
        }

        [Theory]
        [InlineData(2, "Przedwiośnie", "Stefan Żeromski", "Nowa Era", 1924, "9781429498876")]
        [InlineData(2, "Dziady", "Adam Mickiewicz", null, 1820, "9781911414001")]
        [InlineData(2, "Dziady", "Adam Mickiewicz", "Wydawca", 1820, "9781911414001")]
        [InlineData(2, "Dziady", "Adam Mickiewicz", "Wydawca", null, "9781911414001")]
        public void PutShouldUpdateBook(int id, string title, string author, string publisher, int? year, string isbn)
        {
            var changedBook = new Book
            {
                Id = id,
                Title = title,
                Author = author,
                Publisher = publisher,
                Year = year,
                ISBN = isbn
            };
            var controller = new BooksController(_context);
            var booksCount = _context.Books.Count();

            controller.Put(changedBook);

            Assert.Equal(_context.Books.Count(), booksCount);
            var addedBook = _context.Books.Where((book) => book.Id == changedBook.Id);
            Assert.NotNull(addedBook);
            Assert.Equal(1, addedBook.Count());
            Assert.StrictEqual(changedBook, addedBook.First());
        }

        [Theory]
        [InlineData(6, "Przedwiośnie", "Stefan Żeromski", "Nowa Era", 1924, "9781429498876")]
        [InlineData(6, "Dziady", "Adam Mickiewicz", null, 1820, "9781911414001")]
        [InlineData(6, "Dziady", "Adam Mickiewicz", "Wydawca", 1820, "9781911414001")]
        [InlineData(6, "Dziady", "Adam Mickiewicz", "Wydawca", null, "9781911414001")]
        public void PutShouldAddBook(int id, string title, string author, string publisher, int? year, string isbn)
        {
            var newBook = new Book
            {
                Id = id,
                Title = title,
                Author = author,
                Publisher = publisher,
                Year = year,
                ISBN = isbn
            };
            var controller = new BooksController(_context);
            var booksCount = _context.Books.Count();

            controller.Put(newBook);

            Assert.Equal(booksCount + 1, _context.Books.Count());
            var addedBook = _context.Books.Where((book) => book.Id == newBook.Id);
            Assert.NotNull(addedBook);
            Assert.Equal(1, addedBook.Count());
            Assert.StrictEqual(newBook, addedBook.First());
        }

        [Theory]
        [InlineData(2, null, null, null, null, null)]
        [InlineData(2, "", "", "", null, "")]
        [InlineData(2, "", "", "", 0, "")]
        [InlineData(2, null, "Stefan Żeromski", "Nowa Era", 1924, "9781429498876")]
        [InlineData(2, "Dziady", "Adam Mickiewicz", "Wydawca", 1820, null)]
        [InlineData(2, "Dziady", "Adam Mickiewicz", "Wydawca", 1820, "97819")]
        [InlineData(2, "Dziady", "Adam Mickiewicz", "Wydawca", 0, "9781911414001")]
        [InlineData(2, "Dziady", "Adam Mickiewicz", "Wydawca", null, "9781911414001")]
        [InlineData(2, "", "Adam Mickiewicz", "Wydawca", 0, "9781911414001")]
        public void PutShouldFailToUpdateBook(int id, string title, string author, string publisher, int? year, string isbn)
        {
            var changedBook = new Book
            {
                Id = id,
                Title = title,
                Author = author,
                Publisher = publisher,
                Year = year,
                ISBN = isbn
            };
            var controller = new BooksController(_context);

            controller.Put(changedBook);

            //TODO Add Asserst
        }

        [Theory]
        [InlineData(6, null, null, null, null, null)]
        [InlineData(6, "", "", "", null, "")]
        [InlineData(6, "", "", "", 0, "")]
        [InlineData(6, null, "Stefan Żeromski", "Nowa Era", 1924, "9781429498876")]
        [InlineData(6, "Dziady", "Adam Mickiewicz", "Wydawca", 1820, null)]
        [InlineData(6, "Dziady", "Adam Mickiewicz", "Wydawca", 1820, "97819")]
        [InlineData(6, "Dziady", "Adam Mickiewicz", "Wydawca", 0, "9781911414001")]
        [InlineData(6, "Dziady", "Adam Mickiewicz", "Wydawca", null, "9781911414001")]
        [InlineData(6, "", "Adam Mickiewicz", "Wydawca", 0, "9781911414001")]
        public void PutShouldFailToAddBook(int id, string title, string author, string publisher, int? year, string isbn)
        {
            var changedBook = new Book
            {
                Id = id,
                Title = title,
                Author = author,
                Publisher = publisher,
                Year = year,
                ISBN = isbn
            };
            var controller = new BooksController(_context);

            controller.Put(changedBook);

             //TODO Add Asserst
        }

        //TODO Write Delete Test

    }
}
