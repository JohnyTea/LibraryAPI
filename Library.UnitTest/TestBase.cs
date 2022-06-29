using Library.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.UnitTest
{
    public class TestBase : IDisposable
    {
        protected readonly ApplicationDataBaseContext _context;

        public TestBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDataBaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDataBaseContext(options);
            _context.Database.EnsureCreated();

            ApplicationDataBaseInitializer.Initialize(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}