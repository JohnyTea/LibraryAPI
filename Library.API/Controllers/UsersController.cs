using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.API.Data;
using Library.API.Models;
using Library.API.Services;

namespace Library.API.Controllers
{
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
            return await _library.Users.Get();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            return await _library.Users.Get(id);
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

            await _library.Users.Add(newUser);
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

            if (await UserExists(id))
            {
                await _library.Users.Edit(id, updatedUser);
            }
            else
            {
                await _library.Users.Add(updatedUser);
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

        private async Task<bool> UserExists(int id)
        {
            try
            {
                await _library.Users.Get(id);
                return true;
            }
            catch (ArgumentNullException) {
                return false;
            }
        }
    }
}
