

namespace Library.UnitTest.ControllerTests;

public class BooksServiceTests : TestBase
{

    [Fact]
    public async void GetAll_Should_ReturnAllBook()
    {

        var service = new BookService(_context);

        var result = await service.GetAllAsync();

        Assert.Equal(_context.Books.Count(), result.Count());

    }

    [Theory]
    [InlineData(2)]
    [InlineData(5)]
    public async void GetByID_Should_ReturnAllOne(int ID)
    {
        var correctBook = _context.Books.First(book => book.Id == ID);
        var service = new BookService(_context);

        var result = await service.GetByIdAsync(ID);

        Assert.NotNull(result);
        Assert.Equal(result.Id, ID);
        Assert.Equal(result.Title, correctBook.Title);
        Assert.Equal(result.ISBN, correctBook.ISBN);
        Assert.Equal(result.Year, correctBook.Year);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2345)]
    [InlineData(-1263)]
    [InlineData(-1)]
    public async Task GetByID_Should_ThrowElementNotFoundException(int ID)
    {
        var service = new BookService(_context);

        await Assert.ThrowsAsync<ElementNotFoundException>(() => service.GetByIdAsync(ID));
    }


    [Theory]
    [InlineData("Przedwiośnie", "Stefan Żeromski", "Nowa Era", 1924, "9781429498876")]
    [InlineData("Przedwiośnie", null, "Nowa Era", 1924, "9781429498876")]
    [InlineData("Przedwiośnie", "Stefan Żeromski", null, 1924, "9781429498876")]
    [InlineData("Przedwiośnie", "Stefan Żeromski", "Nowa Era", 0, "9781429498876")]
    public async void Add_Should_AddNewBook(string title, string author, string publisher, int year, string isbn)
    {
        BookService service = new(_context);
        int bookCountBeforePost = _context.Books.Count();
        Book newBook = new()
        {
            Author = author,
            Publisher = publisher,
            ISBN = isbn,
            Title = title,
            Year = year
        };

        await service.AddAsync(newBook);

        int bookCountAfterPost = _context.Books.Count();

        Assert.Equal(bookCountBeforePost + 1, bookCountAfterPost);
        var addedBook = _context.Books.FirstOrDefault(book =>
                book.Author == author &&
                book.Title == title &&
                book.ISBN == isbn &&
                book.Publisher == publisher &&
                book.Year == year
        );
        Assert.NotNull(addedBook);
    }

    [Fact]
    public async void Add_Should_AddExistingBook()
    {
        Book firstBook = _context.Books.First();
        Book bookToPost = new Book
        {
            Author = firstBook.Author,
            Title = firstBook.Title,
            ISBN = firstBook.ISBN,
            Publisher = firstBook.Publisher,
            Year = firstBook.Year
        };
        BookService service = new(_context);
        int bookCountBeforePost = _context.Books.Count();


        await service.AddAsync(bookToPost);


        int bookCountAfterPost = _context.Books.Count();

        Assert.Equal(bookCountBeforePost + 1, bookCountAfterPost);
        var addedBook = _context.Books.Where(book =>
                book.Author == bookToPost.Author &&
                book.Title == bookToPost.Title &&
                book.ISBN == bookToPost.ISBN &&
                book.Publisher == bookToPost.Publisher &&
                book.Year == bookToPost.Year
        );
        Assert.NotNull(addedBook);
        Assert.Equal(2, addedBook.Count());
    }

    [Theory]
    [InlineData(2, "Przedwiośnie", "Stefan Żeromski", "Nowa Era", 1924, "9781429498876")]
    [InlineData(2, "Dziady", null, "Nowa Era", 1924, "9781429498876")]
    [InlineData(2, "Dziady", "Adam Mickiewicz", null, 1924, "9781429498876")]
    [InlineData(2, "Dziady", "Adam Mickiewicz", "Nowa Era", 0, "9781429498876")]
    public async void Edit_Should_UpdateBook(int id, string title, string author, string publisher, int year, string isbn)
    {
        BookService service = new(_context);
        int booksCountBeforePut = _context.Books.Count();
        Book updatedBook = new()
        {
            Author = author,
            Publisher = publisher,
            ISBN = isbn,
            Title = title,
            Year = year
        };

        await service.EditAsync(id, updatedBook);

        int booksCountAfterPut = _context.Books.Count();

        Assert.Equal(booksCountBeforePut, booksCountAfterPut);
        var changedBook = _context.Books.Where((book) => book.Id == id);
        Assert.NotNull(changedBook);
        Assert.Equal(1, changedBook.Count());
        Assert.StrictEqual(updatedBook, changedBook.First());
    }

    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    public async void Delete_Should_DeleteBook(int id)
    {
        int oldBooksCount = _context.Books.Count();
        BookService service = new(_context);

        await service.DeleteByIdAsync(id);

        int newBooksCount = _context.Books.Count();
        Assert.Equal(oldBooksCount-1, newBooksCount);
        var deletedBook = _context.Books.FirstOrDefault((book) => book.Id == id);
        Assert.Null(deletedBook);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(132)]
    public async void Delete_Should_NotDeleteBook(int id)
    {
        int oldBooksCount = _context.Books.Count();
        BookService service = new(_context);

        await service.DeleteByIdAsync(id);

        int newBooksCount = _context.Books.Count();
        Assert.Equal(oldBooksCount, newBooksCount);
        var deletedBook = _context.Books.FirstOrDefault((book) => book.Id == id);
        Assert.Null(deletedBook);
    }

}
