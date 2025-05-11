using System;
using System.Collections.Generic;
using System.Linq;
using Library_Management.Domain;
using Library_Management.Repository;
using Library_Management.Exceptions;
using Library_Management.Service;
using Library_Management.Validator;

namespace Library_Management.Service
{
    public class BookService
    {
        private readonly RepositoryFile _repository;

        // Constructor: Initializes the BookService with a repository to interact with book data.
        public BookService(RepositoryFile repository)
        {
            _repository = repository;
        }

        // Method: Adds a new book to the system.
        // Parameters: Guid id - unique identifier of the book, string title - title of the book, string author - author of the book, int quantity - available quantity of the book.
        public void AddBook(Guid id, string title, string author, int quantity)
        {
            var book = new Book(id, title, author, quantity);
            BookValidator.ValidateBook(book);  
            _repository.Add(book);  
        }

        // Method: Retrieves a book by its unique identifier.
        // Parameters: Guid id - unique identifier of the book.
        // Returns: Book object with the specified ID.
        public Book GetBook(Guid id)
        {
            return _repository.Get(id);  
        }

        // Method: Retrieves all books in the system.
        // Returns: An enumerable collection of all books.
        public IEnumerable<Book> GetAllBooks()
        {
            return _repository.GetAll();  
        }

        // Method: Searches for books based on title and/or author.
        // Parameters: string title - optional title filter, string author - optional author filter.
        // Returns: An enumerable collection of books matching the search criteria.
        public IEnumerable<Book> SearchBooks(string title = null, string author = null)
        {
            return _repository.Find(title, author);  
        }

        // Method: Updates the details of an existing book.
        // Parameters: Guid id - unique identifier of the book, string title - new title, string author - new author, int quantity - new quantity of the book.
        public void UpdateBook(Guid id, string title, string author, int quantity)
        {
            var book = new Book(id, title, author, quantity);
            BookValidator.ValidateBook(book);  
            _repository.Update(book);  
        }

        // Method: Deletes a book from the system by its unique identifier.
        // Parameters: Guid id - unique identifier of the book to be deleted.
        public void DeleteBook(Guid id)
        {
            _repository.Delete(id);  
        }

        // Method: Borrows a book, decreasing its stock by the specified amount.
        // Parameters: Guid id - unique identifier of the book, int amount - number of books to borrow.
        public void BorrowBook(Guid id, int amount)
        {
            var book = _repository.Get(id);  
            book.DecreaseStock(amount);  
            _repository.SaveFile();
        }

        // Method: Returns a book, increasing its stock by the specified amount.
        // Parameters: Guid id - unique identifier of the book, int amount - number of books to return.
        public void ReturnBook(Guid id, int amount)
        {
            var book = _repository.Get(id);  
            book.IncreaseStock(amount);  
            _repository.SaveFile();
        }
        
        // Method: Returns the most borrowed books, ordered by the number of borrowings.
        // Parameters: int top - the number of top borrowed books to return (default is 5).
        // Returns: A list of the most borrowed books, ordered from most to least borrowed.
        public List<Book> GetMostBorrowedBooks(int top = 5)
        {
            var books = _repository.GetAll().ToList();  // Get all books from repository

            // Bubble sort to order books by most borrowed (based on InitialQuantity - Quantity)
            for (int i = 0; i < books.Count - 1; i++)
            {
                for (int j = 0; j < books.Count - i - 1; j++)
                {
                    int borrowedBooks1 = books[j].InitialQuantity - books[j].Quantity;
                    int borrowedBooks2 = books[j + 1].InitialQuantity - books[j + 1].Quantity;
                    if (borrowedBooks1 < borrowedBooks2)
                    {
                        // Swap if the current book is less borrowed than the next one
                        var temp = books[j];
                        books[j] = books[j + 1];
                        books[j + 1] = temp;
                    }
                }
            }

            // Take top 'n' books from the sorted list
            return books.Take(top).ToList();
        }
    }
}
