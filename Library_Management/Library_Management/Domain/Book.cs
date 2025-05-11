using System;
using Library_Management.Exceptions;

namespace Library_Management.Domain
{
    public class Book
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; }
        
        public string Author { get; set; }
        
        public int Quantity { get; set; }
        
        public int InitialQuantity { get; set; }
        
        public Book(Guid id, string title, string author, int quantity)
        {
            Id = id;
            Title = title;
            Author = author;
            Quantity = quantity;
            InitialQuantity = quantity;
        }

        // Method: Increases the stock of the book by a given amount
        // Parameter: amount - the number of copies to add
        // Exception: Throws DomainException if the amount is less than or equal to zero
        public void IncreaseStock(int amount)
        {
            if (amount <= 0)
                throw new DomainException("Invalid amount for stock increase.");

            if (Quantity + amount > InitialQuantity)
                throw new DomainException($"Cannot return {amount} books. Total would exceed the initial stock of {InitialQuantity}.");

            Quantity += amount;
        }


        // Method: Decreases the stock of the book by a given amount
        // Parameter: amount - the number of copies to subtract
        // Exception: Throws DomainException if the amount is invalid or results in negative stock
        public void DecreaseStock(int amount)
        {
            if (amount <= 0)
                throw new DomainException("Invalid amount for stock decrease.");
            
            if (Quantity - amount < 0)
                throw new DomainException("Not enough stock available for borrowing.");
            
            Quantity -= amount;
        }
    }
}