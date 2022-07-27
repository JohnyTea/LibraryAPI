using Library.API.Models;

namespace Library.API.Services
{
    public interface ILibraryService
    {
        public IBookService Books { get; }
        public IUserService Users { get; }

        public Task Borrow(int userID, int bookID);
        public Task Return(int bookID);
    }

    public interface IUserService
    {
        public Task<List<User>> Get();
        public Task<User> Get(int id);
        public Task Add(User newUser);
        public Task Edit(int id, User updatedUser);
        public Task Delete(int id);
    }

    public interface IBookService
    {
        public Task<List<Book>> Get();
        public Task<Book> Get(int id);
        public Task Add(Book newBook);
        public Task Edit(int id, Book updatedBook);
        public Task Delete(int id);
    }
}