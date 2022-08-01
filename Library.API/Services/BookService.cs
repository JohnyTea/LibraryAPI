using Library.API.Data;
using Library.API.Exceptions;

namespace Library.API.Services;

public class BookService : IBookService
{
    private readonly ApplicationDataBaseContext _context;

    public BookService(ApplicationDataBaseContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllAsync()
    {
        var books = await _context.Books.Include(b => b.Borrower).ToListAsync();
        if (books is null) {
            throw new ElementNotFoundException();
        }
        return books;
    }

    public async Task<Book> GetByIdAsync(int id)
    {
        var book = await _context.Books.Include(b => b.Borrower).FirstOrDefaultAsync(book => book.Id == id);
        if (book is null)
        {
            throw new ElementNotFoundException("Book not found");
        }
        return book;
    }

    public async Task AddAsync(Book newBook)
    {  
        await _context.Books.AddAsync(newBook);
        await _context.SaveChangesAsync();
    }

    public async Task EditAsync(int id, Book updatedBook)
    {
        var oldBook = await _context.Books.FirstOrDefaultAsync(book => book.Id == id);
        if (oldBook is null) {
            throw new ElementNotFoundException();
        }

        oldBook.Author = updatedBook.Author;
        oldBook.Title = updatedBook.Title;
        oldBook.ISBN = updatedBook.ISBN;
        oldBook.Publisher = updatedBook.Publisher;
        oldBook.Year = updatedBook.Year;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var book = await _context.Books.FirstOrDefaultAsync(book => book.Id == id);
        if (book is null) { return; }
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }
}
