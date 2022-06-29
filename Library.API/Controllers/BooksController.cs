using Library.API.Data;
using Library.API.Models;

namespace Library.API.Controllers;

[Route("books")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly ApplicationDataBaseContext _db;

    public BooksController(ApplicationDataBaseContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IEnumerable<Book> Get()
    {
        throw new NotImplementedException();
    }

       
    [HttpGet("{id}")]
    public Book Get(int id)
    {
        throw new NotImplementedException();
    }

        
    [HttpPost]
    public int Post([FromBody] Book newBook)
    {
        throw new NotImplementedException();
    }

        
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Book updatedBook)
    {
        throw new NotImplementedException();
    }

        
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }
}

