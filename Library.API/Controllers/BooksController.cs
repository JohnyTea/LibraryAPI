using Library.API.Data;
using Library.API.Models;
using System.ComponentModel.DataAnnotations;

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
        return _db.Books;
    }

       
    [HttpGet("{id}")]
    public Book Get(int id)
    {
        return _db.Books.FirstOrDefault(book => book.Id == id);
    }

        
    [HttpPost]
    public void Post([FromBody] Book newBook)
    {
        var context = new ValidationContext(newBook, serviceProvider: null, items: null);
        var validationResults = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(newBook, context, validationResults, true);
        if (!isValid) {
            return;
        }
        _db.Books.Add(newBook);
        _db.SaveChanges();
    }

        
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Book updatedBook)
    {
        var context = new ValidationContext(updatedBook, serviceProvider: null, items: null);
        var validationResults = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(updatedBook, context, validationResults, true);
        if (!isValid)
        {
            return;
        }

        bool bookExist = _db.Books.Any(book => book.Id == id);
        if (bookExist)
        {
            UpdateBookAt(id, updatedBook);
        }
        else
        {
            _db.Books.Add(updatedBook);
        }
        _db.SaveChanges();
    }

    private void UpdateBookAt(int id, Book updatedBook)
    {
        var bookToUpdate = _db.Books.First(book => book.Id == id);
        bookToUpdate.Author = updatedBook.Author;
        bookToUpdate.Publisher = updatedBook.Publisher;
        bookToUpdate.Title = updatedBook.Title;
        bookToUpdate.Year = updatedBook.Year;
        bookToUpdate.ISBN = updatedBook.ISBN;
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        var bookToRemove = _db.Books.FirstOrDefault(book => book.Id == id);
        if(bookToRemove is not null)
        {
            _db.Remove(bookToRemove);
        }
        _db.SaveChanges();
    }
}

