using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Library_Management.Domain;
using Library_Management.Exceptions;

namespace Library_Management.Repository
{
    public class RepositoryFile
    {
        private List<Book> _books = new List<Book>();
        private readonly string _filePath;

        // Constructor: Initializes the repository and loads books from CSV file.
        // Params: none
        // Returns: void
        public RepositoryFile(string filePath)
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
            //Console.WriteLine("Saving to: " + _filePath);

            LoadFromFile();
        }

        // Loads all books from the CSV file into memory.
        // Params: none
        // Returns: void
        private void LoadFromFile()
        {
            if (!File.Exists(_filePath))
                return;

            try
            {
                var lines = File.ReadAllLines(_filePath);
                var books = new List<Book>();

                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    var id = Guid.Parse(parts[0]);
                    var title = parts[1];
                    var author = parts[2];
                    var quantity = int.Parse(parts[3]);
                    var initialQuantity = int.Parse(parts[4]);
                    var book = new Book(id, title, author, quantity);
                    book.InitialQuantity = initialQuantity;
                    books.Add(book);
                }

                _books = books;
            }
            catch (Exception)
            {
                throw new RepositoryException("Error while loading CSV file.");
            }
        }

        // Saves all in-memory books to the CSV file.
        // Params: none
        // Returns: void
        private void SaveToFile()
        {
            try
            {
                var lines = new List<string>();
                foreach (var b in _books)
                {
                    var line = b.Id + ";" + b.Title + ";" + b.Author + ";" + b.Quantity + ";" + b.InitialQuantity;
                    lines.Add(line);
                }

                File.WriteAllLines(_filePath, lines);
            }
            catch (Exception)
            {
                throw new RepositoryException("Error while saving CSV file.");
            }
        }

        // Adds a new book to the repository.
        // Params: book - the Book object to add
        // Returns: the added Book
        public Book Add(Book book)
        {
            if (_books.Any(b => b.Id == book.Id))
                throw new RepositoryException("Book already exists.");

            _books.Add(book);
            SaveToFile();
            return book;
        }

        // Retrieves a book by its ID.
        // Params: id - the Guid ID of the book
        // Returns: the found Book
        public Book Get(Guid id)
        {
            for (int i = 0; i < _books.Count; i++)
            {
                if (_books[i].Id == id)
                    return _books[i];
            }
            throw new RepositoryException("Book not found.");
        }

        // Returns all books in the repository.
        // Params: none
        // Returns: List of Book
        public List<Book> GetAll()
        {
            var allBooks = new List<Book>();
            foreach (var b in _books)
            {
                allBooks.Add(b);
            }
            return allBooks;
        }

        // Searches for books by title and/or author substring.
        // Params: title - partial or full title; author - partial or full author name
        // Returns: List of matching Book
        public List<Book> Find(string title = "", string author = "")
        {
            var results = new List<Book>();
            foreach (var b in _books)
            {
                bool matchesTitle = string.IsNullOrEmpty(title) 
                                    || (b.Title != null && b.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0);
                bool matchesAuthor = string.IsNullOrEmpty(author) 
                                     || (b.Author != null && b.Author.IndexOf(author, StringComparison.OrdinalIgnoreCase) >= 0);
                if (matchesTitle && matchesAuthor)
                {
                    results.Add(b);
                }
            }
            return results;
        }

        // Updates an existing book's information.
        // Params: book - the Book with updated data
        // Returns: void
        public void Update(Book book)
        {
            var existing = Get(book.Id);
            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.Quantity = book.Quantity;
            SaveToFile();
        }

        // Deletes a book by ID.
        // Params: id - the Guid ID of the book to delete
        // Returns: void
        public void Delete(Guid id)
        {
            var book = Get(id);
            _books.Remove(book);
            SaveToFile();
        }

        public void SaveFile()
        {
            SaveToFile();
        }
    }
}
