namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var contextDb = new BookShopContext();
            Console.WriteLine(RemoveBooks(contextDb));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            bool hasPassed = Enum.TryParse(typeof(AgeRestriction), command, true, out object? agerestObj);
            AgeRestriction ageRestriction;

            if (hasPassed)
            {
                ageRestriction = (AgeRestriction)agerestObj;

                var books = context.Books
                 .Where(b => b.AgeRestriction == ageRestriction)
                 .OrderBy(b => b.Title)
                 .Select(b => b.Title)
                 .ToArray();

                return string.Join(Environment.NewLine, books);
            }

            return null;

        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new { b.Title, b.Price })
                .ToArray();


            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.ToLower().Split().ToArray();

            var books = context.BooksCategories
                .Where(b => categories.Contains(b.Category.Name.ToLower()))
                .Select(b => b.Book.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime dateParsed = DateTime.ParseExact(date, "dd-MM-yyyy", null);

            var books = context.Books
                .Where(b => b.ReleaseDate < dateParsed)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType.ToString()} - ${b.Price:f2}")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authorBooks = context
                .Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => a.FirstName + " " + a.LastName)
                .OrderBy(name => name)
                .ToArray();

            return string.Join(Environment.NewLine, authorBooks);
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var bookTitlesContaining = context
                .Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return String.Join(Environment.NewLine, bookTitlesContaining);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var booksByAuthor = context
                .Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToArray();

            return String.Join(Environment.NewLine, booksByAuthor);
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int countOfBooks = context
                .Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();

            return countOfBooks;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var countCopiesByAuthor = context
                .Authors
                .OrderByDescending(a => a.Books.Sum(b => b.Copies))
                .Select(a => $"{a.FirstName} {a.LastName} - {a.Books.Sum(b => b.Copies)}")
                .ToArray();

            return string.Join(Environment.NewLine, countCopiesByAuthor);
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var totalProfitByCategory = context
                .Categories
                .OrderByDescending(c => c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price))
                .ThenBy(c => c.Name)
                .Select(c => $"{c.Name} ${c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price):f2}")
                .ToArray();

            return string.Join(Environment.NewLine, totalProfitByCategory);
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var mostRecentBooks = context
                .Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Books = c.CategoryBooks
                        .OrderByDescending(cb => cb.Book.ReleaseDate)
                        .Select(cb => new
                        {
                            BookTitle = cb.Book.Title,
                            BookYear = cb.Book.ReleaseDate.Value.Year
                        })
                        .Take(3)
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var c in mostRecentBooks)
            {
                sb.AppendLine($"--{c.CategoryName}");
                foreach (var b in c.Books)
                {
                    sb.AppendLine($"{b.BookTitle} ({b.BookYear})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var booksBefore2010 = context
                .Books
                .Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var book in booksBefore2010)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var booksToRemove = context
                .Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            context.RemoveRange(booksToRemove);
             context.SaveChanges();

            return booksToRemove.Count();
        }

    }
}


