using Library.API.Data;

namespace Library.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ApplicationDataBaseContext _context;
    private readonly ILibraryService _library;

    public UsersController(ApplicationDataBaseContext context)
    {
        _context = context;
        _library = new LibraryService(_context);
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        return await _library.Users.GetAllAsync();
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(int id)
    {
        return await _library.Users.GetByIdAsync(id);
    }

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<User>> Post([FromBody] UserDto user)
    {
        User newUser = new()
        {
            UserName = user.UserName,
            BirthDate = user.BirthDate
        };

        await _library.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetUser", new { id = newUser.Id }, newUser);
    }

    // PUT: api/Users/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, UserDto user)
    {
        User updatedUser = new()
        {
            UserName = user.UserName,
            BirthDate = user.BirthDate
        };

        if (await UserExistsAsync(id))
        {
            await _library.Users.EditAsync(id, updatedUser);
        }
        else
        {
            await _library.Users.AddAsync(updatedUser);
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _library.Books.DeleteAsync(id);
        await _context.SaveChangesAsync();
        return Ok();
    }

    // POST: api/Book/Borrow
    [HttpPost("BorrowBook")]
    public async Task<IActionResult> Borrow([FromBody] BorrowBookDto request) {

        await _library.Borrow(request.UserID, request.BookID);
        await _context.SaveChangesAsync();
        return Ok();
    }

    //Delete: api/Book/Return
    [HttpDelete("Return/{bookID}")]
    public async Task<IActionResult> Return(int bookID)
    {
        await _library.Return(bookID);
        await _context.SaveChangesAsync();
        return Ok();
    }

    private async Task<bool> UserExistsAsync(int id)
    {
        try
        {
            await _library.Users.GetByIdAsync(id);
            return true;
        }
        catch (ArgumentNullException) {
            return false;
        }
    }
}

