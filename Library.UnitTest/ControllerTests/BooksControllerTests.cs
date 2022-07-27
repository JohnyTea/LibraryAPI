using Library.API.Controllers;
using Library.API.Data;
using Library.API.Models;
using Library.UnitTest.Mocks;

namespace Library.UnitTest.ControllerTests
{
    public class BooksControllerTests : TestBase
    {
    #region Get
        [Fact]
        public async void GetShouldReturnAllBooks()
        {

            var controller = new BooksController(_context);

            var result = (await controller.Get()).Value;

            Assert.Equal(_context.Books.Count(), result.Count());

        }

        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        public async void GetShouldReturnBook(int ID)
        {
            var controller = new BooksController(_context);

            var result = (await controller.Get(ID)).Value;

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
        public async void GetShouldReturnNull(int ID)
        {
            var controller = new BooksController(_context);

            var result = (await controller.Get(ID)).Value;

            Assert.Null(result);
        }

        #endregion

        #region Post
        [Theory]
        [InlineData(1000, "Przedwiośnie", "Stefan Żeromski", "Nowa Era", 1924, "9781429498876")]
        [InlineData(1000, "Przedwiośnie", null, "Nowa Era", 1924, "9781429498876")]
        [InlineData(1000, "Przedwiośnie", "Stefan Żeromski", null, 1924, "9781429498876")]
        [InlineData(1000, "Przedwiośnie", "Stefan Żeromski", "Nowa Era", 0, "9781429498876")]
        public async void PostShouldAddBook(int id, string title, string author, string publisher, int year, string isbn)
        {
            BooksController controller = new(_context);
            int bookCountBeforePost = _context.Books.Count();
            BookDto newBook = new()
            {
                Author = author,
                Publisher = publisher,
                ISBN = isbn,
                Title = title,
                Year = year
            };

            var result = (await controller.Post(newBook));

            int bookCountAfterPost = _context.Books.Count();

            Assert.Equal(bookCountBeforePost + 1, bookCountAfterPost);
            Assert.NotNull(result);
            /*
            Assert.Equal(title, result.Title);
            Assert.Equal(author, result.Author);
            Assert.Equal(publisher, result.Publisher);
            Assert.Equal(year, result.Year);
            Assert.Equal(isbn, result.ISBN);
            */
        }

        [Fact]
        public async void PostShouldAddExistingBook()
        {
            Book bookToPost = _context.Books.First();
            bookToPost.Id = 0;
            BooksController controller = new(_context);
            int bookCountBeforePost = _context.Books.Count();

            var result = (await controller.Post(bookToPost)).Value;

            int bookCountAfterPost = _context.Books.Count();

            Assert.Equal(bookCountBeforePost + 1, bookCountAfterPost);
            Assert.NotNull(result);
            Assert.Equal(bookToPost.Title, result.Title);
            Assert.Equal(bookToPost.Author, result.Author);
            Assert.Equal(bookToPost.Publisher, result.Publisher);
            Assert.Equal(bookToPost.Year, result.Year);
            Assert.Equal(bookToPost.ISBN, result.ISBN);
        }

        #endregion

        #region Put

        [Theory]
        [InlineData(2, "Przedwiośnie", "Stefan Żeromski", "Nowa Era", 1924, "9781429498876")]
        [InlineData(2, "Dziady", null, "Nowa Era", 1924, "9781429498876")]
        [InlineData(2, "Dziady", "Adam Mickiewicz", null, 1924, "9781429498876")]
        [InlineData(2, "Dziady", "Adam Mickiewicz", "Nowa Era", 0, "9781429498876")]
        public async void PutShouldUpdateBook(int id, string title, string author, string publisher, int year, string isbn)
        {
            BooksController controller = new(_context);
            int booksCountBeforePut = _context.Books.Count();
            Book updatedBook = new()
            {
                Author = author,
                Publisher = publisher,
                ISBN = isbn,
                Title = title,
                Year = year
            };

            await controller.Put(id, updatedBook);

            int booksCountAfterPut = _context.Books.Count();

            var addedBook = _context.Books.Where((book) => book.Id == id);
            Assert.Equal(booksCountBeforePut, booksCountAfterPut);
            Assert.NotNull(addedBook);
            Assert.Equal(1, addedBook.Count());
            Assert.StrictEqual(updatedBook, addedBook.First());
        }

        [Theory]
        [InlineData(7, 0, "Przedwiośnie", "Stefan Żeromski", "Nowa Era", 1924, "9781429498876")]
        [InlineData(7, 0, "Dziady", null, "Nowa Era", 1924, "9781429498876")]
        [InlineData(7, 0, "Dziady", "Adam Mickiewicz", null, 1924, "9781429498876")]
        [InlineData(7, 0, "Dziady", "Adam Mickiewicz", "Nowa Era", 0, "9781429498876")]
        [InlineData(7, 7, "Dziady", "Adam Mickiewicz", "Nowa Era", 0, "9781429498876")]
        public async void PutShouldAddBook(int id, int bookID, string title, string author, string publisher, int year, string isbn)
        {
            Book newBook = new()
            {
                Id = bookID,
                Author = author,
                Publisher = publisher,
                ISBN = isbn,
                Title = title,
                Year = year
            };

            BooksController controller = new(_context);
            int booksCountBeforePut = _context.Books.Count();

            await controller.Put(id, newBook);
            int booksCountAfterPut = _context.Books.Count();

            var addedBook = _context.Books.Where((book) => book.Id == id);
            Assert.Equal(booksCountBeforePut + 1, booksCountAfterPut);
            Assert.NotNull(addedBook);
            Assert.Equal(1, addedBook.Count());
            Assert.StrictEqual(newBook, addedBook.First());
        }

        #endregion

        #region Delete
        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        public async void DeleteShouldDeleteBook(int id)
        {
            int oldBooksCount = _context.Books.Count();
            var controller = new BooksController(_context);

            await controller.Delete(id);

            int newBooksCount = _context.Books.Count();
            Assert.Equal(oldBooksCount-1, newBooksCount);
            var deletedBook = _context.Books.FirstOrDefault((book) => book.Id == id);
            Assert.Null(deletedBook);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(132)]
        public async void DeleteShouldNotDeleteBook(int id)
        {
            int oldBooksCount = _context.Books.Count();
            var controller = new BooksController(_context);

            await controller.Delete(id);

            int newBooksCount = _context.Books.Count();
            Assert.Equal(oldBooksCount, newBooksCount);
            var deletedBook = _context.Books.FirstOrDefault((book) => book.Id == id);
            Assert.Null(deletedBook);
        }

        #endregion

    }
}
