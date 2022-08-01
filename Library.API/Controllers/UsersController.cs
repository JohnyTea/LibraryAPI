using Library.API.Data;
using Library.API.Exceptions;

namespace Library.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ApplicationDataBaseContext context;
    private readonly ILibraryService library;

    public UsersController(ApplicationDataBaseContext context)
    {
        this.context = context;
        library = new LibraryService(this.context);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        return await library.Users.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(int id)
    {
        return await library.Users.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Post([FromBody] UserDto user)
    {
        User newUser = new()
        {
            UserName = user.UserName,
            BirthDate = user.BirthDate
        };

        await library.Users.AddAsync(newUser);
        await context.SaveChangesAsync();
        return CreatedAtAction("GetUser", new { id = newUser.Id }, newUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, UserDto user)
    {
        User updatedUser = new()
        {
            Id = id,
            UserName = user.UserName,
            BirthDate = user.BirthDate
        };

        if (await UserExistsAsync(id))
        {
            await library.Users.EditAsync(id, updatedUser);
        }
        else
        {
            await library.Users.AddAsync(updatedUser);
        }

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await library.Books.DeleteByIdAsync(id);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("BorrowBook")]
    public async Task<IActionResult> Borrow([FromBody] BorrowedBookDto request) {

        await library.Borrow(request.UserID, request.BookID);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("Return/{bookID}")]
    public async Task<IActionResult> Return(int bookID)
    {
        await library.Return(bookID);
        await context.SaveChangesAsync();
        return Ok();
    }

    private async Task<bool> UserExistsAsync(int id)
    {
        try
        {
            await library.Users.GetByIdAsync(id);
            return true;
        }
        catch (ElementNotFoundException) {
            return false;
        }
    }
}

