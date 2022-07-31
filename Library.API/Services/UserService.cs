using Library.API.Data;
using Library.API.Exceptions;

namespace Library.API.Services;

public class UserService : IUserService
{
    private readonly ApplicationDataBaseContext _context;

    public UserService(ApplicationDataBaseContext context)
    {
        _context = context;
    }
    public async Task<List<User>> GetAllAsync()
    {
        var users = await _context.Users.Include(b => b.Books).ToListAsync();
        if (users is null) {
            throw new ElementNotFoundException();
        }
        return users;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var user = await _context.Users.Include(b => b.Books).FirstOrDefaultAsync(book => book.Id == id);
        if (user is null) {
            throw new ElementNotFoundException();
        }
        return user;
    }

    public async Task AddAsync(User newUser)
    {
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
    }

    public async Task EditAsync(int id, User updatedUser)
    {
        var oldUser = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        if (oldUser is null) {
            throw new ElementNotFoundException();
        }
        oldUser.UserName = updatedUser.UserName;
        oldUser.BirthDate = updatedUser.BirthDate;

        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        if (user is  null) { return; }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

}
