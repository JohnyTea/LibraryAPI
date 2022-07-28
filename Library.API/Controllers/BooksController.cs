using Library.API.Data;
using Library.API.Models;
using Library.API.Services;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Library.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly ApplicationDataBaseContext _context;
    private readonly ILibraryService _library;


    public BooksController(ApplicationDataBaseContext context)
    {
        _context = context;
        _library = new LibraryService(_context);
    }

    // GET: api/Books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> Get()
    {
        return await _library.Books.Get();
    }

    // GET: api/Books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> Get(int id)
    {
        return await _library.Books.Get(id);
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<Book>> Post([FromBody] BookDto book)
    {
        Book newBook = new()
        {
            Author = book.Author,
            Title = book.Title,
            Publisher = book.Publisher,
            Year = book.Year,
            ISBN = book.ISBN,
        };

        await _library.Books.Add(newBook);
        await _context.SaveChangesAsync();
        return CreatedAtAction("Get", new { id = newBook.Id }, newBook);
    }

    // PUT: api/Users/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] BookDto book)
    {
        Book updatedBook = new()
        {
            Author = book.Author,
            Title = book.Title,
            Publisher = book.Publisher,
            Year = book.Year,
            ISBN = book.ISBN,
        };

        if (await BookExist(id))
        {
            await _library.Books.Edit(id, updatedBook);
        }
        else
        {
            await _library.Books.Add(updatedBook);
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _library.Books.Delete(id);
        await _context.SaveChangesAsync();
        return Ok();
    }

    private async Task<bool> BookExist(int id)
    {
        try {
            await _library.Books.Get(id);
            return true;
        }
        catch (ElementNotFoundException)
        {
            return false;
        }
    }
}

