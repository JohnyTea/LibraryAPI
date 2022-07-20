using Library.API.Controllers;
using Library.API.Models;
using Library.UnitTest.Mocks;

namespace Library.UnitTest.ControllerTests
{
    public class BooksControllerTests : TestBase
    {
    #region Get
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

        #endregion

        #region Post
        [Theory]
        [InlineData(1000, "Przedwiośnie", "Stefan Żeromski", "Nowa Era", 1924, "9781429498876")]
        [InlineData(1000, "Przedwiośnie", null, "Nowa Era", 1924, "9781429498876")]
        [InlineData(1000, "Przedwiośnie", "Stefan Żeromski", null, 1924, "9781429498876")]
        [InlineData(1000, "Przedwiośnie", "Stefan Żeromski", "Nowa Era", 0, "9781429498876")]
        public void PostShouldAddBook(int id, string title, string author, string publisher, int year, string isbn)
        {
            BooksController controller = new(_context);
            int bookCountBeforePost = _context.Books.Count();
            Book newBook = new()
            {
                Id = id,
                Author = author,
                Publisher = publisher,
                ISBN = isbn,
                Title = title,
                Year = year
            };

            controller.Post(newBook);
            int bookCountAfterPost = _context.Books.Count();

            Assert.Equal(bookCountBeforePost+1, bookCountAfterPost);
        }

        [Fact]
        public void PostShouldAddExistingBook()
        {
            Book bookToPost = _context.Books.First();
            bookToPost.Id = 0;
            BooksController controller = new(_context);
            int bookCountBeforePost = _context.Books.Count();

            controller.Post(bookToPost);
            int bookCountAfterPost = _context.Books.Count();

            Assert.Equal(bookCountBeforePost + 1, bookCountAfterPost);
        }

        [Theory]
        [InlineData("Dziady", "Adam Mickiewicz", "Nowa Era", 1926, "978142949887")]
        [InlineData("Dziady", "Adam Mickiewicz", "Nowa Era", 1926, "97814294988765")]
        public void PostShouldNotAddBook(string title, string author, string publisher, int year, string isbn)
        {
            int bookCountBeforePost = _context.Books.Count();
            var controller = new BooksController(_context);
            Book newBook = new()
            {
                Author = author,
                Publisher = publisher,
                ISBN = isbn,
                Title = title,
                Year = year
            };


            controller.Post(newBook);
            int bookCountAfterPost = _context.Books.Count();

            Assert.Equal(bookCountBeforePost, bookCountAfterPost);
        }

        #endregion

        #region Put

        [Theory]
        [InlineData(2, "Przedwiośnie", "Stefan Żeromski", "Nowa Era", 1924, "9781429498876")]
        [InlineData(2, "Dziady", null, "Nowa Era", 1924, "9781429498876")]
        [InlineData(2, "Dziady", "Adam Mickiewicz", null, 1924, "9781429498876")]
        [InlineData(2, "Dziady", "Adam Mickiewicz", "Nowa Era", 0, "9781429498876")]
        public void PutShouldUpdateBook(int id, string title, string author, string publisher, int year, string isbn)
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

            controller.Put(id, updatedBook);
            int booksCountAfterPut = _context.Books.Count();

            Assert.Equal(booksCountBeforePut, booksCountAfterPut);

            var addedBook = _context.Books.Where((book) => book.Id == id);
            Assert.NotNull(addedBook);
            Assert.Equal(1, addedBook.Count());
            Assert.StrictEqual(updatedBook, addedBook.First());
        }

        [Theory]
        [InlineData(7, "Przedwiośnie", "Stefan Żeromski", "Nowa Era", 1924, "9781429498876")]
        [InlineData(7, "Dziady", null, "Nowa Era", 1924, "9781429498876")]
        [InlineData(7, "Dziady", "Adam Mickiewicz", null, 1924, "9781429498876")]
        [InlineData(7, "Dziady", "Adam Mickiewicz", "Nowa Era", 0, "9781429498876")]
        public void PutShouldAddBook(int id, string title, string author, string publisher, int year, string isbn)
        {
            Book newBook = new()
            {
                Author = author,
                Publisher = publisher,
                ISBN = isbn,
                Title = title,
                Year = year
            };

            BooksController controller = new(_context);
            int booksCountBeforePut = _context.Books.Count();

            controller.Put(id, newBook);
            int booksCountAfterPut = _context.Books.Count();

            Assert.Equal(booksCountBeforePut + 1, booksCountAfterPut);

            var addedBook = _context.Books.Where((book) => book.Id == id);
            Assert.NotNull(addedBook);
            Assert.Equal(1, addedBook.Count());
            Assert.StrictEqual(newBook, addedBook.First());
        }

        [Theory]
        [InlineData(2, "Dziady", "Adam Mickiewicz", "Nowa Era", 1926, "978142949887")]
        [InlineData(2, "Dziady", "Adam Mickiewicz", "Nowa Era", 1926, "97814294988765")]
        public void PutShouldFailToUpdateBook(int id, string title, string author, string publisher, int year, string isbn)
        {
            Book incorectBook = new()
            {
                Author = author,
                Publisher = publisher,
                ISBN = isbn,
                Title = title,
                Year = year
            };
            int booksCountBeforePut = _context.Books.Count();
            var controller = new BooksController(_context);
            Book correctBookBeforePut = _context.Books.First((book) => book.Id == id);

            controller.Put(id, incorectBook);

            int booksCountAfterPut = _context.Books.Count();
            Assert.Equal(booksCountBeforePut, booksCountAfterPut);
            var correctBookAfterPut = _context.Books.Where((book) => book.Id == id);
            Assert.NotNull(correctBookAfterPut);
            Assert.Equal(1, correctBookAfterPut.Count());
            Assert.StrictEqual(correctBookBeforePut, correctBookAfterPut.FirstOrDefault());
        }

        [Theory]
        [InlineData(7, "Dziady", "Adam Mickiewicz", "Nowa Era", 1926, "978142949887")]
        [InlineData(7, "Dziady", "Adam Mickiewicz", "Nowa Era", 1926, "97814294988765")]
        public void PutShouldFailToAddBook(int id, string title, string author, string publisher, int year, string isbn)
        {
            Book incorectBook = new()
            {
                Author = author,
                Publisher = publisher,
                ISBN = isbn,
                Title = title,
                Year = year
            };
            int booksCountBeforePut = _context.Books.Count();
            var controller = new BooksController(_context);

            controller.Put(id, incorectBook);

            int booksCountAfterPut = _context.Books.Count();
            Assert.Equal(booksCountBeforePut, booksCountAfterPut);
        }

        #endregion

        #region Delete
        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        public void DeleteShouldDeleteBook(int id)
        {
            int oldBooksCount = _context.Books.Count();
            var controller = new BooksController(_context);

            controller.Delete(id);

            int newBooksCount = _context.Books.Count();
            Assert.Equal(oldBooksCount-1, newBooksCount);
            var deletedBook = _context.Books.FirstOrDefault((book) => book.Id == id);
            Assert.Null(deletedBook);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(132)]
        public void DeleteShouldNotDeleteBook(int id)
        {
            int oldBooksCount = _context.Books.Count();
            var controller = new BooksController(_context);

            controller.Delete(id);

            int newBooksCount = _context.Books.Count();
            Assert.Equal(oldBooksCount, newBooksCount);
            var deletedBook = _context.Books.FirstOrDefault((book) => book.Id == id);
            Assert.Null(deletedBook);
        }

        #endregion

    }
}
