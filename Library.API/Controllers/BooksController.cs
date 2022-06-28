using Library.API.Data;

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
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

       
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

        
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

        
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

        
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}

