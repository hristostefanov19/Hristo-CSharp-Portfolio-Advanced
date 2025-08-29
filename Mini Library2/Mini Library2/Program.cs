using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public bool IsBorrowed { get; set; }
}

class Program
{
    static string filePath = "library.txt";

    static void Main()
    {
        List<Book> books = LoadBooks();

        while (true)
        {
            Console.WriteLine("\n--- Mini Library ---");
            Console.WriteLine("1. Show all books");
            Console.WriteLine("2. Add book");
            Console.WriteLine("3. Borrow book");
            Console.WriteLine("4. Return book");
            Console.WriteLine("5. Remove book");
            Console.WriteLine("6. Search books");
            Console.WriteLine("7. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": ShowBooks(books); break;
                case "2": AddBook(books); break;
                case "3": BorrowBook(books); break;
                case "4": ReturnBook(books); break;
                case "5": RemoveBook(books); break;
                case "6": SearchBooks(books); break;
                case "7": SaveBooks(books); return;
                default: Console.WriteLine("Invalid choice!"); break;
            }
        }
    }

    static List<Book> LoadBooks()
    {
        List<Book> books = new List<Book>();
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                books.Add(new Book
                {
                    Title = parts[0],
                    Author = parts[1],
                    Year = int.Parse(parts[2]),
                    IsBorrowed = bool.Parse(parts[3])
                });
            }
        }
        return books;
    }

    static void SaveBooks(List<Book> books)
    {
        var lines = books.Select(b => $"{b.Title}|{b.Author}|{b.Year}|{b.IsBorrowed}");
        File.WriteAllLines(filePath, lines);
    }

    static void ShowBooks(List<Book> books)
    {
        if (books.Count == 0) { Console.WriteLine("No books found!"); return; }
        foreach (var b in books)
            Console.WriteLine($"{b.Title} | {b.Author} | {b.Year} | Borrowed: {b.IsBorrowed}");
    }

    static void AddBook(List<Book> books)
    {
        Console.Write("Title: "); string title = Console.ReadLine();
        Console.Write("Author: "); string author = Console.ReadLine();
        Console.Write("Year: "); int year = int.Parse(Console.ReadLine());
        books.Add(new Book { Title = title, Author = author, Year = year, IsBorrowed = false });
        SaveBooks(books);
        Console.WriteLine("Book added.");
    }

    static void BorrowBook(List<Book> books)
    {
        Console.Write("Title to borrow: "); string title = Console.ReadLine();
        var book = books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (book != null)
        {
            if (!book.IsBorrowed) { book.IsBorrowed = true; SaveBooks(books); Console.WriteLine("Book borrowed."); }
            else Console.WriteLine("Book is already borrowed.");
        }
        else Console.WriteLine("Book not found.");
    }

    static void ReturnBook(List<Book> books)
    {
        Console.Write("Title to return: "); string title = Console.ReadLine();
        var book = books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (book != null)
        {
            if (book.IsBorrowed) { book.IsBorrowed = false; SaveBooks(books); Console.WriteLine("Book returned."); }
            else Console.WriteLine("Book was not borrowed.");
        }
        else Console.WriteLine("Book not found.");
    }

    static void RemoveBook(List<Book> books)
    {
        Console.Write("Title to remove: "); string title = Console.ReadLine();
        int removed = books.RemoveAll(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (removed > 0) { SaveBooks(books); Console.WriteLine("Book removed."); }
        else Console.WriteLine("Book not found.");
    }

    static void SearchBooks(List<Book> books)
    {
        Console.Write("Enter search term: "); string term = Console.ReadLine();
        var results = books.Where(b => b.Title.Contains(term, StringComparison.OrdinalIgnoreCase)
                                    || b.Author.Contains(term, StringComparison.OrdinalIgnoreCase)
                                    || b.Year.ToString() == term).ToList();
        if (results.Count == 0) Console.WriteLine("No matching books found.");
        else foreach (var b in results) Console.WriteLine($"{b.Title} | {b.Author} | {b.Year} | Borrowed: {b.IsBorrowed}");
    }
}
