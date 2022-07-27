using Library.API.Data;
using Library.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Services;

class BookService : IBookService
{
    private readonly ApplicationDataBaseContext _context;

    public BookService(ApplicationDataBaseContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> Get()
    {
        return await _context.Books.Include(b => b.Borrower).ToListAsync();
    }

    public async Task<Book> Get(int id)
    {
        return await _context.Books.Include(b => b.Borrower).FirstOrDefaultAsync(book => book.Id == id);
    }

    public async Task Add(Book newBook)
    {
       await _context.Books.AddAsync(newBook);
    }

    public async Task Edit(int id, Book updatedBook)
    {
        var oldBook = await _context.Books.FirstOrDefaultAsync(book => book.Id == id);
        if (oldBook is null) return;

        oldBook.Author = updatedBook.Author;
        oldBook.Title = updatedBook.Title;
        oldBook.ISBN = updatedBook.ISBN;
        oldBook.Publisher = updatedBook.Publisher;
        oldBook.Year = updatedBook.Year;
    }

    public async Task Delete(int id)
    {
        var book = await _context.Books.FirstOrDefaultAsync(book => book.Id == id);
        if (book is not null) _context.Books.Remove(book);
    }
}
