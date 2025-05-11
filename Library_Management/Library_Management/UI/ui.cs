using System;
using System.Linq;
using Library_Management.Domain;
using Library_Management.Service;

namespace Library_Management.UI
{
    public class UserInterface
    {
        private readonly BookService _service;

        public UserInterface(BookService service)
        {
            this._service = service;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n--- Library Management ---");
                Console.WriteLine("1. Add book");
                Console.WriteLine("2. View all books");
                Console.WriteLine("3. Update book");
                Console.WriteLine("4. Delete book");
                Console.WriteLine("5. Search books");
                Console.WriteLine("6. Borrow book");
                Console.WriteLine("7. Return book");
                Console.WriteLine("8. Show most borrowed books");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");
                var option = Console.ReadLine();

                try
                {
                    switch (option)
                    {
                        case "1": AddBook(); break;
                        case "2": ViewBooks(); break;
                        case "3": UpdateBook(); break;
                        case "4": DeleteBook(); break;
                        case "5": SearchBooks(); break;
                        case "6": BorrowBook(); break;
                        case "7": ReturnBook(); break;
                        case "8": ShowMostBorrowedBooks(); break;
                        case "0": return;
                        default: Console.WriteLine("Invalid option."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private void AddBook()
        {
            Console.Write("Title: ");
            var title = Console.ReadLine();
            Console.Write("Author: ");
            var author = Console.ReadLine();
            Console.Write("Quantity: ");
            var quantity = int.Parse(Console.ReadLine());
            _service.AddBook(Guid.NewGuid(), title, author, quantity);
            Console.WriteLine($"Book added.");
        }

        private void ViewBooks()
        {
            foreach (var book in _service.GetAllBooks())
            {
                PrintBook(book);
            }
        }

        private void UpdateBook()
        {
            Console.Write("Enter book ID to update: ");
            var id = Guid.Parse(Console.ReadLine());
            Console.Write("New Title: ");
            var title = Console.ReadLine();
            Console.Write("New Author: ");
            var author = Console.ReadLine();
            Console.Write("New Quantity: ");
            var quantity = int.Parse(Console.ReadLine());
            _service.UpdateBook(id, title, author, quantity);
            Console.WriteLine("Book updated.");
        }

        private void DeleteBook()
        {
            Console.Write("Enter book ID to delete: ");
            var id = Guid.Parse(Console.ReadLine());
            _service.DeleteBook(id);
            Console.WriteLine("Book deleted.");
        }

        private void SearchBooks()
        {
            Console.Write("Filter by title (leave empty for none): ");
            var title = Console.ReadLine();
            Console.Write("Filter by author (leave empty for none): ");
            var author = Console.ReadLine();

            var results = _service.SearchBooks(title, author);
            foreach (var book in results)
            {
                PrintBook(book);
            }
        }

        private void PrintBook(Book book)
        {
            Console.WriteLine($"ID: {book.Id}\nTitle: {book.Title}\nAuthor: {book.Author}\nQuantity: {book.Quantity}\n");
        }
        
        private void BorrowBook()
        {
            Console.Write("Enter book ID to borrow: ");
            var id = Guid.Parse(Console.ReadLine());
            Console.Write("Enter quantity to borrow: ");
            var amount = int.Parse(Console.ReadLine());
            _service.BorrowBook(id, amount);
            Console.WriteLine("Book borrowed successfully.");
        }
        
        private void ReturnBook()
        {
            Console.Write("Enter book ID to return: ");
            var id = Guid.Parse(Console.ReadLine());
            Console.Write("Enter quantity to return: ");
            var amount = int.Parse(Console.ReadLine());
            _service.ReturnBook(id, amount);
            Console.WriteLine("Book returned successfully.");
        }
        
        private void ShowMostBorrowedBooks()
        {
            Console.Write("How many top books to display? ");
            var count = int.Parse(Console.ReadLine());
            var topBooks = _service.GetMostBorrowedBooks(count);
    
            Console.WriteLine($"\nTop {count} most borrowed books:");
            foreach (var book in topBooks)
            {
                int borrowed = book.InitialQuantity - book.Quantity;
                Console.WriteLine($"Title: {book.Title} | Author: {book.Author} | Times Borrowed: {borrowed}");
            }
        }
    }
}