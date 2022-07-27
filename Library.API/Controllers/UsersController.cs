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
            try
            {
                var result = await _library.Users.Get();
                return result is not null ? result : NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            try
            {
                var result = await _library.Users.Get(id);
                return result is not null ? result : NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] UserDto user)
        {
            try
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
            catch (NullReferenceException)
            {
                return Problem("Entity set 'ApplicationDataBaseContext.Book' is null.");
            }
            catch (Exception)
            {
                return Problem();
            }

            
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UserDto user)
        {
            try
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
            catch (NullReferenceException)
            {
                return Problem("Entity set 'ApplicationDataBaseContext.Users' is null.");
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _library.Books.Delete(id);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        // POST: api/Book/Borrow
        [HttpPost("BorrowBook")]
        public async Task<IActionResult> Borrow([FromBody] BorrowBookDto request) {

            try
            {
                await _library.Borrow(request.UserID, request.BookID);
                await _context.SaveChangesAsync();
                return Ok();

            }
            catch (ArgumentNullException) {
                return NotFound();
            }
        }

        //Delete: api/Book/Return
        [HttpDelete("Return/{id}")]
        public async Task<IActionResult> Return(int bookID)
        {

            try
            {
                await _library.Return(bookID);
                await _context.SaveChangesAsync();
                return Ok();

            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
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
