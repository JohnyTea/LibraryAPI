using Library.API.Data;
using Library.API.Exceptions;
namespace Library.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookControler : ControllerBase
{
    private readonly ApplicationDataBaseContext _context;
    private readonly ILibraryService _library;

    public BookControler(ApplicationDataBaseContext context)
    {
        _context = context;
        _library = new LibraryService(_context);
    }

    // GET: api/Books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> Get()
    {
        return await _library.Books.GetAllAsync();
    }

    // GET: api/Books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> Get(int id)
    {
        return await _library.Books.GetByIdAsync(id);
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

        await _library.Books.AddAsync(newBook);
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

        if (await BookExistAsync(id))
        {
            await _library.Books.EditAsync(id, updatedBook);
        }
        else
        {
            await _library.Books.AddAsync(updatedBook);
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _library.Books.DeleteAsync(id);
        await _context.SaveChangesAsync();
        return Ok();
    }

    private async Task<bool> BookExistAsync(int id)
    {
        try {
            await _library.Books.GetByIdAsync(id);
            return true;
        }
        catch (ElementNotFoundException)
        {
            return false;
        }
    }
}

