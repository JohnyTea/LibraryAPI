using Library.API.Data;
using Library.API.Models;

namespace Library.UnitTest.Mocks
{
    internal class ApplicationDataBaseInitializer
    {
        internal static void Initialize(ApplicationDataBaseContext context)
        {
            AddBooksTo(context);
        }

        private static void AddBooksTo(ApplicationDataBaseContext context)
        {
            var books = new[] {
                new Book { Id = 6, Author = "Ashish Pandya ", ISBN = "9798560910570", Publisher = null, Title = "Sankat Mochan", Year = 2020 },
                new Book { Id = 1, Author = "Ruth Hogan", ISBN = "9780062473554", Publisher = "William Morrow Paperbacks", Title = "The Keeper of Lost Things: A Novel", Year = 2017 },
                new Book { Id = 2, Author = "Rita Mace Walston ", ISBN = "9781970137675", Publisher = "William Morrow Paperbacks", Title = "Paper and Ink, Flesh and Blood", Year = 2020 },
                new Book { Id = 3, Author = "Rita Mace Walston", ISBN = "9788514637675", Publisher = "Nowe Era", Title = "History of Tom Jones, a Foundling", Year = 2012 },
                new Book { Id = 4, Author = "Henryk Sienkiewicz", ISBN = "9788420733838", Publisher = "Anaya Infantil", Title = "Quo Vadis", Year = 1896 },
                new Book { Id = 5, Author = "Henryk Sienkiewicz", ISBN = "9789684168664", Publisher = "clasicos auriga", Title = "Quo Vadis", Year = 1896 }
            };

            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}