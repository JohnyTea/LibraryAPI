using Library.API.Data;
using Library.API.Exceptions;
namespace Library.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookControler : ControllerBase
{
    private readonly ApplicationDataBaseContext context;
    private readonly ILibraryService library;

    public BookControler(ApplicationDataBaseContext context)
    {
        this.context = context;
        library = new LibraryService(this.context);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> Get()
    {
        return await library.Books.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> Get(int id)
    {
        return await library.Books.GetByIdAsync(id);
    }

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

        await library.Books.AddAsync(newBook);
        await context.SaveChangesAsync();
        return CreatedAtAction("Get", new { id = newBook.Id }, newBook);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] BookDto book)
    {
        Book updatedBook = new()
        {
            Id = id,
            Author = book.Author,
            Title = book.Title,
            Publisher = book.Publisher,
            Year = book.Year,
            ISBN = book.ISBN,
        };

        if (await BookExistAsync(id))
        {
            await library.Books.EditAsync(id, updatedBook);
        }
        else
        {
            await library.Books.AddAsync(updatedBook);
        }

        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await library.Books.DeleteByIdAsync(id);
        await context.SaveChangesAsync();
        return Ok();
    }

    private async Task<bool> BookExistAsync(int id)
    {
        try {
            await library.Books.GetByIdAsync(id);
            return true;
        }
        catch (ElementNotFoundException)
        {
            return false;
        }
    }
}

