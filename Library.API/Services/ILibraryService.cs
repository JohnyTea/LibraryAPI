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
        public Task<List<User>> GetAllAsync();
        public Task<User> GetByIdAsync(int id);
        public Task AddAsync(User newUser);
        public Task EditAsync(int id, User updatedUser);
        public Task DeleteAsync(int id);
    }

    public interface IBookService
    {
        public Task<List<Book>> GetAllAsync();
        public Task<Book> GetByIdAsync(int id);
        public Task AddAsync(Book newBook);
        public Task EditAsync(int id, Book updatedBook);
        public Task DeleteAsync(int id);
    }
}