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

        try {
            var result = await _library.Books.Get();
            return result is not null ? result : NotFound();
        }
        catch (Exception) {
            return BadRequest();
        }
    }

    // GET: api/Books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> Get(int id)
    {
        try {
            var result = await _library.Books.Get(id);
            return result is not null ? result : NotFound();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<Book>> Post([FromBody] BookDto book)
    {
        try
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
        catch (NullReferenceException)
        {
            return Problem("Entity set 'ApplicationDataBaseContext.Book' is null.");
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    // PUT: api/Users/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] BookDto book)
    {
        try{
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
        catch (NullReferenceException)
        {
            return Problem("Entity set 'ApplicationDataBaseContext.Books' is null.");
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try {
            await _library.Books.Delete(id);
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception)
        {
            return Problem();
        }
        
    }

    private async Task<bool> BookExist(int id)
    {
        try {
            await _library.Books.Get(id);
            return true;
        }
        catch (ArgumentNullException)
        {
            return false;
        }
    }
}

