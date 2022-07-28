using Library.API.Data;

namespace Library.API.Services;

public class LibraryService : ILibraryService
{
    private readonly ApplicationDataBaseContext _context;

    public IBookService Books { get; private set; }

    public IUserService Users { get; private set; }

    public LibraryService(ApplicationDataBaseContext context)
    {
        _context = context;
        Books = new BookService(_context);
        Users = new UserService(_context);
    }
    public async Task Borrow(int userID, int bookID)
    {
        var book = await Books.Get(bookID);
        var user = await Users.Get(userID);

        book.SetBorrower(user);
    }

    public async Task Return(int bookID)
    {
        var book = await Books.Get(bookID);

        book.RemoveBorrower();
    }
}

