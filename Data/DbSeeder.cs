using Library.Pg.Models;

namespace Library.Pg.Data;

public class DbSeeder
{
    public static void Seed(LibraryContext db)
    {
        if (db.Books.Any())
        {
            return;
        }

        var books = new[]
        {
            new Book
            {
                Title = "Clean Code",
                Author = "Robert Martin",
                Year = 2008,
                Pages = 464,
                Price = 42.50m,
                IsRead = true,
                AddedAt = DateTime.UtcNow
            },
            new Book
            {
                Title = "The Pragmatic Programmer",
                Author = "Andrew Hunt",
                Year = 1999,
                Pages = 352,
                Price = 38.00m,
                IsRead = true,
                AddedAt = DateTime.UtcNow
            },
            new Book
            {
                Title = "Designing Data-Intensive Applications",
                Author = "Martin Kleppmann",
                Year = 2017,
                Pages = 616,
                Price = 55.90m,
                IsRead = false,
                AddedAt = DateTime.UtcNow
            },
            new Book
            {
                Title = "Refactoring",
                Author = "Martin Fowler",
                Year = 2018,
                Pages = 448,
                Price = 47.25m,
                IsRead = false,
                AddedAt = DateTime.UtcNow
            },
            new Book
            {
                Title = "Code Complete",
                Author = "Steve McConnell",
                Year = 2004,
                Pages = 960,
                Price = 51.00m,
                IsRead = true,
                AddedAt = DateTime.UtcNow
            },
            new Book
            {
                Title = "SQL Antipatterns",
                Author = "Bill Karwin",
                Year = 2010,
                Pages = 328,
                Price = 34.75m,
                IsRead = false,
                AddedAt = DateTime.UtcNow
            }
        };

        db.Books.AddRange(books);
        db.SaveChanges();
    }
}