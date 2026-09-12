using Library.Pg.Data;

namespace Library.Pg;

public class Program
{
    public static async Task Main(string[] args)
    {
        using (var db = new LibraryContext())
        {
            Console.WriteLine("Recreating database...");
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Console.WriteLine("Seeding database...");
            DbSeeder.Seed(db);
            
            Queries.GetAllBooksByYear(db);                       
            Queries.GetBookPublishedAfterYear(db, 2005);         
            Queries.GetAllBooksByAuthor(db, "Robert C. Martin"); 
            Queries.GetLongestBook(db);                        
            Queries.HasUnreadBooks(db);                       
            Queries.GetCountOfReadBooks(db);                 
            Queries.GetTitlesAndYears(db);                     
            Queries.GetBooksPage(db, pageNumber: 2, pageSize: 3);
            Queries.GetFirstVsFind(db);                         
            Queries.GetUnreadBooksTotalPrice(db);              
            Queries.SearchByAuthor(db, "Martin");                    
            
            Queries.MarkBookAsRead(db, "SQL Antipatterns");     
            Queries.DeleteBook(db, "The Pragmatic Programmer"); 
            Queries.BreakItOnPurpose(db, "SQL Antipatterns");   
            
            Queries.ToQueryString(db);              
            Queries.AsNoTrackingComparision(db);                     
            
            var searchResults = Queries.Search(db, minYear: 2000, author: null, isRead: true);
            Console.WriteLine($"Search results count: {searchResults.Count}");
            
            await Queries.AsyncOperations(db);
            
            Queries.DemonstrateUtcConstraint(db);               
            Queries.GenerateDdl(db);                     
            Queries.ServerGrouping(db);             
        }
    }
}