using System;

namespace LibraryManagementSystem
{
    // --- STEP 2: DEFINE BOOK CLASS ---
    public class Book : IComparable<Book>
    {
        public string BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public Book(string bookId, string title, string author)
        {
            BookId = bookId;
            Title = title;
            Author = author;
        }

        // Required to sort books alphabetically by Title for Binary Search
        public int CompareTo(Book other)
        {
            if (other == null) return 1;
            return string.Compare(this.Title, other.Title, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return $"[ID: {BookId}] '{Title}' by {Author}";
        }
    }

    // --- STEP 3: IMPLEMENT SEARCH ALGORITHMS ---
    public class LibrarySearch
    {
        // 1. LINEAR SEARCH
        public static int LinearSearchByTitle(Book[] library, string targetTitle)
        {
            for (int i = 0; i < library.Length; i++)
            {
                if (string.Equals(library[i].Title, targetTitle, StringComparison.OrdinalIgnoreCase))
                {
                    return i; // Found: return index
                }
            }
            return -1; // Not found
        }

        // 2. BINARY SEARCH
        public static int BinarySearchByTitle(Book[] sortedLibrary, string targetTitle)
        {
            int low = 0;
            int high = sortedLibrary.Length - 1;

            while (low <= high)
            {
                int mid = low + (high - low) / 2; // Prevents integer overflow
                int comparison = string.Compare(sortedLibrary[mid].Title, targetTitle, StringComparison.OrdinalIgnoreCase);

                if (comparison == 0)
                {
                    return mid; // Found!
                }
                else if (comparison < 0)
                {
                    low = mid + 1; // Target is in the right/upper half
                }
                else
                {
                    high = mid - 1; // Target is in the left/lower half
                }
            }
            return -1; // Not found
        }
    }

    // --- TEST DRIVER ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Initializing Library Catalog Engine ---\n");

            // Unsorted initial catalog array
            Book[] library = new Book[]
            {
                new Book("B01", "The Hobbit", "J.R.R. Tolkien"),
                new Book("B02", "1984", "George Orwell"),
                new Book("B03", "To Kill a Mockingbird", "Harper Lee"),
                new Book("B04", "The Great Gatsby", "F. Scott Fitzgerald"),
                new Book("B05", "Moby Dick", "Herman Melville")
            };

            string searchTitle = "The Great Gatsby";

            // --- Test Linear Search ---
            Console.WriteLine($"[Linear Search] Querying for: '{searchTitle}'");
            int linearIndex = LibrarySearch.LinearSearchByTitle(library, searchTitle);
            PrintResult(linearIndex, library);

            // --- Test Binary Search ---
            // Step 1: Data must be sorted alphabetically by title first!
            Console.WriteLine("\nSorting catalog alphabetically for Binary Search...");
            Array.Sort(library);

            Console.WriteLine("\nSorted Catalog:");
            foreach (var book in library) Console.WriteLine($" - {book.Title}");

            Console.WriteLine($"\n[Binary Search] Querying for: '{searchTitle}'");
            int binaryIndex = LibrarySearch.BinarySearchByTitle(library, searchTitle);
            PrintResult(binaryIndex, library);
        }

        private static void PrintResult(int index, Book[] library)
        {
            if (index != -1)
            {
                Console.WriteLine($" -> Match Found at position [{index}]: {library[index]}\n");
            }
            else
            {
                Console.WriteLine(" -> Match Not Found in library records.\n");
            }
        }
    }
}