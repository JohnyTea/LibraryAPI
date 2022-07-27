using Library.API.Data;
using Library.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Services
{
    class UserService : IUserService
    {
        private readonly ApplicationDataBaseContext _context;

        public UserService(ApplicationDataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<User>> Get()
        {
            return await _context.Users.Include(b => b.Books).ToListAsync();
        }

        public async Task<User> Get(int id)
        {
            return await _context.Users.Include(b => b.Books).FirstOrDefaultAsync(book => book.Id == id);
        }

        public async Task Add(User newUser)
        {
            await _context.Users.AddAsync(newUser);
        }

        public async Task Edit(int id, User updatedUser)
        {
            var oldUser = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (oldUser is null) return;
            oldUser.UserName = updatedUser.UserName;
            oldUser.BirthDate = updatedUser.BirthDate;
        }
        public async Task Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
            if(user is not null) _context.Users.Remove(user);
        }

    }
}