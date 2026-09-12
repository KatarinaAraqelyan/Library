using Library.Pg.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Pg.Data;

public class Queries
{
    public static void GetAllBooksByYear(LibraryContext db)
    {
        Console.WriteLine($"\n\n-----All books By Year-----");

        var books = db.Books.OrderBy(b => b.Year).ToList();
        if (!books.Any())
        {
            Console.WriteLine($"There is no books");
            return;
        }
        foreach (var b in books)
        {
            Console.WriteLine($" Book year - {b.Year}, Book title -  {b.Title}, Book Author - {b.Author}");
        }
    }

    public static void GetBookPublishedAfterYear(LibraryContext db, int year)
    {
        Console.WriteLine($"\n\n-----Books published after {year}-----");
        int currentYear = DateTime.Now.Year;

        if (year > currentYear)
        {
            Console.WriteLine("Invalid request (year cannot be in the future)");
            return; 
        }
        
        var books = db.Books
            .Where(b => b.Year > year)
            .OrderBy(b => b.Title)
            .ToList();

        if (!books.Any())
        {
            Console.WriteLine($"No books found published after {year}");
            return;
        }

        foreach (var b in books)
        {
            Console.WriteLine($"Book title - {b.Title}, Book year - ({b.Year})");
        }
    }

    public static void GetAllBooksByAuthor(LibraryContext db, string author = " ")
    {
        Console.WriteLine($"\n\n-----All books by{author}-----");
        
        var books = db.Books
            .Where(b => b.Author == author)
            .ToList();

        if (!books.Any())
        {
            Console.WriteLine($"No books found published by {author}");
            return;
        }
        
        foreach (var b in books)
        {
            Console.WriteLine($"{b.Title} by {b.Author}");
        }
    }

    public static void GetLongestBook(LibraryContext db)
    {
        Console.WriteLine("\n\n----- Longest book -----");

        var longestBook = db.Books
            .OrderByDescending(b => b.Pages)
            .FirstOrDefault();

        if (longestBook == null)
        {
            Console.WriteLine("There is no books");
            return;
        }

        Console.WriteLine($"Book Title - {longestBook.Title} ({longestBook.Pages} pages)");
    }

    public static void HasUnreadBooks(LibraryContext db)
    {
        Console.WriteLine("\n\n----- Has unread books -----");
        bool hasUnread = db.Books.Any(b => !b.IsRead);
        Console.WriteLine($"Answer - {hasUnread}");
    }

    public static void GetCountOfReadBooks(LibraryContext db)
    {
        Console.WriteLine("\n\n----- Count of read books -----");
        int count = db.Books.Count(b => b.IsRead);
        Console.WriteLine($"Count - {count}");
    }

    public static void GetTitlesAndYears(LibraryContext db)
    {
        Console.WriteLine("\n\n----- Titles and year -----");
        var books = db.Books
            .Select(b => new {  b.Title, b.Year })
            .ToList();
        
        if (!books.Any())
        {
            Console.WriteLine($"There is no books");
            return;
        }
        
        foreach (var b in books)
        {
            Console.WriteLine($"Book Title - {b.Title}, Book Year - {b.Year}");
        }

        Console.WriteLine($"ChangeTracker entries count - {db.ChangeTracker.Entries().Count()}");
    }

    public static void GetBooksPage(LibraryContext db, int pageNumber = 2, int pageSize = 3)
    {
        Console.WriteLine($"\n\n----- Page - {pageNumber} -----");

        var page = db.Books
            .OrderBy(b => b.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        if (!page.Any())
        {
            Console.WriteLine($"There is no books");
            return;
        }
        
        foreach (var b in page)
        {
            Console.WriteLine(b.Title);
        }
    }

    public static void GetFirstVsFind(LibraryContext db)
    {
        Console.WriteLine("\n\n----- Find vs First -----");
        var book1= db.Books.First(b => b.BookId == 1);
        var book2 = db.Books.Find(1);


        bool isSame = ReferenceEquals(book1, book2);
        Console.WriteLine($"Are both calls returned the same object instance? {isSame}");
    }

    public static void GetUnreadBooksTotalPrice(LibraryContext db)
    {
        Console.WriteLine("\n\n----- Total price of unread books -----");
        decimal totalPrice = db.Books
            .Where(b => !b.IsRead)
            .Sum(b => b.Price);

        Console.WriteLine($"Price - {totalPrice}");
    }

    public static void SearchByAuthor(LibraryContext db, string name)
    {
        Console.WriteLine($"\n\n----- Search '{name}' -----");

        var list1 = db.Books.Where(b => b.Author.Contains(name)).ToList();
        Console.WriteLine($"Contains count - {list1.Count}");

        var list2 = db.Books.Where(b => EF.Functions.ILike(b.Author, $"%{name}%")).ToList();
        Console.WriteLine($"ILike count - {list2.Count}");
    }
    
    public static void MarkBookAsRead(LibraryContext db, string title)
    {
        Console.WriteLine("\n\n----- Mark Book as Read -----");

        var book = db.Books.First(b => b.Title == title);
        Console.WriteLine($"State before change: {db.Entry(book).State}");
        book.IsRead = true;
        Console.WriteLine($"State after change: {db.Entry(book).State}");

        db.SaveChanges();
        Console.WriteLine("Changes saved successfully.");
    }

    public static void DeleteBook(LibraryContext db, string title)
    {
        Console.WriteLine("\n\n----- Delete Book -----");
        int countBefore = db.Books.Count();
        Console.WriteLine($"Books count after {countBefore}");
        
        var book = db.Books.FirstOrDefault(b => b.Title == title);
        if (book != null)
        {
            db.Books.Remove(book);
            db.SaveChanges();
        }
        Console.WriteLine("Successfully deleted");

        int countAfter = db.Books.Count();
        Console.WriteLine($"Books count after {countAfter}");
    }

    public static void BreakItOnPurpose(LibraryContext db, string title)
    {
        Console.WriteLine("\n\n----- Break It On Purpose -----");

        var book = db.Books.First(b => b.Title == title);
        book.IsRead = true;
        db.Books.Add(book);

        try
        {
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during SaveChanges: {ex.Message}");
        }
    }
    
    public static void ToQueryString(LibraryContext db)
    {
        Console.WriteLine("\n\n----- ToQueryString ----- ");

        var query = db.Books
            .Where(b => b.Year > 2005)
            .OrderBy(b => b.Title);

        string sql = query.ToQueryString();
        Console.WriteLine($"SQL: + {sql}");
        var result = query.ToList();
    }

    public static void AsNoTrackingComparision(LibraryContext db)
    {
        Console.WriteLine("\n\n----- AsNoTracking Comparison -----");

        db.ChangeTracker.Clear();
        var trackedBooks = db.Books.OrderBy(b => b.Year).ToList();
        Console.WriteLine($"Tracked Entries Count: {db.ChangeTracker.Entries().Count()}");

        db.ChangeTracker.Clear();
        var untrackedBooks = db.Books.AsNoTracking().OrderBy(b => b.Year).ToList();
        Console.WriteLine($"Untracked Entries Count: {db.ChangeTracker.Entries().Count()}");
    }

    public static List<Book> Search(LibraryContext db, int? minYear, string? author, bool? isRead)
    {
        Console.WriteLine("\n\n----- Dynamic Search -----");

        IQueryable<Book> query = db.Books;

        if (minYear.HasValue)
        {
            query = query.Where(b => b.Year >= minYear.Value);
        }

        if (!string.IsNullOrWhiteSpace(author))
        {
            query = query.Where(b => b.Author == author);
        }

        if (isRead.HasValue)
        {
            query = query.Where(b => b.IsRead == isRead.Value);
        }
        return query.ToList();
    }
    
    public static async Task AsyncOperations(LibraryContext db)
    {
        Console.WriteLine("\n\n----- Async Operations -----");

        var books = await db.Books.Where(b => b.Year > 2005).ToListAsync();
        var longestBook = await db.Books.OrderByDescending(b => b.Pages).FirstOrDefaultAsync();
        bool hasUnread = await db.Books.AnyAsync(b => !b.IsRead);
        int readCount = await db.Books.CountAsync(b => b.IsRead);
        decimal totalPrice = await db.Books.Where(b => !b.IsRead).SumAsync(b => b.Price);

        var singleBook = await db.Books.SingleOrDefaultAsync(b => b.Title == "Clean Code");
        if (singleBook != null)
        {
            singleBook.IsRead = true;
            await db.SaveChangesAsync();
        }
    }
    
    public static void DemonstrateUtcConstraint(LibraryContext db)
    {
        Console.WriteLine("\n\n----- UTC Timestamp Requirement -----");

        var invalidBook = new Book
        {
            Title = "Invalid Timestamp Book",
            Author = "Test Author",
            Year = 2024,
            Pages = 150,
            Price = 29.99m,
            IsRead = false,
            AddedAt = DateTime.Now 
        };

        db.Books.Add(invalidBook);

        try
        {
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: ");
            Console.WriteLine(ex.Message);
        }
        finally
        {
            db.ChangeTracker.Clear();
            Console.WriteLine("Change Tracker cleared.");
        }
    }

    public static void GenerateDdl(LibraryContext db)
    {
        Console.WriteLine("\n\n----- Generate DDL Script -----");
        string ddl = db.Database.GenerateCreateScript();
        Console.WriteLine(ddl);
    }

    public static void ServerGrouping(LibraryContext db) 
    {
        Console.WriteLine("\n\n-----  Server Side GroupBy -----");

        var authorStats = db.Books
            .GroupBy(b => b.Author)
            .Select(g => new
            {
                Author = g.Key,
                Count = g.Count(),
                Total = g.Sum(x => x.Price)
            })
            .OrderByDescending(x => x.Count)
            .ToList();

        foreach (var stat in authorStats)
        {
            Console.WriteLine($"Author: {stat.Author}, Count: {stat.Count}, Total Price: ${stat.Total}");
        }
    }
}